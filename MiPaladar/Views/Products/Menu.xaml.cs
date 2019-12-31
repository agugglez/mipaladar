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

using MiPaladar.Entities;
using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for MenuControl.xaml
    /// </summary>
    public partial class MenuUserControl : UserControl
    {
        public MenuUserControl()
        {
            InitializeComponent();
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    List<Product> toRemove = new List<Product>();

        //    foreach (Product item in dgMenu.SelectedItems)
        //    {
        //        toRemove.Add(item);
        //    }

        //    var menuViewModel = (MenuViewModel)this.DataContext;

        //    foreach (var item in toRemove)
        //    {
        //        menuViewModel.RemoveProduct(item);
        //    }
        //}

        //private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        productsCV.Refresh();
        //    }
        //    catch (Exception)
        //    {
                
        //    }            
        //}

        //private void tbMenuBuscar_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(dgMenu.ItemsSource);

        //    myCollectionView.Filter = new Predicate<object>(MenuFilterPredicate);
        //}                

        //private void CheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    productsCV.Refresh();
        //}

        //private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    productsCV.Refresh();
        //}

        //private void DataGrid_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        //{
        //    if (e.Command == DataGrid.DeleteCommand)
        //    {
        //        if (!(MessageBox.Show("Are you sure you want to delete?",
        //            "Please confirm.",
        //            MessageBoxButton.YesNo) == MessageBoxResult.Yes))
        //        {
        //            // Cancel Delete.
        //            e.Handled = true;
        //        }
        //    } 
        //}
    }
}