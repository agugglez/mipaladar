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

using System.Windows.Media.Animation;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for VentasControl.xaml
    /// </summary>
    public partial class Ventas : UserControl
    {
        //enum MenuOption { MiniMenu, MiniInventory, TotalsWaiter};

        public Ventas()
        {
            InitializeComponent(); 

            //valeControl.Content = App.Ctx.Vales;
            //ccValeHeader.Content = App.Ctx.Vales;
            //ccValeTotal.Content = App.Ctx.Vales;
            //idBox.Content = accounter.Vales;
            //dgValeTotals.ItemsSource = App.Ctx.Vales;

            //CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(App.Ctx.Vales);

            //myCollectionView.SortDescriptions.Add(new System.ComponentModel.SortDescription("DateTimeCreated",System.ComponentModel.ListSortDirection.Ascending));
        }

        //private void Menu_Click(object sender, RoutedEventArgs e)
        //{
        //    HandleClickOn(MenuOption.MiniMenu);
        //}
        //private void Inventory_Click(object sender, RoutedEventArgs e)
        //{
        //    HandleClickOn(MenuOption.MiniInventory);
        //}

        //private void Totals_Click(object sender, RoutedEventArgs e)
        //{
        //    HandleClickOn(MenuOption.TotalsWaiter);
        //}


        //void HandleClickOn(MenuOption option) 
        //{
        //    if (!IsPaneExpanded())
        //    {
        //        HideAllButThis(option);

        //        ExpandPane();

        //        if (!IsOptionVisible(option)) ShowIt(option);
        //    }
        //    else 
        //    {
        //        if (IsOptionVisible(option)) ContractPane();

        //        else 
        //        {
        //            HideAllButThis(option);

        //            ShowIt(option);
        //        }
        //    }
        //}        

        //bool IsPaneExpanded() { return LeftPane.Width > 0;}

        //bool IsOptionVisible(MenuOption option)
        //{
        //    bool result;
        //    switch (option)
        //    {
        //        case MenuOption.MiniMenu:
        //            result = miniMenu.Visibility == Visibility.Visible && miniMenu.Opacity == 1;
        //            break;
        //        case MenuOption.MiniInventory:
        //            result = miniInventory.Visibility == Visibility.Visible && miniInventory.Opacity == 1;
        //            break;
        //        case MenuOption.TotalsWaiter:
        //            result = totalsWaiter.Visibility == Visibility.Visible && totalsWaiter.Opacity == 1;
        //            break;
        //        default:
        //            result = false;
        //            break;
        //    }

        //    return result;
        //}

        //void HideAllButThis(MenuOption option)
        //{
        //    if (option != MenuOption.MiniMenu) 
        //    {
        //        miniMenu.Opacity = 0;
        //        miniMenu.Visibility = Visibility.Collapsed;
        //    }
        //    if (option != MenuOption.MiniInventory)
        //    {
        //        miniInventory.Opacity = 0;
        //        miniInventory.Visibility = Visibility.Collapsed;
        //    }
        //    if (option != MenuOption.TotalsWaiter)
        //    {
        //        totalsWaiter.Opacity = 0;
        //        totalsWaiter.Visibility = Visibility.Collapsed;
        //    }
        //}

        //void ExpandPane()
        //{
        //    ((Storyboard)this.Resources["ExpandingStoryboard"]).Begin();
        //}
        //void ContractPane() 
        //{
        //    ((Storyboard)this.Resources["ContractingStoryboard"]).Begin();
        //}
        
        //void ShowIt(MenuOption option) 
        //{
        //    switch (option)
        //    {
        //        case MenuOption.MiniMenu:
        //            miniMenu.Visibility = Visibility.Visible;
        //            ((Storyboard)this.Resources["ShowMiniMenuStoryboard"]).Begin();
        //            break;
        //        case MenuOption.MiniInventory:
        //            miniInventory.Visibility = Visibility.Visible;
        //            ((Storyboard)this.Resources["ShowMiniInventoryStoryboard"]).Begin();
        //            break;
        //        case MenuOption.TotalsWaiter:
        //            totalsWaiter.Visibility = Visibility.Visible;
        //            ((Storyboard)this.Resources["ShowTotalsWaiterStoryboard"]).Begin();
        //            break;
        //        default:
        //            break;
        //    }
        //}        

        //private void NewVale_Click(object sender, RoutedEventArgs e)
        //{
        //    //create a new folder if didnt exist already
        //    //with today's date
        //    CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(App.Ctx.Vales);

        //    SalesOrder v = myCollectionView.CurrentItem as SalesOrder;

        //    //accounter.CreateNewVale();
        //    App.Ctx.CreateNewVale(v.PriceList);

        //    //OnPropertyChanged("LastVale");
        //}

        //private void close_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        //{
        //    if (MessageBox.Show("Está seguro que desea cerrar el vale?", "",
        //        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    {
        //        return;
        //    }

        //    e.Handled = true;
        //}

        //private void delete_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        //{
        //    if (MessageBox.Show("Esta seguro que desea eliminar el vale actual?", "",
        //        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    {
        //        AskPasswordView pc = new AskPasswordView();
        //        if (pc.ShowDialog() == true)
        //        {
        //            return;
        //        }
        //    }            

        //    e.Handled = true;
        //}

        //private void Button_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{

        //}

        //private void Button_PreviewCanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        //{

        //}

        //private void Print_Click(object sender, RoutedEventArgs e)
        //{
        //    //get current vale
        //    CollectionView myCollectionView =
        //        (CollectionView)CollectionViewSource.GetDefaultView(App.Ctx.Vales);

        //    SalesOrder currentVale = myCollectionView.CurrentItem as SalesOrder;

        //    Printer.PrintVale(currentVale);
        //}

        //private void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    string position = App.Personal.LoggedInUser.Position.Name;

        //    if (position == "Jefe Turno")
        //    {
        //        dpDate.IsEnabled = false;

        //        removeBtn.Visibility = Visibility.Hidden;
        //    }
        //}         
    }
}
