using System.ComponentModel.DataAnnotations;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Patients
{
    public class PatientUpdateRequest : Request, IRequest<CommandResponse>
    {
        public int UserId { get; set; }
        
        public decimal? Weight { get; set; }
        
        public decimal? Height { get; set; }

        public List<int> DoctorIds { get; set; } = new List<int>();
    }
    
    public class PatientUpdateHandler : Service<Patient>, IRequestHandler<PatientUpdateRequest, CommandResponse>
    {
        public PatientUpdateHandler(DbContext db) : base(db) { }

        protected override IQueryable<Patient> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(p => p.PatientDoctors);
        }

        public async Task<CommandResponse> Handle(PatientUpdateRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false).SingleOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Patient not found!");
            
            if (await Query().AnyAsync(r => r.Id != request.Id && r.UserId == request.UserId, cancellationToken))
                return Error("Patient with the same User ID exists!");
            
            Delete(entity.PatientDoctors);
            
            entity.UserId = request.UserId;
            entity.Weight = request.Weight;
            entity.Height = request.Height;
            entity.DoctorIds = request.DoctorIds;
            
            Update(entity);

            return Success("Patient updated successfully.", entity.Id, entity.Guid);
        }
    }    
}
