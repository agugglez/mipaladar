using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;

using MiPaladar.ViewModels;
using MiPaladar.Views;

namespace MiPaladar.Services
{
    public interface IWindowManager
    {
        //void ShowProducts(MenuViewModel menuVM);
        //void ShowCategories(CategoriesViewModel catsVM);
        //void ShowStaff(PersonalViewModel pVM);

        //void ShowCuadre(CuadreViewModel cuadreVM);
        //void ActivateCuadre();
        //void ShowPurchases(ComprasViewModel cVM);
        //void ActivatePurchases();
        //void ShowTotals(TotalesViewModel tVM);
        //void ActivateTotals();
        //void ShowOrders(OrdersViewModel ordersVM);
        //void ActivateOrders();
        //void ShowInventory(AlmacenViewModel almacenVM);
        //void ActivateInventory();

        //void ShowOperations(OperationsViewModel operationsVM);
        //void ActivateOperations();        

        void Associate<ViewModel, View>() 
            where ViewModel : ViewModelBase 
            where View : Window;

        //void Associate<ViewModel, View>(double height, double width)
        //    where ViewModel : ViewModelBase
        //    where View : Window;

        /// <summary>
        /// Returns true if there exists a view holding a viewmodel of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool Exists<T>() where T : ViewModelBase;

        /// <summary>
        /// Returns true if there exists a view holding viewmodel 
        /// </summary>
        /// <param name="viewmodel"></param>
        bool Exists(ViewModelBase viewmodel);

        /// <summary>
        /// Returns true if there exists a viewmodel that satisfies condition
        /// </summary>
        /// <param name="condition"></param>
        bool Exists(Predicate<ViewModelBase> condition);

        /// <summary>
        /// Maximizes the first window holding a viewmodel of type T and brings it to the foreground
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Activate<T>() where T : ViewModelBase;

        /// <summary>
        /// Maximizes the window holding viewmodel and brings it to the foreground
        /// </summary>
        /// <param name="viewmodel"></param>
        void Activate(ViewModelBase viewmodel);

        /// <summary>
        /// Maximizes the window holding the viewmodel satistying condition and brings it to the foreground
        /// </summary>
        /// <param name="condition"></param>
        void Activate(Predicate<ViewModelBase> condition);

        void Show(ViewModelBase viewmodel);

        /// <summary>
        /// Shows a window owned by another. The child window will close if the owner closes
        /// </summary>
        /// <param name="viewmodel">child window viewmodel</param>
        /// <param name="ownerViewModel">owner window viewmodel</param>
        void ShowChildWindow(ViewModelBase viewmodel, ViewModelBase ownerViewModel);

        /// <summary>
        /// Shows a dialog, centers the new dialog to its owner
        /// </summary>
        /// <param name="dialogViewModel"></param>
        /// <param name="ownerViewModel"></param>
        /// <returns></returns>
        bool? ShowDialog(ViewModelBase dialogViewModel, ViewModelBase ownerViewModel);

        /// <summary>
        /// Close this viewmodel's view
        /// </summary>
        /// <param name="viewmodel"></param>
        void Close(ViewModelBase viewmodel);

        void Close(Predicate<ViewModelBase> condition);

        /// <summary>
        /// Closes all windows except MainWindow
        /// </summary>
        void CloseAllWindowsButMain();
    }

    //class ViewSettings 
    //{
    //    public double Height;
    //    public double Width;
    //    public Type ViewType;
    //}

    public class WindowManager : IWindowManager
    {
        //List<Window> windows = new List<Window>();
        Dictionary<ViewModelBase, Window> windows = new Dictionary<ViewModelBase, Window>();
        Dictionary<Type, Type> associatedViews = new Dictionary<Type, Type>();
        Dictionary<ViewModelBase, ViewModelBase> parents = new Dictionary<ViewModelBase, ViewModelBase>();

        int defaultheight = 600;
        int defaultwidth = 800;

        #region Associate Methods

        public void Associate<ViewModel, View>()
            where ViewModel : ViewModelBase
            where View : Window
        {
            //ViewSettings vs = new ViewSettings();
            //vs.Height = defaultheight;
            //vs.Width = defaultwidth;
            //vs.ViewType = typeof(View);
            associatedViews.Add(typeof(ViewModel), typeof(View));
        }

        //public void Associate<ViewModel, View>(double height, double width)
        //    where ViewModel : ViewModelBase
        //    where View : Window
        //{
        //    ViewSettings vs = new ViewSettings();
        //    vs.Height = height;
        //    vs.Width = width;
        //    vs.ViewType = typeof(View);
        //    associatedViews.Add(typeof(ViewModel), vs);
        //}

        #endregion
        
        #region Exists Methods

        public bool Exists<T>() where T : ViewModelBase
        {
            foreach (var item in windows)
            {
                if (item.Key.GetType() == typeof(T)) return true;
            }
            return false;
            //return windows.ContainsKey(typeof(T));
        }
        public bool Exists(ViewModelBase viewmodel)
        {
            foreach (var item in windows)
            {
                if (item.Key == viewmodel) return true;
            }
            return false;
        }
        public bool Exists(Predicate<ViewModelBase> condition) 
        {
            foreach (var item in windows)
            {
                if (condition(item.Key)) return true;
            }
            return false;
        }


        #endregion

        #region Activate Methods

        public void Activate<T>() where T : ViewModelBase
        {
            Window targetWindow = windows.First(x => x.Key.GetType() == typeof(T)).Value;

            if (targetWindow.WindowState == WindowState.Minimized) targetWindow.WindowState = WindowState.Normal;

            targetWindow.Activate();
        }

