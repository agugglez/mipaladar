//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.Entities;
//using MiPaladar.MVVM;

//namespace MiPaladar.ViewModels
//{
//    public class ProductQuantityViewModel : ViewModelBase
//    {
//        //PatternMatcher patternMatcher;

//        public ProductQuantityViewModel() { }

//        //public ProductQuantityViewModel(MainWindowViewModel appvm)
//        //{
//        //    this.unitMeasure = appvm.UnitMeasureManager.Unit;
//        //    //umFamily = appvm.UnitMeasureManager.Quantity;

//        //    //patternMatcher = appvm.PatternMatcher;
//        //}

//        //public ProductQuantityViewModel( Product prod)
//        //{            
//        //    this.product = prod;
//        //    //this.umFamily = prod.UMFamily;

//        //    //patternMatcher = appvm.PatternMatcher;
//        //}

//        public ProductQuantityViewModel(Product prod, double quantity, UnitMeasure um) 
//        {
//            this.quantity = quantity;
//            this.unitMeasure = um;
//            //this.umFamily = um.UMFamily;
//            this.product = prod;

//            //patternMatcher = appvm.PatternMatcher;
//        }

//        double quantity;
//        public double Quantity
//        {
//            get { return quantity; }
//            set
//            {
//                if (quantity != value) 
//                {
//                    OnQuantityChanging();
//                    quantity = value;
//                    OnPropertyChanged("Quantity");
//                    OnQuantityChanged();
//                }
//            }
//        }

//        protected virtual void OnQuantityChanging() { }
//        protected virtual void OnQuantityChanged() { }

//        UnitMeasure unitMeasure;
//        public UnitMeasure UnitMeasure
//        {
//            get { return unitMeasure; }
//            set
//            {
//                if (unitMeasure != value) 
//                {
//                    OnUnitMeasureChanging();
//                    unitMeasure = value;
//                    OnPropertyChanged("UnitMeasure");
//                    OnUnitMeasureChanged();
//                }                
//            }
//        }

//        protected virtual void OnUnitMeasureChanging() { }
//        protected virtual void OnUnitMeasureChanged() { }

//        //bool whiteSpaceError;
//        ///// <summary>
//        ///// wether or not an empty string is considered as an unsuccessfull string
//        ///// </summary>
//        //public bool WhiteSpaceError
//        //{
//        //    get { return whiteSpaceError; }
//        //    set { whiteSpaceError = value; }
//        //}

//        //string quantityExpression;
//        //public string QuantityExpression
//        //{
//        //    get 
//        //    {
//        //        if (quantityExpression == null && unitMeasure != null) 
//        //            quantityExpression = quantity + unitMeasure.Caption;
//        //        return quantityExpression;
//        //    }
//        //    set
//        //    {
//        //        quantityExpression = value;

//        //        ParseQuantityExpression(quantityExpression);                

//        //        OnPropertyChanged("QuantityExpression");
//        //    }
//        //}

//        //private void ParseQuantityExpression(string quantityExpression)
//        //{
//        //    if (string.IsNullOrWhiteSpace(quantityExpression))
//        //    {
//        //        HasErrors = whiteSpaceError;
//        //        if (whiteSpaceError) ErrorMessage = "Espeficique una cantidad";
//        //    }
//        //    else
//        //    {
//        //        float float_part; UnitMeasure um_part;

//        //        if (patternMatcher.ParseQuantityString(quantityExpression, out float_part, out um_part))
//        //        {
//        //            if (product != null && umFamily != um_part.UMFamily)
//        //            {
//        //                HasErrors = true;
//        //                ErrorMessage = "La unidad de medida no coincide con este producto.";
//        //            }
//        //            else
//        //            {
//        //                OnQuantityExpressionChanging();

//        //                Quantity = float_part;
//        //                UnitMeasure = um_part;

//        //                //only call this when everything is alright
//        //                OnQuantityExpressionChanged();

//        //                HasErrors = false;
//        //                ErrorMessage = string.Empty;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            HasErrors = true;
//        //            ErrorMessage = "Error!";
//        //        }
//        //    }
//        //}

//        //protected virtual void OnQuantityExpressionChanging() { }
//        //protected virtual void OnQuantityExpressionChanged() { }

//        Product product;
//        public Product Product 
//        {
//            get { return product; }
//            set 
//            {
//                if (product != value) 
//                {
//                    //OnProductChanging();
//                    product = value;
//                    OnPropertyChanged("Product");
//                    //if (product != null) umFamily = product.UMFamily;
//                    //ParseQuantityExpression(quantityExpression);
//                    //OnProductChanged();
//                }                
//            }
//        }

//        public int ProductId
//        {
//            get { return product.Id; }
//        }

//        //UMFamily umFamily;
//        //public UMFamily UMFamily 
//        //{
//        //    get { return umFamily; }
//        //    set 
//        //    {
//        //        umFamily = value;
//        //        //ParseQuantityExpression(quantityExpression);
//        //    }
//        //}

//        //protected virtual void OnProductChanging() { }
//        //protected virtual void OnProductChanged() { }

//        //bool hasErrors;
//        //public bool HasErrors 
//        //{
//        //    get { return hasErrors; }
//        //    set
//        //    {
//        //        hasErrors = value;
//        //        OnPropertyChanged("HasErrors");
//        //    }
//        //}

//        //string errorMessage;

//        //public string ErrorMessage
//        //{
//        //    get { return errorMessage; }
//        //    set 
//        //    {
//        //        errorMessage = value;
//        //        OnPropertyChanged("ErrorMessage");
//        //    }
//        //}

//        //void CheckErrors() 
//        //{
//        //    if (string.IsNullOrWhiteSpace(quantityExpression))
//        //    {
//        //        HasErrors = whiteSpaceError;
//        //        if (whiteSpaceError) ErrorMessage = "Espeficique una cantidad";
//        //        return;
//        //    }

//        //    float float_part; UnitMeasure um_part;
//        //    if (patternMatcher.ParseQuantityString(quantityExpression, out float_part, out um_part))
//        //    {
//        //        if (product != null && umFamily != um_part.UMFamily)
//        //        {
//        //            HasErrors = true;
//        //            ErrorMessage = "La unidad de medida no coincide con este producto.";
//        //        }
//        //        else
//        //        {
//        //            HasErrors = false;
//        //            ErrorMessage = string.Empty;
//        //        }
//        //    }
//        //    else
//        //    {
//        //        HasErrors = true;
//        //        ErrorMessage = "Error!";
//        //    }
//        //}
//    }
//}