using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data; 
using System.Collections.Generic;
using System.Linq;

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

    }
}
