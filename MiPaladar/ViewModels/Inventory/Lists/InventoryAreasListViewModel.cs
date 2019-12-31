using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;

using System.Collections.ObjectModel;
using System.Windows.Input;
using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class InventoryAreasListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public InventoryAreasListViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
        }

        protected override void OnDispose()
        {
            if (first_time) return;

            foreach (var item in appvm.InventoryAreasOC)
            {
                item.PropertyChanged -= inventoryArea_PropertyChanged;
            }
        }

        public override string DisplayName
        {
            get { return "AREAS DE INVENTARIO"; }
        }

        bool first_time = true;
        public ObservableCollection<Inventory> InventoryAreas 
        {
            get
            {
                if (first_time) 
                {
                    first_time = false;

                    foreach (var item in appvm.InventoryAreasOC)
                    {
                        item.PropertyChanged += inventoryArea_PropertyChanged;
                    }
                }
                return appvm.InventoryAreasOC;
            }
        }

        void inventoryArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name") 
            {
                appvm.SaveChanges();
            }
            else if (e.PropertyName == "IsFloor")
            {
                Inventory inv_sender = (Inventory)sender;
                if (inv_sender.IsFloor)
                {
                    //mark all other areas with IsFloor=false
                    foreach (var item in appvm.InventoryAreasOC)
                    {
                        if (item != inv_sender) item.IsFloor = false;
                    }
                }
                appvm.SaveChanges();
            }
        }

        public Inventory SelectedInventoryArea { get; set; }

        #region New Inventory Area Command

        //bool creating;
        //public bool Creating
        //{
        //    get { return creating; }
        //    set
        //    {
        //        creating = value;
        //        OnPropertyChanged("Creating");
        //    }
        //}

        RelayCommand newCommand;
        public ICommand NewCommand
        {
            get 
            {
                if (newCommand == null)
                    newCommand = new RelayCommand(x => this.New());
                return newCommand; 
            }
        }

        //bool CanNew { get { return !creating && !editing; } }

        void New()
        {
            Inventory newIA = new Inventory();
            newIA.Name = "Nueva área de Inventario";

            appvm.Context.Inventories.AddObject(newIA);
            appvm.InventoryAreasOC.Add(newIA);

            appvm.SaveChanges();

            newIA.PropertyChanged += inventoryArea_PropertyChanged;
        }

        #endregion

        //#region Edit Command

        //public Inventory EditingInventoryArea { get; set; }

        //bool editing;
        //public bool Editing
        //{
        //    get { return editing; }
        //    set 
        //    {
        //        editing = value;
        //        OnPropertyChanged("Editing");
        //    }
        //}        

        //RelayCommand editCommand;
        //public ICommand EditCommand
        //{
        //    get
        //    {
        //        if (editCommand == null)
        //            editCommand = new RelayCommand(x => this.Edit(), x => this.CanEdit);
        //        return editCommand;
        //    }
        //}

        //bool CanEdit { get { return !creating && !editing && SelectedInventoryArea != null; } }

        //void Edit()         
        //{
        //    EditingInventoryArea = SelectedInventoryArea;
        //    NameValue = SelectedInventoryArea.Name;
        //    Editing = true;
        //}

        //#endregion

        //#region Ok Command

        //string nameValue;
        //public string NameValue
        //{
        //    get { return nameValue; }
        //    set 
        //    {
        //        nameValue = value;
        //        OnPropertyChanged("NameValue");
        //        CheckNameRepeated();
        //    }
        //}

        //bool nameRepeated;
        //void CheckNameRepeated() 
        //{
        //    if (string.IsNullOrWhiteSpace(nameValue)) return;

        //    foreach (var inventoryArea in InventoryAreas)
        //    {
        //        if (inventoryArea.Name == nameValue)
        //        {
        //            if (editing && inventoryArea == EditingInventoryArea) continue;
                    
        //            nameRepeated = true;
        //            CommandManager.InvalidateRequerySuggested();
        //            return;
        //        }
        //    }

        //    nameRepeated = false;
        //}

        //RelayCommand okCommand;
        //public ICommand OkCommand
        //{
        //    get
        //    {
        //        if (okCommand == null)
        //            okCommand = new RelayCommand(x => this.DoOk(), x => this.CanOk);
        //        return okCommand;
        //    }
        //}

        //bool CanOk 
        //{
        //    get { return !string.IsNullOrWhiteSpace(nameValue) && !nameRepeated; }
        //}

        //void DoOk() 
        //{
        //    if (creating)
        //    {
        //        Inventory inv = new Inventory();
        //        inv.Name = nameValue;

        //        appvm.Context.Inventories.AddObject(inv);
        //        appvm.InventoryAreasOC.Add(inv);

        //        Creating = false;
        //    }
        //    else if (editing) 
        //    {
        //        EditingInventoryArea.Name = nameValue;
        //        Editing = false;
        //    }

        //    appvm.SaveChanges();
        //}
        //#endregion

        //#region Cancel Command

        //RelayCommand cancelCommand;
        //public ICommand CancelCommand
        //{
        //    get
        //    {
        //        if (cancelCommand == null)
        //            cancelCommand = new RelayCommand(x => this.DoCancel());
        //        return cancelCommand;
        //    }
        //}

        //void DoCancel()
        //{
        //    Creating = false;
        //    Editing = false;
        //}

        //#endregion

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                    removeCommand = new RelayCommand(x => this.DoRemove(SelectedInventoryArea));
                return removeCommand;
            }
        }

        bool CanRemove { get { return SelectedInventoryArea != null; } }

        void DoRemove(Inventory invAreaToRemove)
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = "¿Está seguro que desea eliminar \"" + invAreaToRemove.Name + "\"?";

            if (msgBox.ShowYesNoDialog(message)==true) 
            {
                invAreaToRemove.PropertyChanged -= inventoryArea_PropertyChanged;

                //if (invAreaToRemove.IsFloor)
                //{
                //    msgBox.ShowMessage("El piso no se puede eliminar porque de ahí se rebajan las ventas");
                //    return;
                //}

                //remove inventory items from appvm
                foreach (var item in invAreaToRemove.InventoryItems)
                {
                    appvm.InventoryItemsOC.Remove(item);
                }

                var ts = base.GetService<ITransactionService>();

                //remove transfers related in the Inventory To association
                List<Transfer> transfersToRemove = new List<Transfer>(invAreaToRemove.TransfersTo);

                foreach (Transfer item in invAreaToRemove.TransfersFrom)
                {
                    if (!transfersToRemove.Contains(item)) transfersToRemove.Add(item);
                }

                foreach (Transfer item in transfersToRemove)
                {
                    ts.RemoveTransfer(item, false);
                }

                List<Production> productionsToRemove = new List<Production>(invAreaToRemove.Productions);

                foreach (var item in productionsToRemove)
                {
                    ts.RemoveProduction(item, false);
                }

                List<Adjustment> adjustmentsToRemove = new List<Adjustment>(invAreaToRemove.Adjustments);

                foreach (var item in adjustmentsToRemove)
                {
                    ts.RemoveAdjustment(item, false);
                }

                appvm.InventoryAreasOC.Remove(invAreaToRemove);

                appvm.Context.Inventories.DeleteObject(invAreaToRemove);

                appvm.SaveChanges();
            }
        }

        #endregion

        //#region MarkAsFloor Command

        //RelayCommand markAsFloorCommand;
        //public ICommand MarkAsFloorCommand 
        //{
        //    get 
        //    {
        //        if (markAsFloorCommand == null)
        //            markAsFloorCommand = new RelayCommand(x => this.MarkAsFloor(), x => this.CanMark);
        //        return markAsFloorCommand;
        //    }
        //}

        //bool CanMark { get { return SelectedInventoryArea != null && !SelectedInventoryArea.IsFloor; } }

        //void MarkAsFloor() 
        //{
        //    foreach (var item in InventoryAreas)
        //    {
        //        item.IsFloor = false;
        //    }

        //    SelectedInventoryArea.IsFloor = true;

        //    appvm.SaveChanges();
        //}

        //#endregion
    }
}
