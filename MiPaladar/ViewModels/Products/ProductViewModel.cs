using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Windows.Input;
using System.Windows.Data;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Services;
using System.IO;
using System.Windows;

namespace MiPaladar.ViewModels
{

    public class ProductViewModel : ViewModelBase, IScreen//, IDataErrorInfo
    {
        //Product product;
        MainWindowViewModel appvm;
        ProductManager productManager;
        //PatternMatcher patternMatcher;

        Product product;
        Action<Product> onProductCreated;
        Action<Product> onRemoved;

        //design
        public ProductViewModel() { }

        ////create new product
        public ProductViewModel(MainWindowViewModel appvm, Action<Product> onProductCreated, Action<Product> onRemoved)
        {
            this.appvm = appvm;
            this.productManager = appvm.ProductManager;
            //this.patternMatcher = appvm.PatternMatcher;
            this.onProductCreated = onProductCreated;
            this.onRemoved = onRemoved;

            umFamily = appvm.UnitMeasureManager.Quantity;
            //costUM = appvm.UnitMeasureManager.Unit;
            //costQtty = 1;

            //costQtty = 1;
            minimumStockUM = costUM = appvm.UnitMeasureManager.Unit;
            
            //recipeQtty = 1;
            //recipeUM = appvm.UnitMeasureManager.Unit;

            creating = true;
            HasPendingChanges = true;
        }

        public ProductViewModel(MainWindowViewModel appvm, Product product, Action<Product> onRemoved)
        {
            this.appvm = appvm;
            this.productManager = appvm.ProductManager;
            //this.patternMatcher = appvm.PatternMatcher;
            this.onRemoved = onRemoved;

            this.product = product;

            CopyFromProduct();
            HasPendingChanges = false;
        }

        public MainWindowViewModel AppVM { get { return appvm; } }

        public override string DisplayName
        {
            get { return string.IsNullOrWhiteSpace(Name) ? "Nuevo Producto" : Name; }
        }

        public Product WrappedProduct { get { return product; } }

        #region Copy Methods

        private void CopyFromProduct()
        {
            Code = product.Code;
            Name = product.Name;
            SalePrice = product.SalePrice;
            Description = product.Description;
            //IsPurchasable = product.IsPurchasable;
            //IsIngredient = product.IsIngredient;
            
            isRecipe = product.IsRecipe;
            OnPropertyChanged("IsRecipe");

            IsStorable = product.IsStorable;
            NotInMenu = product.NotInMenu;
            ProductionArea = product.ProductionArea;
            IsProduced = product.IsProduced;
            //IsEntrant = product.IsEntrant;
            
            umFamily = product.UMFamily;
            UMIsOtherThanQtty = umFamily != appvm.UnitMeasureManager.Quantity;
            OnPropertyChanged("UMFamily");
            PrintString = product.PrintString;
            //HasDifferentFormats = product.HasDifferentFormats;
                        
            //RecipeQuantity = product.RecipeQuantity;
            //RecipeUnitMeasure = product.RecipeUnitMeasure;
                        
            //cost            
            //costPrice = product.CostPrice;
            //OnPropertyChanged("CostPrice");
            //costQtty = product.CostQuantity;
            //OnPropertyChanged("CostQuantity");            

            PriceRate = product.CostPrice / (decimal)product.CostQuantity;
            CostUnitMeasure = product.CostUnitMeasure;

            //quantity in inventory
            if (product.CurrentExistence.Count > 0)
            {
                double total_qtty = product.CurrentExistence.Sum(x => x.Quantity);
                decimal total_cost = product.CurrentExistence.Sum(x => x.Cost);
                if (total_qtty > 0) AverageCost = total_cost / (decimal)(total_qtty / product.CostUnitMeasure.ToBaseConversion);
            }

            MinimumStock = product.MinimumStock;
            MinimumStockUM = product.MinimumStockUM;

            //image
            ImageFileName = product.ImageFileName;
            BuildImageFullPath();

            ingredients.Clear();
            foreach (var item in product.Ingredients)
            {
                IngredientViewModel ic = new IngredientViewModel(this, item.Quantity, item.UnitMeasure, item.IngredientProduct, OnIngredientCostChanged);
                //ic.Quantity = item.Quantity;
                //ic.IngredientProduct = item.IngredientProduct;

                ingredients.Add(ic);
            }

            relatedCategories.Clear();
            foreach (var item in product.RelatedCategories)
            {
                ProductIndexViewModel pic = new ProductIndexViewModel(this);
                pic.Category = item.Category;
                pic.IsMain = item.IsMain;

                relatedCategories.Add(pic);
            }

            //formats.Clear();
            //foreach (var item in product.ProductFormats)
            //{
            //    ProductFormatViewModel pfvm = new ProductFormatViewModel(item.Quantity, item.UnitMeasure, item.Product, 
            //        () => HasPendingChanges = true);

            //    formats.Add(pfvm);
            //}
        }

        private void BuildImageFullPath()
        {
            if (string.IsNullOrWhiteSpace(imageFileName)) return;

            string tempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                App.ApplicationFolderName);
            tempFolder = Path.Combine(tempFolder, App.AppProductsFolderName);

            ImageFullPath = Path.Combine(tempFolder, imageFileName);
        }                

