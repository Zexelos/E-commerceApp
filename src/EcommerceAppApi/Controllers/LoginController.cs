using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginController(
            IConfiguration configuration,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserModel userModel)
        {
            IActionResult response = Unauthorized();
            var authenticateResult = AuthenticateUser(userModel);

            if (authenticateResult)
            {
                var tokenString = await GenerateJSONWebToken(userModel);
                return Ok(new { token = tokenString });
            }
            return response;
        }

        private async Task<string> GenerateJSONWebToken(UserModel userModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var user = await _userManager.FindByNameAsync(userModel.Email);
            var roles = await _userManager.GetClaimsAsync(user);
            var claim = new Claim(string.Empty, string.Empty);

            foreach (var item in roles)
            {
                if (item.Type == "IsAdmin" && item.Value == "True")
                {
                    claim = item;
                    break;
                }
                else if (item.Type == "IsEmployee" && item.Value == "True")
                {
                    claim = item;
                }
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(claim.Type, claim.Value),
                new Claim("UserId", user.Id)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool AuthenticateUser(UserModel userModel)
        {
            var result = _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, true, false).Result;
            return result.Succeeded;
        }
    }
}
