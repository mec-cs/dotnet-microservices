using Patients.APP.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Patients.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly PatientsDb _db;

        private readonly IWebHostEnvironment _environment;

        public DatabaseController(PatientsDb db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [HttpGet, Route("~/api/SeedDb")]
        public IActionResult Seed()
        {
            var doctorPatients = _db.DoctorPatients.ToList();
            _db.DoctorPatients.RemoveRange(doctorPatients);
            
            var doctors = _db.Doctors.ToList();
            _db.Doctors.RemoveRange(doctors);
            
            var patients = _db.Patients.ToList();
            _db.Patients.RemoveRange(patients);
            
            var branches = _db.Branches.ToList();
            _db.Branches.RemoveRange(branches);

            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='DoctorPatients';");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Doctors';");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Patients';");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Branches';");

            // Create Branches
            var cardiologyBranch = new Branch
            {
                Guid = Guid.NewGuid().ToString(),
                Title = "Cardiology"
            };
            
            var neurologyBranch = new Branch
            {
                Guid = Guid.NewGuid().ToString(),
                Title = "Neurology"
            };
            
            var pediatricsBranch = new Branch
            {
                Guid = Guid.NewGuid().ToString(),
                Title = "Pediatrics"
            };
            
            var orthopedicsBranch = new Branch
            {
                Guid = Guid.NewGuid().ToString(),
                Title = "Orthopedics"
            };
            
            var dermatologyBranch = new Branch
            {
                Guid = Guid.NewGuid().ToString(),
                Title = "Dermatology"
            };

            _db.Branches.Add(cardiologyBranch);
            _db.Branches.Add(neurologyBranch);
            _db.Branches.Add(pediatricsBranch);
            _db.Branches.Add(orthopedicsBranch);
            _db.Branches.Add(dermatologyBranch);
            
            _db.SaveChanges();

            // Create Doctors
            var doctor1 = new Doctor
            {
                Guid = Guid.NewGuid().ToString(),
                UserId = 1,
                GroupId = 1,
                BranchId = cardiologyBranch.Id
            };
            
            var doctor2 = new Doctor
            {
                Guid = Guid.NewGuid().ToString(),
                UserId = 2,
                GroupId = 1,
                BranchId = cardiologyBranch.Id
            };
            
            var doctor3 = new Doctor
            {
                Guid = Guid.NewGuid().ToString(),
                UserId = 3,
                GroupId = 1,
                BranchId = neurologyBranch.Id
            };
            
            var doctor4 = new Doctor
            {
                Guid = Guid.NewGuid().ToString(),
                UserId = 4,
                GroupId = 1,
                BranchId = pediatricsBranch.Id
            };
            
            var doctor5 = new Doctor
            {
                Guid = Guid.NewGuid().ToString(),
                UserId = 5,
                GroupId = 1,
                BranchId = orthopedicsBranch.Id
            };
            
            var doctor6 = new Doctor
            {
                Guid = Guid.NewGuid().ToString(),
                UserId = 6,
                GroupId = 1,
                BranchId = dermatologyBranch.Id
            };

            _db.Doctors.Add(doctor1);
            _db.Doctors.Add(doctor2);
            _db.Doctors.Add(doctor3);
            _db.Doctors.Add(doctor4);
            _db.Doctors.Add(doctor5);
            _db.Doctors.Add(doctor6);
            
            _db.SaveChanges();

            // Create Patients
            var patient1 = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = 165.5m,
                Weight = 68.2m,
                UserId = 10,
                GroupId = 1
            };
            
            var patient2 = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = 178.0m,
                Weight = 82.5m,
                UserId = 11,
                GroupId = 1
            };
            
            var patient3 = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = 160.0m,
                Weight = 55.8m,
                UserId = 12,
                GroupId = 1
            };
            
            var patient4 = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = 175.3m,
                Weight = 75.0m,
                UserId = 13,
                GroupId = 1
            };
            
            var patient5 = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = 162.7m,
                Weight = 60.3m,
                UserId = 14,
                GroupId = 1
            };
            
            var patient6 = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = 180.5m,
                Weight = 88.9m,
                UserId = 15,
                GroupId = 1
            };
            
            var patient7 = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = 158.2m,
                Weight = 52.4m,
                UserId = 16,
                GroupId = 1
            };
            
            var patient8 = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = 172.8m,
                Weight = 79.6m,
                UserId = 17,
                GroupId = 1
            };

            _db.Patients.Add(patient1);
            _db.Patients.Add(patient2);
            _db.Patients.Add(patient3);
            _db.Patients.Add(patient4);
            _db.Patients.Add(patient5);
            _db.Patients.Add(patient6);
            _db.Patients.Add(patient7);
            _db.Patients.Add(patient8);
            
            _db.SaveChanges();

            // Create Doctor-Patient relationships
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor1.Id,
                PatientId = patient1.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor1.Id,
                PatientId = patient2.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor2.Id,
                PatientId = patient2.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor2.Id,
                PatientId = patient3.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor3.Id,
                PatientId = patient3.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor3.Id,
                PatientId = patient4.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor4.Id,
                PatientId = patient4.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor4.Id,
                PatientId = patient5.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor5.Id,
                PatientId = patient5.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor5.Id,
                PatientId = patient6.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor6.Id,
                PatientId = patient6.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor6.Id,
                PatientId = patient7.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor1.Id,
                PatientId = patient8.Id
            });
            
            _db.DoctorPatients.Add(new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctor3.Id,
                PatientId = patient8.Id
            });

            _db.SaveChanges();

            return Ok("Database seed successful.");
        }
    }
}