using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for PersonalPage.xaml
    /// </summary>
    public partial class Personal : UserControl
    {
        public Personal()
        {
            InitializeComponent();

            //dgDependientes.ItemsSource = App.Ctx.EmployeesOC;
        }

        //private void add_dependiente_Click(object sender, RoutedEventArgs e)
        //{
        //    if (tbDependiente.Text == String.Empty)
        //        MessageBox.Show("no ha escrito un nombre para el nuevo dependiente");
        //    else
        //    {
        //        Employee emp = new Employee();
        //        emp.Name = tbDependiente.Text.Trim();
        //        App.Ctx.Employees.AddObject(emp);
        //        App.Ctx.SaveChanges();

        //        //App.Personal.AddEmployee(tbDependiente.Text.Trim(), App.Personal.Positions[0], null);
                
        //        tbDependiente.Clear();
        //        //save the changes
        //        //SerializeDependientes();
        //    }
        //}
        
        //private void edit_dependiente_Click(object sender, RoutedEventArgs e)
        //{
        //    Waiter dep = dgDependientes.SelectedItem as Waiter;

        //    if (dep == null)
        //        MessageBox.Show("seleccione el dependiente que quiere eliminar");
        //    else
        //    {
        //        App.Navigate(new NewEmployee(dep));
        //    }
        //}

        //private void remove_dependiente_Click(object sender, RoutedEventArgs e)
        //{
        //    Waiter dep = dgDependientes.SelectedItem as Waiter;

        //    if (dep == null)
        //        MessageBox.Show("seleccione el dependiente que quiere eliminar");
        //    else
        //    {
        //        if (MessageBox.Show("Esta seguro?", null, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //        {
        //            App.Personal.RemoveEmployee(dep.ID);
        //        }
        //        //save the changes
        //        //SerializeDependientes();
        //    }
        //}

        //private void dgDependientes_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        //{
        //    if (e.Command == DataGrid.DeleteCommand) 
        //    {
        //        if (!(MessageBox.Show("Are you sure you want to delete?", "Please confirm.", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
        //        {
        //            // Cancel Delete.
        //            e.Handled = true;
        //        }
        //    }
        //}

        //private void remove_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!(MessageBox.Show("Está seguro?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
        //        //cancel
        //        e.Handled = true;
        //}
        
    }
}
