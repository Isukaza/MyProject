using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private static DbContextOptions<ApplicationContext> options;
        public MainWindow()
        {
            InitializeComponent();
            options = ApplicationContext.CreateOption();
            InterfaceElements.CreateTable(DbList);
        }

        //Методы реализации функционала интерфейса 
        //Вывести данные в бд
        private void Button_show_Click(object sender, RoutedEventArgs e)
        {
            DbList.ItemsSource = CRUD.Showdb(new List<DBdate>(), options);
        }

        //Удаление элемента 
        private void Button_delete_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxIdStaff.Text)
                  && !string.IsNullOrWhiteSpace(TextBoxStaff.Text)
                  && TextBoxCompany.Text.Trim() is "Microsoft" or "Google"
                  && CheckDepts(TextBoxDepts.Text.Trim())
                  && !string.IsNullOrWhiteSpace(TextBoxLogin.Text)
                  && !string.IsNullOrWhiteSpace(TextBoxPassword.Text)
                  )
            {
                if (CRUD.Del(options, Convert.ToInt32(TextBoxIdStaff.Text), TextBoxStaff.Text, ReturnMaxDepts(TextBoxDepts.Text.Trim()), TextBoxLogin.Text))
                {
                    DbList.ItemsSource = CRUD.Showdb(new List<DBdate>(), options);
                }
            }
            else
            {
                MessageBox.Show("Ошибка данных");
            }
        }

        //Добавление записи в БД
        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxStaff.Text)
                 && TextBoxCompany.Text.Trim() is "Microsoft" or "Google"
                 && CheckDepts(TextBoxDepts.Text.Trim())
                 && !string.IsNullOrWhiteSpace(TextBoxLogin.Text)
                 && !string.IsNullOrWhiteSpace(TextBoxPassword.Text)
                 )
            {
                if (CRUD.Add(options, TextBoxStaff.Text, TextBoxCompany.Text, ReturnMaxDepts(TextBoxDepts.Text.Trim()), TextBoxLogin.Text, TextBoxPassword.Text))
                {
                    DbList.ItemsSource = CRUD.Showdb(new List<DBdate>(), options);
                }
            }
            else
            {
                MessageBox.Show("Ошибка данных");
            }
        }

        //Применить изменения к строке после внесения изменений 
        private void Button_change_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{!string.IsNullOrWhiteSpace(TextBoxStaff.Text)}" +
                $"{TextBoxCompany.Text.Trim() is "Microsoft" or "Google"}" +
                $"{CheckDepts(TextBoxDepts.Text.Trim())}" +
                $"{!string.IsNullOrWhiteSpace(TextBoxLogin.Text)}" +
                $"{!string.IsNullOrWhiteSpace(TextBoxPassword.Text)}");

            if (!string.IsNullOrWhiteSpace(TextBoxStaff.Text)
                && TextBoxCompany.Text.Trim() is "Microsoft" or "Google"
                && CheckDepts(TextBoxDepts.Text.Trim())
                && !string.IsNullOrWhiteSpace(TextBoxLogin.Text)
                && !string.IsNullOrWhiteSpace(TextBoxPassword.Text)
                )
            {
                if (CRUD.Change(options, Convert.ToInt32(TextBoxIdStaff.Text), TextBoxStaff.Text, ReturnMaxDepts(TextBoxDepts.Text.Trim()), TextBoxLogin.Text, TextBoxPassword.Text))
                {
                    DbList.ItemsSource = CRUD.Showdb(new List<DBdate>(), options);
                }
            }
            else
            {
                MessageBox.Show("Ошибка данных");
            }
        }

        //Вывод выбранной строки в TextBox из LixtView для редактирования 
        private void DbList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DbList.SelectedItem != null)
            {
                string[] MyString = DbList.SelectedItem.ToString().Split('\\');

                TextBoxIdStaff.Text = MyString[0];

                TextBoxStaff.Text = MyString[1];

                TextBoxCompany.Text = MyString[2];

                TextBoxDepts.Text = MyString[3];

                TextBoxLogin.Text = MyString[4];

                TextBoxPassword.Text = MyString[5];
            }
        }

        //Технические методы
        //Проверка правильности ввода отделов
        private static bool CheckDepts(string str)
        {
            foreach (string s in str.Split(' ').Distinct())
            {
                if (s is not ("prog" or "design"))
                {
                    return false;
                }
            }
            return true;
        }

        //Удаление дубликатов отделов
        private static List<string> ReturnMaxDepts(string str)
        {
            List<string> NewString = new(str.Split(' '));
            NewString = NewString.Distinct().ToList();
            return NewString;
        }
    }

    public class DBdate
    {
        public int IdUser { get; set; }
        public string NameStaff { get; set; }
        public string CompanyName { get; set; }
        public string Departament { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public override string ToString()
        {
            return $"{IdUser}\\{NameStaff}\\{CompanyName}\\{Departament}\\{Login}\\{Password}";
        }
    }
}
