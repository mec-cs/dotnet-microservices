using System.ComponentModel.DataAnnotations;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Doctors
{
    public class DoctorUpdateRequest : Request, IRequest<CommandResponse>
    {
        public int UserId { get; set; }

        public List<int> PatientIds { get; set; } = new List<int>();
    }
    
    public class DoctorUpdateHandler : Service<Doctor>, IRequestHandler<DoctorUpdateRequest, CommandResponse>
    {
        public DoctorUpdateHandler(DbContext db) : base(db) { }

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
            
            Delete(entity.DoctorPatients);
            
            entity.UserId = request.UserId;
            entity.PatientIds = request.PatientIds;
            
            Update(entity);

            return Success("Doctor updated successfully.", entity.Id, entity.Guid);
        }
    }  
}
