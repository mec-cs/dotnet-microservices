using System.ComponentModel.DataAnnotations;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Patients.APP.Features.Branches
{
    public class BranchCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(64)]
        public string Title { get; set; }
    
    }

    public class BranchCreateHandler: Service<Domain.Branch>, IRequestHandler<BranchCreateRequest, CommandResponse>
    {
        public BranchCreateHandler(DbContext db) : base(db) { }

        public async Task<CommandResponse> Handle(BranchCreateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(branch => branch.Title == request.Title.Trim(), cancellationToken))
                return Error("Branch with the same name exists!");

            var entity = new Domain.Branch
            {
                Title = request.Title.Trim(),
            };

            Create(entity);

            return Success("Branch created successfully.", entity.Id);
        }
    }
}