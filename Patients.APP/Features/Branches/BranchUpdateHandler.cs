using System.ComponentModel.DataAnnotations;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Branches
{
    public class BranchUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(25)]
        public string Title { get; set; }
    }
    
    public class BranchUpdateHandler : Service<Branch>, IRequestHandler<BranchUpdateRequest, CommandResponse>
    {
        public BranchUpdateHandler(DbContext db) : base(db) {}

        public async Task<CommandResponse> Handle(BranchUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(branch => branch.Id != request.Id && branch.Title == request.Title.Trim(), cancellationToken))
                return Error("Branch with the same name exists!");
            
            var entity = await Query(false).SingleOrDefaultAsync(branch => branch.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Branch not found!");

            entity.Title = request.Title.Trim();

            Update(entity);

            return Success("Branch updated successfully", entity.Id, entity.Guid);
        }
    }
}
