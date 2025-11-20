using Core.APP.Domain;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Doctors
{
    public class PatientDto : Entity
    {
        public int DoctorCount { get; set; }
    }
    
    public class DoctorQueryResponse : Response
    {
        public int UserId { get; set; }
        
        public int GroupId { get; set; }
        
        public int BranchId { get; set; }
        
        public int PatientsCount { get; set; }

        public List<int> PatientIds { get; set; }

        public List<PatientDto> Patients { get; set; }
    }
    
    public class DoctorQueryRequest : Request, IRequest<IQueryable<DoctorQueryResponse>> { }
    
    public class DoctorQueryHandler : Service<Doctor>, IRequestHandler<DoctorQueryRequest, IQueryable<DoctorQueryResponse>>
    {
        public DoctorQueryHandler(DbContext db) : base(db) { }
        
        protected override IQueryable<Doctor> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(dr => dr.DoctorPatients).ThenInclude(dp => dp.Patient)
                .OrderBy(dr => dr.UserId);
        }
        
        public Task<IQueryable<DoctorQueryResponse>> Handle(DoctorQueryRequest request, CancellationToken cancellationToken)
        {
            var query = Query().Select(dr => new DoctorQueryResponse()
            {
                Id = dr.Id,
                Guid = dr.Guid,
                UserId = dr.UserId,
                GroupId = dr.GroupId,
                BranchId = dr.BranchId,
                
                PatientsCount = dr.DoctorPatients.Count,
                PatientIds = dr.PatientIds,
                Patients = dr.DoctorPatients.Select(dp => new PatientDto
                {
                    Id = dp.PatientId,
                    DoctorCount = dp.Patient.PatientDoctors.Count,
                    Guid = dp.Patient.Guid
                }).ToList()
            });

            return Task.FromResult(query);
        }
    }    
}