        void CopyToProduct()
        {
            //save changes
            if (product.Code != code) product.Code = code;
            if (product.Name != Name) product.Name = Name;
            if (product.SalePrice != SalePrice) product.SalePrice = SalePrice;
            if (product.Description != Description) product.Description = Description;
            //if (product.IsPurchasable != IsPurchasable) product.IsPurchasable = IsPurchasable;
            //if (product.IsIngredient != IsIngredient) product.IsIngredient = IsIngredient;
            if (product.IsRecipe != IsRecipe) product.IsRecipe = IsRecipe;
            if (product.IsStorable != IsStorable) product.IsStorable = IsStorable;
            if (product.NotInMenu != NotInMenu) product.NotInMenu = NotInMenu;
            if (product.IsProduced != IsProduced) product.IsProduced = IsProduced;
            //if (product.IsEntrant != IsEntrant) product.IsEntrant = IsEntrant;

            if (product.ProductionArea != ProductionArea) product.ProductionArea = ProductionArea;
            if (product.UMFamily != UMFamily) product.UMFamily = UMFamily;

            //default cost qtty when creating
            if (product.CostQuantity == 0) product.CostQuantity = 1;

            decimal oldPriceRate = product.CostPrice / (decimal)product.CostQuantity;

            if (costUM != product.CostUnitMeasure || oldPriceRate != priceRate)
            {
                product.CostPrice = priceRate;
                product.CostQuantity = 1;
                product.CostUnitMeasure = costUM;
            }
            //cost item
            //if (product.PurchasePrice != PriceRate) product.PurchasePrice = PriceRate;            
            
            //image
            if (product.ImageFileName != imageFileName) product.ImageFileName = imageFileName;
            if (product.PrintString != PrintString) product.PrintString = printString;
            //if (product.HasDifferentFormats != hasDifferentFormats) product.HasDifferentFormats = hasDifferentFormats;

            //if (product.RecipeQuantity != recipeQtty) product.RecipeQuantity = recipeQtty;
            //if (product.RecipeUnitMeasure != recipeUM) product.RecipeUnitMeasure = recipeUM;

            if (product.MinimumStockUM != minimumStockUM) product.MinimumStockUM = minimumStockUM;
            if (product.MinimumStock != minimumStock) product.MinimumStock = minimumStock;            

            //clear categories
            while (product.RelatedCategories.Count > 0)
            {
                ProductIndex pi = product.RelatedCategories.First();
                appvm.Context.ProductIndexes.DeleteObject(pi);
            }
            foreach (var item in RelatedCategories)
            {
                ProductIndex pi = new ProductIndex();
                pi.Category = item.Category;
                pi.IsMain = item.IsMain;

                product.RelatedCategories.Add(pi);
            }
            //clear ingredients
            while (product.Ingredients.Count > 0)
            {
                Ingredient ing = product.Ingredients.First();
                appvm.Context.Ingredients.DeleteObject(ing);
            }
            foreach (var item in Ingredients)
            {
                Ingredient ing = new Ingredient();
                ing.Quantity = item.Quantity;
                ing.IngredientProduct = item.Product;
                ing.UnitMeasure = item.UnitMeasure;

                product.Ingredients.Add(ing);
            }

            //clear formats
            //while (product.ProductFormats.Count > 0)
            //{
            //    ProductFormat pf = product.ProductFormats.First();
            //    appvm.Context.ProductFormats.DeleteObject(pf);
            //}
            //foreach (var item in Formats)
            //{
            //    ProductFormat pf = new ProductFormat();
            //    pf.Quantity = item.Quantity;
            //    pf.UnitMeasure = item.UnitMeasure;
            //    pf.Product = item.Product;

            //    product.ProductFormats.Add(pf);
            //}

            //appvm.SaveChanges();
        }

        //refresh a product's cost, the sum of ingredients's cost
        //void RefreshRecipeCost(Product recipe)
        //{
        //    float total_cost = 0;
        //    foreach (var item in recipe.Ingredients)
        //    {
        //        if (item.IngredientProduct == null) continue;

        //        total_cost += item.IngredientProduct.PurchasePrice * item.Quantity;
        //    }

        //    recipe.PurchasePrice = total_cost;
        //}
        void RefreshMyCost()
        {
            decimal total_cost = 0;
            foreach (var item in this.Ingredients)
            {
                if (item.Product == null) continue;

                total_cost += item.ItemCost;
                //total_cost += item.Product.PurchasePrice * (decimal)(item.Quantity * item.UnitMeasure.ToBaseConversion);
            }

            PriceRate = total_cost;
        }

        #endregion        

        #region Properties

        int code;
        public int Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");

