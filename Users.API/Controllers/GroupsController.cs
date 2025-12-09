using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.APP.Features.Groups;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public GroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = await _mediator.Send(new GroupQueryRequest());

            var list = await query.ToListAsync();

            return Ok(list);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = await _mediator.Send(new GroupQueryRequest());
            
            var item = await query.SingleOrDefaultAsync(groupResponse => groupResponse.Id == id);
            
            if (item is null)
                return NotFound();
            
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(GroupCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                
                if (response.IsSuccessful)
                {
                    return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
                }

                return BadRequest(response);
            }

            return BadRequest(ModelState);
        }
        
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(GroupUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);

                if (response.IsSuccessful)
                {
                    return NoContent();
                }

                return BadRequest(response);
            }

            return BadRequest(ModelState);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new GroupDeleteRequest() { Id = id });
            
            if (response.IsSuccessful)
            {
                return NoContent(); 
            }

            return BadRequest(response);
        }
    }
}
