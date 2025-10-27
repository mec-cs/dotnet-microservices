using System.ComponentModel.DataAnnotations;
using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Locations.APP.Features.Country
{
    public class CountryUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(125)]
        public string CountryName { get; set; }
    }

    public class CountriesUpdateHandler : Service<Domain.Country>, IRequestHandler<CountryUpdateRequest, CommandResponse>
    {
        public CountriesUpdateHandler(DbContext db) : base(db) {}

        public async Task<CommandResponse> Handle(CountryUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(country => country.Id != request.Id && country.CountryName == request.CountryName.Trim(), cancellationToken))
                return Error("Country with the same name exists!");
            
            var entity = await Query(false).SingleOrDefaultAsync(country => country.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Country not found!");

            entity.CountryName = request.CountryName.Trim();

            Update(entity);

            return Success("Country updated successfully", entity.Id);
        }
    }
}
