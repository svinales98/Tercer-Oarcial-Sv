using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace OptativoApiNet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuracion;

        public AuthController(IConfiguration configuration)

        {
            _configuracion = configuration;
        }

        [HttpPost("login")]
        public IActionResult Post([FromBody] LoginModel login)
        {
            var userIsValid = ValidUser();

            if (!userValid)
            {
                return Unauthorized();
            }
            var token = GenerateJWT(login.UserName, JwtRegisteredClaimNames);

            return Ok(new { Jwt = token });
        }

        private bool ValidUser(LoginModel login)
        {
            return login.UserName == "admin" && login.Password == "12345";
        }

       

        private object GenerateJWT(string Username, JwtRegisteredClaimNames JwtRegisteredClaimNames)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub,UserName)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuracion["Jwt:Issuer"],
                audience: _configuracion["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(320),
                signingCredential:credential
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

            
        }
        

       
    }

    public class LoginModel
    {
        public string UserName { get; set}
        public string Password { get; set}
    }
}

    
