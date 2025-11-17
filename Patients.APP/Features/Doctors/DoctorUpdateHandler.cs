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
        [Required, StringLength(25)]
        public string Name { get; set; }
    }
    
    public class DoctorUpdateHandler : Service<Doctor>, IRequestHandler<DoctorUpdateRequest, CommandResponse>
    {
        public DoctorUpdateHandler(DbContext db) : base(db) { }

        public async Task<CommandResponse> Handle(DoctorUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(r => r.Id != request.Id && r.Name == request.Name.Trim(), cancellationToken))
                return Error("Doctor with the same name exists!");

            var entity = await Query(false).SingleOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Doctor not found!");

            entity.Name = request.Name.Trim();

            Update(entity);

            return Success("Doctor updated successfully.", entity.Id);
        }
    }  
}
