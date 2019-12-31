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
using System.Windows.Shapes;

using System.Collections.ObjectModel;
using System.Collections;
using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for ListDialog.xaml
    /// </summary>
    public partial class CategoriesList : Window
    {
        public CategoriesList()
        {
            InitializeComponent();
        }

        //private void add_category_Click(object sender, RoutedEventArgs e)
        //{
        //    if (tbCategory.Text == String.Empty)
        //        MessageBox.Show("No ha escrito un nombre para la nueva categoría");
        //    else
        //    {
        //        //check if there isnt a category with that name
        //        foreach (var item in App.Ctx.Categories)
        //        {
        //            if (item.Name == tbCategory.Text.Trim()) 
        //            {
        //                MessageBox.Show("Ya existe una categoría con ese nombre");
        //                return;
        //            }
        //        }

        //        Category c = new Category();
        //        c.Name = tbCategory.Text.Trim();

        //        App.Ctx.CategoriesOC.Add(c);

        //        tbCategory.Clear();
        //        //save the changes
        //        //SerializeDependientes();
        //    }
        //}

        //private void remove_category_Click(object sender, RoutedEventArgs e)
        //{
        //    Category cat = dgCategories.SelectedItem as Category;    

        //    if (cat == null)
        //        MessageBox.Show("seleccione la categoria que quiere eliminar");
        //    else
        //    {
        //        if (MessageBox.Show("Esta seguro que desea eliminar la categoria \"" + cat.Name + "\"?", "",
        //            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //        {
        //            if (cat.RelatedProducts.Count > 0)
        //                MessageBox.Show("La categoria no se puede eliminar porque contiene elementos");
        //            else
        //                App.Ctx.CategoriesOC.Remove(cat);
        //        }                
        //        //save the changes
        //        //SerializeDependientes();
        //    }
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
