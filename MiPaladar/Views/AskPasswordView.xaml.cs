using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using System.IO;
using System.Security.Cryptography;

using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for Password.xaml
    /// </summary>
    public partial class AskPasswordView : Window
    {
        //AskPasswordViewModel viewmodel;

        public AskPasswordView()
        {
            InitializeComponent();
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (passPB.Password == "15091937")
        //        DialogResult = true;
        //    else 
        //    {
        //        MessageBox.Show("Wrong Password");
        //        passPB.SelectAll();
        //    }

        //    //Close();
        //}

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Keyboard.Focus(passPB);
        //}

        //private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    viewmodel = (AskPasswordViewModel)e.NewValue;

        //    viewmodel.PasswordAccepted += new EventHandler(viewmodel_PasswordAccepted);
        //    viewmodel.PasswordWrong += new EventHandler(viewmodel_PasswordWrong);

        //}

        //void viewmodel_PasswordAccepted(object sender, EventArgs e)
        //{
        //    DialogResult = true;
        //}
        //void viewmodel_PasswordWrong(object sender, EventArgs e)
        //{
        //    passPB.SelectAll();
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    viewmodel.Password = passPB.Password;
        //}        

    }
}
