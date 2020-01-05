using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using MiPaladar.ViewModels;
using MiPaladar.Entities;
using System.Windows.Media;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for ProductListView.xaml
    /// </summary>
    public partial class InventoryView : UserControl
    {
        public InventoryView()
        {
            InitializeComponent();

            //DataContextChanged += ProductListView_DataContextChanged;
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //double click
            if (e.ClickCount == 2)
            {
                TextBlock tb = (TextBlock)sender;

                var parentTreeView = FindParentTreeView(tb);

                if (parentTreeView.SelectedItem == null) return;

                InventoryViewModel ivm = (InventoryViewModel)DataContext;
                ivm.OpenProductCommand.Execute(parentTreeView.SelectedItem);

                e.Handled = true;
            }
        }

        TreeView FindParentTreeView(TextBlock tb)
        {
            dynamic fe = VisualTreeHelper.GetParent(tb);

            while (!(fe is TreeView))
            {
                fe = VisualTreeHelper.GetParent(fe);  
            }

            return fe as TreeView;
        }

        //void ProductListView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.NewValue is ProductsListViewModel)
        //    {
        //        //generate a column for each inventory area
        //        //dgItems.Columns.Clear();

        //        ProductsListViewModel plvm = (ProductsListViewModel)e.NewValue;
                
        //        foreach (var invArea in plvm.AppVM.InventoryAreasOC)
        //        {
        //            AddColumn(invArea);
        //        }
        //    }
        //}

        //void AddColumn(Inventory inventoryArea) 
        //{
        //    DataGridTextColumn dgtc = new DataGridTextColumn();
        //    dgtc.MinWidth = 100;

        //    dgtc.Header = inventoryArea.Name;
        //    dgtc.SortMemberPath = string.Format("InventoryItems[{0}].Quantity", inventoryArea.Id);

        //    MultiBinding mb = new MultiBinding();
        //    mb.StringFormat = "{0:0.##} {1}";
        //    Binding b1 = new Binding(string.Format("InventoryItems[{0}].Quantity", inventoryArea.Id));
        //    Binding b2 = new Binding(string.Format("InventoryItems[{0}].UnitMeasure.Caption", inventoryArea.Id));
        //    mb.Bindings.Add(b1);
        //    mb.Bindings.Add(b2);

        //    dgtc.Binding = mb;

        //    dgItems.Columns.Add(dgtc);
        //}

        //private void groupByCategory_Checked(object sender, RoutedEventArgs e)
        //{
        //    CollectionViewSource cvs = (CollectionViewSource)this.FindResource("cvsItems");

        //    if (cvs.GroupDescriptions.Count > 0)
        //    {
        //        cvs.GroupDescriptions.Clear();
        //    }
        //    else 
        //    {
        //        PropertyGroupDescription pgd = new PropertyGroupDescription("Category.Name");
        //        cvs.GroupDescriptions.Add(pgd);
        //    }
        //}
    }
}
