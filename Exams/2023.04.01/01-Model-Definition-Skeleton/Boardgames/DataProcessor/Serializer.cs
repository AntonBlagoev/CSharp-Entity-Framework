namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var result = context.Creators
                .Where(c => c.Boardgames.Count() > 0)
                .ToArray()
                .Select(c => new ExportCreatorDto
                {
                    BoardgamesCount = c.Boardgames.Count(),
                    CreatorName = $"{c.FirstName} {c.LastName}",
                    Boardgames = c.Boardgames
                    .Select(bg => new ExportBoardgameDto
                    {
                        Name = bg.Name,
                        YearPublished = bg.YearPublished,
                    })
                    .OrderBy(c => c.Name)
                    .ToArray()
                })
                .OrderByDescending(x => x.BoardgamesCount)
                .ThenBy(x => x.CreatorName)
                .ToArray();

            return xmlHelper.Serialize(result, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {

            var result = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(x => x.Boardgame.YearPublished >= year && x.Boardgame.Rating <= rating))
                .ToArray()
                .Select(s => new
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                    .Where(x => x.Boardgame.YearPublished >= year && x.Boardgame.Rating <= rating)
                    .ToArray()
                    .Select(bg => new
                    {
                        Name = bg.Boardgame.Name,
                        Rating = bg.Boardgame.Rating,
                        Mechanics = bg.Boardgame.Mechanics,
                        Category = bg.Boardgame.CategoryType.ToString(),
                    })
                    .OrderByDescending(x => x.Rating)
                    .ThenBy(x => x.Name)
                    .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Count())
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }
    }
}