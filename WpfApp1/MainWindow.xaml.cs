using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace WpfApp
{
    public partial class MainWindow : Window
    {
        static DbContextOptions<ApplicationContext> options;
        public MainWindow()
        {
            InitializeComponent();
            options = ApplicationContext.Create_option();
            Interface_elements.Create_table(DB_list);
        }

        //Методы реализации функционала интерфейса 
        //Вывести данные в бд
        private void Button_show_Click(object sender, RoutedEventArgs e)
        {
            //Заполнение ListView
            DB_list.ItemsSource = CRUD.Showdb(new List<DBdate>(), options);
        }

        //Удаление элемента 
        private void Button_delete_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBox_Staff.Text)
                && TextBox_Company.Text.Trim() is "Microsoft" or "Google"
                && Check_Depts(TextBox_Depts.Text.Trim())
                && !string.IsNullOrWhiteSpace(TextBox_Login.Text)
                && !string.IsNullOrWhiteSpace(TextBox_Password.Text)
                )
            {
                if (CRUD.Del(options, Convert.ToInt32(TextBox_Id_Staff.Text), TextBox_Staff.Text, Return_max_Depts(TextBox_Depts.Text.Trim()), TextBox_Login.Text))
                    DB_list.ItemsSource = CRUD.Showdb(new List<DBdate>(), options);
            }
            else
            {
                MessageBox.Show("Ошибка данных");
            }
        }

        //Добавление записи в БД
        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBox_Staff.Text)
                && TextBox_Company.Text.Trim() is "Microsoft" or "Google"
                && Check_Depts(TextBox_Depts.Text.Trim())
                && !string.IsNullOrWhiteSpace(TextBox_Login.Text)
                && !string.IsNullOrWhiteSpace(TextBox_Password.Text)
                )
            {
                if (CRUD.Add(options, TextBox_Staff.Text, TextBox_Company.Text, Return_max_Depts(TextBox_Depts.Text.Trim()), TextBox_Login.Text, TextBox_Password.Text))
                {
                    DB_list.ItemsSource = CRUD.Showdb(new List<DBdate>(), options);
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
            if (!string.IsNullOrWhiteSpace(TextBox_Staff.Text)
                && TextBox_Company.Text.Trim() is "Microsoft" or "Google"
                && Check_Depts(TextBox_Depts.Text.Trim())
                && !string.IsNullOrWhiteSpace(TextBox_Login.Text)
                && !string.IsNullOrWhiteSpace(TextBox_Password.Text)
                )
            {
                if (CRUD.Change(options, System.Convert.ToInt32(TextBox_Id_Staff.Text), TextBox_Staff.Text, Return_max_Depts(TextBox_Depts.Text.Trim()), TextBox_Login.Text, TextBox_Password.Text))
                {
                    DB_list.ItemsSource = CRUD.Showdb(new List<DBdate>(), options);
                }
            }
            else
            {
                MessageBox.Show("Ошибка данных");
            }
        }

        //Вывод выбранной строки в TextBox из LixtView для редактирования 
        private void DB_list_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DB_list.SelectedItem != null)
            {
                string[] mystring = DB_list.SelectedItem.ToString().Split('\\');

                TextBox_Id_Staff.Text = mystring[0];

                TextBox_Staff.Text = mystring[1];

                TextBox_Company.Text = mystring[2];

                TextBox_Depts.Text = mystring[3];

                TextBox_Login.Text = mystring[4];

                TextBox_Password.Text = mystring[5];
            }
        }

        //Технические методы
        //Проверка правильности ввода отделов
        private static bool Check_Depts(string str)
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
        private static List<string> Return_max_Depts(string str)
        {
            List<string> new_string = new(str.Split(' '));
            new_string = new_string.Distinct().ToList();
            return new_string;
        }
    }

    public class DBdate
    {
        public int Id_User { get; set; }
        public string Name_Staff { get; set; }
        public string Company_Name { get; set; }
        public string Departament { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public override string ToString()
        {
            return $"{Id_User}\\{Name_Staff}\\{Company_Name}\\{Departament}\\{Login}\\{Password}";
        }
    }
}
