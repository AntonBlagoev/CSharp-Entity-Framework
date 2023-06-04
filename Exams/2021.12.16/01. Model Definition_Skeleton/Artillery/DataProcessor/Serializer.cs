
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Artillery.Utilities;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var result = context.Shells
                .Where(s => s.ShellWeight > shellWeight)
                .Select(s => new
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                        .Where(g => (int)g.GunType == 3)
                        .Select(g => new
                        {
                            GunType = g.GunType.ToString(),
                            GunWeight = g.GunWeight,
                            BarrelLength = g.BarrelLength,
                            Range = g.Range > 3000 ? "Long-range" : "Regular range",
                        })
                        .OrderByDescending(g => g.GunWeight)
                        .ToArray()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();


            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var result = context.Guns
                .Where(x => x.Manufacturer.ManufacturerName == manufacturer)
                .Select(x => new ExportGunDto
                {
                    Manufacturer = x.Manufacturer.ManufacturerName,
                    GunType = x.GunType.ToString(),
                    GunWeight = x.GunWeight,
                    BarrelLength = x.BarrelLength,
                    Range = x.Range,
                    Countries = x.CountriesGuns
                        .Where(c => c.Country.ArmySize > 4500000)
                        .Select(cg => new ExportCountryDto
                        {
                            CountryName = cg.Country.CountryName,
                            ArmySize = cg.Country.ArmySize
                        })
                        .OrderBy(c => c.ArmySize)
                        .ToArray()
                })
                .OrderBy(x => x.BarrelLength)
                .ToArray();


            return xmlHelper.Serialize(result, "Guns");
        }
    }
}
