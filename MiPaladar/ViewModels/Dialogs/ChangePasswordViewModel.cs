using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;

using MiPaladar.ViewModels;
using MiPaladar.Services;
using MiPaladar.MVVM;
using MiPaladar.Entities;

namespace MiPaladar.ViewModels
{
    public class ChangePasswordViewModel :  ViewModelBase
    {
        MainWindowViewModel appvm;

        public ChangePasswordViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
        }

        public override string DisplayName
        {
            get { return "Cambiar Contraseña"; }
        }
                
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        bool wrongPassword;
        public bool WrongPassword 
        {
            get { return wrongPassword; }
            set
            {
                wrongPassword = value;
                OnPropertyChanged("WrongPassword");
            }
        }

        bool passwordMismatch;
        public bool PasswordMismatch 
        {
            get { return passwordMismatch; }
            set
            {
                passwordMismatch = value;
                OnPropertyChanged("PasswordMismatch");
            }
        }

        RelayCommand changePasswordCommand;
        public ICommand ChangePasswordCommand 
        {
            get 
            {
                if (changePasswordCommand == null)
                    changePasswordCommand = new RelayCommand(x => DoChangePassword());
                return changePasswordCommand;
            }
        }

        void DoChangePassword() 
        {
            WrongPassword = false; PasswordMismatch = false;

            var encrypter = base.GetService<IEncrypter>();

            if (!encrypter.CheckUserPassword(appvm.LoggedInUser, OldPassword))
                WrongPassword = true;

            else if (NewPassword != ConfirmNewPassword)
                PasswordMismatch = true;

            else
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    Employee targetEmployee = unitOfWork.EmployeeRepository.GetById(appvm.LoggedInUser.Id);

                    encrypter.ChangeUserPassword(targetEmployee, NewPassword);

                    unitOfWork.SaveChanges();
                    OnPasswordChanged();
                }                
            }
        }

        public event EventHandler PasswordChanged;

        protected void OnPasswordChanged()
        {
            EventHandler handler = this.PasswordChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
