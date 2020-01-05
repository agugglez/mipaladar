using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Services;

using System.Windows;
using System.ComponentModel;

namespace MiPaladar.MVVM
{
    public abstract class ViewModelWithClose : ViewModelBase, IScreen
    {
        bool hasPendingChanges;
        public bool HasPendingChanges
        {
            get { return hasPendingChanges; }
            set
            {
                hasPendingChanges = value;
                OnPropertyChanged("HasPendingChanges");
            }
        }

        protected abstract bool CanSave { get; }
        protected abstract void Save();        
        
        protected bool selfClosing = false;

        #region Closing Event

        public void HandleClosing(object sender, CancelEventArgs e)
        {
            if (!IsSelfClosing()) e.Cancel = !TryToClose();
        }

        #endregion

        public bool IsSelfClosing()
        {
            return selfClosing;
        }

        public bool TryToClose()
        {
            if (hasPendingChanges && this.CanSave)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                if (msgBox != null)
                {
                    var result = msgBox.Show("¿Desea guardar los cambios antes de salir?",
                        "Guardar cambios",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Cancel)
                        return false;

                    if (result == MessageBoxResult.Yes)
                        this.Save();
                }
            }
            return true;
        }

        protected void CloseMe()
        {
            selfClosing = true;

            var windowManager = base.GetService<IWindowManager>();
            windowManager.Close(this);
        }
    }
}
