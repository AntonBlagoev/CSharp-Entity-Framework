namespace VaporStore.DataProcessor
{ 
    using Data;
    using Newtonsoft.Json;
    using System.Globalization;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ExportDto;
    using VaporStore.Utilities;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var result = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .ToArray()
                .Select(g => new
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games
                        .Where(ga => ga.Purchases.Count > 0)
                        .Select(ga => new
                        {
                            Id = ga.Id,
                            Title = ga.Name,
                            Developer = ga.Developer.Name,
                            Tags = string.Join(", ", ga.GameTags.Select(gt => gt.Tag.Name).ToArray()),
                            Players = ga.Purchases.Count,
                        })
                        .OrderByDescending(ga => ga.Players)
                        .ThenBy(ga => ga.Id)
                        .ToArray(),
                    TotalPlayers = g.Games.Sum(ga => ga.Purchases.Count)
                })
                .OrderByDescending(g => g.TotalPlayers)
                .ThenBy(g => g.Genre)
                .ToArray();



            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var result = context.Users
                .Where(u => u.Cards.Any(x => x.Purchases.Any()))
                .ToArray()
                .Select(u => new ExportUserDto
                {
                    Username = u.Username,
                    Purchases = context.Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type.ToString() == purchaseType)
                        .Select(p => new ExportPurchaseDto
                        {
                            CardNumber = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            GameTitle = new ExportGameDto
                            {
                                Title = p.Game.Name,
                                Genre = p.Game.Genre.Name.ToString(),
                                Price = p.Game.Price,
                            }
                        })
                        .OrderBy(x => x.Date)
                        .ToArray(),
                    TotalSpent = context.Purchases
                            .ToArray()
                            .Where(p => p.Card.User.Username == u.Username && p.Type.ToString() == purchaseType)
                            .Sum(p => p.Game.Price)
                })
                .Where(u => u.Purchases.Length > 0)
                .OrderByDescending(u => u.Purchases.Sum(x => x.GameTitle.Price))
                .ThenBy(x => x.Username)
                .ToArray();






                return xmlHelper.Serialize(result, "Users");
        }
    }
}