using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.Classes;

namespace MiPaladar.ViewModels
{
    //public class FaenaItemViewModel : ProductQuantityViewModel
    //{
    //    FaenaViewModel parentFaena;

    //    public FaenaItemViewModel(MainWindowViewModel appvm, FaenaViewModel parentFaena, Product prod, float qtty, UnitMeasure um)
    //        : base(appvm, prod, qtty, um)
    //    {
    //        this.parentFaena = parentFaena;

    //        //base.WhiteSpaceError = true;
    //    }

    //    public FaenaItemViewModel(MainWindowViewModel appvm, FaenaViewModel parentFaena, FaenaItem faenaItem)
    //        : base(appvm, faenaItem.Product, faenaItem.Quantity, faenaItem.UnitMeasure)
    //    {
    //        this.parentFaena = parentFaena;
    //        //base.WhiteSpaceError = true;
    //    }

    //    protected override void OnQuantityChanged()
    //    {
    //        parentFaena.HasPendingChanges = true;
    //    }

    //    protected override void OnUnitMeasureChanged()
    //    {
    //        parentFaena.HasPendingChanges = true;
    //    }
    //}
}
