using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Patients.APP.Domain;

public class PatientsDbFactory : IDesignTimeDbContextFactory<PatientsDb>
{
    const string CONNECTIONSTRING = "data source=LocationsDB";
        
    public PatientsDb CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PatientsDb>();
        optionsBuilder.UseSqlite(CONNECTIONSTRING);
        return new PatientsDb(optionsBuilder.Options);
    }
}