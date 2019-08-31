using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using campsApi;
using campsApi.Services;
using campsApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace campsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly campsContext _context;
        private readonly IUserService _userService;
        private readonly IHashingService _hashingService;
        public UsersController(campsContext context, IUserService userService,IHashingService hashingService)
        {
            _context = context;
            _userService = userService;
            _hashingService = hashingService;
        }

        // GET: api/Users
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            var users = _userService.GetUsers();
            
            return Ok(users);

        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(long id)
        {
            var users = _userService.GetUsersById(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }
        [HttpPut("changepassword/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword (long id, Users model)
        {
            var user =await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (user!=null)
            {
            user.Passwordhash= _hashingService.ComputeSha256Hash(model.Passwordhash, user.Passwordsalt);
                _context.Update(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Invalid User");
         
        }
        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutUsers(long id, Users model)
        {

            var fullname = model.Fullname;
            var role = model.Role;
          ;
            var email = model.Email;

            var users = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (users != null)
            {
                users.Fullname = fullname;
                users.Role = role;
              
                users.Email = email;
                _context.Update(users);
              await  _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Invalid User");
        }

        // POST: api/Users
        [HttpPost("authenticate")]

        public async Task<ActionResult<Users>> PostUsers([FromBody]Users model)
        {
           var authenitcatedUser= _userService.Authenticate(model.Email, model.Passwordhash);
            if(authenitcatedUser != null)
            {
                return Ok(authenitcatedUser);
            }
            return BadRequest("Invalid Username or password");
        }
        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Users>> CreateUsers([FromBody]Users model)
        {
            var fullname = model.Fullname;
            var role = model.Role;
            var salt = SaltGenerator.SaltMethod();
            var passwordHash = _hashingService.ComputeSha256Hash(model.Passwordhash, salt);
            var email = model.Email;
            if (model != null)
            {
                var user = new Users()
                {
                    Fullname = fullname,
                    Role = role,
                    Passwordsalt=salt,
                    Passwordhash=passwordHash,
                    Email=email
                    
                };
                _context.Users.Add(user);
             await   _context.SaveChangesAsync();
                user.Passwordhash = null;
                user.Passwordsalt = null;
                return Ok(user);
            }
            return BadRequest("Something went wrong");
        }
     
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Users>> DeleteUsers(long id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return users;
        }

        private bool UsersExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
