namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.DataProcessor.ExportDto;
    using Theatre.Utilities;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var result = context.Theatres
                //.ToArray()
                //.Where(x => x.NumberOfHalls >= numbersOfHalls && x.Tickets.Count() >= 20)
                .Where(th =>
                    th.Tickets.Where(r => r.RowNumber <= 5).Sum(r => r.RowNumber) >= 20 &&
                    th.NumberOfHalls >= numbersOfHalls)
                .Select(th => new
                {
                    Name = th.Name,
                    Halls = th.NumberOfHalls,
                    TotalIncome = th.Tickets.Where(ti => ti.RowNumber <= 5).Sum(ti => ti.Price),
                    Tickets = th.Tickets
                    .Where(t => t.RowNumber <= 5)
                    .Select(t => new
                    {
                        Price = t.Price,
                        RowNumber = t.RowNumber,
                    })
                    .OrderByDescending(t => t.Price)
                    .ToArray()
                })
                .OrderByDescending(th => th.Halls)
                .ThenBy(th => th.Name)
                .ToArray();

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            XmlHelper xmlHelper = new XmlHelper();
            var result = context.Plays
                .Where(p => p.Rating <= raiting)
                .ToArray()
                .Select(p => new ExportPlayDto
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                    .Where(a => a.IsMainCharacter == true)
                    .Select(a => new ExportActorDto
                    {
                        FullName = a.FullName,
                        MainCharacter = $"Plays main character in '{p.Title}'."
                    })
                    .OrderByDescending(a => a.FullName)
                    .ToArray()
                })
                .OrderBy(x => x.Title)
                .ThenByDescending(x => x.Genre)
                .ToArray();





            return xmlHelper.Serialize(result, "Plays");
        }
    }
}
