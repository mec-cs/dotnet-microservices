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
        [Required, StringLength(25)]
        public string Name { get; set; }
        
        public decimal? Weight { get; set; }
        
        public decimal? Height { get; set; }
    }
    
    public class PatientUpdateHandler : Service<Patient>, IRequestHandler<PatientUpdateRequest, CommandResponse>
    {
        public PatientUpdateHandler(DbContext db) : base(db) { }

        public async Task<CommandResponse> Handle(PatientUpdateRequest request, CancellationToken cancellationToken)
        {
            
            if (await Query().AnyAsync(r => r.Id != request.Id, cancellationToken))
                return Error("Patient with the same name exists!");
            
            var entity = await Query(false).SingleOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Patient not found!");
            
            entity.Weight = request.Weight;
            entity.Height = request.Height;
            
            Update(entity);

            return Success("Patient updated successfully.", entity.Id, entity.Guid);
        }
    }    
}
