using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SampleWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class AuthenticateController : ControllerBase
    {
        public IConfiguration _configuration { get; }
        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// گرفتن توکن با نام کاربری و رمز عبور
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Authenticate(string userName, string password)
        {
            var secretKey = _configuration.GetValue<string>("TokenKey");
            var tokenTimeOut = _configuration.GetValue<int>("TokenTimeOut");
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, "Ali Rezaei"),
                        new Claim("email", "test@gmail.com"),
                }),

                Expires = DateTime.UtcNow.AddMinutes(tokenTimeOut),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            var model = new AuthenticateViewModel()
            {
                RefreshToken = "",
                Token = token
            };

            return Ok(model);
        }
    }
}
