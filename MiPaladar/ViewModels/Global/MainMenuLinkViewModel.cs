using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

using MiPaladar.Entities;
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class MainMenuLinkViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        //Func<Employee, bool> isVisibleForUser;

        public MainMenuLinkViewModel(MainWindowViewModel appvm, string displayName, ViewModelBase[] subMenus)
        {
            this.appvm = appvm;
            //this.isVisibleForUser = isVisibleForUser;

            DisplayName = displayName;

            subMenuLinks = new ReadOnlyCollection<ViewModelBase>(subMenus);

            selectedSubMenu = subMenus.FirstOrDefault();

            //if (subMenus != null && subMenus.Length > 0)
            //{

            //}
        }

        ReadOnlyCollection<ViewModelBase> subMenuLinks;
        public ReadOnlyCollection<ViewModelBase> SubMenuLinks 
        {
            get { return subMenuLinks; }
        }

        ViewModelBase selectedSubMenu;
        public ViewModelBase SelectedSubMenu
        {
            get { return selectedSubMenu; }
            set
            {
                selectedSubMenu = value;
                OnPropertyChanged("SelectedSubMenu");
            }
        }

        //public bool IsVisible
        //{
        //    get { return isVisibleForUser(appvm.LoggedInUser); }
        //}

        //public void UpdateVisibility()
        //{
        //    OnPropertyChanged("IsVisible");
        //}
    }
}
