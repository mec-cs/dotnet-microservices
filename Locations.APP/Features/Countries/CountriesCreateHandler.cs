using System.ComponentModel.DataAnnotations;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Locations.APP.Features.Country
{
    public class CountryCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(64)]
        public string CountryName { get; set; }
    }

    public class CityCreateHandler : Service<Domain.Country>, IRequestHandler<CountryCreateRequest, CommandResponse>
    {
        public CityCreateHandler(DbContext db) : base(db) {}

        public async Task<CommandResponse> Handle(CountryCreateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(country => country.CountryName == request.CountryName.Trim(), cancellationToken))
                return Error("Country with the same name exists!");

            var entity = new Domain.Country
            {
                CountryName = request.CountryName.Trim()
            };

            Create(entity);

            return Success("Country created successfully", entity.Id);
        }
    }
}
