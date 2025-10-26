﻿using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Roles
{
    public class RoleUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }
    }

    public class RoleUpdateHandler : Service<Role>, IRequestHandler<RoleUpdateRequest, CommandResponse>
    {
        public RoleUpdateHandler(DbContext db) : base(db)
        {
        }
        
        public async Task<CommandResponse> Handle(RoleUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(r => r.Id != request.Id && r.Name == request.Name.Trim(), cancellationToken))
                return Error("Role with the same name exists!");

            var entity = await Query(false).SingleOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Role not found!");

            entity.Name = request.Name.Trim();
            
            Update(entity);

            return Success("Role updated successfully.", entity.Id);
        }
    }
}
