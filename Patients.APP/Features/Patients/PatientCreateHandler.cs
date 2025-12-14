using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Core.APP.Models;
using Core.APP.Services;
using Core.APP.Services.HTTP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;
using Patients.APP.Features.Shared;

namespace Patients.APP.Features.Patients
{
    public class PatientCreateRequest: Request, IRequest<CommandResponse>
    {
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public int? GroupId { get; set; }

        public List<int> DoctorIds { get; set; } = new List<int>();

        [JsonIgnore]
        public string UsersApiUrl { get; set; }
    }

    public class PatientCreateHandler : Service<Patient>, IRequestHandler<PatientCreateRequest, CommandResponse>
    {
        private readonly HttpServiceBase _httpService;

        public PatientCreateHandler(DbContext db, HttpServiceBase httpService) : base(db) 
        {
            _httpService = httpService;
        }
        
        public async Task<CommandResponse> Handle(PatientCreateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(patient => patient.UserId == request.UserId, cancellationToken))
                return Error("Patient with the same User ID exists!");

            var user = await _httpService.GetFromJson<UserApiResponse>(request.UsersApiUrl, request.UserId, cancellationToken);
            if (user == null)
                return Error("User not found!");
            
            var entity = new Patient()
            {
                Height = request.Height,
                Weight = request.Weight,
                UserId = request.UserId,
                GroupId = request.GroupId,
                DoctorIds = request.DoctorIds
            };

            Create(entity);

            return Success("Patient created successfully.", entity.Id, entity.Guid);
        }
    }
    
}
