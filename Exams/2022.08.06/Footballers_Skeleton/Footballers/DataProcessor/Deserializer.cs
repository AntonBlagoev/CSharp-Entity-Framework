namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();
            ImportCoachDto[] coachesDtos = xmlHelper.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");

            ICollection<Coach> validCoaches = new HashSet<Coach>();
            foreach (ImportCoachDto coachDto in coachesDtos)
            {
                if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Footballer> validFootballers = new HashSet<Footballer>();
                foreach (ImportFootballerDto footballerDto in coachDto.Footballers)
                {
                    if (!IsValid(footballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = DateTime.ParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ContractEndDate = DateTime.ParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BestSkillType = (BestSkillType)footballerDto.BestSkillType,
                        PositionType = (PositionType)footballerDto.PositionType
                    };

                    if (footballer.ContractStartDate >= footballer.ContractEndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validFootballers.Add(footballer);
                }

                Coach coach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality,
                    Footballers = validFootballers
                };

                validCoaches.Add(coach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }
            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportTeamDto[]? teamsDtos = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            ICollection<Team> validTeams = new HashSet<Team>();

            int[] contextFootballersIds = context.Footballers.Select(x => x.Id).ToArray();

            foreach (ImportTeamDto teamDto in teamsDtos!)
            {
                if (!IsValid(teamDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (teamDto.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team()
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies
                };

                foreach (var footballerId in teamDto.FootballersIds!.Distinct())
                {
                    if (!contextFootballersIds.Contains(footballerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    TeamFootballer teamFootballer = new TeamFootballer()
                    {
                        Team = team,
                        FootballerId = footballerId,
                    };
                    team.TeamsFootballers.Add(teamFootballer);
                }

                validTeams.Add(team);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }
            context.Teams.AddRange(validTeams);
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
