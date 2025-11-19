using Core.APP.Models;
using Core.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Locations.APP.Domain;

namespace Locations.APP.Features.City
{
    public class CityCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(64)]
        public string CityName { get; set; }
        
        [Required]
        public int CountryId { get; set; }
    }

    public class CitiesCreateHandler : Service<Domain.City>, IRequestHandler<CityCreateRequest, CommandResponse>
    {
        public CitiesCreateHandler(DbContext db) : base(db) {}
        
        public async Task<CommandResponse> Handle(CityCreateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(city => city.CityName == request.CityName.Trim(), cancellationToken))
                return Error("City with the same name exists!");

            var entity = new Domain.City
            {
                CityName = request.CityName.Trim(),
                CountryId = request.CountryId
            };

            Create(entity);

            return Success("City created successfully.", entity.Id, entity.Guid);
        }
    }
}
