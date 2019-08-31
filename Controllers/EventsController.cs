using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using campsApi;
using Microsoft.AspNetCore.Authorization;

namespace campsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly campsContext _context;

        public EventsController(campsContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Events>>> GetEvents()
        {
            var events= await _context.Events.ToListAsync();
            return Ok(events);
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Events>> GetEvents(long id)
        {
            var events = await _context.Events.FindAsync(id);

            if (events == null)
            {
                return NotFound();
            }

            return Ok(events);
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutEvents(long id, [FromBody]Events model)
        {
            var venue = model.Venue;
            var speakerId = model.Speaker;
            var name = model.NameOfEvent;
            var eventsCheck = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if(model!= null)
            {
                eventsCheck.Venue = venue;
                eventsCheck.Speaker = speakerId;
                eventsCheck.NameOfEvent = name;
                _context.Update(eventsCheck);
               await _context.SaveChangesAsync();
                return Ok(eventsCheck);
            }
            return BadRequest();
        }

        // POST: api/Events
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Events>> PostEvents(Events model)
        {
            var venue = model.Venue;
            var speakerId = model.Speaker;
            var name=model.NameOfEvent;
            try
            {
                if (model != null)
                {
                    var Event = new Events()
                    {
                        Venue = venue,
                        Speaker = speakerId,
                        NameOfEvent = name
                    };
                    _context.Events.Add(Event);
                    await _context.SaveChangesAsync();
                    return Ok(Event);
                }

            }
            catch (Exception e)
            {
                throw new Exception (e.Message);

            }
            return BadRequest();
           
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Events>> DeleteEvents(long id)
        {
            var events = await _context.Events.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }

            _context.Events.Remove(events);
            await _context.SaveChangesAsync();

            return events;
        }

        private bool EventsExists(long id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
