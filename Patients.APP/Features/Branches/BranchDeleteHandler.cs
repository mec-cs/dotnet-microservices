using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Branches
{
    public class BranchDeleteRequest : Request, IRequest<CommandResponse> {}


    public class BranchDeleteHandler : Service<Branch>, IRequestHandler<BranchDeleteRequest, CommandResponse>
    {
        public BranchDeleteHandler(DbContext db) : base(db) { }
    
        protected override IQueryable<Branch> Query(bool isNoTracking = true)
        {
            return base.Query().Include(r => r.Doctors);
        }

        public async Task<CommandResponse> Handle(BranchDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false).SingleOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Branch not found!");

            Delete(entity.Doctors);

            Delete(entity);

            return Success("Role deleted successfully.", entity.Id, entity.Guid);
        }
    
    
    }
}