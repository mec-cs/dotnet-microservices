using System.ComponentModel.DataAnnotations;
using Core.APP.Domain;

namespace Patients.APP.Domain;

public class Doctor: Entity
{
    [Required]
    public int BranchId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    public int GroupId { get; set; }
    
    [Required, StringLength(25)]
    public string Name { get; set; }
    
    [Required] 
    public Branch Branch { get; set; }
    
    public List<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();
}