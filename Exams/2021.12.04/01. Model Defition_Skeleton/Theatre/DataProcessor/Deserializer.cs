namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;
    using Theatre.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();
            ImportPlayDto[] playsDtos = xmlHelper.Deserialize<ImportPlayDto[]>(xmlString, "Plays");
            ICollection<Play> validPlays = new HashSet<Play>();
            string[] validGenres = new string[] { "Drama", "Comedy", "Romance", "Musical" };

            foreach (ImportPlayDto dto in playsDtos)
            {
                if (!IsValid(dto) || !validGenres.Contains(dto.Genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (dto.Duration.Substring(0, 2) == "00")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                Play play = new Play()
                {
                    Title = dto.Title,
                    Duration = TimeSpan.ParseExact(dto.Duration, "c", CultureInfo.InvariantCulture),
                    Rating = dto.Rating,
                    Genre = Enum.Parse<Genre>(dto.Genre),
                    Description = dto.Description,
                    Screenwriter = dto.Screenwriter,
                };
                validPlays.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
            }
            context.Plays.AddRange(validPlays);
            context.SaveChanges();  

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();
            ImportCastDto[] castsDtos = xmlHelper.Deserialize<ImportCastDto[]>(xmlString, "Casts");
            ICollection<Cast> validCasts = new HashSet<Cast>();

            foreach (ImportCastDto dto in castsDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Cast cast = new Cast()
                {
                    FullName = dto.FullName,
                    IsMainCharacter = dto.IsMainCharacter,
                    PhoneNumber = dto.PhoneNumber,
                    PlayId  = dto.PlayId,
                };

                validCasts.Add(cast);
                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, cast.IsMainCharacter == true ? "main" : "lesser"));
            }


            context.Casts.AddRange(validCasts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        
        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportTheatreDto[] theatersDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);
            ICollection<Theatre> validTheatres = new HashSet<Theatre>();

            foreach (ImportTheatreDto dto in theatersDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre()
                {
                    Name = dto.Name,
                    NumberOfHalls = dto.NumberOfHalls,
                    Director = dto.Director,    
                };

                foreach (var id in dto.Tickets)
                {
                    if (!IsValid(id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Ticket ticketDto = new Ticket() 
                    { 
                        Price = id.Price,
                        RowNumber = id.RowNumber,
                        PlayId = id.PlayId,
                    };
                    theatre.Tickets.Add(ticketDto);
                }
                validTheatres.Add(theatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }

            context.Theatres.AddRange(validTheatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
