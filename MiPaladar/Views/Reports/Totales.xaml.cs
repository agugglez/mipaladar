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

using MiPaladar.Classes;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for TotatelView.xaml
    /// </summary>
    public partial class Totales : Window
    {
        public Totales()
        {
            InitializeComponent();            

            //dpFrom.SelectedDate = DateTime.Today;
            //dpTo.SelectedDate = DateTime.Today;

            ////dgTotales.ItemsSource = App.Accounter.TotalsByProduct;

            //CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsByProduct);

            //myCollectionView.Filter = new Predicate<object>(FilterPredicate);

            //compras
            //CollectionView myComprasView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsCompras);

            //myComprasView.Filter = new Predicate<object>(FilterCompras);

            //myComprasView.GroupDescriptions.Clear();

            //if (myComprasView.CanGroup == true)
            //{
            //    PropertyGroupDescription groupDescription
            //        = new PropertyGroupDescription("Product.Category.Name");

            //    myComprasView.GroupDescriptions.Add(groupDescription);
            //}

            //
            //dgTotalesDeps.ItemsSource = App.Accounter.TotalsByDependiente;

            //init lista de precios
            //InitListasPrecios();

            //InitListasProductos();

            //cbTotalesPor.SelectedIndex = 0;
            //cbCentro.SelectedIndex = 0;
            //cbListas.SelectedIndex = 0;

            //App.Menu.ListasPrecios.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ListasPrecios_CollectionChanged);

            //App.Menu.ListasProductos.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ListasProductos_CollectionChanged);
        }

        //void ListasProductos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        //    {
        //        ListaProductos lp = (ListaProductos)e.NewItems[0];

        //        cbTotalesPor.Items.Add(lp.Name);
        //    }
        //    else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
        //    {
        //        ListaProductos lp = (ListaProductos)e.OldItems[0];

        //        cbTotalesPor.Items.Remove(lp.Name);
        //    }
        //}

        //void ListasPrecios_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) 
        //    {
        //        ListaPrecio lp = (ListaPrecio)e.NewItems[0];

        //        cbCentro.Items.Add(lp.Name);
        //    }
        //    else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) 
        //    {
        //        ListaPrecio lp = (ListaPrecio)e.OldItems[0];

        //        cbCentro.Items.Remove(lp.Name);
        //    }
        //}

        //public void InitListasPrecios() 
        //{
        //    cbCentro.Items.Add("Todos");

        //    foreach (ListaPrecio lp in App.Menu.ListasPrecios)
        //    {
        //        cbCentro.Items.Add(lp.Name);
        //    }
        //}

        //public void InitListasProductos()
        //{

        //    foreach (ListaProductos lp in App.Menu.ListasProductos)
        //    {
        //        cbTotalesPor.Items.Add(lp.Name);
        //    }
        //}

        //private void AddGrouping(object sender, RoutedEventArgs e)
        //{
        //    CollectionView myView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsByProduct);

        //    myView.GroupDescriptions.Clear();

        //    if (myView.CanGroup == true)
        //    {
        //        PropertyGroupDescription groupDescription
        //            = new PropertyGroupDescription("Product.MainCategory.Name");
        //        myView.GroupDescriptions.Add(groupDescription);
        //    }
        //    else
        //        return;
        //}
        //private void RemoveGrouping(object sender, RoutedEventArgs e)
        //{
        //    CollectionView myView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsByProduct);
        //    myView.GroupDescriptions.Clear();
        //}

        //private void cbCentro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ComboBoxItem cbi = (ComboBoxItem)e.AddedItems[0];

        //    string priceListaName = (string)cbi.Content;

        //    DateTime from = dpFrom.SelectedDate.Value;
        //    DateTime to = dpTo.SelectedDate.Value;

        //    if (priceListaName == "Todos")
        //    {
        //        App.Accounter.UpdateTotals(from, to, null);
        //    }
        //    else 
        //    {
        //        ListaPrecio lp = App.Menu.ListasPrecios.Single<ListaPrecio>(x => x.Name == priceListaName);

        //        App.Accounter.UpdateTotals(from, to, lp);
        //    }            
        //}

        //private void cbDimension_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //if (dgTotales == null) return;

        //    CollectionView myView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsByProduct);
                        
        //    //products
        //    if (cbTotalesPor.SelectedIndex == 0)
        //    {
        //        //myView.Filter = new Predicate<object>(FilterPredicate);

        //        //filterGrid.Visibility = Visibility.Visible;
        //        dgTotales.Visibility = Visibility.Visible;
        //        //dgTotalsIngs.Visibility = Visibility.Collapsed;
        //        dgTotalesDeps.Visibility = Visibility.Collapsed;
        //        dgTotalesMesas.Visibility = Visibility.Collapsed;
        //        dgTotalesCentros.Visibility = System.Windows.Visibility.Collapsed;

        //        toolbar.Visibility = System.Windows.Visibility.Visible;
                
        //    }
        //        //waiters
        //    else if (cbTotalesPor.SelectedIndex == 1)
        //    {
        //        //filterGrid.Visibility = Visibility.Collapsed;
        //        dgTotales.Visibility = Visibility.Collapsed;
        //        //dgTotalsIngs.Visibility = Visibility.Collapsed;
        //        dgTotalesDeps.Visibility = Visibility.Visible;
        //        dgTotalesMesas.Visibility = Visibility.Collapsed;
        //        dgTotalesCentros.Visibility = System.Windows.Visibility.Collapsed;

        //        //myView.Filter = null;

        //        toolbar.Visibility = System.Windows.Visibility.Collapsed;
        //    }
        //        //Mesas
        //    else if (cbTotalesPor.SelectedIndex == 2)
        //    {
        //        dgTotales.Visibility = Visibility.Collapsed;
        //        dgTotalesDeps.Visibility = Visibility.Collapsed;
        //        dgTotalesMesas.Visibility = System.Windows.Visibility.Visible;
        //        dgTotalesCentros.Visibility = System.Windows.Visibility.Collapsed;

        //        toolbar.Visibility = System.Windows.Visibility.Collapsed;
        //    }
        //        //Centros
        //    else if (cbTotalesPor.SelectedIndex == 3)
        //    {
        //        dgTotales.Visibility = Visibility.Collapsed;
        //        dgTotalesDeps.Visibility = Visibility.Collapsed;
        //        dgTotalesMesas.Visibility = System.Windows.Visibility.Collapsed;
        //        dgTotalesCentros.Visibility = System.Windows.Visibility.Visible;                    

        //        toolbar.Visibility = System.Windows.Visibility.Collapsed;
        //    }
        //}

        //private void tbBuscar_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsByProduct);

        //    myCollectionView.Refresh();
        //}

        //private void tbComprasBuscar_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsCompras);

        //    myCollectionView.Refresh();
        //}

        //public bool FilterPredicate(object b)
        //{
        //    TotalByProduct tbp = b as TotalByProduct;

        //    //
        //    bool cond1 = false;

        //    string prefix = tbBuscar.Text.Trim();

        //    if (tbp.Product.Name != null) 
        //        cond1 = tbp.Product.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0;

        //    //
        //    bool cond2 = false;

        //    if (rbTodos.IsChecked == true) cond2 = true;
        //    else if (rbProdsOnMenu.IsChecked == true) cond2 = !tbp.Product.NotInMenu;
        //    else if (rbIngredients.IsChecked == true) cond2 = tbp.Product.IsIngredient;            

        //    //
        //    bool cond3 = false;

        //    if (!ckbListas.IsChecked == true) cond3 = true;
        //    else
        //    {
        //        Category selectedLP = (Category)cbListas.SelectedItem;

        //        var queryREsult = from p in selectedLP.RelatedProducts
        //                          where p.Product.Id == tbp.Product.Id
        //                          select p;

        //        cond3 = queryREsult.Count() > 0;
        //    }                       

        //    return cond1 && cond2 && cond3;
        //}

        //public bool FilterCompras(object b)
        //{
        //    TotalByProduct tbp = b as TotalByProduct;

        //    //
        //    bool cond1 = false;

        //    string prefix = tbComprasBuscar.Text.Trim();

        //    if (tbp.Product.Name != null) cond1 = tbp.Product.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0;

        //    //
        //    bool cond2 = true;

        //    //if (rbTodos.IsChecked == true) cond2 = true;
        //    //else if (rbProdsOnMenu.IsChecked == true) cond2 = !tbp.Product.Hidden;
        //    //else if (rbIngredients.IsChecked == true) cond2 = tbp.Product.IsIngredient;

        //    //
        //    bool cond3 = true;

        //    //if (!ckbListas.IsChecked == true) cond3 = true;
        //    //else
        //    //{
        //    //    ListaProductos selectedLP = (ListaProductos)cbListas.SelectedItem;

        //    //    foreach (ProductItem pi in selectedLP.ProductItems)
        //    //    {
        //    //        if (pi.Product == tbp.Product) { cond3 = true; break; }
        //    //    }
        //    //}

        //    return cond1 && cond2 && cond3;
        //}

        //private void Update_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dpFrom.SelectedDate != null && dpTo.SelectedDate != null)
        //    {
        //        DateTime from = dpFrom.SelectedDate.Value;
        //        DateTime to = dpTo.SelectedDate.Value;

        //        //if (from == to)
        //        //    accounter.UpdateTotals(from);
        //        //else

        //        //bool calc_compras = rbContable.IsChecked == true;


        //        if (!ckbCentro.IsChecked == true)
        //            App.Accounter.UpdateTotals(from, to);
        //        else
        //            App.Accounter.UpdateTotals(from, to, (PriceList)cbCentro.SelectedItem);
        //    }
        //}

        //private void ckbCentro_Checked(object sender, RoutedEventArgs e)
        //{
        //    CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsByProduct);
        //    myCollectionView.Refresh();
        //}

        //private void ckbCentro_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsByProduct);
        //    myCollectionView.Refresh();
        //}

        //private void cbListas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsByProduct);
        //    myCollectionView.Refresh();
        //}

        //private void lbDimension_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    CollectionView myView = (CollectionView)CollectionViewSource.GetDefaultView(App.Accounter.TotalsByProduct);

        //    string dimension = ((ListBoxItem)lbDimension.SelectedItem).Content as string;

        //    if (dimension == "Producto")
        //    {
        //        myView.Filter = new Predicate<object>(FilterPredicate);

        //        //filterGrid.Visibility = Visibility.Visible;
        //        dgTotales.Visibility = Visibility.Visible;
        //        //dgTotalsIngs.Visibility = Visibility.Collapsed;
        //        dgTotalesDeps.Visibility = Visibility.Collapsed;
        //    }
        //    else if (dimension == "Dependiente") 
        //    {
        //        //filterGrid.Visibility = Visibility.Collapsed;
        //        dgTotales.Visibility = Visibility.Collapsed;
        //        //dgTotalsIngs.Visibility = Visibility.Collapsed;
        //        dgTotalesDeps.Visibility = Visibility.Visible;

        //        myView.Filter = null;
        //    }
        //}        
    }
}
