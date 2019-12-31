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
using MiPaladar.Entities;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class SoldProductsByEmployee : Window
    {

        public SoldProductsByEmployee()
        {
            InitializeComponent();

            DataContextChanged += SoldProductsByEmployee_DataContextChanged;
        }

        void SoldProductsByEmployee_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is SoldProductsByEmployeeViewModel)
            {
                //generate a column for each inventory area
                //dgItems.Columns.Clear();

                SoldProductsByEmployeeViewModel vm = (SoldProductsByEmployeeViewModel)e.NewValue;

                vm.EmployeesUpdated += vm_EmployeesUpdated;
                
            }
            if (e.OldValue is SoldProductsByEmployeeViewModel)
            {
                SoldProductsByEmployeeViewModel vm = (SoldProductsByEmployeeViewModel)e.NewValue;

                vm.EmployeesUpdated -= vm_EmployeesUpdated;
            }
        }

        void vm_EmployeesUpdated(object sender, EventArgs e)
        {
            SoldProductsByEmployeeViewModel vm = (SoldProductsByEmployeeViewModel)sender;

            //remove all columns but first
            List<DataGridColumn> toRemove = new List<DataGridColumn>();

            for (int i = 0 ; i < myDG.Columns.Count; i++)
            {
                DataGridColumn current = myDG.Columns[i];
                if ((string)current.Header != "Producto") toRemove.Add(current);
            }

            foreach (var item in toRemove)
            {
                myDG.Columns.Remove(item);
            }

            foreach (var invArea in vm.PresentEmployees)
            {
                AddColumn(invArea);
            }
        }

        void AddColumn(Employee emp)
        {
            DataGridTextColumn dgtc = new DataGridTextColumn();
            dgtc.MinWidth = 100;

            dgtc.Header = emp.Name;
            dgtc.SortMemberPath = string.Format("QuantityItems[{0}].Quantity", emp.Id);

            MultiBinding mb = new MultiBinding();
            mb.StringFormat = "{0:0.##} {1}";
            Binding b1 = new Binding(string.Format("QuantityItems[{0}].Quantity", emp.Id));
            Binding b2 = new Binding(string.Format("QuantityItems[{0}].UnitMeasure.Caption", emp.Id));
            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);

            dgtc.Binding = mb;

            myDG.Columns.Add(dgtc);
        }

    }
}
