using Core.APP.Domain;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Branches
{
    public class DoctorDto : Entity
    {
        public int PatientCount { get; set; }
    }
    
    public class BranchQueryResponse : Response
    {
        public string Title { get; set; }
        
        public int DoctorCount { get; set; }

        public List<DoctorDto> Doctors { get; set; }
    }
    
    public class BranchQueryRequest : Request, IRequest<IQueryable<BranchQueryResponse>> { }

    public class BranchQueryHandler : Service<Branch>, IRequestHandler<BranchQueryRequest, IQueryable<BranchQueryResponse>>
    {
        public BranchQueryHandler(DbContext db) : base(db) { }
        
        protected override IQueryable<Branch> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(b => b.Doctors)
                .OrderBy(r => r.Title);
        }
        
        public Task<IQueryable<BranchQueryResponse>> Handle(BranchQueryRequest request, CancellationToken cancellationToken)
        {
            var query = Query().Select(b => new BranchQueryResponse()
            {
                Id = b.Id,
                Title = b.Title,
                Guid = b.Guid,
                
                DoctorCount = b.Doctors.Count,
                Doctors = b.Doctors.Select(dr => new DoctorDto
                {
                    Id = dr.Id,
                    PatientCount = dr.DoctorPatients.Count,
                    Guid = dr.Guid
                }).ToList()
            });

            return Task.FromResult(query);
        }
    }    
}
