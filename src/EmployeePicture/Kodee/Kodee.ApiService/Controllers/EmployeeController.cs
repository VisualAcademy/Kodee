using Kodee.ApiService.Models;
using Kodee.ApiService.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kodee.ApiService.Controllers
{
    [Authorize]
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

        #region POST
        // POST api/<EmployeeController>
        // 새로운 직원을 생성
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeViewModel), StatusCodes.Status201Created)] // 201: 생성된 리소스를 반환
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // 400: 잘못된 요청일 경우
        public async Task<IActionResult> Post([FromBody] EmployeeViewModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 유효성 검사 실패 시 400 반환
            }

            // 요청 데이터를 Employee 엔터티로 변환하여 저장
            //var employee = new Employee
            //{
            //    FirstName = value.FirstName,
            //    LastName = value.LastName,
            //};
            var employee = value.Adapt<Employee>();

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            //var response = new EmployeeViewModel(employee.Id, employee.FirstName, employee.LastName);
            var response = employee.Adapt<EmployeeViewModel>();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response); // 생성된 직원 정보 반환
        }
        #endregion

        #region PUT
        // PUT api/<EmployeeController>/5
        // 기존 직원 정보를 업데이트
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // 204: 성공적으로 업데이트하고 콘텐츠 없음
        [ProducesResponseType(StatusCodes.Status404NotFound)] // 404: 요청한 직원이 없을 경우
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // 400: 잘못된 요청일 경우
        public async Task<IActionResult> Put(long id, [FromBody] EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 유효성 검사 실패 시 400 반환

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound(); // 직원이 없으면 404 반환

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                    return NotFound(); // 업데이트 중 직원이 삭제되었을 경우 404 반환

                throw;
            }

            return NoContent(); // 성공 시 204 반환
        } 

        private bool EmployeeExists(long id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
        #endregion

        #region DELETE
        // DELETE api/<EmployeeController>/5
        // 특정 직원 데이터를 삭제
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // 204: 성공적으로 삭제하고 콘텐츠 없음
        [ProducesResponseType(StatusCodes.Status404NotFound)] // 404: 요청한 직원이 없을 경우
        public async Task<IActionResult> Delete(long id)
        {
            // 직원 정보와 관련된 사진 데이터를 포함하여 조회
            var employee = await _context.Employees
                .Include(e => e.Photos)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound(); // 직원이 없으면 404 반환

            // 직원 데이터와 관련된 사진 데이터를 먼저 삭제
            _context.Photos.RemoveRange(employee.Photos);

            // 직원 데이터 삭제
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent(); // 성공 시 204 반환
        } 
        #endregion
    }
}
