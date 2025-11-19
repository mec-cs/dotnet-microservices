using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Doctors
{
    public class DoctorDeleteRequest : Request, IRequest<CommandResponse>
    {
    }
    
    public class DoctorDeleteHandler : Service<Doctor>, IRequestHandler<DoctorDeleteRequest, CommandResponse>
    {
        public DoctorDeleteHandler(DbContext db) : base(db) { }
        
        protected override IQueryable<Doctor> Query(bool isNoTracking = true)
        {
            return base.Query().Include(dr => dr.DoctorPatients);
        }

        public async Task<CommandResponse> Handle(DoctorDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false).SingleOrDefaultAsync(dr => dr.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Doctor not found!");
            
            Delete(entity.DoctorPatients);
            
            Delete(entity);

            return Success("Doctor deleted successfully.", entity.Id, entity.Guid);
        }
    }    
}
