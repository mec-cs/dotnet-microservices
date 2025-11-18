using System.ComponentModel.DataAnnotations;
using Core.APP.Domain;

namespace Patients.APP.Domain;

public class Patient: Entity
{
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public int UserId { get; set; }
    public int? GroupId { get; set; }
    
    public List<DoctorPatient> PatientDoctors { get; set; } = new List<DoctorPatient>();

}