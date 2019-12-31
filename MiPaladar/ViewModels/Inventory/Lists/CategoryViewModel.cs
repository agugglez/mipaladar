//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using System.ComponentModel;
//using System.Windows.Data;
//using System.Windows.Input;
//using System.Collections.ObjectModel;

//using MiPaladar.Entities;
//using MiPaladar.Services;

//namespace MiPaladar.ViewModels
//{
//    public class CategoryViewModel : ViewModelBase
//    {
//        //the real category
//        Category category;
//        IConfirmator confirmator;

//        MainWindowViewModel appvm;

//        bool creating;

//        //creating a new one
//        public CategoryViewModel(MainWindowViewModel appvm) 
//        {
//            this.appvm = appvm;
//            this.confirmator = appvm.Confirmator;

//            creating = true;

//            HasPendingChanges = true;
//        }

//        public CategoryViewModel(MainWindowViewModel appvm, Category c)
//        {
//            category = c;

//            this.appvm = appvm;
//            this.confirmator = appvm.Confirmator;

//            name = c.Name;
//        }

//        public override string DisplayName
//        {
//            get
//            {
//                return creating ? "Nueva Categoría" : "Categorías: " + name;
//            }
//        }

//        public Category WrappedCategory
//        {
//            get { return category; }
//        }

//        string name;
//        public string Name
//        {
//            get { return name; }
//            set 
//            {
//                name = value;
//                OnPropertyChanged("Name");
//                HasPendingChanges = true;
//            }
//        }

//        //bool relatedProductsFirstTime = true;

//        //ObservableCollection<ProductIndex> relatedProductsOC;

//        ////ICollectionView icvRelatedProducts;
//        //public ObservableCollection<ProductIndex> RelatedProducts 
//        //{
//        //    get
//        //    {
//        //        if (relatedProductsOC == null)
//        //        {
//        //            relatedProductsOC = new ObservableCollection<ProductIndex>(category.RelatedProducts);

//        //            ////sort the list
//        //            //ObservableCollection<ProductIndex> temp = new ObservableCollection<ProductIndex>(category.RelatedProducts);
                    
//        //            //CollectionViewSource cvs = new CollectionViewSource();
//        //            //cvs.Source = temp;
//        //            //ICollectionView view = cvs.View;

//        //            //view.SortDescriptions.Add(new SortDescription("Index", ListSortDirection.Ascending));

//        //            //foreach (var item in view)
//        //            //{
//        //            //    relatedProductsOC.Add((ProductIndex)item);
//        //            //}
//        //        }
//        //        return relatedProductsOC;
//        //    }
//        //}

//        bool hasPendingChanges;
//        public bool HasPendingChanges
//        {
//            get { return hasPendingChanges; }
//            set
//            {
//                hasPendingChanges = value;
//                OnPropertyChanged("HasPendingChanges");
//            }
//        }

             
//    }
//}
