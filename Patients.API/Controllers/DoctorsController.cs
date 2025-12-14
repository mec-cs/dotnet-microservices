using Core.APP.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patients.APP.Features.Doctors;

namespace Patients.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMediator _mediator;
        
        public DoctorsController(ILogger<DoctorsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromServices] IConfiguration configuration)
        {
            try
            {
                var request = new DoctorUserQueryRequest { UsersApiUrl = configuration["UsersApiUrl"] };
                var list = await _mediator.Send(request);
                
                if (list.Any())
                    return Ok(list);
                
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("DoctorsGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during DoctorsGet.")); 
            }
        }
        
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id, [FromServices] IConfiguration configuration)
        {
            try
            {
                var request = new DoctorUserQueryRequest { UsersApiUrl = configuration["UsersApiUrl"] };
                var list = await _mediator.Send(request);
                
                var item = list.FirstOrDefault(r => r.Id == id);
                if (item is not null)
                    return Ok(item);
                
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("DoctorsGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during DoctorsGetById.")); 
            }
        }

		
        [HttpPost]
        public async Task<IActionResult> Post(DoctorCreateRequest request, [FromServices] IConfiguration configuration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.UsersApiUrl = configuration["UsersApiUrl"];
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        return Ok(response);
                    }
                    
                    ModelState.AddModelError("DoctorsPost", response.Message);
                }
                
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("DoctorsPost Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during DoctorsPost.")); 
            }
        }

        
        [HttpPut]
        public async Task<IActionResult> Put(DoctorUpdateRequest request, [FromServices] IConfiguration configuration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.UsersApiUrl = configuration["UsersApiUrl"];
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        return Ok(response);
                    }
                    
                    ModelState.AddModelError("DoctorsPut", response.Message);
                }
                
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("DoctorsPut Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during DoctorsPut.")); 
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new DoctorDeleteRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                
                ModelState.AddModelError("DoctorsDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("DoctorsDelete Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during DoctorsDelete.")); 
            }
        }
    }
}
