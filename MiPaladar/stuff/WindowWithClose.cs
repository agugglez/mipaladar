using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

using MiPaladar.MVVM;

namespace MiPaladar.Stuff
{
    public class WindowWithClose : Window
    {
        public WindowWithClose()
        {
            this.DataContextChanged += Window_DataContextChanged;
        }

        bool firstTime = true;
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (firstTime)
            {
                ViewModelWithClose vm = (ViewModelWithClose)e.NewValue;

                this.Closing += vm.HandleClosing;

                firstTime = false;
            }

        }
    }
}
