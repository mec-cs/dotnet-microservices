using Core.APP.Domain;

namespace Patients.APP.Domain;

public class DoctorPatient: Entity
{
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    
    public Patient Patient { get; set; }
    public Doctor Doctor { get; set; }
}