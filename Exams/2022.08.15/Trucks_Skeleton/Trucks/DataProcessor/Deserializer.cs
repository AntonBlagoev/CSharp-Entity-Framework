namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();
            ImportDespatcherDto[] despatchersDtos = xmlHelper.Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");

            ICollection<Despatcher> validDespatchers = new HashSet<Despatcher>();
            foreach (ImportDespatcherDto despatcherDto in despatchersDtos)
            {
                if (!IsValid(despatcherDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Truck> validTrucks = new HashSet<Truck>();

                foreach (ImportTruckDto truckDto in despatcherDto.Trucks!)
                {
                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = new Truck()
                    {
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = (CategoryType)truckDto.CategoryType,
                        MakeType = (MakeType)truckDto.MakeType
                    };

                    validTrucks.Add(truck);
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = despatcherDto.Name,
                    Position = despatcherDto.Position,
                    Trucks = validTrucks
                };

                validDespatchers.Add(despatcher);

                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcherDto.Name, despatcher.Trucks.Count));

            }
            context.Despatchers.AddRange(validDespatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportClientDto[]? clientsDtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);

            ICollection<Client> validClients = new HashSet<Client>();
            ICollection<int> existingTruckIds = context.Trucks
                .Select(t => t.Id)
                .ToArray();

            foreach (ImportClientDto clientDto in clientsDtos!)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (clientDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type,
                };

                foreach (int truckId in clientDto.TruckId.Distinct())
                {
                    if (!existingTruckIds.Contains(truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck ct = new ClientTruck()
                    {
                        Client = client,
                        TruckId = truckId
                    };

                    client.ClientsTrucks.Add(ct);
                }

                validClients.Add(client);

                sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }

            context.Clients.AddRange(validClients);
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