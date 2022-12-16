using Microsoft.EntityFrameworkCore;
using PhoneCentre.Models;

namespace CallCentreTask.Data
{
    public class CallerDb : DbContext
    {
        public DbSet<Call> Calls { get; set; }
        public DbSet<T_Event> Events { get; set; }
        public DbSet<T_Event_Type> Events_Type { get; set; }

        //Creates a connection to the database using the connection string in Resources.resx
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder) =>
            dbContextOptionsBuilder.UseSqlServer(PhoneCentre.Properties.Resources.connectionString, options => options.EnableRetryOnFailure(3));
    }
}
