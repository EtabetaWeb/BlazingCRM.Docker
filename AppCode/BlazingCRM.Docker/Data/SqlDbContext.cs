using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlazingCRM.Docker.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext()
        {
            if (!Database.GetService<IRelationalDatabaseCreator>().Exists())
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(@"Data Source=Database\BlazingCRM.db");
        }
        public DbSet<Cliente> Clienti { get; set; }
    }

}
