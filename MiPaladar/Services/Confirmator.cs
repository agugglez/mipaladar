using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;

namespace MiPaladar.Services
{
    public interface IConfirmator 
    {
        bool AskForConfirmation(string message);
    }
    public class Confirmator : IConfirmator
    {
        public bool AskForConfirmation(string message)
        {
            //Window owner = Application.Current.MainWindow;
            var result = MessageBox.Show(message, "Confimación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) return true;
            
            return false;
        }
    }
}
