using Microsoft.EntityFrameworkCore;

namespace BlogViewExample
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Blog> Blogs { get; set; }

        internal async Task SeedAsync()
        {
            await Database.EnsureCreatedAsync();

            Blogs.AddRange(new Blog
            {
                Title = "Awesome blog",
                CreatedOn = new DateTime(2022, 12, 21),
                Posts = new List<Post>
                {
                    new Post
                    {
                        Content = "First post",
                        CreatedOn = new DateTime(2022,12,22)
                    },
                    new Post
                    {
                        Content = "Second post",
                        CreatedOn = new DateTime(2022,12,22)
                    }
                }
            },
            new Blog
            {
                Title = "Another Awesome blog",
                CreatedOn = new DateTime(2021, 12, 21),
                Posts = new List<Post>
                {
                    new Post
                    {
                        Content = "First post",
                        CreatedOn = new DateTime(2021,12,22)
                    },
                    new Post
                    {
                        Content = "Second post",
                        CreatedOn = new DateTime(2021,12,22)
                    }
                }
            });

            await SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().HasMany(b => b.Posts).WithOne();
        }
    }

    public class Blog : IBlogView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public interface IBlogView
    {
        int Id { get; }
        string Title { get; }
    }

    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
