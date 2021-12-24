using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfApp
{
    public class CRUD
    {
        //Добавление данных в БД 
        public static bool Add(DbContextOptions<ApplicationContext> options, string NStaff, string NCompany, List<string> NDepts, string Login, string Password)
        {
            using ApplicationContext db = new(options);
            if (db.Staffs.Include(p => p.Profile).FirstOrDefault(l => l.Profile.Login == Login) == null)
            {
                Company SelectCompany = db.Companies.FirstOrDefault(c => c.Name == NCompany);
                if (SelectCompany == null)
                {
                    SelectCompany = new() { Name = NCompany };
                }

                Staff NewStaff = new() { Name = NStaff, Company = SelectCompany };
                StaffProfile Profile = new() { Login = Login, Password = Password, Staff = NewStaff };

                foreach (string line in NDepts)
                {
                    Dept Add_dept = db.Depts.FirstOrDefault(d => d.Department == line);
                    if (Add_dept != null)
                    {
                        NewStaff.Depts.Add(Add_dept);
                    }
                }

                db.StaffProfiles.Add(Profile);
                db.Staffs.Add(NewStaff);
                db.SaveChanges();
                return true;
            }
            else
            {
                MessageBox.Show("Пользователь с такой учётной записью уже существует");
                return false;
            }
        }

        //Вывод данных БД
        public static List<DBdate> Showdb(List<DBdate> item, DbContextOptions<ApplicationContext> options)
        {
            using ApplicationContext db = new(options);
            string buff = "";

            foreach (Staff Staff in db.Staffs.Include(c => c.Company).Include(p => p.Profile).Include(d => d.Depts).ToList())
            {
                foreach (Dept deptwrite in Staff.Depts)
                {
                    buff += deptwrite.Department + " ";
                }

                item.Add(new DBdate
                {
                    IdUser = Staff.Id,
                    NameStaff = Staff.Name,
                    CompanyName = Staff.Company.Name,
                    Departament = buff,
                    Login = Staff.Profile.Login,
                    Password = Staff.Profile.Password
                });
                buff = "";
            }
            return item;
        }

        //Изменение данных в БД 
        public static bool Change(DbContextOptions<ApplicationContext> options, int IdStaff, string NStaff, List<string> NDepts, string Login, string Password)
        {
            using ApplicationContext db = new(options);
            Staff RenewStaff = db.Staffs.Include(c => c.Depts).Include(p => p.Profile).FirstOrDefault(s => s.Id == IdStaff);

            if (RenewStaff != null)
            {
                RenewStaff.Name = NStaff;
                RenewStaff.Profile.Login = Login;
                RenewStaff.Depts.Clear();
                RenewStaff.Profile.Password = Password;

                foreach (string line in NDepts)
                {
                    Dept RenewDept = db.Depts.FirstOrDefault(d => d.Department == line);
                    if (RenewDept != null)
                    {
                        RenewStaff.Depts.Add(RenewDept);
                    }
                }

                db.SaveChanges();
                return true;
            }
            return false;
        }

        //Удаление данных из БД
        public static bool Del(DbContextOptions<ApplicationContext> options, int IdStaff, string NStaff, List<string> NDepts, string Login)
        {
            using ApplicationContext db = new(options);
            StaffProfile Profile = db.StaffProfiles.Include(s => s.Staff).FirstOrDefault(s => s.Id == IdStaff);
            if (Profile != null && Profile.Login == Login && Profile.Staff.Name == NStaff)
            {
                Staff DelStaff = db.Staffs.FirstOrDefault(p => p.Id == IdStaff);
                if (Profile != null)
                {
                    db.StaffProfiles.Remove(Profile);
                    db.SaveChanges();
                }

                foreach (string str in NDepts)
                {
                    Dept DelDept = db.Depts.FirstOrDefault(c => c.Department == str);
                    if (DelDept != null && DelStaff != null)
                    {
                        DelStaff.Depts.Remove(DelDept);
                        db.SaveChanges();
                    }
                }

                if (DelStaff != null)
                {
                    db.Staffs.Remove(DelStaff);
                    db.SaveChanges();
                }
                return true;
            }
            return false;
        }
    }
}