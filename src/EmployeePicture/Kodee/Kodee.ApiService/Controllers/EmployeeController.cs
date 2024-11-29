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

        #region GetById
        // GET api/<EmployeeController>/5
        // 특정 ID의 직원 정보를 조회
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeViewModel), StatusCodes.Status200OK)] // 200: 성공적으로 데이터를 반환
        [ProducesResponseType(StatusCodes.Status404NotFound)] // 404: 요청한 직원이 없을 경우
        public async Task<IActionResult> Get(int id)
        {
            // 직원 정보 및 관련된 사진 데이터를 포함하여 조회
            var employee = await _context.Employees
                .Include(e => e.Photos)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound(); // 직원이 없으면 404 반환 
            }

            //var response = new EmployeeViewModel(employee.Id, employee.FirstName, employee.LastName);
            var response = employee.Adapt<EmployeeViewModel>();

            return Ok(response); // 직원 데이터를 반환
        }
        #endregion

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
