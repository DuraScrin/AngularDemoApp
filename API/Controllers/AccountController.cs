using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
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

            UserDto userDto = new()
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
            return Ok(userDto);
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

            UserDto userDto = new()
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
            return Ok(userDto);
        }

        private async Task<bool> UserExists(string userName)
        {
            bool exists = await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
            return exists;
        }
    }
}