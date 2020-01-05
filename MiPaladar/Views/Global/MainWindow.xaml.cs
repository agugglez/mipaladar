using System;
using System.Windows;
using System.Windows.Controls;

using MiPaladar.Views;
using MiPaladar.ViewModels;
using MiPaladar.Services;
using MiPaladar.MVVM;

using System.Windows.Input;

namespace MiPaladar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
            : base()
        {
            InitializeComponent();

            //LoadTab(new Ventas(), "Ventas");

            //LoadTab(new Cuadre(), "Cuadre");

            //nav_Frame.Navigate(new Uri("/Views/Login.xaml", UriKind.Relative));

            //SwitchVisibility();

            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
            //this.Closed += Window_Closed;
            //this.DataContextChanged += MainWindow_DataContextChanged;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var windowManager = ServiceContainer.GetService<IWindowManager>();
            windowManager.CloseAllWindowsButMain();
        }        

        //void viewmodel_ImportFinished(object sender, EventArgs e)
        //{
        //    GoToLoginWindow();
        //}

        //private void closeMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    Close();
        //}

        //private void Logout_Click(object sender, RoutedEventArgs e)
        //{
        //    GoToLoginWindow();
        //}

        //void GoToLoginWindow()
        //{
        //    MainWindowViewModel mwVM = (MainWindowViewModel)this.DataContext;

        //    Login login = new Login(mwVM);

        //    Close();

        //    login.Show();
        //}       

        //string backup_folder = @"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\Backup\";
        //string target_database = "RestaurantDB";
        //string temp_backup = "myTestBackUp.bak";
        //string backup_name = "RestaurantDB-Full Database Backup";

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

        //private void MenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    //App.Ctx.Connection.Close();
        //}
    }  
}