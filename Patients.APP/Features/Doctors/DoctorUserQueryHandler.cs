using Core.APP.Models;
using Core.APP.Services;
using Core.APP.Services.HTTP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;
using Patients.APP.Features.Shared;
using System.Text.Json.Serialization;
using Core.APP.Domain;

namespace Patients.APP.Features.Doctors
{
    public class PatientDto : Entity
    {
        public int DoctorCount { get; set; }
    }
    
    public class DoctorUserQueryResponse : Response
    {
        public int UserId { get; set; }
        
        public int GroupId { get; set; }
        
        public int BranchId { get; set; }
        
        public int PatientsCount { get; set; }

        public List<int> PatientIds { get; set; }

        public List<PatientDto> Patients { get; set; }
        
        public UserBasicInfo User { get; set; }
    }

    public class DoctorUserQueryRequest : Request, IRequest<List<DoctorUserQueryResponse>>
    {
        [JsonIgnore]
        public string UsersApiUrl { get; set; }
    }

    public class DoctorUserQueryHandler : Service<Doctor>, IRequestHandler<DoctorUserQueryRequest, List<DoctorUserQueryResponse>>
    {
        private readonly HttpServiceBase _httpService;

        public DoctorUserQueryHandler(DbContext db, HttpServiceBase httpService) : base(db)
        {
            _httpService = httpService;
        }

        protected override IQueryable<Doctor> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(dr => dr.DoctorPatients).ThenInclude(dp => dp.Patient)
                .OrderBy(dr => dr.UserId);
        }

        public async Task<List<DoctorUserQueryResponse>> Handle(DoctorUserQueryRequest request, CancellationToken cancellationToken)
        {
            var doctors = await Query().Select(dr => new DoctorUserQueryResponse
            {
                Id = dr.Id,
                Guid = dr.Guid,
                UserId = dr.UserId,
                GroupId = dr.GroupId,
                BranchId = dr.BranchId,
                PatientsCount = dr.DoctorPatients.Count,
                PatientIds = dr.PatientIds,
                Patients = dr.DoctorPatients.Select(dp => new PatientDto
                {
                    Id = dp.PatientId,
                    DoctorCount = dp.Patient.PatientDoctors.Count,
                    Guid = dp.Patient.Guid
                }).ToList()
            }).ToListAsync(cancellationToken);

            var uniqueUserIds = doctors.Select(d => d.UserId).Distinct().ToList();
            var userDetails = new Dictionary<int, UserBasicInfo>();

            foreach (var userId in uniqueUserIds)
            {
                var userResponse = await _httpService.GetFromJson<UserApiResponse>(request.UsersApiUrl, userId, cancellationToken);
                if (userResponse != null)
                {
                    userDetails[userId] = UserBasicInfo.FromUserApiResponse(userResponse);
                }
            }

            foreach (var doctor in doctors)
            {
                if (userDetails.TryGetValue(doctor.UserId, out var user))
                {
                    doctor.User = user;
                }
            }

            return doctors;
        }
    }
}

