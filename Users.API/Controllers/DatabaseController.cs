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
            
            var adminRoleId = _db.Roles.SingleOrDefault(r => r.Name == "Admin").Id;
            var userRoleId = _db.Roles.SingleOrDefault(r => r.Name == "User").Id;

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
                            new UserRole() { RoleId = adminRoleId, Guid = Guid.NewGuid().ToString() }
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
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    // Doctors (UserIds 3-8)
                    new User()
                    {
                        FirstName = "Dr. Michael",
                        LastName = "Smith",
                        UserName = "doctor1",
                        Password = "doctor1",
                        Gender = Genders.Man,
                        BirthDate = new DateTime(1975, 3, 15),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.5m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Dr. Sarah",
                        LastName = "Johnson",
                        UserName = "doctor2",
                        Password = "doctor2",
                        Gender = Genders.Woman,
                        BirthDate = new DateTime(1982, 7, 22),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.8m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Dr. David",
                        LastName = "Williams",
                        UserName = "doctor3",
                        Password = "doctor3",
                        Gender = Genders.Man,
                        BirthDate = new DateTime(1978, 11, 8),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.6m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Dr. Emily",
                        LastName = "Brown",
                        UserName = "doctor4",
                        Password = "doctor4",
                        Gender = Genders.Woman,
                        BirthDate = new DateTime(1985, 5, 12),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.7m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Dr. James",
                        LastName = "Davis",
                        UserName = "doctor5",
                        Password = "doctor5",
                        Gender = Genders.Man,
                        BirthDate = new DateTime(1979, 9, 30),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.4m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Dr. Lisa",
                        LastName = "Miller",
                        UserName = "doctor6",
                        Password = "doctor6",
                        Gender = Genders.Woman,
                        BirthDate = new DateTime(1983, 2, 18),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.9m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    // Patients (UserIds 9-16)
                    new User()
                    {
                        FirstName = "Alice",
                        LastName = "Anderson",
                        UserName = "patient1",
                        Password = "patient1",
                        Gender = Genders.Woman,
                        BirthDate = new DateTime(1990, 4, 5),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.2m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Bob",
                        LastName = "Wilson",
                        UserName = "patient2",
                        Password = "patient2",
                        Gender = Genders.Man,
                        BirthDate = new DateTime(1988, 6, 20),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.3m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Carol",
                        LastName = "Martinez",
                        UserName = "patient3",
                        Password = "patient3",
                        Gender = Genders.Woman,
                        BirthDate = new DateTime(1992, 8, 14),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.1m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Daniel",
                        LastName = "Taylor",
                        UserName = "patient4",
                        Password = "patient4",
                        Gender = Genders.Man,
                        BirthDate = new DateTime(1987, 12, 3),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.5m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Emma",
                        LastName = "Thomas",
                        UserName = "patient5",
                        Password = "patient5",
                        Gender = Genders.Woman,
                        BirthDate = new DateTime(1991, 1, 25),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.0m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Frank",
                        LastName = "Jackson",
                        UserName = "patient6",
                        Password = "patient6",
                        Gender = Genders.Man,
                        BirthDate = new DateTime(1989, 10, 7),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.6m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Grace",
                        LastName = "White",
                        UserName = "patient7",
                        Password = "patient7",
                        Gender = Genders.Woman,
                        BirthDate = new DateTime(1993, 3, 19),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.4m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    },
                    new User()
                    {
                        FirstName = "Henry",
                        LastName = "Harris",
                        UserName = "patient8",
                        Password = "patient8",
                        Gender = Genders.Man,
                        BirthDate = new DateTime(1986, 7, 11),
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.2m,
                        IsActive = true,
                        Guid = Guid.NewGuid().ToString(),
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole() { RoleId = userRoleId, Guid = Guid.NewGuid().ToString() }
                        }
                    }
                }
            });

            _db.SaveChanges();

            return Ok("Database seed successful.");
        }
    }
}