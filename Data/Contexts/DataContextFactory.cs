

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;



    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>, IDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Project\\DataStorage_Assignment\\Data\\Database\\local_database.mdf;Integrated Security=True;Connect Timeout=30");
            return new DataContext(optionsBuilder.Options);
        }

        public DataContext CreateDbContext(string[] args)
        {
            return CreateDbContext(); //  Now supports both design-time and runtime usage
        }
    }
