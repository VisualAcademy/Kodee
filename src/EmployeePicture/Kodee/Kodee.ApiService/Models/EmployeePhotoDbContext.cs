using Microsoft.EntityFrameworkCore;

namespace Kodee.ApiService.Models;

// DbContext를 상속한 클래스: EF Core가 사용할 데이터베이스 컨텍스트 정의
public class EmployeePhotoDbContext : DbContext
{
    // 매개변수가 없는 기본 생성자 (테스트나 특별한 경우에 사용 가능)
    public EmployeePhotoDbContext() : base()
    {

    }

    // DI를 통해 옵션을 받아 사용하는 생성자 (실제 앱 실행 시 주로 사용)
    public EmployeePhotoDbContext(DbContextOptions<EmployeePhotoDbContext> options)
        : base(options)
    {

    }

    // Employees 테이블에 해당하는 DbSet - 직원 정보를 관리
    public DbSet<Employee> Employees { get; set; }

    // Photos 테이블에 해당하는 DbSet - 사진 정보를 관리
    public DbSet<Photo> Photos { get; set; }
}
