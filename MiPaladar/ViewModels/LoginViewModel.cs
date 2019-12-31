using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Services;
using MiPaladar.Entities;

using System.Windows.Input;
using System.ComponentModel;

namespace MiPaladar.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        Action<string> onLoginSuccess;

        public LoginViewModel(Action<string> onLoginSuccess) 
        {
            this.onLoginSuccess = onLoginSuccess;
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        #region Errors

        bool userNameRequired;
        public bool UserNameRequired 
        {
            get { return userNameRequired; }
            set
            {
                userNameRequired = value;
                OnPropertyChanged("UserNameRequired");
            }
        }

        bool wrongUserAndOrPassword;
        public bool WrongUserAndOrPassword 
        {
            get { return wrongUserAndOrPassword; }
            set
            {
                wrongUserAndOrPassword = value;
                OnPropertyChanged("WrongUserAndOrPassword");
            }
        }

        bool accessDenied;
        public bool AccessDenied 
        {
            get { return accessDenied; }
            set
            {
                accessDenied = value;
                OnPropertyChanged("AccessDenied");
            }
        }

        #endregion

        bool busy;
        public bool Busy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged("Busy");
            }
        }

        int progress;
        public int Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    OnPropertyChanged("Progress");
                }
            }
        }

        RelayCommand loginCommand;
        public ICommand LoginCommand 
        {
            get 
            {
                if (loginCommand == null)
                    loginCommand = new RelayCommand(x => this.Login(UserName, Password), x => this.CanLogin);
                return loginCommand;
            }
        }

        bool CanLogin { get { return !busy; } }                

        void Login(string username, string password)
        {
            Busy = true;
            CommandManager.InvalidateRequerySuggested();

            //GoToMainWindow(); return;

            UserNameRequired = false;
            WrongUserAndOrPassword = false;
            AccessDenied = false;

            //checklogin
            
            if (loginWorker == null)
            {
                loginWorker = new BackgroundWorker();

                loginWorker.DoWork += new DoWorkEventHandler(loginWorker_DoWork);
                loginWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loginWorker_RunWorkerCompleted);
            }

            loginWorker.RunWorkerAsync(new string[] { username, password });
            
        }

        //#region Loader Worker

        //BackgroundWorker loaderWorker;

        //void loaderWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    appvm.Load((BackgroundWorker)sender);
        //}

        //private void loaderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    Progress = e.ProgressPercentage;
        //}

        //void loaderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    appvm.LoginSuccessful(UserName);

        //    Busy = false;
        //}

        //#endregion        

        #region Login Worker

        BackgroundWorker loginWorker;
        void loginWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string user = ((string[])e.Argument)[0];
            string pwd = ((string[])e.Argument)[1];
            
            e.Result = CheckLogin(user, pwd);
        }

        void loginWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Busy = false;

            LoginResult result = (LoginResult)e.Result;

            switch (result)
            {
                case LoginResult.UserNameRequired:
                    UserNameRequired = true;
                    break;
                case LoginResult.WrongUserAndOrPassword:
                    WrongUserAndOrPassword = true;
                    break;
                case LoginResult.AccessDenied:
                    AccessDenied = true;
                    break;
                case LoginResult.Success:
                    if (onLoginSuccess != null) onLoginSuccess(UserName);
                    break;
                default:
                    break;
            }            
        }

        //void LoadData() 
        //{
        //    //success, load data
        //    if (appvm.FirstLogin)
        //    {
        //        if (loaderWorker == null)
        //        {
        //            loaderWorker = new BackgroundWorker();

        //            loaderWorker.DoWork += new DoWorkEventHandler(loaderWorker_DoWork);
        //            loaderWorker.WorkerReportsProgress = true;
        //            loaderWorker.ProgressChanged += new ProgressChangedEventHandler(loaderWorker_ProgressChanged);
        //            loaderWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loaderWorker_RunWorkerCompleted);
        //        }

        //        loaderWorker.RunWorkerAsync();
        //    }
        //    //go directly to app
        //    else
        //    {
        //        appvm.LoginSuccessful(UserName);
        //    }
        //}

        #endregion        

        LoginResult CheckLogin(string username, string password) 
        {
            var encrypter = base.GetService<IEncrypter>();

            //no username specified
            if (string.IsNullOrWhiteSpace(username))
            {
                return LoginResult.UserNameRequired;
            }

            Employee userEntered = FindUserFromName(username);

            //wrong username and/or password
            if (userEntered == null || !encrypter.CheckUserPassword(userEntered, password))
            {
                return LoginResult.WrongUserAndOrPassword;
            }

            if (!userEntered.Role.CanLogin)
            //if (!UserManager.UserHasAccessPermission(userEntered))
            {
                return LoginResult.AccessDenied;
            }

            return LoginResult.Success;
        }

        private Employee FindUserFromName(string username)
        {
            RestaurantDBEntities ctx = new RestaurantDBEntities();

            var queryResult = from user in ctx.Employees
                              where user.Name == username
                              select user;

            if (queryResult.Count() != 1) return null;

            return queryResult.First();
        }     
    }

    enum LoginResult { UserNameRequired, WrongUserAndOrPassword, AccessDenied, Success }
}
