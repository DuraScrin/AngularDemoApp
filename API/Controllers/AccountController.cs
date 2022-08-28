using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName))
                return BadRequest("Username is taken.");

            using var hmac = new HMACSHA512();
            AppUser user = new()
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn(LoginDto loginDto)
        {
            AppUser user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.UserName.ToLower());

            if(user is null)
                return Unauthorized("Invalid username.");

            using HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password.");
            }

            return Ok(user);
        }

        private async Task<bool> UserExists(string userName)
        {
            bool exists = await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
            return exists;
        }
    }
}