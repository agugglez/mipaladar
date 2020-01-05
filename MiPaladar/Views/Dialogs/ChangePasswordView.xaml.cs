using System;
using System.Windows;
using System.Windows.Controls;

using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePasswordView : Window
    {
        public ChangePasswordView()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.OldPassword = oldPB.Password;
            viewmodel.NewPassword = firstPB.Password;
            viewmodel.ConfirmNewPassword = secondPB.Password;

            //string oldPass = oldPB.Password;

            //string passwd = firstPB.Password;

            //string confirmPass = secondPB.Password;

            //string storedPass = App.Personal.LoggedInUser.Password;

            //bool verify = storedPass == null ? oldPass == string.Empty : Personnel.verifyMd5Hash(oldPass, App.Personal.LoggedInUser.Password);

            //if (verify)
            //{
            //    if (passwd == confirmPass)
            //    {
            //        string newpass = Personnel.getMd5Hash(passwd);

            //        App.Personal.LoggedInUser.Password = newpass;

            //        App.NavigateBack();
            //    }
            //    else MessageBox.Show("Asegúrese de teclear bien las nuevas contraseñas");
            //}
            //else MessageBox.Show("La contraseña antigua está mal");

        }
        ChangePasswordViewModel viewmodel;
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewmodel = (ChangePasswordViewModel)e.NewValue;

            viewmodel.PasswordChanged += new EventHandler(viewmodel_PasswordChanged);
        }

        void viewmodel_PasswordChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Contraseña cambiada con éxito!");

            oldPB.Password = "";
            firstPB.Password = "";
            secondPB.Password = "";
        }
    }
}
