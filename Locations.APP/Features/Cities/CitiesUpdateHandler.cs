using System.ComponentModel.DataAnnotations;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Locations.APP.Features.City
{
    public class CityUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(64)]
        public string CityName { get; set; }

        public int CountryId { get; set; }
    }

    public class CitiesUpdateHandler : Service<Domain.City>, IRequestHandler<CityUpdateRequest, CommandResponse>
    {
        public CitiesUpdateHandler(DbContext db) : base(db) {}

        public async Task<CommandResponse> Handle(CityUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(city => city.Id != request.Id && city.CityName == request.CityName.Trim(), cancellationToken))
                return Error("City with the same name exists!");
            
            var entity = await Query(false).SingleOrDefaultAsync(city => city.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("City not found!");

            entity.CityName = request.CityName.Trim();
            entity.CountryId = request.CountryId;

            Update(entity);

            return Success("City updated successfully.", entity.Id, entity.Guid);
        }
    }
}
