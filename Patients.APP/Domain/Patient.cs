using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.APP.Domain;

namespace Patients.APP.Domain;

public class Patient: Entity
{
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public int UserId { get; set; }
    public int? GroupId { get; set; }
    
    public List<DoctorPatient> PatientDoctors { get; set; } = new List<DoctorPatient>();

    [NotMapped]
    public List<int> DoctorIds
    {
        get => PatientDoctors.Select(pd => pd.DoctorId).ToList();
        set => PatientDoctors = value.Select(doctorId => new DoctorPatient() { DoctorId = doctorId, Guid = System.Guid.NewGuid().ToString() }).ToList();
    }
}