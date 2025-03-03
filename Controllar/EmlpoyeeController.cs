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
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
            //this method first check whether the user is valid or not then the token is get generated and stored in the cookies and also sent as the response 
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
                //here the token is get stored in the cookies for the future purpose
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
            //the claims is used to store the user name for future purpose
            var claims=new List<Claim>
            {
                new Claim(ClaimTypes.Name,name)
            };
            //here the token is get generated in that token the username is get stored through the claims
            //ihgigiugughugujhuigkujgbkugiugiujgbiugiugbiugiug this is the key used to generate the token you may give random value in it 
            var token=new JwtSecurityToken(
                issuer:"",
                audience:"",
                claims:claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials:new SigningCredentials( new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ihgigiugughugujhuigkujgbkugiugiujgbiugiugbiugiug")),SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        ///First create a table name Credentials wiht column id,name,password(Just for testing purpose) and add sample data in it
        ///In Postman  http://localhost:5226/api/employees/auth with this url in the post request
        /////In The body provide the username and password in the json format 
        ///{"name":"jo","password":"jo123"} Like this
        ///then when you give the request you receive the token like
        ///eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiam8iLCJleHAiOjE3NDExMTQ4MDZ9.EtumdmVculnOV8yfkKvfzrGeAdwmJDG8Lmkn11vVUuI
        ///in the another get request in the postman http://localhost:5226/api/employees
        ///under the authorization section in the Type select the bearer Token and paste the above token in it
        ///and hit the send button you will get the details from the table without the token you will not able to get the data from the 
        ///table wiht this url  http://localhost:5226/api/employees
        ///the token is also get stored in the cookies for future purpose


    }
}
