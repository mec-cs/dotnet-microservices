using System.ComponentModel.DataAnnotations;
using Core.APP.Domain;

namespace Locations.APP.Domain
{
    public class City : Entity
    {
        [Required, StringLength(64)]
        public string CityName { get; set; }
        
        [Required]
        public int CountryId { get; set; }
        
        public Country Country { get; set; }
    }
}
