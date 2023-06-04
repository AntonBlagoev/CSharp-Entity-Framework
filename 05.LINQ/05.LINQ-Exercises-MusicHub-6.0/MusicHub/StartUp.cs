namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;

    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here

            string result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
        }
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            //You need to write method string ExportAlbumsInfo(MusicHubDbContext context, int producerId) in the StartUp class that receives a ProducerId.
            //Export all albums which are produced by the provided ProducerId.
            //For each Album, get the Name, ReleaseDate in format the "MM/dd/yyyy", ProducerName, the Album Songs with each Song Name,
            //Price (formatted to the second digit) and the Song WriterName.Sort the Songs by Song Name (descending) and by Writer (ascending).
            //At the end export the Total Album Price with exactly two digits after the decimal place. Sort the Albums by their Total Price (descending).

            //-AlbumName: Devil's advocate
            //-ReleaseDate: 07 / 21 / 2018
            //-ProducerName: Evgeni Dimitrov
            //-Songs:
            //---#1
            //---SongName: Numb
            //---Price: 13.99
            //---Writer: Kara - lynn Sharpous
            //---#2
            //---SongName: Ibuprofen
            //---Price: 26.50
            //--- Writer: Stanford Daykin
            //-AlbumPrice: 40.49
            //…


            StringBuilder sb = new StringBuilder();

            var albumsInfo = context.Albums
            .Where(a => a.ProducerId!.Value == producerId)
            .ToArray()
            .OrderByDescending(a => a.Price)
            .Select(a => new
            {
                AlbumName = a.Name,
                ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                ProducerName = a.Producer!.Name,
                Songs = a.Songs
                    .Select(s => new
                    {
                        SongName = s.Name,
                        Price = s.Price.ToString("f2"),
                        Writer = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(s => s.Writer)
                    .ToArray(),
                AlbumPrice = a.Price.ToString("f2")
            })
            .ToArray();

            foreach (var album in albumsInfo)
            {
                sb
                    .AppendLine($"-AlbumName: {album.AlbumName}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                    .AppendLine($"-ProducerName: {album.ProducerName}")
                    .AppendLine($"-Songs:");

                int songNumber = 1;

                foreach (var s in album.Songs)
                {
                    sb
                        .AppendLine($"---#{songNumber}")
                        .AppendLine($"---SongName: {s.SongName}")
                        .AppendLine($"---Price: {s.Price}")
                        .AppendLine($"---Writer: {s.Writer}");

                    songNumber++;
                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice}");
            }

            return sb.ToString().TrimEnd();

        }
        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            //You need to write method string ExportSongsAboveDuration(MusicHubDbContext context, int duration) in the StartUp class that receives Song duration(integer, in seconds).
            //Export the songs which are above the given duration.For each Song, export its Name, Performer Full Name, Writer Name, Album Producer and Duration(in format("c")).
            //Sort the Songs by their Name(ascending), and then by Writer(ascending). If a Song has more than one Performer, export all of them and sort them(ascending).
            //If there are no Performers for a given song, don't print the "---Performer" line at all.

            //-Song #1
            //---SongName: Away
            //---Writer: Norina Renihan
            //---Performer: Lula Zuan
            //---AlbumProducer: Georgi Milkov
            //---Duration: 00:05:35
            //-Song #2
            //---SongName: Bentasil
            //---Writer: Mik Jonathan
            //---Performer: Zabrina Amor
            //---AlbumProducer: Dobromir Slavchev
            //---Duration: 00:04:03

            StringBuilder sb = new StringBuilder();

            var sonsInfo = context.Songs
                .AsEnumerable() // for s.Duration.TotalSeconds
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    Writer = s.Writer.Name,
                    Performers = s.SongPerformers
                        .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                        .OrderBy(p => p)
                        .ToArray(),
                    AlbumProducer = s.Album!.Producer!.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.SongName)
                .ThenBy(w => w.Writer)
                .ToArray();

            int songNumber = 1;

            foreach (var s in sonsInfo)
            {
                sb
                    .AppendLine($"-Song #{songNumber}")
                    .AppendLine($"---SongName: {s.SongName}")
                    .AppendLine($"---Writer: {s.Writer}");

                foreach (var performer in s.Performers)
                {
                    sb.AppendLine($"---Performer: {performer}");
                }

                sb
                   .AppendLine($"---AlbumProducer: {s.AlbumProducer}")
                   .AppendLine($"---Duration: {s.Duration}");

                songNumber++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