        public void Activate(ViewModelBase wsvm)
        {
            Window targetWindow = windows[wsvm];

            if (targetWindow.WindowState == WindowState.Minimized) targetWindow.WindowState = WindowState.Normal;

            targetWindow.Activate();
        }

        public void Activate(Predicate<ViewModelBase> condition)
        {
            foreach (var item in windows)
            {
                if (condition(item.Key)) 
                {
                    Window targetWindow = item.Value;

                    if (targetWindow.WindowState == WindowState.Minimized) targetWindow.WindowState = WindowState.Normal;

                    targetWindow.Activate();
                }
            }
        }

        #endregion        

        #region Show Methods

        public void Show(ViewModelBase viewmodel)
        {
            //ViewSettings vs = associatedViews[viewmodel.GetType()];
            Type targetType = associatedViews[viewmodel.GetType()];

            //create window
            Window window = (Window)Activator.CreateInstance(targetType);

            window.DataContext = viewmodel;

            //window.Title = viewmodel.DisplayName;
            if (Double.IsNaN(window.Height)) 
            {
                window.Height = defaultheight;
                window.Width = defaultwidth;
            }            

            window.Closed += new EventHandler(newWindow_Closed);

            //add to dictionary
            windows[viewmodel] = window;

            //show it
            window.Show();
        }

        public void ShowChildWindow(ViewModelBase viewmodel, ViewModelBase ownerViewModel) 
        {
            //ViewSettings vs = associatedViews[viewmodel.GetType()];
            Type targetType = associatedViews[viewmodel.GetType()];

            //create window
            Window window = (Window)Activator.CreateInstance(targetType);

            window.DataContext = viewmodel;

            //window.Title = viewmodel.DisplayName;
            if (Double.IsNaN(window.Height) )
            {
                window.Height = defaultheight;
                window.Width = defaultwidth;
            }

            if (ownerViewModel != null) 
            {
                //Window ownerWindow = windows[ownerViewModel];

                parents[viewmodel] = ownerViewModel;
            }

            window.Closed += new EventHandler(newWindow_Closed);

            //add to dictionary
            windows[viewmodel] = window;

            //show it
            window.Show();
        }

        public bool? ShowDialog(ViewModelBase dialogViewModel, ViewModelBase ownerViewModel)
        {
            //ViewSettings vs = associatedViews[dialogViewModel.GetType()];
            Type targetType = associatedViews[dialogViewModel.GetType()];

            //create window
            Window dialogWindow = (Window)Activator.CreateInstance(targetType);

            dialogWindow.DataContext = dialogViewModel;

            //dialogWindow.Title = dialogViewModel.DisplayName;
            //dialogWindow.Height = vs.Height;
            //dialogWindow.Width = vs.Width;

            if (ownerViewModel != null) 
            {
                Window ownerWindow = windows[ownerViewModel];

                dialogWindow.Owner = ownerWindow;

                dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            dialogWindow.Closed += new EventHandler(newWindow_Closed);

            //add to dictionary
            windows[dialogViewModel] = dialogWindow;

            //show it
            return dialogWindow.ShowDialog();
        }

        #endregion

        #region Close Methods

        public void Close(ViewModelBase viewmodel) 
        {
            windows[viewmodel].Close();
        }

        public void Close(Predicate<ViewModelBase> condition) 
        {
            Window window_to_close = null;
            foreach (var item in windows)
            {
                if (condition(item.Key))
                {
                    window_to_close = item.Value;
                    break;
                }
            }

            if (window_to_close != null) window_to_close.Close();
        }                

        public void CloseAllWindowsButMain()
        {
            List<Window> windowsToClose = new List<Window>();

            foreach (var item in windows)
            {
                Window currentWindow = item.Value;

                if (!(currentWindow is MainWindow)) 
                {
                    //close event will remove it from the list
                    windowsToClose.Add(currentWindow);
                }
            }

            foreach (var item in windowsToClose)
            {
                item.Close();
            }
            //while (windows.Count > 0) 
            //{
            //    var first_window = windows.First();

            //    if (first_window.Value != null)
            //    {
            //        first_window.Value.Close();
            //        //windows.Remove(first_window.Key);
            //    }
            //}           
        }

        private void newWindow_Closed(object sender, EventArgs e)
        {
            Window w = (Window)sender;

            ViewModelBase viewmodel = (ViewModelBase)w.DataContext;

            viewmodel.Dispose();
            //viewmodel.CloseCommand.Execute(null);

            windows.Remove(viewmodel);

            //close window's children if it has any
            CloseChildren(viewmodel);

            //remove parent entry if it has
            parents.Remove(viewmodel);            
        }

        private void CloseChildren(ViewModelBase viewmodel)
        {
            //if viewmodel appears as parent of someone
            while (parents.ContainsValue(viewmodel))
            {
                var item = parents.First(x => x.Value == viewmodel);

                windows[item.Key].Close();
            }
                
            //List<ViewModelBase> children = new List<ViewModelBase>();
            //for (int i = 0; i < parents.Count; i++)
            //{
            //    var currentItem = parents.ElementAt(i);
            //    if (currentItem.Value == viewmodel) children.Add(currentItem.Key);
            //}
            //child viewmodels
            //var children = parents.Where(x => x.Value == viewmodel).Select(x => x.Key);

            //foreach (var item in children)
            //{
            //    windows[item].Close();
            //}            
        }

        #endregion        
    }
}
