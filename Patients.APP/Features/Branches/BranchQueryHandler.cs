using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Branches
{
    public class BranchQueryResponse : Response
    {
        public string Title { get; set; }
        
        public int DoctorCount { get; set; }

        public string Doctors { get; set; }
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
                Guid = b.Guid,
                Title = b.Title,
                
                DoctorCount = b.Doctors.Count,
                Doctors = string.Join(", ", b.Doctors.Select(dr => dr.Name))
            });

            return Task.FromResult(query);
        }
    }    
}
