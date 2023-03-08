using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infra.DBAccess
{
    public class DbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public DbContextFactory(Action<DbContextOptionsBuilder> configureDb)
        {
            _configureDbContext = configureDb;
        }

        public DatabaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new();
            _configureDbContext(optionsBuilder);
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}