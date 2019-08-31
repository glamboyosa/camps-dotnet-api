using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using campsApi;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace campsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampUsersController : ControllerBase
    {
        private readonly campsContext _context;
        private readonly IEmailSender _emailSender;
        public CampUsersController(campsContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: api/CampUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampUsers>>> GetCampUsers()
        {
            var campUsers = await _context.CampUsers.ToListAsync();
            return Ok(campUsers);              
        }

        // GET: api/CampUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CampUsers>> GetCampUsers(long id)
        {
            var campUsers = await _context.CampUsers.FindAsync(id);

            if (campUsers == null)
            {
                return NotFound();
            }

            return Ok(campUsers);
        }

        // PUT: api/CampUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampUsers(long id, CampUsers model)
        {
            var FirstName = model.FirstName;
            var LastName = model.LastName;
            var Email = model.Email;
            var PhoneNumber = model.PhoneNumber;
            var campusers = await _context.CampUsers.FirstOrDefaultAsync(x => x.Id == id);
        // you could alternatively try the findasync method
            if(model != null)
            {
                campusers.FirstName = model.FirstName;
                campusers.LastName = model.LastName;
                campusers.Email = model.Email;
                campusers.PhoneNumber = model.PhoneNumber;
                _context.Update(campusers);
                await _context.SaveChangesAsync();
                return Ok();

            }
            return BadRequest();
        }

        // POST: api/CampUsers
        [HttpPost]
        public async Task<ActionResult<CampUsers>> PostCampUsers(CampUsers model)
        {
            var firstName = model.FirstName;
            var lastName = model.LastName;
            var email = model.Email;
            var phoneNumber = model.PhoneNumber;
    
            if(model != null)
            {
                var campusers = new CampUsers()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PhoneNumber = phoneNumber

                };
              
                  _context.CampUsers.Add(campusers);
                await _context.SaveChangesAsync();

                // string str = "<!DOCTYPE html>< html lang = 'en' >< head >< meta charset = 'UTF-8' />< meta name = 'viewport' content = 'width=device-width, initial-scale=1.0' />< meta http-equiv = 'X-UA-Compatible' content = 'ie=edge' />< title > Document </ title >< style > @import url('https://fonts.googleapis.com/css?family=Amatic+SC&display=swap');* {margin: 0;padding: 0;} body {width: 100 %;background-color: rgb(236, 233, 233);font-size: 25px;font-family: 'Amatic SC', cursive;} .classes {background-color: rgb(236, 235, 235);margin-left: 14 %;width: 70 %;padding: 10px 30px;text-align: center;} h1 { margin-top: 20px; margin-bottom: 20px; font-size: 180 %;text-align: center;} p {  margin-top: 20px; margin-bottom: 20px;text-align: center;font-size: 150 %;}</ style ></ head >< body >< div class='classes'><h1>Welcome to Camp Kahluahi</h1><p>Thank you for registering for camp Kahluahi { model.FirstName}. <br />We eagerly await you on the 25th of this month</p><p>Regards, camp Kahluahi family</p></div></body></html>"
                await _emailSender.SendEmailAsync(model.Email, "Welcome To Camp Kahluahi",
               $"<p>Glad to have you join us {model.FirstName}!</p><br> <p>We hope to see you there!. </p> <p>Love from the Camp Kahluahi team</p><br><footer><p>Please ignore this, it's Osa i'm testing sending emails from a .Net application xxx");
                return Ok(campusers);
            }
            return BadRequest();
          

       
        }

        // DELETE: api/CampUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CampUsers>> DeleteCampUsers(long id)
        {
            var campUsers = await _context.CampUsers.FindAsync(id);
            if (campUsers == null)
            {
                return NotFound();
            }

            _context.CampUsers.Remove(campUsers);
            await _context.SaveChangesAsync();

            return campUsers;
        }

        private bool CampUsersExists(long id)
        {
            return _context.CampUsers.Any(e => e.Id == id);
        }
    }
}
