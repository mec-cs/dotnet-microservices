using Core.APP.Models;
using Users.APP.Domain;

namespace Patients.APP.Features.Shared
{
    // Full response from Users API
    public class UserApiResponse : Response
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal Score { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? GroupId { get; set; }
        public List<int> RoleIds { get; set; }
        public string FullName { get; set; }
        public string GenderF { get; set; }
        public string BirthDateF { get; set; }
        public string RegistrationDateF { get; set; }
        public string ScoreF { get; set; }
        public string IsActiveF { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Group { get; set; }
        public List<string> Roles { get; set; }
    }

    // Lightweight DTO with only the fields we need
    public class UserBasicInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public string Address { get; set; }
        public List<int> RoleIds { get; set; }
        public int? GroupId { get; set; }

        public static UserBasicInfo FromUserApiResponse(UserApiResponse user)
        {
            if (user == null) return null;

            return new UserBasicInfo
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Address = user.Address,
                RoleIds = user.RoleIds ?? new List<int>(),
                GroupId = user.GroupId
            };
        }
    }
}

