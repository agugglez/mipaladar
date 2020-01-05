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
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class CategoryListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        //ObservableCollection<CategoryViewModel> categoryVMs;

        //Action<int> onCategoryCreated;
        //Action<int> onCategoryRemoved;
        //Action<int> onCategoryModified;

        public CategoryListViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
        }

        public override string DisplayName
        {
            get { return "CATEGORIAS"; }
        }

        #region Global Events

        //protected override void OnDispose()
        //{
        //    UnLoadEvents();
        //}

        void SetUpEvents()
        {
            appvm.GlobalEventsManager.CategoryCreated += GlobalEventsManager_CategoryCreated;
            appvm.GlobalEventsManager.CategoryModified += GlobalEventsManager_CategoryModified;
            appvm.GlobalEventsManager.CategoryRemoved += GlobalEventsManager_CategoryRemoved;
        }

        private void UnLoadEvents()
        {
            appvm.GlobalEventsManager.CategoryCreated -= GlobalEventsManager_CategoryCreated;
            appvm.GlobalEventsManager.CategoryModified -= GlobalEventsManager_CategoryModified;
            appvm.GlobalEventsManager.CategoryRemoved -= GlobalEventsManager_CategoryRemoved;
        }

        void GlobalEventsManager_CategoryRemoved(object sender, CategoryInfoEventArgs e)
        {
            ReCreateList();
        }

        void GlobalEventsManager_CategoryModified(object sender, CategoryInfoEventArgs e)
        {
            ReCreateList();
        }

        void GlobalEventsManager_CategoryCreated(object sender, CategoryInfoEventArgs e)
        {
            ReCreateList();
        } 

        #endregion               

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

        CategoryRowViewModel selectedRowCategory;
        public CategoryRowViewModel SelectedCategory 
        {
            get { return selectedRowCategory; }
            set
            {
                selectedRowCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

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
            var windowManager = base.GetService<IWindowManager>();

            CategoryViewModel cvm = new CategoryViewModel(appvm);

            windowManager.ShowDialog(cvm, appvm);
        }

        //void OnCategoryCreated(Category newCategory)
        //{
        //    ReCreateList();

        //    //if (onCategoryCreated != null) onCategoryCreated(newCategory.Id);
        //}

        #endregion

        

        #region Expand Category Command

        RelayCommand expandCommand;
        public ICommand ExpandCommand
        {
            get
            {
                if (expandCommand == null)
                    expandCommand = new RelayCommand(x => ExpandCategory(selectedRowCategory), x => this.CanExpand);
                return expandCommand;
            }
        }

        bool CanExpand { get { return selectedRowCategory != null; } }

        void ExpandCategory(CategoryRowViewModel row)
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase vmb) =>
            {
                if (!(vmb is CategoryViewModel)) return false;

                CategoryViewModel cvm = (CategoryViewModel)vmb;

                return cvm.CategoryId == row.Id;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                string name = null;
                if (row.Name.Contains(":"))
                {
                    name = row.Name.Split(':').Last();
                }
                else { name = row.Name; }

                CategoryViewModel cvm = new CategoryViewModel(appvm, row.Id, name);

                windowManager.ShowDialog(cvm, appvm);                
            }
        }

        //void OnCategoryRemoved(Category cat) 
        //{
        //    ReCreateList();

        //    //if (onCategoryRemoved != null) onCategoryRemoved(cat.Id);
        //}
        //void OnCategoryModified(Category cat)
        //{
        //    ReCreateList();

        //    //if (onCategoryModified != null) onCategoryModified(cat.Id);
        //}
        
        void ReCreateList()
        {
            categories.Clear();

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                var inventorySvc = base.GetService<IInventoryService>();

                inventorySvc.CreateCategoryList(unitOfWork, categories, NamingModes.FullName);
            }
        }

        //Category GetCategoryFromId(int cat_id)
        //{
        //    return appvm.CategoriesOC.Single(x => x.Id == cat_id);
        //}

        #endregion

        ObservableCollection<CategoryRowViewModel> categories;
        public ObservableCollection<CategoryRowViewModel> Categories
        {
            get 
            {
                if (categories == null)
                {
                    categories = new ObservableCollection<CategoryRowViewModel>();

                    ReCreateList();

                    SetUpEvents();
                }
                
                return categories; 
            }
        }        

        //void category_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "Name") 
        //    {
        //        appvm.SaveChanges();
        //    }
        //}

    }
}