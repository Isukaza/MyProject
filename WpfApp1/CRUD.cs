using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfApp
{
    public class CRUD
    {
        //Добавление данных в БД 
        public static bool Add(DbContextOptions<ApplicationContext> options, string Nstaff, string Ncompany, List<string> Ndepts, string Login, string Password)
        {
            using ApplicationContext db = new(options);
            if (db.Staffs.Include(p => p.Profile).FirstOrDefault(l => l.Profile.Login == Login) == null)
            {
                Company Selectcompany = db.Companies.FirstOrDefault(c => c.Name == Ncompany);
                if (Selectcompany == null)
                {
                    Selectcompany = new() { Name = Ncompany };
                }
                Staff Newstaff = new() { Name = Nstaff, Company = Selectcompany };
                StaffProfile Profile = new() { Login = Login, Password = Password, Staff = Newstaff };

                foreach (string line in Ndepts)
                {
                    Dept Add_dept = db.Depts.FirstOrDefault(d => d.Department == line);
                    if (Add_dept != null)
                    {
                        Newstaff.Depts.Add(Add_dept);
                    }
                }

                db.StaffProfiles.Add(Profile);
                db.Staffs.Add(Newstaff);
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
                    Id_User = Staff.Id,
                    Name_Staff = Staff.Name,
                    Company_Name = Staff.Company.Name,
                    Departament = buff,
                    Login = Staff.Profile.Login,
                    Password = Staff.Profile.Password
                });
                buff = "";
            }
            return item;
        }

        //Изменение данных в БД 
        public static bool Change(DbContextOptions<ApplicationContext> options, int Idstaff, string Nstaff, List<string> Ndepts, string Login, string Password)
        {
            using ApplicationContext db = new(options);
            Staff Renew_staff = db.Staffs.Include(c => c.Depts).Include(p => p.Profile).FirstOrDefault(s => s.Id == Idstaff);

            if (Renew_staff != null)
            {
                Renew_staff.Name = Nstaff;
                Renew_staff.Profile.Login = Login;
                Renew_staff.Depts.Clear();
                Renew_staff.Profile.Password = Password;

                foreach (string line in Ndepts)
                {
                    Dept Renew_dept = db.Depts.FirstOrDefault(d => d.Department == line);
                    if (Renew_dept != null)
                    {
                        Renew_staff.Depts.Add(Renew_dept);
                    }
                }

                db.SaveChanges();
                return true;
            }
            return false;
        }

        //Удаление данных из БД
        public static bool Del(DbContextOptions<ApplicationContext> options, int Idstaff, string Nstaff, List<string> Ndepts, string Login)
        {
            using ApplicationContext db = new(options);
            StaffProfile Profile = db.StaffProfiles.Include(s => s.Staff).FirstOrDefault(s => s.Id == Idstaff);
            if (Profile != null && Profile.Login == Login && Profile.Staff.Name == Nstaff)
            {
                Staff Delstaff = db.Staffs.FirstOrDefault(p => p.Id == Idstaff);
                if (Profile != null)
                {
                    db.StaffProfiles.Remove(Profile);
                    db.SaveChanges();
                }

                foreach (string str in Ndepts)
                {
                    Dept Del_dept = db.Depts.FirstOrDefault(c => c.Department == str);
                    if (Del_dept != null && Delstaff != null)
                    {
                        Delstaff.Depts.Remove(Del_dept);
                        db.SaveChanges();
                    }
                }

                if (Delstaff != null)
                {
                    db.Staffs.Remove(Delstaff);
                    db.SaveChanges();
                }
                return true;
            }
            return false;
        }
    }
}