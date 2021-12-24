using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp
{
    public class Interface_elements
    {
        public static void Create_table(ListView DB_list)
        {
            GridView myGridView = new();
            myGridView.AllowsColumnReorder = true;

            //Формирование столбцов таблицы ListView
            myGridView.Columns.Add(new()
            {
                DisplayMemberBinding = new Binding("Id_User"),
                Header = "User ID",
                Width = 50
            });

            myGridView.Columns.Add(new()
            {
                DisplayMemberBinding = new Binding("Name_Staff"),
                Header = "Staff",
                Width = 75
            });

            myGridView.Columns.Add(new()
            {
                DisplayMemberBinding = new Binding("Company_Name"),
                Header = "Company",
                Width = 75
            });
            myGridView.Columns.Add(new()
            {
                DisplayMemberBinding = new Binding("Departament"),
                Header = "Dept",
                Width = 75
            });
            myGridView.Columns.Add(new()
            {
                DisplayMemberBinding = new Binding("Login"),
                Header = "Login",
                Width = 75
            });
            myGridView.Columns.Add(new()
            {
                DisplayMemberBinding = new Binding("Password"),
                Header = "Password",
                Width = 150
            });

            //Вывод сформированно таблицы в ListView
            DB_list.View = myGridView;
        }
    }
}
