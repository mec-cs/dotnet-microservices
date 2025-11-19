using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Patients
{
    public class PatientQueryResponse : Response
    {
        public int DoctorCount { get; set; }

        public string Doctors { get; set; }
    }
    
    public class PatientQueryRequest : Request, IRequest<IQueryable<PatientQueryResponse>> { }
    
    public class PatientQueryHandler : Service<Patient>, IRequestHandler<PatientQueryRequest, IQueryable<PatientQueryResponse>>
    {
        public PatientQueryHandler(DbContext db) : base(db) { }

        protected override IQueryable<Patient> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(p => p.PatientDoctors).ThenInclude(pd => pd.Doctor)
                .OrderBy(p => p.Id);
        }

        public Task<IQueryable<PatientQueryResponse>> Handle(PatientQueryRequest request, CancellationToken cancellationToken)
        {
            var query = Query().Select(p => new PatientQueryResponse()
            {
                Id = p.Id,
                Guid = p.Guid,
                
                DoctorCount = p.PatientDoctors.Count,
                Doctors = string.Join(", ", p.PatientDoctors.Select(pd => pd.Doctor.Id))
            });

            return Task.FromResult(query);
        }
    }    
}
