using Kodee.ApiService.Models;
using Kodee.ApiService.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kodee.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeePhotoDbContext _context;

        public EmployeeController(EmployeePhotoDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var employees = await _context.Employees
                //.Select(e => new EmployeeViewModel(e.Id, e.FirstName, e.LastName))
                .ProjectToType<EmployeeViewModel>()
                .ToListAsync();

            return Ok(employees);
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
