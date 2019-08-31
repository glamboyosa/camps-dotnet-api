using campsApi.Helpers;
using campsApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace campsApi.Services
{
   public interface IUserService
    {
        Users Authenticate(string email, string password);
   IEnumerable<Users> GetUsers();
        Users GetUsersById(long id);
    }

    public class UserService : IUserService
    {
        private readonly campsContext _context;
        private readonly AppSettings _appSettings;
        private readonly IHashingService _hashingService;
        public UserService(IOptions<AppSettings> appsettings, campsContext context, IHashingService hashingService)
        {
            _context = context;
            _appSettings = appsettings.Value;
            _hashingService = hashingService;
        }
        /* Seeding the DB*/
        public static void Initialize(IServiceProvider serviceProvider)
        {
           using (var scope= serviceProvider.CreateScope())
            {
                using(var context= scope.ServiceProvider.GetRequiredService<campsContext>())
                {
                    if (!context.Users.Any())
                    {
                        string salt = SaltGenerator.SaltMethod();
                        HashingService hash = new HashingService();
                        context.Users.Add(new Users() { Fullname = "Admin", Email = "ogbemudiatimothy@gmail.com", Passwordsalt = salt, Role = Role.Admin, Passwordhash = hash.ComputeSha256Hash("Admin", salt) });
                        context.SaveChanges();
                    }
                    //Seed all the other stuff
                   
                }
            }
        }
        public Users Authenticate(string email, string password)
        {
            var users = _context.Users.FirstOrDefault(x => x.Email == email);
            if (users == null) return null;
            var salt = users.Passwordsalt;
            var verifyPassword = _hashingService.ComputeSha256Hash(password, salt);

            //find the user if email and password matches
            var user = _context.Users.FirstOrDefault(x => x.Email == email && x.Passwordhash == verifyPassword);
            if (user == null)
            {
                return null;
            }
            //authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Passwordhash = null;
            user.Passwordsalt = null;
            return user;
        }
        public IEnumerable<Users> GetUsers()
        {
            return _context.Users.Select(x => new Users()
            {
                Id=x.Id,
                Email=x.Email,
                Token=x.Token,
                Fullname=x.Fullname,
                Role=x.Role,
                Passwordhash=null,
                Passwordsalt=null
                
            }).ToList();
        }
        public Users GetUsersById(long id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user != null) { user.Passwordhash = null; user.Passwordsalt = null; }
            return user;
        }
    }
}
