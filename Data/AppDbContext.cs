using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Data
{
    public class AppDbContext :DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration){
            Configuration=configuration;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));

        }
        public DbSet<Employee> Employees {get; set;}
    }
}