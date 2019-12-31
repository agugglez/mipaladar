using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.Classes;

namespace MiPaladar.ViewModels
{
    public class TransferItemViewModel : ProductQuantityViewModel
    {
        //MainWindowViewModel appvm;
        //TransferItem transferItem;
        TransferViewModel parentTransfer;

        //public TransferItemViewModel(MainWindowViewModel appvm, TransferViewModel parentTransfer) : base(appvm) 
        //{
        //    this.parentTransfer = parentTransfer;
        //}

        public TransferItemViewModel(TransferViewModel parentTransfer, TransferItem transferItem)
            : base(transferItem.Product, transferItem.Quantity, transferItem.UnitMeasure)
        {
            //this.appvm = appvm;
            //this.transferItem = transferItem;
            this.parentTransfer = parentTransfer;

            //base.WhiteSpaceError = true;
        }

        public TransferItemViewModel(TransferViewModel parentTransfer, Product prod, double qtty, UnitMeasure um)
            : base(prod, qtty, um)
        {
            //this.appvm = appvm;
            //this.transferItem = transferItem;
            this.parentTransfer = parentTransfer;

            //base.WhiteSpaceError = true;
        }

        protected override void OnQuantityChanged()
        {
            parentTransfer.HasPendingChanges = true;
        }

        protected override void OnUnitMeasureChanged()
        {
            parentTransfer.HasPendingChanges = true;
        }

        //public TransferItem TransferItem { get { return transferItem; } }
    }
}
