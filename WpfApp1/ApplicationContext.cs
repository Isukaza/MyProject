using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace WpfApp
{
    //Определение сущностей БД
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } // название компании

        public List<Staff> Staffs { get; set; } = new List<Staff>();

    }

    public class Staff
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CompanyId { get; set; }      // внешний ключ
        public Company Company { get; set; }    // навигационное свойство
        public StaffProfile Profile { get; set; }
        public List<Dept> Depts { get; set; } = new List<Dept>();
    }

    public class StaffProfile
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public int StaffId { get; set; } // внешний ключ
        public Staff Staff { get; set; } // навигационное свойство
    }

    public class Dept
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public List<Staff> Staffs { get; set; } = new List<Staff>();
    }

    //Конструктор контекста
    public class ApplicationContext : DbContext
    {

        public DbSet<Company> Companies { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffProfile> StaffProfiles { get; set; }
        public DbSet<Dept> Depts { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                    : base(options)
        {
            if (Database.EnsureCreated())
            {
                Defaultdata.Defaultset(options);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                        .HasKey(c => c.Id);
        }
        public static DbContextOptions<ApplicationContext> Create_option()
        {
            ConfigurationBuilder builder = new();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла dbsetting.json
            builder.AddJsonFile("dbsetting.json");
            // создаем конфигурацию
            IConfigurationRoot config = builder.Build();

            return new DbContextOptionsBuilder<ApplicationContext>().UseSqlServer(config.GetConnectionString("DefaultConnection")).Options;
        }
    }
}