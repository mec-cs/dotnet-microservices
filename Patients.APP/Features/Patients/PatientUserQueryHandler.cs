using Core.APP.Models;
using Core.APP.Services;
using Core.APP.Services.HTTP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;
using Patients.APP.Features.Shared;
using System.Text.Json.Serialization;

namespace Patients.APP.Features.Patients
{
    public class PatientUserQueryResponse : Response
    {
        public int UserId { get; set; }
        public int? GroupId { get; set; }
        public string DoctorIds { get; set; }
        
        public List<int> DoctorIdsList { get; set; }
        
        public decimal? Height  { get; set; }
        
        public decimal? Weight  { get; set; }
        
        public UserBasicInfo User { get; set; }
    }

    public class PatientUserQueryRequest : Request, IRequest<List<PatientUserQueryResponse>>
    {
        [JsonIgnore]
        public string UsersApiUrl { get; set; }
    }

    public class PatientUserQueryHandler : Service<Patient>, IRequestHandler<PatientUserQueryRequest, List<PatientUserQueryResponse>>
    {
        private readonly HttpServiceBase _httpService;

        public PatientUserQueryHandler(DbContext db, HttpServiceBase httpService) : base(db)
        {
            _httpService = httpService;
        }

        protected override IQueryable<Patient> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(p => p.PatientDoctors).ThenInclude(pd => pd.Doctor)
                .OrderBy(p => p.Id);
        }

        public async Task<List<PatientUserQueryResponse>> Handle(PatientUserQueryRequest request, CancellationToken cancellationToken)
        {
            var patients = await Query().Select(p => new PatientUserQueryResponse
            {
                Id = p.Id,
                GroupId = p.GroupId,
                Height = p.Height,
                Weight = p.Weight,
                Guid = p.Guid,
                UserId = p.UserId,
                DoctorIds = string.Join(", ", p.PatientDoctors.Select(pd => pd.Doctor.Id)),
                DoctorIdsList = p.DoctorIds
            }).ToListAsync(cancellationToken);

            var uniqueUserIds = patients.Select(p => p.UserId).Distinct().ToList();
            var userDetails = new Dictionary<int, UserBasicInfo>();

            foreach (var userId in uniqueUserIds)
            {
                var userResponse = await _httpService.GetFromJson<UserApiResponse>(request.UsersApiUrl, userId, cancellationToken);
                if (userResponse != null)
                {
                    userDetails[userId] = UserBasicInfo.FromUserApiResponse(userResponse);
                }
            }

            foreach (var patient in patients)
            {
                if (userDetails.TryGetValue(patient.UserId, out var user))
                {
                    patient.User = user;
                }
            }

            return patients;
        }
    }
}

