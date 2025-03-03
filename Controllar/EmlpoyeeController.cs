using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data; 
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers 
{
    [Route("api/employees")] 
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context) 
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = _context.Employees.OrderByDescending(c => c.Id).ToList();   

            if (employees == null || employees.Count == 0)
            {
                return NotFound("No employees found in the database.");
            }

            return Ok(employees);
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] Credential credential)
        {
            if (credential == null || string.IsNullOrEmpty(credential.name) || string.IsNullOrEmpty(credential.password))
            {
                return BadRequest("Invalid credentials.");
            }
            var user = _context.Credentials
                .FromSqlRaw("SELECT * FROM \"Credentials\" WHERE name = {0} AND password = {1}",
                            credential.name, credential.password)
                .FirstOrDefault();

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }else{
                var token =GenerateToken(credential.name);
                Response.Cookies.Append("jwt",token,new CookieOptions
                {
                    HttpOnly=true,
                    Secure=true,
                    SameSite=SameSiteMode.Strict,
                    Expires=DateTime.UtcNow.AddDays(1)
                });
                return Ok(token);
            }

            
        }

        private String GenerateToken (String name){
            var claims=new List<Claim>
            {
                new Claim(ClaimTypes.Name,name)
            };
            var token=new JwtSecurityToken(
                issuer:"",
                audience:"",
                claims:claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials:new SigningCredentials( new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ihgigiugughugujhuigkujgbkugiugiujgbiugiugbiugiug")),SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
