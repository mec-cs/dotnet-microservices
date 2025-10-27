using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Locations.APP.Domain
{

    public class LocationsDbFactory : IDesignTimeDbContextFactory<LocationsDb>
    {
        const string CONNECTIONSTRING = "data source=LocationsDB";
        
        public LocationsDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocationsDb>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new LocationsDb(optionsBuilder.Options);
        }
    }
}
