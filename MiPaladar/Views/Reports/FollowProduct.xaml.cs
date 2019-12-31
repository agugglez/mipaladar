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
using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    public partial class FollowProduct : Window
    {
        public FollowProduct()
        {
            InitializeComponent();
        }

        #region Mouse Busy

        #endregion

        bool justGotFocus;

        private void AutoCompleteBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (justGotFocus) { justGotFocus = false; return; }

            if (e.Key == Key.Return)
            {
                //Keyboard.Focus(btnAdd);
                btnAdd.Focus();
            }
        }

        private void DatePicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                FollowProductViewModel cpvm = (FollowProductViewModel)this.DataContext;

                if (cpvm.CanFind) cpvm.UpdateSearch();
            }
        }

        

    }
}
