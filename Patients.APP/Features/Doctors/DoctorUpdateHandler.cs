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
    public class DoctorUpdateRequest : Request, IRequest<CommandResponse>
    {
        public int UserId { get; set; }

        public List<int> PatientIds { get; set; } = new List<int>();

        [JsonIgnore]
        public string UsersApiUrl { get; set; }
    }
    
    public class DoctorUpdateHandler : Service<Doctor>, IRequestHandler<DoctorUpdateRequest, CommandResponse>
    {
        private readonly HttpServiceBase _httpService;

        public DoctorUpdateHandler(DbContext db, HttpServiceBase httpService) : base(db) 
        {
            _httpService = httpService;
        }

        protected override IQueryable<Doctor> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(d => d.DoctorPatients);
        }

        public async Task<CommandResponse> Handle(DoctorUpdateRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false).SingleOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Doctor not found!");
            
            if (await Query().AnyAsync(r => r.Id != request.Id && r.UserId == request.UserId, cancellationToken))
                return Error("Doctor with the same User ID exists!");

            var user = await _httpService.GetFromJson<UserApiResponse>(request.UsersApiUrl, request.UserId, cancellationToken);
            if (user == null)
                return Error("User not found!");
            
            Delete(entity.DoctorPatients);
            
            entity.UserId = request.UserId;
            entity.PatientIds = request.PatientIds;
            
            Update(entity);

            return Success("Doctor updated successfully.", entity.Id, entity.Guid);
        }
    }  
}
