using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Restaurant4you_API.Models;
using Restaurant4you_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Restaurant4you_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        //public static User user = new User();
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, ApplicationDbContext db)
        {
            _configuration = configuration;
            this.db = db;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromForm]String username, [FromForm] String password)
        {
            if (!db.Users.Where(x => x.Username == username).Any())
            {
                User user = new User();

                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.Username = username;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Role = "User";

                db.Users.Add(user);
                db.SaveChanges();

                return Ok(user);
            }
            else
            {
                return BadRequest("Nome de utilizador já esta a ser utilizado.");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(String username, String password)
        {
            User user = db.Users.Where(a => a.Username == username).FirstOrDefault();

            if (user.Username != username)
            {
                return BadRequest("Utilizador não encontrado.");
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password incorreta.");
            }

            string token = CreateToken(user);

            user.TokenCreated = DateTime.Now;
            user.TokenExpires = DateTime.Now.AddDays(7);
            db.SaveChanges();

            return Ok(token);
        }


        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: user.TokenExpires,
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
