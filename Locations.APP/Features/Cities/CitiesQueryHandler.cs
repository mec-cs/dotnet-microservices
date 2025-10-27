using Core.APP.Models;
using Core.APP.Services;
using Locations.APP.Features.Country;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Locations.APP.Features.City
{
    public class CityQueryRequest : Request, IRequest<IQueryable<CityQueryResponse>>
    {
        public int? CountryId { get; set; }
    }

    public class CityQueryResponse : Response
    {
        public string CityName { get; set; }
    }

    public class CitiesQueryHandler : Service<Domain.City>, IRequestHandler<CityQueryRequest, IQueryable<CityQueryResponse>>
    {
        protected override IQueryable<Domain.City> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(city => city.Country).OrderBy(city => city.CityName);
        }

        public CitiesQueryHandler(DbContext db) : base(db) {}

        public Task<IQueryable<CityQueryResponse>> Handle(CityQueryRequest request, CancellationToken cancellationToken)
        {
            var entityQuery = Query();

            if (request.CountryId.HasValue)
                entityQuery = entityQuery.Where(city => city.CountryId == request.CountryId.Value);

            var query = entityQuery.Select(city => new CityQueryResponse
            {
                Id = city.Id,
                Guid = city.Guid,
                CityName = city.CityName
            });

            return Task.FromResult(query);
        }
    }
}
