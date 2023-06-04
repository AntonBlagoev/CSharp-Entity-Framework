namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;
    using Z.EntityFramework.Plus;

    public class StartUp
    {
        public static void Main()
        {
            using var dbContext = new BookShopContext();
            //DbInitializer.ResetDatabase(dbContext);

            // string ageRestriction = Console.ReadLine();
            //int year = int.Parse(Console.ReadLine());

            //string input = Console.ReadLine();
            //int input = int.Parse(Console.ReadLine());

            //string result = IncreasePrices(dbContext);

            //Console.WriteLine(result);

            //RemoveBooks(dbContext);
            Console.WriteLine(RemoveBooks(dbContext));
        }


        // 02. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext dbContext, string command)
        {
            try
            {
                AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

                string[] bookTitles = dbContext.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();
                return string.Join(Environment.NewLine, bookTitles);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // 03. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            string[] goldenBooks = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, goldenBooks);
        }

        // 04. Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            string[] books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        // 05. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            string[] books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        // 06. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            string[] books = context.Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        // 07. Released Before Date

        // Return the title, edition type and price of all books that are released before a given date.
        // The date will be a string in the format "dd-MM-yyyy".
        //Return all of the rows in a single string, ordered by release date(descending).

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            DateTime dateTime = Convert.ToDateTime(date, new CultureInfo("fr-FR"));

            var books = context.Books
                .Where(b => b.ReleaseDate < dateTime)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 08. Author Search

        // Return the full names of authors, whose first name ends with a given string.
        // Return all names in a single string, each on a new row, ordered alphabetically.

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName
                })
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        // 09. Book Search

        // Return the titles of the book, which contain a given string. Ignore casing.
        // Return all titles in a single string, each on a new row, ordered alphabetically

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        // 10. Book Search by Author

        // Return all titles of books and their authors' names for books, which are written by authors whose last names start with the given string.
        // Return a single string with each title on a new row.Ignore casing.Order by BookId ascending.

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    b.Author.FirstName,
                    b.Author.LastName
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FirstName} {book.LastName})");
            }

            return sb.ToString().TrimEnd();
        }

        // 11. Count Books

        // Return the number of books, which have a title longer than the number given as an input.
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var count = context.Books
                 .Where(b => b.Title.Length > lengthCheck)
                 .Count();

            return count;
        }

        // 12. Total Book Copies

        // Return the total number of book copies for each author. Order the results descending by total book copies.
        // Return all results in a single string, each on a new line.

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var totalBookCopies = context.Authors
                .Select(a => new
                {
                    AuthorFullName = $"{a.FirstName} {a.LastName}",
                    TotalCopies = a.Books.Sum(b => b.Copies)

                })
                .OrderByDescending(b => b.TotalCopies)
                .ToArray();


            foreach (var author in totalBookCopies)
            {
                sb.AppendLine($"{author.AuthorFullName} - {author.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        // 13. Profit by Category

        // Return the total profit of all books by category.
        // Profit for a book can be calculated by multiplying its number of copies by the price per single book.
        // Order the results by descending by total profit for a category and ascending by category name.
        // Print the total profit formatted to the second digit.

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    TotalProfit = c.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price)
                })
                .OrderByDescending(r => r.TotalProfit)
                .ThenBy(r => r.CategoryName)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.CategoryName} ${book.TotalProfit:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 14. Most Recent Books

        // Get the most recent books by categories. The categories should be ordered by name alphabetically.
        // Only take the top 3 most recent books from each category – ordered by release date (descending).
        // Select and print the category name and for each book – its title and release year

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var mostRecentBook = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    BookInfo = c.CategoryBooks
                    .Select(b => new
                    {
                        BookTitle = b.Book.Title,
                        BookReleaseDate = b.Book.ReleaseDate
                    })
                    .OrderByDescending(r => r.BookReleaseDate)
                    .Take(3),
                })
                .ToArray();

            foreach (var category in mostRecentBook)
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.BookInfo)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.BookReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        // 15. Increase Prices

        // Increase the prices of all books released before 2010 by 5.

        public static void IncreasePrices(BookShopContext context)
        {
            var bookReleasedBefore2010 = context.Books
                   .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010)
                   .ToArray();

            foreach (var book in bookReleasedBefore2010)
            {
                book.Price += 5;
            }

            context.SaveChanges();

            // or

            //context.Books
            //    .Where(b => b.ReleaseDate.HasValue &&
            //                b.ReleaseDate.Value.Year < 2010)
            //    .Update(b => new Book() { Price = b.Price + 5 });
        }

        // 16. Remove Books

        // Remove all books, which have less than 4200 copies. Return an int - the number of books that were deleted from the database.
        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemove = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            context.RemoveRange(booksToRemove);
            context.SaveChanges();

            return booksToRemove.Count();
        }
    }
}


