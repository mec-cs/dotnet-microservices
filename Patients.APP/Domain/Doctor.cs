using Core.APP.Domain;

namespace Patients.APP.Domain;

public class Doctor: Entity
{
    public int BranchId { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }
    
    public Branch Branch { get; set; }
    public HashSet<DoctorPatient> DoctorPatients { get; set; } = new HashSet<DoctorPatient>();
}