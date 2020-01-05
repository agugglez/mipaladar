using System;
using System.Windows.Input;

namespace MiPaladar.MVVM
{
    /// <summary>
    /// Represents an actionable item displayed by a View.
    /// </summary>
    public class ActiveCommandViewModel : ViewModelBase
    {
        public ActiveCommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            base.DisplayName = displayName;
            this.Command = command;
        }

        public ICommand Command { get; private set; }

        bool isActive;
        public bool IsActive 
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged("IsActive");
            }
        }
    }
}