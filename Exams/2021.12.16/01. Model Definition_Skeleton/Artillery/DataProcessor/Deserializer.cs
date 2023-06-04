namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Artillery.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();

            ImportCountryDto[] countriesDtos = xmlHelper.Deserialize<ImportCountryDto[]>(xmlString, "Countries");

            ICollection<Country> validCountries = new HashSet<Country>();
            foreach (ImportCountryDto countryDto in countriesDtos)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize
                };
                validCountries.Add(country);
                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(validCountries);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();

            ImportManufacturerDto[] manufacturersDtos = xmlHelper.Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");

            ICollection<Manufacturer> validManufacturers = new HashSet<Manufacturer>();
            foreach (ImportManufacturerDto dto in manufacturersDtos)
            {
                var uniqueManufacturer = validManufacturers.FirstOrDefault(x => x.ManufacturerName == dto.ManufacturerName);

                if (!IsValid(dto) || uniqueManufacturer != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = dto.ManufacturerName,
                    Founded = dto.Founded,
                };
                validManufacturers.Add(manufacturer);

                // The Founded entity will be separated by comma and space ", ".
                // Author solution
                var manufacturerCountry = manufacturer.Founded.Split(", ").ToArray();
                var last = manufacturerCountry.Skip(Math.Max(0, manufacturerCountry.Count() - 2)).ToArray();
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, string.Join(", ", last)));

                //sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, manufacturer.Founded));
            }

            context.Manufacturers.AddRange(validManufacturers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();

            ImportShellDto[] shellsDtos = xmlHelper.Deserialize<ImportShellDto[]>(xmlString, "Shells");

            ICollection<Shell> validShells = new HashSet<Shell>();
            foreach (ImportShellDto dto in shellsDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = dto.ShellWeight,
                    Caliber = dto.Caliber
                };
                validShells.Add(shell);
                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var validGunTypes = new string[] { "Howitzer", "Mortar", "FieldGun", "AntiAircraftGun", "MountainGun", "AntiTankGun" };

            StringBuilder sb = new StringBuilder();
            ImportGunDto[] gunsDtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);
            var validGuns = new HashSet<Gun>();

            foreach (var dto in gunsDtos)
            {
                if (!IsValid(dto) ||
                    !validGunTypes.Contains(dto.GunType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var gun = new Gun
                {
                    ManufacturerId = dto.ManufacturerId,
                    GunWeight = dto.GunWeight,
                    BarrelLength = dto.BarrelLength,
                    NumberBuild = dto.NumberBuild,
                    Range = dto.Range,
                    GunType = (GunType)Enum.Parse(typeof(GunType), dto.GunType),
                    ShellId = dto.ShellId
                };

                foreach (var countryDto in dto.Countries)
                {
                    gun.CountriesGuns.Add(new CountryGun()
                    {
                        CountryId = countryDto.Id,
                        Gun = gun,
                    });
                }
                validGuns.Add(gun);
                sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
            }
            context.Guns.AddRange(validGuns);
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