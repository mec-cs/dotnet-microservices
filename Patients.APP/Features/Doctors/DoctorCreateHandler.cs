using System.ComponentModel.DataAnnotations;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

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
    }
    
    public class DoctorCreateHandler : Service<Doctor>, IRequestHandler<DoctorCreateRequest, CommandResponse>
    {
        public DoctorCreateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(DoctorCreateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(doctor => doctor.UserId == request.UserId, cancellationToken))
                return Error("Doctor with the same User ID exists!");

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
