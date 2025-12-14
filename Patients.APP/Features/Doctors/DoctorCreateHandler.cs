using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Core.APP.Models;
using Core.APP.Services;
using Core.APP.Services.HTTP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;
using Patients.APP.Features.Shared;

namespace Patients.APP.Features.Doctors
{
    public class DoctorCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        public int UserId { get; set; }

        public int GroupId { get; set; }
        
        [Required]
        public int BranchId { get; set; }

        public List<int> PatientIds { get; set; } = new List<int>();

        [JsonIgnore]
        public string UsersApiUrl { get; set; }
    }
    
    public class DoctorCreateHandler : Service<Doctor>, IRequestHandler<DoctorCreateRequest, CommandResponse>
    {
        private readonly HttpServiceBase _httpService;

        public DoctorCreateHandler(DbContext db, HttpServiceBase httpService) : base(db)
        {
            _httpService = httpService;
        }

        public async Task<CommandResponse> Handle(DoctorCreateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(doctor => doctor.UserId == request.UserId, cancellationToken))
                return Error("Doctor with the same User ID exists!");

            var user = await _httpService.GetFromJson<UserApiResponse>(request.UsersApiUrl, request.UserId, cancellationToken);
            if (user == null)
                return Error("User not found!");

            var entity = new Doctor()
            {
                UserId = request.UserId,
                GroupId = request.GroupId,
                BranchId = request.BranchId,
                PatientIds = request.PatientIds
            };

            Create(entity);

            return Success("Doctor created successfully.", entity.Id, entity.Guid);
        }
    }    
}
