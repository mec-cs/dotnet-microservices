using Core.APP.Models;
using Core.APP.Services;
using Locations.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Locations.APP.Features.City
{
    public class CityDeleteRequest : Request, IRequest<CommandResponse> {}

    public class CitiesDeleteHandler : Service<Domain.City>, IRequestHandler<CityDeleteRequest, CommandResponse>
    {
        public CitiesDeleteHandler(DbContext db) : base(db) {}

        public async Task<CommandResponse> Handle(CityDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false).SingleOrDefaultAsync(city => city.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("City not found!");

            Delete(entity);

            return Success("City deleted successfully", entity.Id, entity.Guid);
        }
    }
}
