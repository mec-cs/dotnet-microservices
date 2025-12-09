using Core.APP.Models;
using Locations.APP.Features.City;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Locations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly IMediator _mediator;

        public CitiesController(ILogger<CitiesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        
        // GET: api/Cities
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new CityQueryRequest());
                var list = await response.ToListAsync();

                if (list.Any())
                    return Ok(list);
                
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("CitiesGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during CitiesGet.")); 
            }
        }

        
        // GET: api/Cities/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new CityQueryRequest());
                var item = await response.SingleOrDefaultAsync(r => r.Id == id);

                if (item is not null)
                    return Ok(item);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("CitiesGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during CitiesGetById.")); 
            }
        }

        
		// POST: api/Cities
        [HttpPost]
        public async Task<IActionResult> Post(CityCreateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        return Ok(response);
                    }
                    ModelState.AddModelError("CitiesPost", response.Message);
                }
                
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("CitiesPost Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during CitiesPost.")); 
            }
        }

        
        // PUT: api/Cities
        [HttpPut]
        public async Task<IActionResult> Put(CityUpdateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        return Ok(response);
                    }
                    ModelState.AddModelError("CitiesPut", response.Message);
                }
                
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("CitiesPut Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during CitiesPut.")); 
            }
        }

        
        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new CityDeleteRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("CitiesDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("CitiesDelete Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during CitiesDelete.")); 
            }
        }

        
        [HttpGet("[action]/{countryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCountryId(int countryId)
        {
            var response = await _mediator.Send(new CityQueryRequest() { CountryId = countryId });
            var list = await response.ToListAsync();
            
            if (list.Any())
                return Ok(list);
            
            return NoContent();
        }
    }
}