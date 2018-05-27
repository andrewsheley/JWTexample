using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace trashapi3.Controllers
{
    [Produces("application/json")]
    [Route("api/Authorize")]
    public class AuthorizeController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthorizeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {


            if (request.Username == "asheley" && request.Password == "Password01!")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    //new Claim("CompletedBasicTraining", ""),
                    //new Claim(CustomClaimTypes.EmploymentCommenced,
                    //            new DateTime(2017,12,1).ToString(),
                    //            ClaimValueTypes.DateTime)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "yourdomain.com",
                    audience: "yourdomain.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: creds);

                //return Ok(new
                //{
                //    token = new JwtSecurityTokenHandler().WriteToken(token)
                //});

                return Ok(
                        new TokenResponse() { token = new JwtSecurityTokenHandler().WriteToken(token), alive = 5 }
                    );

            }

            //return BadRequest("Could not verify username and password");
            return Unauthorized();
           
        }


    }

    public class TokenResponse
    {
        public string token { get; set; }
        public int alive { get; set; }
    }


    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}