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
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Описание первичных ключей 
            modelBuilder.Entity<Company>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Staff>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<StaffProfile>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Dept>()
                .HasKey(c => c.Id);

            //Описание связей таблиц
            modelBuilder.Entity<Staff>()
                .HasOne(p => p.Company)
                .WithMany(p => p.Staffs)
                .HasForeignKey(p => p.CompanyId);

            modelBuilder.Entity<Staff>()
                .HasOne(p => p.Profile)
                .WithOne(p => p.Staff)
                .HasForeignKey<StaffProfile>(p => p.StaffId);

            //Данные для базовго заполнения БД
            modelBuilder.Entity<Company>().HasData(
                new Company[]{
                    new Company{ Id = 1, Name = "Microsoft"},
                    new Company{ Id = 2, Name = "Google" }
                });

            modelBuilder.Entity<Dept>().HasData(
                new Dept[] { 
                    new Dept { Id = 1, Department = "prog" },
                    new Dept { Id = 2, Department = "design" } 
                });


            modelBuilder.Entity<Staff>().HasData(
                new Staff[]{
                    new Staff { Id = 1, Name="Tom", CompanyId =1 },
                    new Staff { Id = 2, Name="Alice", CompanyId =2 },
                    new Staff { Id = 3, Name="Sam", CompanyId =1 },
                    new Staff { Id = 4, Name="Sam", CompanyId =2 }
                });

            modelBuilder.Entity<StaffProfile>().HasData(
               new StaffProfile[]{
                    new StaffProfile { Id = 1, Login="Tom", Password = "123dd3", StaffId = 1 },
                    new StaffProfile { Id = 2, Login="Alice", Password = "3g223g32", StaffId = 2 },
                    new StaffProfile { Id = 3, Login="Sam", Password = "g43g45g", StaffId = 3 },
                    new StaffProfile { Id = 4, Login="Gos", Password = "4g34g3g4", StaffId = 4 }
               });
        }
        public static DbContextOptions<ApplicationContext> CreateOption()
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