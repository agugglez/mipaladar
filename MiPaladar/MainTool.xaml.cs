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

using MiPaladar.Views;
using MiPaladar.resources;
using MiPaladar.ViewModels;

namespace MiPaladar
{
    /// <summary>
    /// Interaction logic for MainTool.xaml
    /// </summary>
    public partial class MainTool : UserControl
    {
        public MainTool()
        {
            InitializeComponent();

            LoadTab(new Ventas(), "Ventas");

            LoadTab(new Cuadre(), "Cuadre");

            //nav_Frame.Navigate(new Uri("/Views/Login.xaml", UriKind.Relative));

            //SwitchVisibility();
        }

        void LoadTab(UserControl uc, string title)
        {
            ClosableTabItem ti = new ClosableTabItem();
            ti.Title = title;
            ti.Content = uc;
            tcMain.Items.Add(ti);
            ti.Focus();
        }


        //private void add_ptoVenta_Click(object sender, RoutedEventArgs e)
        //{
        //    if (tbPtoVenta.Text.Trim() == String.Empty)
        //        MessageBox.Show("no ha escrito un nombre para el nuevo punto de venta");
        //    else
        //    {
        //        PuntoDeVenta pdv = new PuntoDeVenta();
        //        pdv.Name = tbPtoVenta.Text.Trim();
        //        App.PtsVenta.Add(pdv);
        //        tbPtoVenta.Clear();
        //        //save the changes
        //        //SerializeDependientes();
        //    }
        //}    

        private void Inicio_Print_Click(object sender, RoutedEventArgs e)
        {
            if (tcMain.SelectedContent is Ventas)
            {
                CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(App.Ctx.Vales);

                SalesOrder currentVale = myCollectionView.CurrentItem as SalesOrder;

                Printer.PrintVale(currentVale);
            }
            else if (tcMain.SelectedContent is Almacen)
            {
                Printer.PrintAlmacen();
            }
        }

        private void changePass_Click(object sender, RoutedEventArgs e)
        {
            //nav_Frame.Navigate(new Uri("/Views/ChangePassword.xaml", UriKind.Relative));
        }

        private void salir_Click(object sender, RoutedEventArgs e)
        {
            //mainMenu.Visibility = System.Windows.Visibility.Collapsed;

            //nav_Frame.Navigate(new Uri("/Views/Login.xaml", UriKind.Relative));

            //if (!all_visible) SwitchVisibility();
        }

        private void ventas_Click(object sender, RoutedEventArgs e)
        {
            LoadTab(new Ventas(), "Ventas");
            //nav_Frame.Navigate(new Ventas());
        }

        private void compras_Click(object sender, RoutedEventArgs e)
        {
            LoadTab(new Compras(), "Compras");
            //nav_Frame.Navigate(new ComprasPage());
        }

        private void totales_Click(object sender, RoutedEventArgs e)
        {
            LoadTab(new Totales(), "Totales");
            //nav_Frame.Navigate(new TotalesPage());
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            MenuUserControl muc = new MenuUserControl();
            MenuViewModel mvm = new MenuViewModel();
            muc.DataContext = mvm;
            LoadTab(muc, "Menú");
            //nav_Frame.Navigate(new MenuPage());
        }

        private void almacen_Click(object sender, RoutedEventArgs e)
        {
            LoadTab(new Almacen(), "Almacén");
            //nav_Frame.Navigate(new Almacen());
        }

        private void tarjetaEstiva_Click(object sender, RoutedEventArgs e)
        {
            LoadTab(new TarjetaEstiba(), "Tarjeta de Estiba");
            //nav_Frame.Navigate(new TarjetaEstiba());
        }

        private void categories_Click(object sender, RoutedEventArgs e)
        {
            Categories cats = new Categories();
            CategoriesViewModel cvm = new CategoriesViewModel();
            cats.DataContext = cvm;
            LoadTab(cats, "Categorías");
            //nav_Frame.Navigate(new CategoriesDialog());
        }

        private void personal_Click(object sender, RoutedEventArgs e)
        {
            PersonalViewModel pvm = new PersonalViewModel();
            Personal p = new Personal();
            p.DataContext = pvm;
            LoadTab(p, "Personal");
            //nav_Frame.Navigate(new Personal());
        }

        private void ptsVenta_Click(object sender, RoutedEventArgs e)
        {
            LoadTab(new PtsVenta(), "Centros");
            //nav_Frame.Navigate(new PtsVentaPage());
        }

        private void cuadre_click(object sender, RoutedEventArgs e)
        {
            LoadTab(new Cuadre(), "Cuadre");
            //nav_Frame.Navigate(new CuadrePage());
        }

        //public void NavigateHome(Waiter user)
        //{
        //    if (user.Position == Position.JefeTurno || user.Position == Position.Dependiente)
        //        SwitchVisibility();

