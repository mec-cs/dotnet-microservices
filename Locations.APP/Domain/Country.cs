using System.ComponentModel.DataAnnotations;
using Core.APP.Domain;

namespace Locations.APP.Domain
{
    public class Country : Entity
    {
        [Required, StringLength(64)]
        public string CountryName { get; set; }

        public HashSet<City> Cities { get; set; } = new HashSet<City>();
    }
}
