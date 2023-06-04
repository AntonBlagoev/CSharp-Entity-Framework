namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();
            ImportCreatorDto[] creatorsDtos = xmlHelper.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

            ICollection<Creator> validCreators = new HashSet<Creator>();

            foreach (ImportCreatorDto creatorDto in creatorsDtos)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName,
                };

                foreach (var game in creatorDto.Boardgames!)
                {
                    if (!IsValid(game))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = new Boardgame()
                    {
                        Name = game.Name,
                        Rating = game.Rating,
                        YearPublished = game.YearPublished,
                        CategoryType = (CategoryType)Enum.Parse(typeof(CategoryType), game.CategoryType),
                        Mechanics = game.Mechanics,
                    };

                    creator.Boardgames.Add(boardgame);
                }
                validCreators.Add(creator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
            }
            context.AddRange(validCreators);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportSellerDto[] sellersDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString)!;

            ICollection<Seller> validSellers = new HashSet<Seller>();

            int[] existingGamesIds = context.Boardgames.Select(x => x.Id).ToArray();

            foreach (ImportSellerDto sellerDto in sellersDtos)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website,
                };

                foreach (int gameId in sellerDto.Boardgames!.Distinct()) 
                { 
                    if (!existingGamesIds.Contains(gameId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    };

                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    {
                        BoardgameId = gameId,
                        Seller = seller,
                    };

                    seller.BoardgamesSellers.Add(boardgameSeller);
                }
                validSellers.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }
            context.AddRange(validSellers);
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
