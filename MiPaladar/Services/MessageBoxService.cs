using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;

namespace MiPaladar.Services
{
    public interface IMessageBoxService
    {
        void ShowMessage(string message);

        bool? ShowYesNoDialog(string message);

        MessageBoxResult Show(string text, string caption, MessageBoxButton buttons, MessageBoxImage image);
    }

    public class MessageBoxService : IMessageBoxService
    {
        public void ShowMessage(string message) 
        {
            MessageBox.Show(message);
        }

        public bool? ShowYesNoDialog(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes || result == MessageBoxResult.OK) return true;
            if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel) return false;

            return null;
        }

        MessageBoxResult IMessageBoxService.Show(string text, string caption, MessageBoxButton buttons, MessageBoxImage image)
        {
            return MessageBox.Show(text, caption, buttons, image);
        }
    }
}
