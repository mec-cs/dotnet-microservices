using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Users.APP.Domain;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly UsersDb _db;
        private readonly IWebHostEnvironment _environment;

        public DatabaseController(UsersDb db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }
        
        [HttpGet, Route("~/api/SeedDb")]
        public IActionResult Seed()
        {
            var userRoles = _db.UserRoles.ToList();
            _db.UserRoles.RemoveRange(userRoles);
            
            var roles = _db.Roles.ToList();
            _db.Roles.RemoveRange(roles);
            
            var users = _db.Users.ToList();
            _db.Users.RemoveRange(users);
            
            var groups = _db.Groups.ToList();
            _db.Groups.RemoveRange(groups);
            
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='UserRoles';");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Roles';");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Users';");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Groups';");
            
            _db.Roles.Add(new Role()
            {
                Name = "Admin",
                Guid = Guid.NewGuid().ToString(),
            });
            _db.Roles.Add(new Role()
            {
                Name = "User",
                Guid = Guid.NewGuid().ToString(),
            });

            _db.SaveChanges();
            
            _db.Groups.Add(new Group()
            {
                Title = "General",
                Guid = Guid.NewGuid().ToString(),
                Users = new List<User>()
                {
                    new User()
                    {
                        Address = "Cankaya",
                        BirthDate = new DateTime(1980, 8, 21),
                        CityId = 6,
                        CountryId = 1,
                        FirstName = "John",
                        Gender = Genders.Man,
                        Guid = Guid.NewGuid().ToString(),
                        IsActive = true,
                        LastName = "Black",
                        Password = "admin",
                        RegistrationDate = DateTime.UtcNow,
                        Score = 3.8M,
                        UserName = "admin",
                        UserRoles = new List<UserRole>()
                        {
                            // Assign Admin role to this user
                            new UserRole() { RoleId = _db.Roles.SingleOrDefault(r => r.Name == "Admin").Id, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        BirthDate = DateTime.Parse("09/13/2004", new CultureInfo("en-US")),
                        CityId = 82,
                        CountryId = 2,
                        FirstName = "Luna",
                        Gender = Genders.Woman,
                        Guid = Guid.NewGuid().ToString(),
                        IsActive = true,
                        LastName = "Leo",
                        Password = "user",
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.7m,
                        UserName = "user",
                        UserRoles = new List<UserRole>()
                        {
                            // Assign User role to this user
                            new UserRole() { RoleId = _db.Roles.SingleOrDefault(r => r.Name == "User").Id, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                }
            });

            _db.SaveChanges();

            return Ok("Database seed successful.");
        }
    }
}