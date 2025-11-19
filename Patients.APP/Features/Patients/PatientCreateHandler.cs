using System.ComponentModel.DataAnnotations;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Patients
{
    public class PatientCreateRequest: Request, IRequest<CommandResponse>
    {
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public int? GroupId { get; set; }
    }

    public class PatientCreateHandler : Service<Patient>, IRequestHandler<PatientCreateRequest, CommandResponse>
    {
        public PatientCreateHandler(DbContext db) : base(db) { }
        
        public async Task<CommandResponse> Handle(PatientCreateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(patient => patient.UserId == request.UserId, cancellationToken))
                return Error("Patient with the same User ID exists!");
            
            var entity = new Patient()
            {
                Height = request.Height,
                Weight = request.Weight,
                UserId = request.UserId,
                GroupId = request.GroupId,
            };

            Create(entity);

            return Success("Patient created successfully.", entity.Id, entity.Guid);
        }
    }
    
}
