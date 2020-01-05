using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        bool creating;
        //Action<Category> onCreated;
        //Action<Category> onModified;
        //Action<Category> onRemoved;

        //creating a new one
        public CategoryViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
            //this.onCreated = onCategoryCreated;

            creating = true;

            HasPendingChanges = true;
        }

        public CategoryViewModel(MainWindowViewModel appvm, int catId, string name)
        {
            this.appvm = appvm;
            //this.onModified = onModified;
            //this.onRemoved = onRemoved;

            this.catId = catId;
            this.name = name;           
        }

        //public CategoryViewModel(MainWindowViewModel appvm, Category c) 
        //{
        //    this.appvm = appvm;
        //    //this.onModified = onModified;
        //    //this.onRemoved = onRemoved;
            
        //    category = c;
        //    this.id = c.Id;
        //    this.name = c.Name;
        //}

        public override string DisplayName
        {
            get
            {
                return creating ? "Nueva Categoría" : "Categorías: " + name;
            }
        }

        //the real category
        int catId;
        public int CategoryId { get { return catId; } }

        //Category category;
        //public Category WrappedCategory
        //{
        //    get { return category; }
        //}

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
                HasPendingChanges = true;
            }
        }

        CategoryRowViewModel selectedParentCategoryRow;
        public CategoryRowViewModel SelectedParentCategoryRow
        {
            get { return selectedParentCategoryRow; }
            set
            {
                selectedParentCategoryRow = value;
                OnPropertyChanged("SelectedParentCategoryRow");

                HasPendingChanges = true;
            }
        }

        ObservableCollection<CategoryRowViewModel> availableCats;
        public ObservableCollection<CategoryRowViewModel> AvailableCategories
        {
            get
            {
                if (availableCats == null)
                {
                    availableCats = new ObservableCollection<CategoryRowViewModel>();

                    using (var unitOfWork = base.GetNewUnitOfWork())
                    {
                        var inventorySvc = base.GetService<IInventoryService>();

                        inventorySvc.CreateCategoryList(unitOfWork, availableCats, NamingModes.SimpleName, x => x.Id != catId);

                        Category category = unitOfWork.CategoryRepository.GetById(catId);

                        if (!creating && category.ParentCategory_Id != null)
                        {
                            int source_id = category.ParentCategory_Id.Value;
                            SelectedParentCategoryRow = availableCats.Single(x => x.Id == source_id);
                        }
                    }                    
                }

                return availableCats;
            }
        }

        //bool relatedProductsFirstTime = true;

        //ObservableCollection<ProductIndex> relatedProductsOC;

        ////ICollectionView icvRelatedProducts;
        //public ObservableCollection<ProductIndex> RelatedProducts 
        //{
        //    get
        //    {
        //        if (relatedProductsOC == null)
        //        {
        //            relatedProductsOC = new ObservableCollection<ProductIndex>(category.RelatedProducts);

        //            ////sort the list
        //            //ObservableCollection<ProductIndex> temp = new ObservableCollection<ProductIndex>(category.RelatedProducts);

        //            //CollectionViewSource cvs = new CollectionViewSource();
        //            //cvs.Source = temp;
        //            //ICollectionView view = cvs.View;

        //            //view.SortDescriptions.Add(new SortDescription("Index", ListSortDirection.Ascending));

        //            //foreach (var item in view)
        //            //{
        //            //    relatedProductsOC.Add((ProductIndex)item);
        //            //}
        //        }
        //        return relatedProductsOC;
        //    }
        //}

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

        #region Save Command

        RelayCommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(x => Save(), x => this.CanSave);
                return saveCommand;
            }
        }

        //bool hasErrors;

        bool CanSave
        {
            get { return !string.IsNullOrWhiteSpace(name); }
        }

        void Save()
        {
            if (hasPendingChanges)
            {
                using (var unitOfWOrk = base.GetNewUnitOfWork())
                {
                    Category category;

                    if (creating)
                    {
                        category = new Category();

                        unitOfWOrk.CategoryRepository.Add(category);
                    }
                    else
                    {
                        category = unitOfWOrk.CategoryRepository.GetById(catId);
                    }

                    if (category.Name != name) category.Name = name;

                    if (ParentCategoryChanged(category))
                    {
                        category.ParentCategory_Id = selectedParentCategoryRow.Id;
                    }

                    unitOfWOrk.SaveChanges();

                    HasPendingChanges = false;

                    if (creating)
                    {
                        catId = category.Id;
                        appvm.GlobalEventsManager.FireCategoryCreated(catId);
                    }
                    else
                    {
                        appvm.GlobalEventsManager.FireCategoryModified(catId);
                    }
                }                
            }

            //CloseMe();
        }

        bool ParentCategoryChanged(Category category)
        {
            if (category.ParentCategory_Id == null && selectedParentCategoryRow == null) return false;

            if (category.ParentCategory_Id != null && selectedParentCategoryRow != null && category.ParentCategory.Id == selectedParentCategoryRow.Id) return false;

            return true;
        }

        //Category GetCategoryFromId(int cat_id)
        //{
        //    return appvm.CategoriesOC.Single(x => x.Id == cat_id);
        //}

        #endregion

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                    removeCommand = new RelayCommand(x => this.Remove());
                return removeCommand;
            }
        }

        bool CanRemove { get { return !creating; } }

        void Remove()
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = string.Format("¿Está seguro que desea eliminar la categoría '{0}'?", Name);

            if (msgBox.ShowYesNoDialog(message) == true)
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    Category category = unitOfWork.CategoryRepository.GetById(catId);

                    var children = category.ChildrenCategories.ToList();
                    //update children's parent to own parent
                    foreach (var item in children)
                    {
                        item.ParentCategory = category.ParentCategory;
                    }

                    unitOfWork.CategoryRepository.Remove(catId);

                    unitOfWork.SaveChanges();                    

                }

                CloseMe();

                appvm.GlobalEventsManager.FireCategoryRemoved(catId);
            }
        }

        private void CloseMe()
        {
            //close window
            var windowManager = base.GetService<IWindowManager>();

            windowManager.Close(this);
        }

        #endregion 


    }
}