                HasPendingChanges = true;
            }
        }

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
                HasPendingChanges = true;

                if (creating) PrintString = value;
            }
        }

        decimal salePrice;
        public decimal SalePrice
        {
            get { return salePrice; }
            set
            {
                salePrice = value;
                OnPropertyChanged("SalePrice");
                HasPendingChanges = true;
            }
        }
        
        //bool isPurchasable;
        //public bool IsPurchasable 
        //{
        //    get { return isPurchasable; }
        //    set
        //    {
        //        isPurchasable = value;
        //        OnPropertyChanged("IsPurchasable");
        //        HasPendingChanges = true;
        //    }
        //}
        //bool isIngredient;
        //public bool IsIngredient
        //{
        //    get { return isIngredient; }
        //    set
        //    {                
        //        isIngredient = value;
        //        OnPropertyChanged("IsIngredient");
        //        HasPendingChanges = true;
        //    }
        //}
        bool isRecipe;
        public bool IsRecipe
        {
            get { return isRecipe; }
            set
            {
                isRecipe = value;
                if (isRecipe)
                    UMFamily = appvm.UnitMeasureManager.Quantity;
                OnPropertyChanged("IsRecipe");
                HasPendingChanges = true;
            }
        }
        bool isStorable;
        public bool IsStorable
        {
            get { return isStorable; }
            set
            {                
                isStorable = value;
                OnPropertyChanged("IsStorable");
                HasPendingChanges = true;
            }
        }
        bool notInMenu;
        public bool NotInMenu
        {
            get { return notInMenu; }
            set
            {
                notInMenu = value;
                OnPropertyChanged("NotInMenu");
                HasPendingChanges = true;
            }
        }

        ProductionArea productionArea;
        public ProductionArea ProductionArea 
        {
            get { return productionArea; }
            set 
            {
                productionArea = value;
                OnPropertyChanged("ProductionArea");
                HasPendingChanges = true;
            }
        }

        bool isProduced;
        public bool IsProduced 
        {
            get { return isProduced; }
            set
            {
                if (isProduced != value)
                {
                    isProduced = value;
                    OnPropertyChanged("IsProduced");
                    HasPendingChanges = true;
                }
            }
        }

        //bool isEntrant;
        //public bool IsEntrant
        //{
        //    get { return isEntrant; }
        //    set
        //    {
        //        if (isEntrant != value)
        //        {
        //            isEntrant = value;
        //            OnPropertyChanged("IsEntrant");
        //            HasPendingChanges = true;
        //        }
        //    }
        //}

        UMFamily umFamily;
        public UMFamily UMFamily 
        {
            get { return umFamily; }
            set
            {
                if (umFamily != value)
                {
                    umFamily = value;
                    OnPropertyChanged("UMFamily");

                    if (umFamily != null)
                    {
                        MinimumStockUM = CostUnitMeasure = umFamily.UnitMeasures.First();
                        //RecipeUnitMeasure = umFamily.UnitMeasures.First();
                    }

                    UMIsOtherThanQtty = umFamily != appvm.UnitMeasureManager.Quantity;

                    //if (recipeQttyItem != null) recipeQttyItem.UMFamily = value;
                    //if (costQttyItem != null) costQttyItem.UMFamily = value;
                    HasPendingChanges = true;
                }
            }
        }

        bool umIsOtherThanQtty;
        public bool UMIsOtherThanQtty
        {
            get { return umIsOtherThanQtty; }
            set
            {
                umIsOtherThanQtty = value;
                OnPropertyChanged("UMIsOtherThanQtty");
            }

        }


        #region Image Properties

        string imageFileName;
        public string ImageFileName
        {
            get { return imageFileName; }
            set
            {
                imageFileName = value;
                OnPropertyChanged("ImageFileName");

                HasPendingChanges = true;
            }
        }

        string fullImagePath;
        public string ImageFullPath
        {
            get { return fullImagePath; }
            set
            {
                fullImagePath = value;
                OnPropertyChanged("ImageFullPath");
                OnPropertyChanged("NoPicture");
            }
        }

        public bool NoPicture
        {
            get { return string.IsNullOrEmpty(fullImagePath); }
        }

        #endregion

        string description;
        public string Description 
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
                HasPendingChanges = true;
            }
        }

        string printString;
        public string PrintString
        {
            get { return printString; }
            set
            {
                printString = value;
                OnPropertyChanged("PrintString");
                HasPendingChanges = true;
            }
        }

        //#region Recipe Quantity Expression

        //float recipeQtty;
        //public float RecipeQuantity
        //{
        //    get { return recipeQtty; }
        //    set 
        //    {
        //        recipeQtty = value;
        //        OnPropertyChanged("RecipeQuantity");
        //        HasPendingChanges = true;
        //    }
        //}

        //UnitMeasure recipeUM;
        //public UnitMeasure RecipeUnitMeasure
        //{
        //    get { return recipeUM; }
        //    set
        //    {
        //        recipeUM = value;
        //        OnPropertyChanged("RecipeUnitMeasure");
        //        HasPendingChanges = true;
        //    }
        //}

        //#endregion        

        #region Product Cost 

        //price by unit measure, will change name later
        decimal priceRate;
        public decimal PriceRate
        {
            get { return priceRate; }
            set
            {
                priceRate = value;
                OnPropertyChanged("PriceRate");

                HasPendingChanges = true;
            }
        }

        decimal average_cost;

        public decimal AverageCost
        {
            get { return average_cost; }
            set 
            {
                average_cost = value;
                OnPropertyChanged("AverageCost");
            }
        }


        //price in cost expression
        //decimal costPrice;
        //public decimal CostPrice
        //{
        //    get { return costPrice; }
        //    set 
        //    {
        //        costPrice = value;
        //        OnPropertyChanged("CostPrice");
        //        HasPendingChanges = true;
        //        //UpdateAtomPrice();
        //    }
        //}
        /// <summary>
        /// cost by base unit measure
        /// </summary>
        //void UpdateAtomPrice() 
        //{
        //    PriceRate = costPrice / (decimal)(costQtty * costUM.ToBaseConversion);
        //}

        //quantity in cost expression
        //double costQtty;
        //public double CostQuantity
        //{
        //    get { return costQtty; }
        //    set 
        //    {
        //        costQtty = value;
        //        OnPropertyChanged("CostQuantity");
        //        HasPendingChanges = true;
        //        //UpdateAtomPrice();
        //    }
        //}

        //quantity in cost expression
        //ProductQuantityViewModel costQttyItem;
        //public ProductQuantityViewModel CostQuantityItem
        //{
        //    get { return costQttyItem; }
        //    set 
        //    {
        //        costQttyItem = value;
        //        OnPropertyChanged("CostQuantityItem");
        //    }
        //}

        //unit measure in cost expression
        UnitMeasure costUM;
        public UnitMeasure CostUnitMeasure
        {
            get { return costUM; }
            set
            {
                costUM = value;
                OnPropertyChanged("CostUnitMeasure");
                HasPendingChanges = true;
                //UpdateAtomPrice();
            }
        }

        //string costQttyString;
        //public string CostQuantityString
        //{
        //    get
        //    {
        //        if (costQttyString == null)
        //            costQttyString = costQtty + costUM.Caption;
        //        return costQttyString;
        //    }
        //    set
        //    {
        //        costQttyString = value;

        //        if (!string.IsNullOrWhiteSpace(costQttyString))
        //        {
        //            patternMatcher.ParseQuantityString(costQttyString, out costQtty, out costUM);

        //            UpdateAtomPrice();
        //        }

        //        OnPropertyChanged("CostQuantityString");
        //    }
        //}

        #endregion        

        double minimumStock;
        public double MinimumStock
        {
            get { return minimumStock; }
            set
            {
                minimumStock = value;
                OnPropertyChanged("MinimumStock");

                HasPendingChanges = true;
            }
        }

        UnitMeasure minimumStockUM;
        public UnitMeasure MinimumStockUM
        {
            get { return minimumStockUM; }
            set
            {
                minimumStockUM = value;
                OnPropertyChanged("MinimumStockUM");

                HasPendingChanges = true;
            }
        }

        #endregion               

        #region Select Image Command

        RelayCommand selectImageCommand;
        public ICommand SelectImageCommand
        {
            get
            {
                if (selectImageCommand == null)
                    selectImageCommand = new RelayCommand(x => SelectImage());
                return selectImageCommand;
            }
        }

        void SelectImage()
        {
            var open_file_dialog = base.GetService<IOpenFileDialogService>();

            string title = "Buscar fichero de reporte";
            string filter = "Imágenes|*.bmp;*.gif;*.ico;*.jpg;*.png;*.wdp;*.tiff";

            if (open_file_dialog.ShowDialog(title, filter) == true)
            {
                ImageFullPath = open_file_dialog.FileName;

                ImageFileName = Path.GetFileName(ImageFullPath);
                
                var copySvc = base.GetService<IFileCopyService>();

                copySvc.CopyImage(ImageFullPath, App.AppProductsFolderName);
            }
        }

        #endregion

        ObservableCollection<ProductIndexViewModel> relatedCategories = new ObservableCollection<ProductIndexViewModel>();
        public ObservableCollection<ProductIndexViewModel> RelatedCategories
        {
            get { return relatedCategories; }
        }        

        #region Add ProductIndex Command

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

        public string SearchTextCategory { get; set; }

        RelayCommand addProductIndexCommand;
        public ICommand AddProductIndexCommand 
        {
            get             
            {
                if (addProductIndexCommand == null)
                    addProductIndexCommand = new RelayCommand(x => AddProductIndex(SelectedCategory), 
                        x => selectedCategory != null);
                return addProductIndexCommand;
            }
        }

        bool CanAddCategory { get { return selectedCategory != null; } }

        void AddProductIndex(Category cat)
        {
            ProductIndexViewModel pi = new ProductIndexViewModel(this);
            //first category becomes main category
            if (relatedCategories.Count == 0) pi.IsMain = true;
            //pi.Product = product;
            pi.Category = cat;

            relatedCategories.Add(pi);
            //appvm.Context.ProductIndexes.AddObject(pi);

            //OnPropertyChanged("HasCategories");

            //appvm.SaveChanges();

            icvCategories.Refresh();

            SelectedCategory = null;
            SearchTextCategory = "";
            OnPropertyChanged("SearchTextCategory");

            HasPendingChanges = true;
        }

        #endregion

        #region Remove ProductIndex Command

        RelayCommand removeProductIndexCommand;
        public ICommand RemoveProductIndexCommand
        {
            get
            {
                if (removeProductIndexCommand == null)
                    removeProductIndexCommand = new RelayCommand(x => RemoveProductIndex((ProductIndexViewModel)x));
                return removeProductIndexCommand;
            }
        }

        void RemoveProductIndex(ProductIndexViewModel pi)
        {
            relatedCategories.Remove(pi);

            //set as main the only category left
            if (relatedCategories.Count == 1) 
            {
                relatedCategories[0].IsMain = true;
            }

            //OnPropertyChanged("HasCategories");

            //appvm.SaveChanges();

            icvCategories.Refresh();

            HasPendingChanges = true;
        }        

        #endregion        
        
        #region Categories To Add

        ICollectionView icvCategories;
        public ICollectionView Categories
        {
            get
            {
                if (icvCategories == null)
                {
                    CollectionViewSource cvs = new CollectionViewSource();
                    cvs.Source = appvm.CategoriesOC;
                    icvCategories = cvs.View;

                    icvCategories.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                    //just show categories not already added
                    icvCategories.Filter = (object o) =>
                    {
                        Category category = (Category)o;

                        foreach (var item in relatedCategories)
                        {
                            if (item.Category == category)
                            {
                                return false;
                            }
                        }

                        return true;
                    };
                }
                return icvCategories;
            }
        }

        #endregion

        #region Ingredients

        ObservableCollection<IngredientViewModel> ingredients = new ObservableCollection<IngredientViewModel>();
        public ObservableCollection<IngredientViewModel> Ingredients
        {
            get { return ingredients; }
        }

        #region Ingredientes To Add

        ICollectionView icvIngredients;
        public ICollectionView IngredientsView
        {
            get
            {
                if (icvIngredients == null)
                {
                    CollectionViewSource cvs = new CollectionViewSource();
                    cvs.Source = appvm.ProductsOC;
                    icvIngredients = cvs.View;

                    icvIngredients.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                    icvIngredients.Filter = x =>
                    {
                        Product p = (Product)x;

                        //if (!p.IsIngredient) return false;

                        //a product cant be ingredient of itself
                        if (p == product) return false;

                        //dont add ingredient twice
                        foreach (var item in Ingredients)
                        {
                            if (item.Product == p) return false;
                        }

                        if (!creating && CheckIngredientCycle(p)) return false;

                        return true;
                    };
                }
                return icvIngredients;
            }
        }

        /// <summary>
        /// check if the current product is a ingredient of p, recursively
        /// </summary>
        /// <returns></returns>
        bool CheckIngredientCycle(Product productToCheck) 
        {
            if (productToCheck.IsRecipe) 
            {
                foreach (var ing in productToCheck.Ingredients)
                {
                    //look for the current product in ingredients
                    if (ing.IngredientProduct == product) return true;

                    //check recursively in each ingredient
                    if (CheckIngredientCycle(ing.IngredientProduct)) return true;
                }
            }
            return false;
        }

        #endregion

        #region Add Ingredient Command

        //ProductQuantityViewModel itemToAdd;
        //public ProductQuantityViewModel ItemToAdd
        //{
        //    get
        //    {
        //        if (itemToAdd == null) itemToAdd = new ProductQuantityViewModel(appvm);
        //        return itemToAdd;
        //    }
        //    set { itemToAdd = value; }
        //}

        //has the form: Number-UM, like in 15g
        //string quantityToAdd;
        //public string QuantityToAdd 
        //{
        //    get { return quantityToAdd; }
        //    set
        //    {
        //        quantityToAdd = value;
        //        OnPropertyChanged("QuantityToAdd");

        //        if (!string.IsNullOrWhiteSpace(quantityToAdd)) 
        //        {
        //            patternMatcher.ParseQuantityString(quantityToAdd, out float_part, out um_part);
        //        }
        //    }
        //}               
        //the float part of new ingredient quantity
        double ingredient_quantity;
        public double IngredientQuantity
        {
            get { return ingredient_quantity; }
            set
            {
                ingredient_quantity = value;
                OnPropertyChanged("IngredientQuantity");
            }
        }
        ////the unit measure part
        //UnitMeasure um_part;
        //public UnitMeasure UMPart 
        //{
        //    get { return um_part; }
        //    set
        //    {
        //        um_part = value;
        //        OnPropertyChanged("UMPart");
        //    }
        //}

        Product ingredientToAdd;
        public Product IngredientToAdd
        {
            get { return ingredientToAdd; }
            set
            {
                ingredientToAdd = value;
                OnPropertyChanged("IngredientToAdd");
            }
        }

        public string SearchTextIngredient { get; set; }

        RelayCommand addIngredientCommand;
        public ICommand AddIngredientCommand
        {
            get
            {
                if (addIngredientCommand == null)
                {
                    addIngredientCommand = new RelayCommand(x => AddIngredient(ingredientToAdd),
                        x => CanAddIngredient);
                }
                return addIngredientCommand;
            }
        }

        bool CanAddIngredient
        {
            get
            {
                return /*!ItemToAdd.HasErrors &&*/ ingredientToAdd != null && ingredient_quantity > 0;
            }
        }

        private void AddIngredient(Product ingredientToAdd)
        {
            var baseUM = ingredientToAdd.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
            IngredientViewModel ing = new IngredientViewModel(this, ingredient_quantity, baseUM, ingredientToAdd, OnIngredientCostChanged);

            //ing.BaseProduct = product;
            //ing.IngredientProduct = ingredientToAdd;
            //ing.Quantity = quantityToAdd;

            ingredients.Add(ing);
            //appvm.SaveChanges();

            RefreshMyCost();

            icvIngredients.Refresh();

            //QuantityPart = 0;
            //UMPart = null;
            IngredientQuantity = 0;
            IngredientToAdd = null;
            SearchTextIngredient = "";
            OnPropertyChanged("SearchTextIngredient");

            //ing.PropertyChanged += new PropertyChangedEventHandler(ing_PropertyChanged);      

            HasPendingChanges = true;
        }

        void OnIngredientCostChanged()
        {
            RefreshMyCost();
        }

        #endregion

        #region Remove Ingredient Command

        public IngredientViewModel SelectedIngredient { get; set; }

        RelayCommand removeIngredientCommand;
        public ICommand RemoveIngredientCommand
        {
            get
            {
                if (removeIngredientCommand == null)
                {
                    removeIngredientCommand = new RelayCommand(x => RemoveIngredient(SelectedIngredient),
                        x => CanRemoveIngredient);
                }
                return removeIngredientCommand;
            }
        }

        bool CanRemoveIngredient
        {
            get { return SelectedIngredient != null; }
        }

        private void RemoveIngredient(IngredientViewModel ingredientToremove)
        {
            ingredients.Remove(ingredientToremove);
            //appvm.SaveChanges();

            RefreshMyCost();

            icvIngredients.Refresh();

            HasPendingChanges = true;
        }

        #endregion

        #endregion     

        bool hasDifferentFormats;
        public bool HasDifferentFormats 
        {
            get { return hasDifferentFormats; }
            set
            {
                hasDifferentFormats = value;
                OnPropertyChanged("HasDifferentFormats");
            }
        }
   
        RelayCommand specifyFormatsCommand;
        public ICommand SpecifyFormatsCommand 
        {
            get 
            {
                if (specifyFormatsCommand == null)
                    specifyFormatsCommand = new RelayCommand(x => SpecifyFormats());
                return specifyFormatsCommand;
            }
        }

        void SpecifyFormats()         
        { 
            HasDifferentFormats = true;
            HasPendingChanges = true;
        }


        //#region Formats

        //ObservableCollection<ProductFormatViewModel> formats = new ObservableCollection<ProductFormatViewModel>();
        //public ObservableCollection<ProductFormatViewModel> Formats
        //{
        //    get { return formats; }
        //}

        //#region Formats To Add

        //ICollectionView icvFormats;
        //public ICollectionView FormatsView
        //{
        //    get
        //    {
        //        if (icvFormats == null)
        //        {
        //            CollectionViewSource cvs = new CollectionViewSource();
        //            cvs.Source = appvm.ProductsOC;
        //            icvFormats = cvs.View;

        //            icvFormats.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

        //            icvFormats.Filter = x =>
        //            {
        //                Product p = (Product)x;

        //                //a product cant be format of itself
        //                if (p == product) return false;

        //                //just products measured by quantity
        //                if (p.UMFamily != appvm.UnitMeasureManager.Quantity) return false;

        //                //dont add product twice
        //                foreach (var item in Formats)
        //                {
        //                    if (item.Product == p) return false;
        //                }

        //                return true;
        //            };
        //        }
        //        return icvFormats;
        //    }
        //}

        //#endregion

        //#region Add Format Command

        //float formatQuantityToAdd;
        //public float FormatQuantityToAdd
        //{
        //    get { return formatQuantityToAdd; }
        //    set
        //    {
        //        formatQuantityToAdd = value;
        //        OnPropertyChanged("FormatQuantityToAdd");
        //    }
        //}

        //Product formatProductToAdd;
        //public Product FormatProductToAdd
        //{
        //    get { return formatProductToAdd; }
        //    set
        //    {
        //        formatProductToAdd = value;
        //        OnPropertyChanged("FormatProductToAdd");
        //    }
        //}

        //public string SearchTextFormat { get; set; }

        //RelayCommand addFormatCommand;
        //public ICommand AddFormatCommand
        //{
        //    get
        //    {
        //        if (addFormatCommand == null)
        //        {
        //            addFormatCommand = new RelayCommand(x => AddFormat(),
        //                x => CanAddFormat);
        //        }
        //        return addFormatCommand;
        //    }
        //}

        //bool CanAddFormat
        //{
        //    get
        //    {
        //        return formatProductToAdd != null && formatQuantityToAdd > 0;
        //    }
        //}

        //private void AddFormat()
        //{
        //    var baseUM = umFamily.UnitMeasures.Single(x => x.IsFamilyBase);

        //    ProductFormatViewModel pfvm = new ProductFormatViewModel(formatQuantityToAdd, baseUM, formatProductToAdd,
        //        () => HasPendingChanges = true);

        //    formats.Add(pfvm);

        //    icvFormats.Refresh();

        //    FormatQuantityToAdd = 0;
        //    FormatProductToAdd = null;
        //    SearchTextFormat = "";
        //    OnPropertyChanged("SearchTextFormat");

        //    HasPendingChanges = true;
        //}

        //#endregion

        //#region Remove Format Command

        //public ProductFormatViewModel SelectedFormat { get; set; }

        //RelayCommand removeFormatCommand;
        //public ICommand RemoveFormatCommand
        //{
        //    get
        //    {
        //        if (removeFormatCommand == null)
        //        {
        //            removeFormatCommand = new RelayCommand(x => RemoveFormat(), x => CanRemoveFormat);
        //        }
        //        return removeFormatCommand;
        //    }
        //}

        //bool CanRemoveFormat
        //{
        //    get { return SelectedFormat != null; }
        //}

        //private void RemoveFormat()
        //{
        //    formats.Remove(SelectedFormat);

        //    icvFormats.Refresh();

        //    HasPendingChanges = true;
        //}

        //#endregion

        //#endregion        
        
        public ObservableCollection<ProductionArea> ProductionAreas 
        {
            get { return appvm.ProductionAreasOC; }
        }

        public IEnumerable<UnitMeasure> UnitMeasures
        {
            get { return appvm.Context.UnitMeasures; }
        }

        public IEnumerable<UMFamily> UMFamilies 
        {
            get { return appvm.Context.UMFamilies; }
        }

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                    removeCommand = new RelayCommand(x => this.Remove(), x => this.CanRemove);
                return removeCommand;
            }
        }

        bool CanRemove { get { return !creating; } }

        void Remove()
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = "¿Está seguro que desea eliminar este producto?";

            if (msgBox.ShowYesNoDialog(message) == true)
            {
                //close this window
                var windowManager = base.GetService<IWindowManager>();

                windowManager.Close(this);

                //let parent remove it
                if (onRemoved != null) onRemoved(product);

                RemoveMe();

                appvm.SaveChanges();
            }
        }

        void RemoveMe()
        {
            //close
            //////clear categories
            //while (toRemove.RelatedCategories.Count > 0)
            //    appvm.Context.ProductIndexes.DeleteObject(toRemove.RelatedCategories.First());

            //remove IngredientProducts
            //var related_ingredientes = from ing in appvm.Context.Ingredients
            //                           where ing.IngredientProduct.Id == prod.Id
            //                           select ing;
            List<Ingredient> ingredients_to_remove = new List<Ingredient>(product.Outgredients);

            foreach (var item in ingredients_to_remove)
            {
                appvm.Context.Ingredients.DeleteObject(item);
            }

            //var related_formats = from fmt in appvm.Context.ProductFormats
            //                      where fmt.Product.Id == toRemove.Id
            //                      select fmt;

            //foreach (var item in related_formats)
            //{
            //    appvm.Context.ProductFormats.DeleteObject(item);
            //}

            List<InventoryItem> items_toRemove = new List<InventoryItem>(product.CurrentExistence);

            foreach (var item in items_toRemove)
            {
                appvm.InventoryItemsOC.Remove(item);
                appvm.Context.InventoryItems.DeleteObject(item);
            }

            List<LineItem> lineitems_toRemove = new List<LineItem>();
            var related_lineitems = from li in appvm.Context.LineItems
                                    where li.Product.Id == product.Id
                                    select li;
            foreach (var item in related_lineitems)
            {
                lineitems_toRemove.Add(item);
            }
            foreach (var item in lineitems_toRemove)
            {
                appvm.Context.LineItems.DeleteObject(item);
            }

            //List<Faena> faenas_toRemove = new List<Faena>(selectedProduct.Faenas);
            //foreach (var item in faenas_toRemove)
            //{
            //    appvm.Context.Faenas.DeleteObject(item);
            //}

            ////null LineItems
            //var related_lineitems = from li in appvm.Context.LineItems
            //                        where li.Product.Id == SelectedProduct.Id
            //                        select li;

            //foreach (var item in related_lineitems)
            //{
            //    item.Product = null;
            //}

            ////delete inventoryitem
            //if (toRemove.CurrentExistence != null) 
            //    appvm.Context.InventoryItems.DeleteObject(toRemove.CurrentExistence);

            ////delete inventory history
            //while (toRemove.Existences.Count > 0)
            //    appvm.Context.InventoryTraces.DeleteObject(toRemove.Existences.First());

            appvm.ProductManager.VentaItems.Remove(product);
            //appvm.ProductManager.PurchasableProducts.Remove(product);
            appvm.ProductManager.InventoryProducts.Remove(product);
            appvm.ProductManager.RecipeProducts.Remove(product);
            //appvm.ProductManager.IngredientProducts.Remove(product);

            appvm.ProductsOC.Remove(product);
            appvm.Context.Products.DeleteObject(product);

            appvm.SaveChanges();

            //RefreshView();

            //OnPropertyChanged("ThereAreNoProducts");
        }       

        #endregion

        //#region Edit Command

        ////bool editing;
        ////public bool Editing
        ////{
        ////    get { return editing; }
        ////    set
        ////    {
        ////        editing = value;
        ////        OnPropertyChanged("Editing");
        ////        //OnPropertyChanged("CreatingOrEditing");
        ////        //OnPropertyChanged("EditButtonVisible");
        ////        //OnPropertyChanged("CancelButtonVisible");
        ////        OnPropertyChanged("HasCategories");
        ////    }
        ////}

        //RelayCommand editCommand;
        //public ICommand EditCommand
        //{
        //    get
        //    {
        //        if (editCommand == null)
        //            editCommand = new RelayCommand(x => Edit());
        //        return editCommand;
        //    }
        //}

        //public void Edit()
        //{
        //    Editing = true;
        //}

        //#endregion

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
            get { return appvm.LoggedInUser.Role.CanEditProducts && !string.IsNullOrWhiteSpace(name); }
        }

        void Save() 
        {
            bool update_recipes_cost_flag = false;

            if (creating)
            {
                product = new Product();
            }
            else
            {
                bool costChanged = CostChanged(product.CostPrice, product.CostQuantity, product.CostUnitMeasure, priceRate, costUM);
                //when the cost of an ingredient changes uptade the cost of recipes where it appears
                if (costChanged)
                {
                    update_recipes_cost_flag = true;
                    //float price_change = PriceRate - product.PurchasePrice;                    
                }
            }            

            CopyToProduct();

            if (creating)
            {
                appvm.Context.Products.AddObject(product);
                appvm.ProductsOC.Add(product);

                onProductCreated(product);

                creating = false;

                icvIngredients.Refresh();
            }

            //re-calculate costs of recipes where it appears
            if (update_recipes_cost_flag) 
            {
                foreach (var outgredient in product.Outgredients)
                {
                    if (outgredient.BaseProduct.IsRecipe)
                    {
                        RefreshRecipeCost(outgredient.BaseProduct);
                        //float change = price_change * outgredient.Quantity * outgredient.UnitMeasure.ToBaseConversion;
                        //outgredient.BaseProduct.PurchasePrice += change;
                        //outgredient.BaseProduct.CostPrice += change;
                    }
                }
            }

            //remove inventory items
            if (!isStorable) 
            {
                List<InventoryItem> toRemove = new List<InventoryItem>(product.CurrentExistence);

                //foreach (var ii in appvm.InventoryItemsOC)
                //{
                //    if (ii.Product == product) toRemove.Add(ii);
                //}

                foreach (var ii in toRemove)
                {
                    appvm.InventoryItemsOC.Remove(ii);
                    appvm.Context.InventoryItems.DeleteObject(ii);
                }
            }

            appvm.SaveChanges();

            HasPendingChanges = false;

            //update collections
            if (!product.NotInMenu && product.UMFamily == appvm.UnitMeasureManager.Quantity &&
                !productManager.VentaItems.Contains(product)) productManager.VentaItems.Add(product);

            //if (product.IsPurchasable && !productManager.PurchasableProducts.Contains(product))
            //    productManager.PurchasableProducts.Add(product);

            if (product.IsStorable && !productManager.InventoryProducts.Contains(product))
                productManager.InventoryProducts.Add(product);

            if (product.IsRecipe && !productManager.RecipeProducts.Contains(product))
                productManager.RecipeProducts.Add(product);

            //if (product.IsIngredient && !productManager.IngredientProducts.Contains(product))
            //    productManager.IngredientProducts.Add(product);

            if (product.NotInMenu || product.UMFamily != appvm.UnitMeasureManager.Quantity)
                productManager.VentaItems.Remove(product);

            //if (!product.IsPurchasable) productManager.PurchasableProducts.Remove(product);

            if (!product.IsStorable) productManager.InventoryProducts.Remove(product);

            if (!product.IsRecipe) productManager.RecipeProducts.Remove(product);

            //if (!product.IsIngredient) productManager.IngredientProducts.Remove(product);
        }

        bool CostChanged(decimal oldPrice, double oldQtty, UnitMeasure oldUM, decimal newPrice, UnitMeasure newUM)
        {
            return (newPrice * (decimal)(oldQtty * oldUM.ToBaseConversion)) != (oldPrice * (decimal)costUM.ToBaseConversion);
        }

        void RefreshRecipeCost(Product recipe)
        {
            decimal total_cost = 0;

            foreach (var item in recipe.Ingredients)
            {
                if (item.IngredientProduct == null) continue;

                Product ing_prod = item.IngredientProduct;
                decimal this_item_cost = ProductManager.GetCurrentCost(ing_prod, item.Quantity, item.UnitMeasure);
                //decimal this_item_cost = ing_prod.CostPrice * (decimal)(item.Quantity * item.UnitMeasure.ToBaseConversion) /
                //  (decimal)(ing_prod.CostQuantity * ing_prod.CostUnitMeasure.ToBaseConversion);
                total_cost += this_item_cost;
            }

            recipe.CostQuantity = 1;
            recipe.CostUnitMeasure = appvm.UnitMeasureManager.Unit;//recipe.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
            recipe.CostPrice = total_cost;
        }

        #endregion

        //#region Create Command

        //RelayCommand createCommand;
        //public ICommand CreateCommand 
        //{
        //    get 
        //    {
        //        if (createCommand == null)
        //            createCommand = new RelayCommand(x => Create(), x => CanCreate);

        //        return createCommand; 
        //    }            
        //}

        //bool CanCreate { get { return UMFamily != null; } }

        //void Create() 
        //{
        //    Product p = new Product();

        //    ProductCost pc = new ProductCost();
        //    p.ProductCost = pc;

        //    CopyToProduct(p);

        //    appvm.ProductsOC.Add(p);

        //    appvm.SaveChanges();

        //    onProductCreated(p);

        //    Creating = false;

        //    icvIngredients.Refresh();
        //}

        //#endregion

        #region Cancel Command

        RelayCommand cancelCommand;
        public ICommand CancelCommand 
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new RelayCommand(x => Cancel());
                return cancelCommand; 
            }
        }

        void Cancel() 
        {
            if (creating)
            {
                var windowManager = base.GetService<IWindowManager>();
                windowManager.Close(this);
            }
            else
            {
                CopyFromProduct();

                icvCategories.Refresh();
                icvIngredients.Refresh();
                //icvFormats.Refresh();

                HasPendingChanges = false;
            }
        }

        #endregion

        bool creating;
        //public bool Creating
        //{
        //    get { return creating; }
        //    set
        //    {
        //        creating = value;
        //        OnPropertyChanged("Creating");
        //        //OnPropertyChanged("CreatingOrEditing");
        //        //OnPropertyChanged("EditButtonVisible");
        //        //OnPropertyChanged("CancelButtonVisible");
                
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

        //public bool HasCategories
        //{
        //    get { return relatedCategories.Count > 0; }
        //}

        //#region IDataErrorInfor Members

        //public string Error
        //{
        //    get
        //    { return null; }
        //}

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        string result = null;
        //        hasErrors = false;

        //        if (columnName == "CostQuantityString")
        //        {
        //            if (costUM.UMFamily != umFamily)
        //            {
        //                result = "La unidad de medida no coincide con el producto";
        //                hasErrors = true;
        //            }
        //        }
        //        else if (columnName == "UMFamily")
        //        {
        //            if (costUM.UMFamily != umFamily)
        //            {
        //                result = "La medida no coincide con el costo";
        //                hasErrors = true;
        //            }
        //        }
        //        return result;
        //    }
        //}

        //#endregion

        bool IScreen.TryToClose()
        {
            if (hasPendingChanges && this.CanSave)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                if (msgBox != null)
                {
                    var result = msgBox.Show("¿Desea guardar los cambios antes de salir?",
                        "Guardar cambios",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Cancel)
                        return false;

                    if (result == MessageBoxResult.Yes)
                        this.Save();
                }
            }
            return true;
        }
    }
}
