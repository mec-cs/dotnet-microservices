using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Patients
{
    public class PatientDeleteRequest : Request, IRequest<CommandResponse> { }
    
    public class PatientDeleteHandler : Service<Patient>, IRequestHandler<PatientDeleteRequest, CommandResponse>
    {
        public PatientDeleteHandler(DbContext db) : base(db) { }
        
        protected override IQueryable<Patient> Query(bool isNoTracking = true)
        {
            return base.Query().Include(p => p.PatientDoctors).ThenInclude(dp => dp.Doctor).OrderBy(p => p.Name);
        }

        public async Task<CommandResponse> Handle(PatientDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false).SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Patient not found!");

            Delete(entity.PatientDoctors);
            
            Delete(entity);

            return Success("Patient deleted successfully.", entity.Id);
        }
    }    
}
