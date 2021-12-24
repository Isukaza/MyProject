//Формирование стартового набора данных
using Microsoft.EntityFrameworkCore;

namespace WpfApp
{
    public static class Defaultdata
    {
        public static void Defaultset(DbContextOptions<ApplicationContext> options)
        {
            using ApplicationContext db = new(options);
            // создание и добавление моделей
            Company microsoft = new() { Name = "Microsoft" };
            Company google = new() { Name = "Google" };

            Staff tom = new() { Name = "Tom", Company = microsoft };
            Staff bob = new() { Name = "Bob", Company = microsoft };
            Staff alice = new() { Name = "Alice", Company = google };

            Dept prog = new() { Department = "prog" };
            Dept desig = new() { Department = "design" };


            StaffProfile data1 = new() { Login = "Gof", Password = "1233f3fsf2345", Staff = tom };
            StaffProfile data2 = new() { Login = "Fwf", Password = "4334f45ghq122", Staff = bob };
            StaffProfile data3 = new() { Login = "Uht", Password = "43gfwef3445h5", Staff = alice };

            db.Depts.AddRange(prog, desig);

            tom.Depts.Add(prog);
            tom.Depts.Add(desig);
            bob.Depts.Add(desig);

            alice.Depts.Add(desig);
            alice.Depts.Add(prog);

            db.StaffProfiles.AddRange(data1, data2, data3);
            db.Companies.AddRange(microsoft, google);
            db.Staffs.AddRange(tom, bob, alice);
            db.SaveChanges();
        }
    }
}

