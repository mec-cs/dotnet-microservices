using System.ComponentModel.DataAnnotations;
using Core.APP.Domain;

namespace Patients.APP.Domain;

public class Branch : Entity
{
    [Required, StringLength(25)] 
    public string Title { get; set; }
    
    public List<Doctor> Doctors { get; set; } = new List<Doctor>();
}