using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.APP.Domain;

namespace Patients.APP.Domain;

public class Doctor: Entity
{
    [Required]
    public int BranchId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    public int GroupId { get; set; }
    
    [Required] 
    public Branch Branch { get; set; }
    
    public List<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();

    [NotMapped]
    public List<int> PatientIds
    {
        get => DoctorPatients.Select(dp => dp.PatientId).ToList();
        set => DoctorPatients = value.Select(patientId => new DoctorPatient() { PatientId = patientId, Guid =  System.Guid.NewGuid().ToString() }).ToList();
    }
}