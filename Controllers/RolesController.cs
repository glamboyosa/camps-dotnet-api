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
    public class RolesController : ControllerBase
    {
        private readonly campsContext _context;

        public RolesController(campsContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            return Ok(roles);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Roles>> GetRoles(long id)
        {
            var roles = await _context.Roles.FindAsync(id);

            if (roles == null)
            {
                return NotFound();
            }

            return roles;
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutRoles(long id, Roles model)
        {

            var role = model.Role;
            var roleCheck = await _context.Roles.FirstOrDefaultAsync(e => e.Id == id);
            if (roleCheck != null)
            {
                roleCheck.Role = role;
                _context.Update(roleCheck);
                await _context.SaveChangesAsync();
                return Ok(roleCheck);
            }
            return BadRequest();
        }

        // POST: api/Roles
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Roles>> PostRoles(Roles model)
        {
            var role = model.Role;
           
           
            if(model != null)
            {
                var Role = new Roles()
                {
                    Role = role
                };
                _context.Roles.Add(Role);
                var roles = await _context.Roles.FirstOrDefaultAsync(e => e.Id == model.Id);
             
                    await _context.SaveChangesAsync();
                    return Ok(Role);
               
            }
            return BadRequest();
           
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Roles>> DeleteRoles(long id)
        {
            var roles = await _context.Roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(roles);
            await _context.SaveChangesAsync();

            return roles;
        }

        private bool RolesExists(long id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
