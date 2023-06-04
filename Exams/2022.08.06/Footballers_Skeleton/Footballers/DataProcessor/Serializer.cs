namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var coaches = context.Coaches
                .Where(c => c.Footballers.Count > 0)
                .Select(c => new ExportCoachDto
                {
                    FootballersCount = c.Footballers.Count,
                    Name = c.Name,
                    Footballers = c.Footballers
                        .Select(f => new ExportFootballerDto
                        {
                            Name = f.Name,
                            PositionType = f.PositionType.ToString()
                        })
                        .OrderBy(f => f.Name)
                        .ToArray()
                })
                .OrderByDescending(c => c.FootballersCount)
                .ThenBy(c => c.Name)
                .ToArray();

            return xmlHelper.Serialize(coaches, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                            .Where(f => f.Footballer.ContractStartDate >= date)
                            .ToArray()
                            .OrderByDescending(f => f.Footballer.ContractEndDate)
                            .ThenBy(f => f.Footballer.Name)
                            .Select(tf => new
                            {
                                FootballerName = tf.Footballer.Name,
                                ContractStartDate = tf.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                                ContractEndDate = tf.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                                BestSkillType = tf.Footballer.BestSkillType.ToString(),
                                PositionType = tf.Footballer.PositionType.ToString()
                            })
                            
                            .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Length)
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();


            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
