using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthService.Database;
using AuthService.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IConfiguration _configuration { get; }
        public AuthController(AuthDbContext authDbContext, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = authDbContext;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public AuthDbContext _db { get; }

        public async Task<IActionResult> GetUsers()
        {
            var users = await _db.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var _userFromDatabase = await _userManager.FindByNameAsync(userForLoginDto.UserName);
            if (_userFromDatabase == null) return NotFound();
            var verfyPasssword = await _signInManager.CheckPasswordSignInAsync(_userFromDatabase, userForLoginDto.Password, false);
            if (verfyPasssword.Succeeded)
            {
                return Ok(new
                {
                    token = await TokenGeneratorAsync(_userFromDatabase),
                    user = _userFromDatabase
                });
            }
            return Unauthorized();
        }

        private async Task<string> TokenGeneratorAsync(User user)
        {
            var claims = new List<Claim>
          {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName,user.UserName),

          };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("Role", role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],

            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
