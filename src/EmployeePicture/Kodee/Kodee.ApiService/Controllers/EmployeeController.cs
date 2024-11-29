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

        #region GetAll
        // GET: api/Employee
        // 모든 직원 목록을 조회
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeViewModel>), StatusCodes.Status200OK)] // 성공 시 반환 타입과 HTTP 상태 코드를 명시
        public async Task<IActionResult> Get()
        {
            // 직원 데이터를 EmployeeViewModel로 변환하여 반환
            var employees = await _context.Employees
                //.Select(e => new EmployeeViewModel(e.Id, e.FirstName, e.LastName))
                .ProjectToType<EmployeeViewModel>()
                .ToListAsync();

            return Ok(employees);
        } 
        #endregion

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
