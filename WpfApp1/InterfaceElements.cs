using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp
{
    public class InterfaceElements
    {
        public static void CreateTable(ListView DbList)
        {
            GridView myGridView = new();
            myGridView.AllowsColumnReorder = true;

            //Формирование столбцов таблицы ListView
            myGridView.Columns.Add(new()
            {
                DisplayMemberBinding = new Binding("IdUser"),
                Header = "User ID",
                Width = 50
            });

            myGridView.Columns.Add(new()
            {
                DisplayMemberBinding = new Binding("NameStaff"),
                Header = "Staff",
                Width = 75
            });

            myGridView.Columns.Add(new()
            {
                DisplayMemberBinding = new Binding("CompanyName"),
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

            //Отрисовка столбцов ListView
            DbList.View = myGridView;
        }
    }
}
