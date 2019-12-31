using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.Windows.Input;
using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    //public class FaenasListViewModel : ViewModelBase
    //{
    //    MainWindowViewModel appvm;
    //    public FaenasListViewModel(MainWindowViewModel appvm)
    //    {
    //        this.appvm = appvm;
    //    }

    //    ObservableCollection<Faena> faenasOC;
    //    public IEnumerable<Faena> Faenas
    //    {
    //        get
    //        {
    //            if (faenasOC == null)
    //            {
    //                faenasOC = new ObservableCollection<Faena>(appvm.Context.Faenas);
    //            }
    //            return faenasOC;
    //        }
    //    }

    //    public Faena SelectedFaena { get; set; }

    //    #region New Faena Command

    //    RelayCommand newFaenaCommand;
    //    public ICommand NewFaenaCommand
    //    {
    //        get
    //        {
    //            if (newFaenaCommand == null)
    //                newFaenaCommand = new RelayCommand(x => DoNewFaena());
    //            return newFaenaCommand;
    //        }
    //    }

    //    void DoNewFaena()
    //    {
    //        var windowManager = base.GetService<IWindowManager>();

    //        FaenaViewModel fvm = new FaenaViewModel(appvm, OnCreated, OnRemoved, OnAssociationChanged);

    //        windowManager.Show(fvm);
    //    }

    //    void OnCreated(Faena newFaena)
    //    {
    //        faenasOC.Add(newFaena);
    //    }

    //    //some properties don't fire propertychanged events
    //    void OnAssociationChanged(Faena faena)
    //    {
    //        int index = faenasOC.IndexOf(faena);

    //        if (index >= 0)
    //        {
    //            faenasOC.RemoveAt(index);
    //            faenasOC.Insert(index, faena);
    //        }
    //    }

    //    #endregion

    //    RelayCommand expandItemCommand;
    //    public ICommand ExpandItemCommand
    //    {
    //        get
    //        {
    //            if (expandItemCommand == null)
    //                expandItemCommand = new RelayCommand(x => this.ExpandItem());
    //            return expandItemCommand;
    //        }
    //    }

    //    void ExpandItem()
    //    {
    //        var windowManager = base.GetService<IWindowManager>();

    //        Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
    //        {
    //            if (!(wsvm is FaenaViewModel)) return false;

    //            FaenaViewModel svm = (FaenaViewModel)wsvm;

    //            return svm.WrappedFaena == SelectedFaena;
    //        };

    //        if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
    //        else
    //        {
    //            FaenaViewModel avm = new FaenaViewModel(appvm, SelectedFaena, OnRemoved, OnAssociationChanged);

    //            windowManager.ShowChildWindow(avm, this);
    //        }
    //    }

    //    void OnRemoved(Faena oldFaena) 
    //    {
    //        faenasOC.Remove(oldFaena);
    //    }
    //}
}
