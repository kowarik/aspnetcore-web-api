using my_books.Data.Models;
using System.Threading;

namespace my_books.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (!context.Books.Any())
                {
                    context.Books.AddRange(
                        new Book()
                        {
                            Title = "First Book Title",
                            Description = "First Book description",
                            IsRead = true,
                            DateRead = DateTime.Now.AddDays(-10),
                            Rate = 4,
                            Genre = "Biography",
                            //Author = "First Author",
                            CoverUrl = "https://....",
                            DateAdded = DateTime.Now,
                        },
                        new Book()
                        {
                            Title = "Second Book Title",
                            Description = "Second Book description",
                            IsRead = false,
                            //DateRead = DateTime.Now.AddDays(-10),
                            //Rate = 4,
                            Genre = "Biography",
                            //Author = "Second Author",
                            CoverUrl = "https://....",
                            DateAdded = DateTime.Now,
                        },
                        new Book()
                        {
                            Title = "Third Book Title",
                            Description = "Third Book description",
                            IsRead = true,
                            DateRead = DateTime.Now.AddDays(-10),
                            Rate = 4,
                            Genre = "Biography",
                            //Author = "Third Author",
                            CoverUrl = "https://....",
                            DateAdded = DateTime.Now,
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
