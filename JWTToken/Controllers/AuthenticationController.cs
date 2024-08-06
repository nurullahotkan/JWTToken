using JWTToken.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        public AuthenticationController(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Users users)
        {
            var user = UserValidate(users);
            if (user == null) return NotFound("Kullanıcı Bulunamadı");

            var token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(Users user)
        {
            if (_jwtSettings.Key == null)
            {
                throw new Exception("Jwt ayarlarında key bulunamadı");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.username!),
                new Claim(ClaimTypes.Role, user.role!)
            };

            var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, expires: DateTime.Now.AddHours(1),signingCredentials:credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Users? UserValidate(Users users)
        {
            return UserList.Users.FirstOrDefault(x => x.username.ToLower() == users.username && x.password == users.password);
        }
    }
}
