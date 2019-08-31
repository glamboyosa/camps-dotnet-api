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
    public class SpeakersController : ControllerBase
    {
        private readonly campsContext _context;

        public SpeakersController(campsContext context)
        {
            _context = context;
        }

        // GET: api/Speakers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Speakers>>> GetSpeakers()
        {
           var speakers= await _context.Speakers.ToListAsync();
            return Ok(speakers);
        }

        // GET: api/Speakers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Speakers>> GetSpeakers(long id)
        {
            var speakers = await _context.Speakers.FindAsync(id);

            if (speakers == null)
            {
                return NotFound();
            }

            return speakers;
        }

        // PUT: api/Speakers/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutSpeakers(long id, Speakers model)
        {
            var fullname = model.Fullname;
            var speakersCheck = await _context.Speakers.FirstOrDefaultAsync(f => f.Id == id);
            if(model != null)
            {
                speakersCheck.Fullname = model.Fullname;
                _context.Speakers.Update(speakersCheck);
                await _context.SaveChangesAsync();
                return Ok(speakersCheck);
            }
            return BadRequest();
        }

        // POST: api/Speakers
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Speakers>> PostSpeakers(Speakers model)
        {
            var fullname = model.Fullname;
            if (model != null)
            {

                var Speakers = new Speakers()
                {
                    Fullname = fullname
                };
                _context.Speakers.Add(Speakers);
                await _context.SaveChangesAsync();
                return Ok(Speakers);
            }
            return BadRequest();
           
        }

        // DELETE: api/Speakers/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Speakers>> DeleteSpeakers(long id)
        {
            var speakers = await _context.Speakers.FindAsync(id);
            if (speakers == null)
            {
                return NotFound();
            }

            _context.Speakers.Remove(speakers);
            await _context.SaveChangesAsync();

            return speakers;
        }

        private bool SpeakersExists(long id)
        {
            return _context.Speakers.Any(e => e.Id == id);
        }
    }
}
