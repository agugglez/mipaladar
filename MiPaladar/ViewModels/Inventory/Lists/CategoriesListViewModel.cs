using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using MiPaladar.Entities;
using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class CategoriesListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        IConfirmator confirmator;

        //ObservableCollection<CategoryViewModel> categoryVMs;

        public CategoriesListViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
            this.confirmator = appvm.Confirmator;

            //categoryVMs = new ObservableCollection<CategoryViewModel>();

            //foreach (var item in appvm.CategoriesOC)
            //{
            //    CategoryViewModel cvm = new CategoryViewModel(appvm, item, confirmator);
            //    categoryVMs.Add(cvm);
            //}

            //base.RequestClose += new EventHandler(CategoriesViewModel_RequestClose);
        }

        //void CategoriesViewModel_RequestClose(object sender, EventArgs e)
        //{
        //    //foreach (var item in categoryVMs)
        //    //{
        //    //    item.RemoveEvents();
        //    //}
        //}

        public override string DisplayName
        {
            get { return "CATEGORIAS"; }
        }

        protected override void OnDispose()
        {
            foreach (var item in appvm.CategoriesOC)
            {
                item.PropertyChanged -= category_PropertyChanged;
            }
        }

        //string newCategoryName;
        //public string NewCategoryName
        //{
        //    get { return newCategoryName; }
        //    set
        //    {
        //        newCategoryName = value;
        //        OnPropertyChanged("NewCategoryName");
        //    }
        //}

        Category selectedCategory;
        public Category SelectedCategory 
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        //#region Expand Item Command

        //RelayCommand expandItemCommand;
        //public ICommand ExpandItemCommand
        //{
        //    get
        //    {
        //        if (expandItemCommand == null)
        //            expandItemCommand = new RelayCommand(x => this.ExpandItem());
        //        return expandItemCommand;
        //    }
        //}

        //void ExpandItem()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
        //    {
        //        if (!(wsvm is CategoryViewModel)) return false;

        //        CategoryViewModel svm = (CategoryViewModel)wsvm;

        //        return svm.WrappedCategory == SelectedCategory;
        //    };

        //    if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
        //    else
        //    {
        //        CategoryViewModel avm = new CategoryViewModel(appvm, SelectedCategory);

        //        windowManager.ShowChildWindow(avm, appvm);
        //    }
        //}

        //#endregion

        #region New Category Command

        RelayCommand newCategoryCommand;
        public ICommand NewCategoryCommand
        {
            get
            {
                if (newCategoryCommand == null)
                    newCategoryCommand = new RelayCommand(x => DoNewCategory());
                return newCategoryCommand;
            }
        }

        void DoNewCategory()
        {
            Category newCat = new Category();
            newCat.Name = "Nueva Categoría";
            
            appvm.CategoriesOC.Add(newCat);

            appvm.Context.Categories.AddObject(newCat);

            appvm.SaveChanges();

            newCat.PropertyChanged += category_PropertyChanged;

            SelectedCategory = newCat;

            //var windowManager = base.GetService<IWindowManager>();

            //CategoryViewModel cvm = new CategoryViewModel(appvm);

            //windowManager.Show(cvm);
        }

        #endregion

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                    removeCommand = new RelayCommand(x => this.Remove(SelectedCategory));
                return removeCommand;
            }
        }

        void Remove(Category categoryToRemove)
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = string.Format("¿Está seguro que desea eliminar la categoría '{0}'?", categoryToRemove.Name);

            if (msgBox.ShowYesNoDialog(message) == true)
            {
                categoryToRemove.PropertyChanged -= category_PropertyChanged;

                appvm.CategoriesOC.Remove(categoryToRemove);

                appvm.Context.Categories.DeleteObject(categoryToRemove);

                appvm.SaveChanges();
            }
        }

        #endregion 

        bool first_time = true;
        //ObservableCollection<CategoryViewModel> categories;
        public ObservableCollection<Category> Categories
        {
            get 
            {
                if (first_time) 
                {
                    first_time = false;

                    foreach (var item in appvm.CategoriesOC)
                    {
                        item.PropertyChanged += category_PropertyChanged;
                    }
                }
                
                return appvm.CategoriesOC; 
            }
        }

        void category_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name") 
            {
                appvm.SaveChanges();
            }
        }

    }
}