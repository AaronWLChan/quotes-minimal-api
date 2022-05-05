using Microsoft.EntityFrameworkCore;
using minimal_api.Models;

namespace minimal_api
{
    class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options) { }

        public DbSet<Quote> Quotes => Set<Quote>();

        public DbSet<Character> Characters => Set<Character>();

        public DbSet<TVShow> TVShows => Set<TVShow>();

    }
}