        //    NavigateHome();
        //}

        public void NavigateHome()
        {
            //mainMenu.Visibility = System.Windows.Visibility.Visible;

            //nav_Frame.Navigate(new Uri("/Views/VentasControl.xaml", UriKind.Relative));
        }

        public void Navigate(Page page)
        {
            //nav_Frame.Navigate(page);
        }

        public void NavigateBack()
        {
            //nav_Frame.GoBack();
        }

        bool all_visible = true;

        private void SoloVentas_Click(object sender, RoutedEventArgs e)
        {
            SwitchVisibility();
        }

        void SwitchVisibility()
        {
            bool allowed = all_visible;

            if (!allowed)
            {
                PasswordControl pc = new PasswordControl();
                allowed = pc.ShowDialog() == true;
            }

            if (allowed)
            {

                //Hide Menus
                for (int i = 2; i < ira_MenuItem.Items.Count; i++)
                {
                    MenuItem mi = ira_MenuItem.Items[i] as MenuItem;

                    mi.Visibility = mi.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                }

                for (int i = 0; i < ver_MenuItem.Items.Count - 1; i++)
                {
                    MenuItem mi = ver_MenuItem.Items[i] as MenuItem;

                    mi.Visibility = mi.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                }

                App.AdminMode = !App.AdminMode;

                all_visible = !all_visible;
            }
        }

        string backup_folder = @"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\Backup\";
        string target_database = "RestaurantDB";
        string temp_backup = "myTestBackUp.bak";
        string backup_name = "RestaurantDB-Full Database Backup";

        //private void Exportar_Click(object sender, RoutedEventArgs e)
        //{
        //    Export(@"C:\Users\agostino\Desktop\thecopy.bak");
        //}

        //private void Importar_Click(object sender, RoutedEventArgs e)
        //{
        //    Import(@"C:\Users\agostino\Desktop\thecopy.bak");
        //}

        //void Export(string destination_file)
        //{
        //    RestaurantDBEntities context = App.Ctx;

        //    string commandText = "BACKUP DATABASE {0} TO  DISK = {1} WITH COPY_ONLY, NOFORMAT, NOINIT,  NAME = {2}, SKIP, NOREWIND, NOUNLOAD,  STATS = 10";

        //    context.ExecuteStoreCommand(commandText, target_database, temp_backup, backup_name);

        //    if (File.Exists(destination_file)) File.Delete(destination_file);

        //    File.Copy(backup_folder + temp_backup, destination_file);
        //}

        //void Import(string source_file)
        //{
        //    //file copying part
        //    string destination_file = backup_folder + temp_backup;

        //    if (File.Exists(destination_file)) File.Delete(destination_file);

        //    File.Copy(source_file, destination_file);

        //    //close current connection
        //    RestaurantDBEntities context = App.Ctx; ;

        //    //sql part
        //    SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
        //    sqlBuilder.DataSource = "(local)";
        //    sqlBuilder.InitialCatalog = "master";
        //    sqlBuilder.IntegratedSecurity = true;

        //    string conString = sqlBuilder.ToString();

        //    using (SqlConnection connection = new SqlConnection(conString))
        //    {
        //        connection.Open();

        //        //forcibly terminate all other connections
        //        string commandText = "USE master ";
        //        commandText += "ALTER DATABASE [RestaurantDB] ";
        //        commandText += "SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";

        //        SqlCommand singleUserCommand = new SqlCommand(commandText, connection);

        //        singleUserCommand.ExecuteNonQuery();

        //        //do restore
        //        commandText = "RESTORE DATABASE @database FROM  DISK = @file WITH REPLACE, NOUNLOAD,  STATS = 10" + Environment.NewLine;

        //        //set database back to multiuser
        //        commandText += "USE master" + Environment.NewLine;
        //        commandText += "ALTER DATABASE [RestaurantDB]" + Environment.NewLine;
        //        commandText += "SET MULTI_USER" + Environment.NewLine;

        //        SqlCommand command = new SqlCommand(commandText, connection);

        //        SqlParameter dbParam = new SqlParameter("database", target_database);
        //        command.Parameters.Add(dbParam);
        //        SqlParameter fileParam = new SqlParameter("file", temp_backup);
        //        command.Parameters.Add(fileParam);

        //        command.ExecuteNonQuery();
        //    }

        //    context.Connection.Open();

        //    //context.ExecuteStoreCommand("RESTORE DATABASE @database FROM DISK = @file",
        //    //    new SqlParameter { ParameterName = "database", Value = "RestaurantDB" },
        //    //    new SqlParameter { ParameterName = "file", Value = "myTestBackUp.bak" });

        //    //context.Connection.Open();
        //    //context.Start(DateTime.Today);
        //}

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //App.Ctx.Connection.Close();
        }
    }
}
