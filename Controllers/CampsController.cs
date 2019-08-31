using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using campsApi;

namespace campsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly campsContext _context;

        public CampsController(campsContext context)
        {
            _context = context;
        }

        // GET: api/Camps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Camps>>> GetCamps()
        {
            return await _context.Camps.ToListAsync();
        }

        // GET: api/Camps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Camps>> GetCamps(long id)
        {
            var camps = await _context.Camps.FindAsync(id);

            if (camps == null)
            {
                return NotFound();
            }

            return camps;
        }

        // PUT: api/Camps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCamps(long id, Camps model)
        {

            var startDate = model.StartDate;
            var endDate = model.EndDate;
            var campsCheck = await _context.Camps.FirstOrDefaultAsync(x => x.Id == id);
            if (model != null)
            {
                campsCheck.StartDate = startDate;
                campsCheck.EndDate = endDate;
                _context.Update(campsCheck);
                await _context.SaveChangesAsync();
                return Ok(campsCheck);
            }
            return BadRequest("Something went wrong, could not update the Database");
        }

        // POST: api/Camps
        [HttpPost]
        public async Task<ActionResult<Camps>> PostCamps(Camps model)
        {
            var startDate = model.StartDate;
            var endDate = model.EndDate;
            //var eventsId = model.Events;
            var name = model.Name;
            var eventsCheck = await _context.Camps.FirstOrDefaultAsync(x => x.Name == model.Name);
            if(model!= null)
            {
                var Camp = new Camps()
                {
                    Name = name,
                    StartDate = startDate,
                    EndDate = endDate,
                };
                _context.Camps.Add(Camp);
                await _context.SaveChangesAsync();
                return Ok(Camp);
            }
            return BadRequest("Something Went wrong. Could not create");
           

           
        }



        private bool CampsExists(long id)
        {
            return _context.Camps.Any(e => e.Id == id);
        }
    }
}
