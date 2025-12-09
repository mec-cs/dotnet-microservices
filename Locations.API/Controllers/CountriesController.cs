using Core.APP.Models;
using Locations.APP.Features.Country;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Locations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> _logger;
        private readonly IMediator _mediator;

        public CountriesController(ILogger<CountriesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        

        // GET: api/Countries
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new CountryQueryRequest());
                var list = await response.ToListAsync();

                if (list.Any())
                    return Ok(list);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("CountriesGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during CountriesGet.")); 
            }
        }

        
        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new CountryQueryRequest());
                var item = await response.SingleOrDefaultAsync(r => r.Id == id);

                if (item is not null)
                    return Ok(item);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("CountriesGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during CountriesGetById.")); 
            }
        }

        
		// POST: api/Countries
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(CountryCreateRequest request)
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
                    ModelState.AddModelError("CountriesPost", response.Message);
                }

                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("CountriesPost Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during CountriesPost.")); 
            }
        }

        
        // PUT: api/Countries
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(CountryUpdateRequest request)
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
                    ModelState.AddModelError("CountriesPut", response.Message);
                }

                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("CountriesPut Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during CountriesPut.")); 
            }
        }

        
        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new CountryDeleteRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("CountriesDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("CountriesDelete Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during CountriesDelete.")); 
            }
        }
	}
}