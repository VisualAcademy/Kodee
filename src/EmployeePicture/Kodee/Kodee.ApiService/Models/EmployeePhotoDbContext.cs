using Microsoft.EntityFrameworkCore;

namespace Kodee.ApiService.Models
{
    public class EmployeePhotoDbContext : DbContext
    {
        public EmployeePhotoDbContext() : base()
        {
            
        }

        public EmployeePhotoDbContext(DbContextOptions<EmployeePhotoDbContext> options)
            : base(options) 
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
