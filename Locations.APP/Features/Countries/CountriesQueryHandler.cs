using Core.APP.Models;
using Core.APP.Services;
using Locations.APP.Features.City;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Locations.APP.Features.Country
{
    public class CountryQueryRequest : Request, IRequest<IQueryable<CountryQueryResponse>> {}

    public class CountryQueryResponse : Response
    {
        public string CountryName { get; set; }
        public List<CityQueryResponse> Cities { get; set; }
    }

    public class CountriesQueryHandler : Service<Domain.Country>, IRequestHandler<CountryQueryRequest, IQueryable<CountryQueryResponse>>
    {
        protected override IQueryable<Domain.Country> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(country => country.Cities).OrderBy(country => country.CountryName);
        }

        public CountriesQueryHandler(DbContext db) : base(db) {}

        public Task<IQueryable<CountryQueryResponse>> Handle(CountryQueryRequest request, CancellationToken cancellationToken)
        {
            var query = Query().Select(country => new CountryQueryResponse
            {
                Id = country.Id,
                Guid = country.Guid,
                CountryName = country.CountryName,
                Cities = country.Cities.OrderBy(city => city.CityName).Select(city => new CityQueryResponse
                {
                    Id = city.Id,
                    Guid = city.Guid,
                    CityName = city.CityName
                }).ToList()
            });

            return Task.FromResult(query);
        }
    }
}
