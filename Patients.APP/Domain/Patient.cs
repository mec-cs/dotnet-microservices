using System.ComponentModel.DataAnnotations;
using Core.APP.Domain;

namespace Patients.APP.Domain;

public class Patient: Entity
{
    [Required, StringLength(25)]
    public string Name { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public int UserId { get; set; }
    public int? GroupId { get; set; }
    
    public List<DoctorPatient> PatientDoctors { get; set; } = new List<DoctorPatient>();

}