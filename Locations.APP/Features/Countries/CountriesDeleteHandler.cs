using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Locations.APP.Features.Country
{

    public class CountryDeleteRequest : Request, IRequest<CommandResponse> {}

    public class CityDeleteHandler : Service<Domain.Country>, IRequestHandler<CountryDeleteRequest, CommandResponse>
    {
        protected override IQueryable<Domain.Country> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(country => country.Cities);
        }

        public CityDeleteHandler(DbContext db) : base(db) {}

        public async Task<CommandResponse> Handle(CountryDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false).SingleOrDefaultAsync(country => country.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Country not found!");
            
            Delete(entity); // it will throw db level exception if any

            return Success("Country deleted successfully.", entity.Id);
        }
    }
}
