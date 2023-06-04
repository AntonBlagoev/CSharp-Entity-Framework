namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Castle.Core.Internal;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ImportDto;
    using VaporStore.Utilities;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportGameDto[] gamesDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString)!;

            ICollection<Game> validGames = new HashSet<Game>();
            ICollection<Developer> existingDevelopers = new HashSet<Developer>();
            ICollection<Genre> genres = new HashSet<Genre>();
            ICollection<Tag> tags = new HashSet<Tag>();

            foreach (ImportGameDto gameDto in gamesDtos)
            {
                if (!IsValid(gameDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime releaseDate;
                bool isValidReleaseDate =
                    DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                if (!isValidReleaseDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                };
                if (gameDto.Tags.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game game = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = releaseDate,
                };

                Developer gameDeveloper = existingDevelopers.FirstOrDefault(d => d.Name == gameDto.Developer)!;
                if (gameDeveloper == null)
                {
                    Developer newDeveloper = new Developer()
                    {
                        Name = gameDto.Developer
                    };
                    existingDevelopers.Add(newDeveloper);
                    gameDeveloper = newDeveloper;
                }
                game.Developer = gameDeveloper;

                Genre gameGenre = genres.FirstOrDefault(g => g.Name == gameDto.Genre)!;
                if (gameGenre == null)
                {
                    Genre newGenre = new Genre()
                    {
                        Name = gameDto.Genre
                    };
                    genres.Add(newGenre);
                    gameGenre = newGenre;
                }
                game.Genre = gameGenre;

                foreach (string tagName in gameDto.Tags)
                {
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    Tag gameTag = tags.FirstOrDefault(t => t.Name == tagName)!;
                    if (gameTag == null)
                    {
                        Tag newGameTag = new Tag()
                        {
                            Name = tagName
                        };
                        tags.Add(newGameTag);
                        gameTag = newGameTag;
                    }
                    game.GameTags.Add(new GameTag()
                    {
                        Game = game,
                        Tag = gameTag
                    });
                }
                if (game.GameTags.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                validGames.Add(game);
                sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));
            }
            context.Games.AddRange(validGames);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportUserDto[] usersDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString)!;

            ICollection<User> validUsers = new HashSet<User>();

            foreach (ImportUserDto userDto in usersDtos)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User user = new User()
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age,
                };

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Card card = new Card()
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.Cvc,
                        Type = (CardType)Enum.Parse(typeof(CardType), cardDto.Type),
                    };
                    user.Cards.Add(card);
                }
                validUsers.Add(user);
                sb.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));
            }
            context.Users.AddRange(validUsers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();
            ImportPurchaseDto[] purchasesDtos = xmlHelper.Deserialize<ImportPurchaseDto[]>(xmlString, "Purchases");
            ICollection<Purchase> validPurchases = new HashSet<Purchase>();

            string[] gamesTitles = context.Games.Select(t => t.Name).ToArray();

            foreach (ImportPurchaseDto purchaseDto in purchasesDtos)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                };

                Game game = context.Games.FirstOrDefault(x => x.Name == purchaseDto.Title)!;
                Card card = context.Cards.FirstOrDefault(x => x.Number == purchaseDto.Card)!;
                DateTime purchaseDate;
                bool isValidPurchaseDate =
                    DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out purchaseDate);

                if (!isValidPurchaseDate || game == null || card == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                };

                Purchase purchase = new Purchase()
                {
                    Game = game,
                    Type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchaseDto.Type),
                    ProductKey = purchaseDto.ProductKey,
                    Card = card,
                    Date = purchaseDate
                };

                validPurchases.Add(purchase);
                sb.AppendLine(string.Format(SuccessfullyImportedPurchase, purchase.Game.Name, card.User.Username));
            }
            context.Purchases.AddRange(validPurchases);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}