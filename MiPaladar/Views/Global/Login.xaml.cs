using System;
using System.Windows;
using System.Windows.Controls;

using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        //MainWindowViewModel appvm;        

        public Login()
        {           
            InitializeComponent();
        }

        private void PbxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginViewModel lvm = (LoginViewModel)this.DataContext;

            lvm.Password = PbxPassword.Password;
        }

        //private void BtnLogin_Click(object sender, RoutedEventArgs e)
        //{
        //    //GoToMainWindow(); return;

        //    string username = TxtUserName.Text.Trim();
        //    string passwd = PbxPassword.Password;

        //    IEncrypter encrypter = appvm.Encrypter;

        //    HideAllRedMessages();

        //    //no username specified
        //    if (string.IsNullOrWhiteSpace(username))
        //    {
        //        TxtbUserNameRequired.Visibility = Visibility.Visible;
        //        return;
        //    }

        //    Employee userEntered = FindUserFromName(username);

        //    //wrong username and/or password
        //    if (userEntered == null || !encrypter.CheckUserPassword(userEntered, passwd))
        //    {
        //        TxtbNotfound.Visibility = Visibility.Visible;
        //        return;
        //    }

        //    if (!userEntered.Permissions.CanLogin)
        //    //if (!UserManager.UserHasAccessPermission(userEntered))
        //    {
        //        TxtbCantAccess.Visibility = Visibility.Visible;
        //        return;
        //    }

        //    appvm.LoggedInUser = userEntered;
            
        //    //success
        //    GoToMainWindow();
            
        //    ////if (username == string.Empty)
        //    ////{
        //    ////    TxtbNotfound.Visibility = System.Windows.Visibility.Visible;
        //    ////    return;
        //    ////}
        //    ////else TxtbNotfound.Visibility = System.Windows.Visibility.Collapsed;

        //    ////if (passwd == String.Empty) 
        //    ////{
        //    ////    TxtbPasswordRequired.Visibility = System.Windows.Visibility.Visible; 
        //    ////    return;
        //    ////}
        //    ////else TxtbPasswordRequired.Visibility = System.Windows.Visibility.Collapsed; 

        //    //bool success = false;
        //    //Waiter employee = null;

        //    //try
        //    //{
        //    //    employee = App.Personal.All.Single<Waiter>(emp => emp.Name == username);

        //    //    if (employee.Password == null) success = passwd == string.Empty;
        //    //    else
        //    //    {
        //    //        string hash = employee.Password;

        //    //        success = Personnel.verifyMd5Hash(passwd, hash);
        //    //    }                
        //    //}
        //    //catch (InvalidOperationException)
        //    //{
        //    //    TxtbNotfound.Visibility = System.Windows.Visibility.Visible;
        //    //}

        //    //if (success) 
        //    //{
        //    //    TxtbNotfound.Visibility = System.Windows.Visibility.Collapsed;

        //    //    App.Personal.LoggedInUser = employee;

        //    //    //App.NavigateHome(employee); 
        //    //}
        //    //else TxtbNotfound.Visibility = System.Windows.Visibility.Visible;
        //}

        //private void HideAllRedMessages()
        //{
        //    TxtbUserNameRequired.Visibility = Visibility.Collapsed;
        //    TxtbNotfound.Visibility = Visibility.Collapsed;
        //    TxtbCantAccess.Visibility = Visibility.Collapsed;
        //}        

        //private Employee FindUserFromName(string username)
        //{
        //    var queryResult = from user in appvm.EmployeesOC
        //                      where user.Name == username
        //                      select user;

        //    if (queryResult.Count() != 1) return null;

        //    return queryResult.First();
        //}

        //void GoToMainWindow() 
        //{
        //    var windowManager = ServiceContainer.GetService<IWindowManager>();

        //    //MainWindow mw = new MainWindow();

        //    Hide();

        //    //appvm.NavigateToPointOfSale();
        //    //appvm.NavigateToVentas();

        //    windowManager.Show(appvm);            

        //    Close();            

        //    //mw.DataContext = appvm;

        //    //mw.Show();
        //}
    }
}
