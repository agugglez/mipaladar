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
using System.IO;
using System.Windows;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.MVVM;
using MiPaladar.Repository;


namespace MiPaladar.ViewModels
{

    public class ProductViewModel : ViewModelWithClose//, IDataErrorInfo
    {
        //Product product;
        MainWindowViewModel appvm;
        //ProductManager productManager;
        //PatternMatcher patternMatcher;

        //int productId;
        //Action<Product> onProductCreated;
        //Action<int> onRemoved;
        //Action<Product> onCategoryChanged;

        //design
        public ProductViewModel() { }

        ////create new product
        public ProductViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
            LoadRolePermissions();
            
            //this.productManager = appvm.ProductManager;
            //this.patternMatcher = appvm.PatternMatcher;
            //this.onProductCreated = onProductCreated;
            //this.onRemoved = onRemoved;

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                UpdateTypeFromFamily();
                UpdateVisibilityFields();

                //TAGS
                restOfTags.Clear();
                foreach (var item in unitOfWork.TagRepository.Get())
                {
                    restOfTags.Add(item);
                }
            }            

            SetUpEvents();

            //minimumStockUM = arbitraryCostUM = appvm.UnitMeasureManager.Unit;
            
            //recipeQtty = 1;
            //recipeUM = appvm.UnitMeasureManager.Unit;

            creating = true;
            HasPendingChanges = true;
        }
        public ProductViewModel(MainWindowViewModel appvm, int prodId)
        {
            this.appvm = appvm;
            LoadRolePermissions();
            //this.productManager = appvm.ProductManager;
            //this.patternMatcher = appvm.PatternMatcher;
            //this.onRemoved = onRemoved;
            //this.onCategoryChanged = onCategoryChanged;

            this.prodId = prodId;

            using (var unitOfWork = base.GetNewUnitOfWork())
            {           

                CopyFromProduct(unitOfWork);
            }

            SetUpEvents();
            
            HasPendingChanges = false;
        }

        void LoadRolePermissions()
        {
            canRemove = appvm.LoggedInUser.Role.CanRemoveProducts;
            canEdit = appvm.LoggedInUser.Role.CanEditProducts;
        }
        //public ProductViewModel(MainWindowViewModel appvm, Product product)
        //{
        //    this.appvm = appvm;
        //    //this.productManager = appvm.ProductManager;
        //    //this.patternMatcher = appvm.PatternMatcher;
        //    //this.onRemoved = onRemoved;
        //    //this.onCategoryChanged = onCategoryChanged;

        //    this.product = product;
        //    this.Id = product.Id;

        //    DefaultValues();

        //    //SetUpEvents();

        //    CopyFromProduct();
        //    HasPendingChanges = false;
        //}

        bool canRemove;
        bool canEdit;

        public override string DisplayName
        {
            get { return string.IsNullOrWhiteSpace(Name) ? "Nuevo Producto" : Name; }
        }

        protected override void OnDispose()
        {
            UnLoadEvents();
        }

        #region Global Events

        void SetUpEvents()
        {
            //categories
            appvm.GlobalEventsManager.CategoryCreated += GlobalEventsManager_CategoryCreated;
            appvm.GlobalEventsManager.CategoryModified += GlobalEventsManager_CategoryModified;
            appvm.GlobalEventsManager.CategoryRemoved += GlobalEventsManager_CategoryRemoved;

            //tags
            //appvm.GlobalEventsManager.TagCreated += GlobalEventsManager_TagCreated;
            //appvm.GlobalEventsManager.TagRemoved += GlobalEventsManager_TagRemoved;
            //appvm.GlobalEventsManager.TagModified += GlobalEventsManager_TagModified;
        }

        void UnLoadEvents()
        {
            appvm.GlobalEventsManager.CategoryCreated -= GlobalEventsManager_CategoryCreated;
            appvm.GlobalEventsManager.CategoryModified -= GlobalEventsManager_CategoryModified;
            appvm.GlobalEventsManager.CategoryRemoved -= GlobalEventsManager_CategoryRemoved;

            //appvm.GlobalEventsManager.TagCreated -= GlobalEventsManager_TagCreated;
            //appvm.GlobalEventsManager.TagRemoved -= GlobalEventsManager_TagRemoved;
            //appvm.GlobalEventsManager.TagModified -= GlobalEventsManager_TagModified;
        }

        //TAGS
        //void GlobalEventsManager_TagCreated(object sender, TagInfoEventArgs e)
        //{
        //    //add new tag to list
        //    restOfTags.Add(appvm.TagsOC.Single(x => x.Id == e.TagId));
        //}

        //void GlobalEventsManager_TagRemoved(object sender, TagInfoEventArgs e)
        //{
        //    //remove it from added tags
        //    var query = tags.Where(x => x.Id == e.TagId);
        //    if (query.Count() > 0)
        //    {
        //        tags.Remove(query.First());
        //    }

        //    //remove it from rest of tags
        //    query = restOfTags.Where(x => x.Id == e.TagId);
        //    if (query.Count() > 0)
        //    {
        //        restOfTags.Remove(query.First());
        //    }
        //}      

        //CATEGORIES
        void GlobalEventsManager_CategoryRemoved(object sender, CategoryInfoEventArgs e)
        {
            SyncCategoryTreeAndSltdCategory();
        }

        void GlobalEventsManager_CategoryModified(object sender, CategoryInfoEventArgs e)
        {
            SyncCategoryTreeAndSltdCategory();
        }

        void GlobalEventsManager_CategoryCreated(object sender, CategoryInfoEventArgs e)
        {
            SyncCategoryTreeAndSltdCategory();
        }

        public void SyncCategoryTreeAndSltdCategory()
        {
            int catId = sltdCategory != null ? sltdCategory.Id : 0;
            CreateCategoryTree();
            UpdateSelectedCategory(catId);
        }

        private void UpdateSelectedCategory(int id)
        {
            if (id == 0) return;

            sltdCategory = CategoryTree.Single(x => x.Id == id);

            OnPropertyChanged("SelectedCategory");
        }

        #endregion

        //public MainWindowViewModel AppVM { get { return appvm; } }

        int prodId;
        public int ProductId { get { return prodId; } }
        //public Product WrappedProduct { get { return product; } }

        #region Copy Methods

        private void CopyFromProduct(IUnitOfWork unitOfWork)
        {
            //Product product = unitOfWork.ProductRepository.Get(x => x.Id == prodId, includeProperties: "Ingredients").First();

            Product product = unitOfWork.ProductRepository.GetById(prodId);

            //PRODUCT TYPE
            if (pType != product.ProductType)
            {
                pType = product.ProductType;
            }

            //NAME
            if (name != product.Name)
            {
                name = product.Name;
            }
            //DESCRIPTION
            if (description != product.Description) description = product.Description;

            //PRINTSTRING
            if (printString != product.PrintString) printString = product.PrintString;   

            //CATEGORY
            if (product.Category != null) sltdCategory = CategoryTree.Single(x => x.Id == product.Category.Id);

            if (pType == ProductType.FinishedGoods || pType == ProductType.CompraVenta)
            {
                //PLU
                if (product.Code != code)
                {
                    code = product.Code;
                }

                //SALE PRICE
                if (salePrice != product.SalePrice)
                {
                    salePrice = product.SalePrice;
                }

                //UNIT MEASURE FAMILY
                if (umFamilyId != 1)
                {
                    umFamilyId = 1;
                }
            }

            if (pType == ProductType.WorkInProcess || pType == ProductType.RawMaterials)
            {
                //UNIT MEASURE FAMILY
                if (umFamilyId != product.UMFamilyId)
                {
                    umFamilyId = product.UMFamilyId;
                }
            }

            UpdateTypeFromFamily();

            UpdateVisibilityFields();
            
            if (pType == ProductType.FinishedGoods || pType == ProductType.WorkInProcess)
            {
                //RECIPE QUANTITY
                if (recipeQtty != product.RecipeQuantity)
                {
                    recipeQtty = product.RecipeQuantity;
                }

                //PRODUCTION PROCESS
                if (productionProcess != product.ProductionProcess) productionProcess = product.ProductionProcess;
            }

            if (pType == ProductType.FinishedGoods )
            {                
                //RECIPE UNIT MEASURE
                if (recipeUMId != 1)
                {
                    recipeUMId = 1;
                }
            }

            if (pType == ProductType.WorkInProcess)
            {
                //RECIPE UNIT MEASURE
                if (recipeUMId != product.RecipeUnitMeasure_Id)
                {
                    recipeUMId = product.RecipeUnitMeasure_Id;                    
                }
            }

            if (pType == ProductType.FinishedGoods || pType == ProductType.WorkInProcess)
            {
                //INGREDIENTS
                CopyIngredientsFromProduct(product);

                //COMPONENTS COST
                UpdateIngredientsCost();
            }

            //ARBITRARY COST
            if (arbitraryCost != product.ArbitraryCost) ArbitraryCost = product.ArbitraryCost;

            if (pType == ProductType.FinishedGoods || pType == ProductType.CompraVenta)
            {
                //ARBITRARY COST UNIT MEASURE
                if (arbitraryCostUMId != 1)
                {
                    arbitraryCostUMId = 1;
                }
            }
            else
            {
                //ARBITRARY COST UNIT MEASURE
                if (arbitraryCostUMId != product.CostUMId)
                {
                    arbitraryCostUMId = product.CostUMId;
                }
            }

            //TOTAL COST
            if (totalCost != product.CostPrice) TotalCost = product.CostPrice;

            if (pType == ProductType.RawMaterials)
            {
                //EDIBLE PART is stored as 0<=x<=1
                if (ediblePart != product.EdiblePart * 100) EdiblePart = product.EdiblePart * 100;
            }
            
            //OUTGREDIENTS
            outgredients.Clear();
            foreach (var item in product.Outgredients)
            {
                OutgredientViewModel ovm = new OutgredientViewModel(item);
                outgredients.Add(ovm);
            }

            //TAGS
            CopyTagsFromProduct(unitOfWork, product);

            //isRecipe = product.IsRecipe;
            //OnPropertyChanged("IsRecipe");

            //IsInMenu = product.IsInMenu;           

            //image
            //ImageFileName = product.ImageFileName;
            //BuildImageFullPath();

            if (DoneByUser != product.DoneByUser) DoneByUser = product.DoneByUser;
        }        

        private void CopyTagsFromProduct(IUnitOfWork unitOfWork, Product product)
        {
            tags.Clear();
            foreach (var item in product.Tags)
            {
                tags.Add(item);
            }

            restOfTags.Clear();
            var query = from t in unitOfWork.TagRepository.Get()
                        where !tags.Contains(t)
                        select t;

            foreach (var item in query)
            {
                restOfTags.Add(item);
            }
        }


        //private void BuildImageFullPath()
        //{
        //    if (string.IsNullOrWhiteSpace(imageFileName)) return;

        //    string tempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        //        App.ApplicationFolderName);
        //    tempFolder = Path.Combine(tempFolder, App.AppProductsFolderName);

        //    ImageFullPath = Path.Combine(tempFolder, imageFileName);
        //}                

        void CopyToProduct(IUnitOfWork unitOfWork, Product product)
        {
            //PRODUCT TYPE
            if (product.ProductType != pType) product.ProductType = pType;
            //NAME
            if (product.Name != Name) product.Name = Name;
            //CATEGORY
            if (CategoryChanged(product)) product.CategoryId = sltdCategory.Id;
            //PLU
            if (product.Code != code) product.Code = code;
            //SALEPRICE
            if (product.SalePrice != SalePrice) product.SalePrice = SalePrice;
            //DESCRIPTION
            if (product.Description != description) product.Description = description;
            //PRODUCTION PROCESS
            if (product.ProductionProcess != productionProcess) product.ProductionProcess = productionProcess;
            //PRINTSTRING
            if (product.PrintString != printString) product.PrintString = printString;
            //if (product.IsRecipe != isRecipe) product.IsRecipe = isRecipe;
            //if (product.IsInMenu != IsInMenu) product.IsInMenu = IsInMenu;

            //UoM FAMILY
            if (product.UMFamilyId != umFamilyId) product.UMFamilyId = umFamilyId;
            //EDIBLE PART
            if (product.EdiblePart * 100 != ediblePart) product.EdiblePart = ediblePart / 100;
            //RECIPE QUANTITY
            if (recipeQtty != product.RecipeQuantity) product.RecipeQuantity = recipeQtty;
            //RECIPE UNIT OF MEASURE
            if (recipeUMId != product.RecipeUnitMeasure_Id) product.RecipeUnitMeasure_Id = recipeUMId;                        
            //ARBITRARY COST
            if (arbitraryCost != product.ArbitraryCost) product.ArbitraryCost = arbitraryCost;
            //ARBITRARY COST UNIT OF MEASURE
            if (arbitraryCostUMId != product.CostUMId) product.CostUMId = arbitraryCostUMId;
            //TOTAL COST
            if (totalCost != product.CostPrice) product.CostPrice = totalCost;            
            
            //image
            //if (product.ImageFileName != imageFileName) product.ImageFileName = imageFileName;         

            //clear tag
            CopyTagsToProduct(product);
            
            CopyIngredientsToProduct(unitOfWork, product);

            if (DoneByUser != product.DoneByUser) product.DoneByUser = DoneByUser;
        }

        bool CategoryChanged(Product product)
        {
            if (product.CategoryId == null && sltdCategory == null) return false;

            if (product.CategoryId != null && sltdCategory != null && product.CategoryId == sltdCategory.Id) return false;

            return true;
        }

        //Category GetCategoryFromId(int cat_id)
        //{
        //    return appvm.CategoriesOC.Single(x => x.Id == cat_id);
        //}
        
        private void CopyTagsToProduct(Product product)
        {
            var product_tags = product.Tags.ToList();

            //check for added tags
            foreach (var item in tags)
            {
                if (product.Tags.Count == 0)
                {
                    product.Tags.Add(item);
                    continue;
                }

                if (product.Tags.FirstOrDefault(x => x.Id == item.Id) == null)
                {
                    product.Tags.Add(item);
                }
            }

            //check for removed tags
            foreach (var item in product_tags)
            {
                var result = tags.FirstOrDefault(x => x.Id == item.Id);

                if (result != null) continue;

                product.Tags.Remove(result);
            }
        }

        private void CopyIngredientsToProduct(IUnitOfWork unitOfWork, Product product)
        {
            var ings_list = product.Ingredients.ToList();

            //check for removed ingredients
            foreach (var item in ings_list)
            {
                if (ingredients.Where(x => x.Id == item.Id).Count() > 0) continue;

                unitOfWork.IngredientRepository.Remove(item.Id);
                //appvm.Context.Ingredients.DeleteObject(item);
                //product.Ingredients.Remove(item);
            }

            //check for added or modified ingredients
            foreach (var item in ingredients)
            {
                //new
                if (item.Id == 0) 
                {
                    Ingredient newIng = new Ingredient();
                    newIng.Quantity = item.Quantity;
                    newIng.UnitMeasureId = item.UnitMeasureId;
                    newIng.IngredientProductId = item.ProductId;

                    product.Ingredients.Add(newIng);

                    unitOfWork.IngredientRepository.Add(newIng);
                    //appvm.Context.Ingredients.AddObject(newIng);                    
                }
                    //modified
                else
                {
                    Ingredient original = product.Ingredients.Single(x => x.Id == item.Id);

                    if (original.Quantity != item.Quantity) original.Quantity = item.Quantity;
                    if (original.UnitMeasureId != item.UnitMeasureId) original.UnitMeasureId = item.UnitMeasureId;
                    if (original.IngredientProductId != item.ProductId) original.IngredientProductId = item.ProductId;
                }
            }
            //while (product.Ingredients.Count > 0)
            //{
            //    Ingredient ing = product.Ingredients.First();
            //    appvm.Context.Ingredients.DeleteObject(ing);
            //}
            //foreach (var item in Ingredients)
            //{
            //    Ingredient ing = new Ingredient();
            //    ing.Quantity = item.Quantity;
            //    ing.IngredientProduct = item.Product;
            //    ing.UnitMeasure = item.UnitMeasure;

            //    product.Ingredients.Add(ing);
            //}
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
        void UpdateIngredientsCost()
        {
            decimal tmp_cost = 0;

            foreach (var item in this.Ingredients)
            {
                tmp_cost += item.ItemCost;
                //total_cost += item.Product.PurchasePrice * (decimal)(item.Quantity * item.UnitMeasure.ToBaseConversion);
            }

            TotalIngredientsCost = tmp_cost;
        }

        #endregion        

        #region Check Unique Name

        BackgroundWorker uniqueNameWorker;

        void CheckUniqueNameAsync()
        {
            if (uniqueNameWorker == null)
            {
                uniqueNameWorker = new BackgroundWorker();

                uniqueNameWorker.DoWork += uniqueNameWorker_DoWork;

                uniqueNameWorker.RunWorkerCompleted += uniqueNameWorker_RunWorkerCompleted;
            }

            if (uniqueNameWorker.IsBusy) return;

            uniqueNameWorker.RunWorkerAsync();
        }

        void uniqueNameWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var invSvc = base.GetService<IInventoryService>();

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                e.Result = creating ? invSvc.NameIsUnique(unitOfWork, name) : invSvc.NameIsUnique(unitOfWork, name, ProductId);            
            }            
        }

        void uniqueNameWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            NameNotUnique = !(bool)e.Result;

            uniqueNameWorker.DoWork -= uniqueNameWorker_DoWork;
            uniqueNameWorker.RunWorkerCompleted -= uniqueNameWorker_RunWorkerCompleted;
            uniqueNameWorker = null;
        }        

        bool nameNotUnique;
        public bool NameNotUnique
        {
            get { return nameNotUnique; }
            set
            {
                nameNotUnique = value;
                OnPropertyChanged("NameNotUnique");
            }
        }

        #endregion

        #region Check Unique PLU

        BackgroundWorker uniquePLUWorker;

        void CheckUniquePLUAsync() 
        {
            if (uniquePLUWorker == null)
            {
                uniquePLUWorker = new BackgroundWorker();

                uniquePLUWorker.DoWork += uniquePLUWorker_DoWork;
                uniquePLUWorker.RunWorkerCompleted += uniquePLUWorker_RunWorkerCompleted;
            }

            if (uniquePLUWorker.IsBusy) return;

            uniquePLUWorker.RunWorkerAsync();
        }

        void uniquePLUWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var invSvc = base.GetService<IInventoryService>();

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                e.Result = creating ? invSvc.PLUIsUnique(unitOfWork, code) : invSvc.PLUIsUnique(unitOfWork, code, ProductId);
            }            
        }

        void uniquePLUWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PLUNotUnique = !(bool)e.Result;

            uniquePLUWorker.DoWork -= uniquePLUWorker_DoWork;
            uniquePLUWorker.RunWorkerCompleted -= uniquePLUWorker_RunWorkerCompleted;
            uniquePLUWorker = null;
        }

        bool pluNotUnique;
        public bool PLUNotUnique
        {
            get { return pluNotUnique; }
            set
            {
                pluNotUnique = value;
                OnPropertyChanged("PLUNotUnique");
            }
        }

        #endregion

        #region Properties

        bool doneByUser;
        public bool DoneByUser
        {
            get { return doneByUser; }
            set
            {
                doneByUser = value;
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

                if (!string.IsNullOrWhiteSpace(name)) CheckUniqueNameAsync();

                CommandManager.InvalidateRequerySuggested();
                
                HasPendingChanges = true;

                OnPropertyChanged("Name");
            }
        }

        ProductType pType = ProductType.FinishedGoods;

        public ProductType ProductType
        {
            get { return pType; }
            set
            {
                if (pType != value)
                {
                    pType = value;

                    UpdateVisibilityFields();

                    HasPendingChanges = true;

                    OnPropertyChanged("ProductType");
                }
            }
        }

        CategoryRowViewModel sltdCategory;
        public CategoryRowViewModel SelectedCategory
        {
            get { return sltdCategory; }
            set
            {
                CategoryRowViewModel oldRow = sltdCategory;

                if (sltdCategory != value)
                {
                    sltdCategory = value;

                    //ADD NEW CATEGORY IN PLACE
                    if (sltdCategory != null && sltdCategory.IsAddNew)
                    {
                        var windowManager = base.GetService<IWindowManager>();

                        CategoryViewModel cvm = new CategoryViewModel(appvm);

                        if (windowManager.ShowDialog(cvm, this) == true)
                        {
                            sltdCategory = categoryTree.Single(x => x.Id == cvm.CategoryId);
                        }
                        else
                        {                            
                            //just for combobox to end up blank
                            CreateCategoryTree();

                            if (oldRow != null) sltdCategory = categoryTree.Single(x => x.Id == oldRow.Id);
                        }
                    }

                    HasPendingChanges = true;

                    OnPropertyChanged("SelectedCategory");
                }
            }
        }

        public bool NoCategory { get { return sltdCategory == null; } }

        int code;
        public int Code
        {
            get { return code; }
            set
            {
                code = value;

                CheckUniquePLUAsync();

                HasPendingChanges = true;

                OnPropertyChanged("Code");
            }
        }

        public bool NoPLU { get { return code == 0; } }

        decimal salePrice;
        public decimal SalePrice
        {
            get { return salePrice; }
            set
            {
                salePrice = value;

                Profit = salePrice - totalCost;

                UpdateRatios();
                
                HasPendingChanges = true;

                OnPropertyChanged("SalePrice");
            }
        }

        public bool NoPrice { get { return salePrice == 0; } }

        double ediblePart = 100;

        public double EdiblePart
        {
            get { return ediblePart; }
            set 
            {
                ediblePart = value;

                HasPendingChanges = true;

                OnPropertyChanged("EdiblePart");
            }
        }

        ObservableCollection<CategoryRowViewModel> categoryTree;
        public ObservableCollection<CategoryRowViewModel> CategoryTree 
        {
            get 
            {
                if (categoryTree == null)
                {
                    categoryTree = new ObservableCollection<CategoryRowViewModel>();

                    CreateCategoryTree();                   
                }
                return categoryTree;
            }
        }

        private void CreateCategoryTree()
        {
            categoryTree.Clear();

            CategoryRowViewModel addNewCategoryRow = new CategoryRowViewModel();
            addNewCategoryRow.Name = "...Añadir Nueva";
            addNewCategoryRow.IsAddNew = true;

            categoryTree.Add(addNewCategoryRow);

            //create category tree
            var inventorySvc = base.GetService<IInventoryService>();
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                inventorySvc.CreateCategoryList(unitOfWork, categoryTree);
            }            
        }

        #region Available UnitOfMeasures

        List<UnitMeasure> allUMs;
        ObservableCollection<UnitMeasure> availableUMs = new ObservableCollection<UnitMeasure>();

        public ObservableCollection<UnitMeasure> AvailableUMs
        {
            get
            {
                if (allUMs == null)
                {
                    UpdateAvailableUMs();
                }
                return availableUMs;
            }
        }

        private void UpdateAvailableUMs()
        {
            if (allUMs == null)
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    allUMs = unitOfWork.UMRepository.Get();
                }
            }

            availableUMs.Clear();
            foreach (var item in allUMs)
            {
                if (item.UMFamilyId == umFamilyId) availableUMs.Add(item);
            }
        }

        #endregion

        #region Unit of Measure Type

        int umFamilyId = 1;
        public int UMFamilyId
        {
            get { return umFamilyId; }
            set
            {
                if (umFamilyId != value)
                {
                    umFamilyId = value;

                    OnPropertyChanged("UMFamilyId");
                }
            }
        }        

        private void UpdateTypeFromFamily()
        {
            if (umFamilyId == 1) { umIsCountType = true; }
            else if (umFamilyId == 2) { umIsWeightType = true; }
            else if (umFamilyId == 3) { umIsVolumeType = true; }
            else
            {
                System.Diagnostics.Debug.Assert(umFamilyId == 4);

                umIsLengthType = true;
            }
        }

        private void UpdateFamilyFromType()
        {
            if (umIsCountType) { UMFamilyId = 1; }
            else if (umIsWeightType) { UMFamilyId = 2; }
            else if (umIsVolumeType) { UMFamilyId = 3; }
            else //if (umIsLengthType)
            {
                System.Diagnostics.Debug.Assert(umIsLengthType);

                UMFamilyId = 4;
            }

            UpdateAvailableUMs();

            //get first um when family changes
            RecipeUnitMeasureId = CostUnitMeasureId = availableUMs.First().Id;

            UpdateVisibilityFields();

            HasPendingChanges = true;
        }

        int umTypeCounter = 0;

        bool umIsCountType;

        public bool UMIsCountType
        {
            get { return umIsCountType; }
            set
            {
                if (umIsCountType != value)
                {
                    umIsCountType = value;
                    OnPropertyChanged("UMIsCountType");

                    if (++umTypeCounter % 2 == 0) UpdateFamilyFromType();
                }
            }
        }

        bool umIsWeightType;

        public bool UMIsWeightType
        {
            get { return umIsWeightType; }
            set
            {
                if (umIsWeightType != value)
                {
                    umIsWeightType = value;

                    if (++umTypeCounter % 2 == 0) UpdateFamilyFromType();
                }
                
            }
        }
        bool umIsVolumeType;

        public bool UMIsVolumeType
        {
            get { return umIsVolumeType; }
            set
            {
                if (umIsVolumeType != value)
                {
                    umIsVolumeType = value;

                    if (++umTypeCounter % 2 == 0) UpdateFamilyFromType();
                }                
            }
        }
        bool umIsLengthType;

        public bool UMIsLengthType
        {
            get { return umIsLengthType; }
            set
            {
                if (umIsLengthType != value)
                {
                    umIsLengthType = value;

                    if (++umTypeCounter % 2 == 0) UpdateFamilyFromType();
                }                
            }
        }

        #endregion        
        
        //#region Image Properties

        //string imageFileName;
        //public string ImageFileName
        //{
        //    get { return imageFileName; }
        //    set
        //    {
        //        imageFileName = value;
        //        OnPropertyChanged("ImageFileName");

        //        HasPendingChanges = true;
        //    }
        //}

        //string fullImagePath;
        //public string ImageFullPath
        //{
        //    get { return fullImagePath; }
        //    set
        //    {
        //        fullImagePath = value;
        //        OnPropertyChanged("ImageFullPath");
        //        OnPropertyChanged("NoPicture");
        //    }
        //}

        //public bool NoPicture
        //{
        //    get { return string.IsNullOrEmpty(fullImagePath); }
        //}

        //#endregion

        string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;

                HasPendingChanges = true;

                OnPropertyChanged("Description");                
            }
        }

        string productionProcess;

        public string ProductionProcess
        {
            get { return productionProcess; }
            set
            {
                productionProcess = value;

                HasPendingChanges = true;

                OnPropertyChanged("ProductionProcess");
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

        #region Recipe Quantity

        double recipeQtty = 1;
        public double RecipeQuantity
        {
            get { return recipeQtty; }
            set
            {
                recipeQtty = value;

                UpdateIngredientsCostRate();

                UpdateTotalCost();
                
                HasPendingChanges = true;

                OnPropertyChanged("RecipeQuantity");
            }
        }

        int recipeUMId = 1;
        public int RecipeUnitMeasureId
        {
            get { return recipeUMId; }
            set
            {
                if (recipeUMId != value)
                {
                    recipeUMId = value;

                    //keep recipe and cost UMs synced
                    arbitraryCostUMId = value;
                    OnPropertyChanged("CostUnitMeasureId");

                    //UpdateIngredientsCostRate();

                    //UpdateTotalCost();

                    HasPendingChanges = true;

                    OnPropertyChanged("RecipeUnitMeasureId");
                }                
            }
        }

        #endregion        

        #region Product Cost 

        //price by unit measure, will change name later
        //decimal priceRate;
        //public decimal PriceRate
        //{
        //    get { return priceRate; }
        //    set
        //    {
        //        priceRate = value;
        //        OnPropertyChanged("PriceRate");

        //        HasPendingChanges = true;
        //    }
        //}

        //decimal average_cost;

        //public decimal AverageCost
        //{
        //    get { return average_cost; }
        //    set 
        //    {
        //        average_cost = value;
        //        OnPropertyChanged("AverageCost");
        //    }
        //}

        //sum of the cost of components
        decimal ingsCost;
        public decimal TotalIngredientsCost
        {
            get { return ingsCost; }
            set
            {
                ingsCost = value;                

                UpdateIngredientsCostRate();

                UpdateTotalCost();

                OnPropertyChanged("TotalIngredientsCost");
            }
        }

        private void UpdateIngredientsCostRate()
        {
            IngredientsCostRate = isRecipe && recipeQtty != 0 ? ingsCost / (decimal)recipeQtty : 0;
        }

        decimal ingsCostRate;
        /// <summary>
        /// cost of ingredient per Unit of Measure specified in Recipe Unit of Measure
        /// </summary>
        public decimal IngredientsCostRate
        {
            get { return ingsCostRate; }
            set 
            {
                ingsCostRate = value;
                OnPropertyChanged("IngredientsCostRate");
            }
        }

        //optional cost that can be added to the components cost
        decimal arbitraryCost;
        public decimal ArbitraryCost
        {
            get { return arbitraryCost; }
            set
            {
                arbitraryCost = value;

                UpdateTotalCost();

                OnPropertyChanged("ArbitraryCost");

                HasPendingChanges = true;
            }
        }

        void UpdateTotalCost() 
        {
            TotalCost = ingsCostRate + arbitraryCost; 
        }

        //total cost of product = components cost + arbitrary cost
        decimal totalCost;
        public decimal TotalCost
        {
            get { return totalCost; }
            set
            {
                totalCost = value;

                if (pType == ProductType.FinishedGoods || pType == ProductType.CompraVenta)
                {
                    Profit = salePrice - totalCost;

                    UpdateRatios();
                }

                HasPendingChanges = true;

                OnPropertyChanged("TotalCost");
            }
        }

        public bool NoCost 
        {
            get { return (pType == ProductType.RawMaterials || pType == ProductType.CompraVenta) && totalCost == 0; } 
        }

        void UpdateRatios() 
        {
            //update cost ratio
            CostToPriceRatio = salePrice > 0 ? totalCost * 100 / salePrice : 0;

            ProfitToPriceRatio = salePrice > 0 ? profit * 100 / salePrice : 0;
        }

        decimal costToPriceRatio;

        public decimal CostToPriceRatio
        {
            get { return costToPriceRatio; }
            set
            {
                costToPriceRatio = value;
                OnPropertyChanged("CostToPriceRatio");
            }
        }

        decimal profit;

        public decimal Profit
        {
            get { return profit; }
            set
            {
                profit = value;
                OnPropertyChanged("Profit");
            }
        }

        decimal profitToPriceRatio;

        public decimal ProfitToPriceRatio
        {
            get { return profitToPriceRatio; }
            set
            {
                profitToPriceRatio = value;
                OnPropertyChanged("ProfitToPriceRatio");
            }
        }

        RelayCommand showCostHelperCmd;
        public RelayCommand ShowCostHelperCommand
        {
            get
            {
                if (showCostHelperCmd == null)
                    showCostHelperCmd = new RelayCommand(x => ShowCostHelper());
                return showCostHelperCmd;
            }
        }

        void ShowCostHelper()
        {
            var windowManager = base.GetService<IWindowManager>();
            CostHelperViewModel ch = new CostHelperViewModel(arbitraryCostUMId, ArbitraryCost);
            
            if (windowManager.ShowDialog(ch, this) == true)
            {
                CostUnitMeasureId = ch.ResultUMId;
                ArbitraryCost = ch.CostResult;
            }
        }

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
        int arbitraryCostUMId = 1;
        public int CostUnitMeasureId
        {
            get { return arbitraryCostUMId; }
            set
            {
                arbitraryCostUMId = value;

                //keep cost and recipe UMs synced
                recipeUMId = value;
                OnPropertyChanged("RecipeUnitMeasureId");
                
                HasPendingChanges = true;

                OnPropertyChanged("CostUnitMeasureId");
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

        #endregion               

        #region Visibility Fields

        //bool umFamilyNotQtty;
        
        //public bool UmFamilyNotQtty
        //{
        //    get { return umFamilyNotQtty; }
        //    set
        //    {
        //        umFamilyNotQtty = value;
        //        OnPropertyChanged("UmFamilyNotQtty");
        //    }
        //}

        bool isInSales;

        public bool IsInSales
        {
            get { return isInSales; }
            set
            {
                isInSales = value;
                OnPropertyChanged("IsInSales");
            }
        }
        bool isRecipe;

        public bool IsRecipe
        {
            get { return isRecipe; }
            set
            {
                isRecipe = value;
                OnPropertyChanged("IsRecipe");
            }
        }

        bool isRawMaterial;

        public bool IsRawMaterial
        {
            get { return isRawMaterial; }
            set
            {
                isRawMaterial = value;
                OnPropertyChanged("IsRawMaterial");
            }
        }

        bool isWIP;
        public bool IsWorkInProgress
        {
            get { return isWIP; }
            set
            {
                isWIP = value;
                OnPropertyChanged("IsWorkInProgress");
            }
        }

        void UpdateVisibilityFields()
        {
            //UmFamilyNotQtty = umFamily != appvm.UnitMeasureManager.Quantity;

            IsInSales = pType == ProductType.FinishedGoods || pType == ProductType.CompraVenta;

            IsRecipe = pType == ProductType.FinishedGoods || pType == ProductType.WorkInProcess;

            IsRawMaterial = pType == ProductType.RawMaterials;

            IsWorkInProgress = pType == ProductType.WorkInProcess;
        }

        #endregion

        //#region Select Image Command

        //RelayCommand selectImageCommand;
        //public ICommand SelectImageCommand
        //{
        //    get
        //    {
        //        if (selectImageCommand == null)
        //            selectImageCommand = new RelayCommand(x => SelectImage());
        //        return selectImageCommand;
        //    }
        //}

        //void SelectImage()
        //{
        //    var open_file_dialog = base.GetService<IOpenFileDialogService>();

        //    string title = "Buscar fichero de reporte";
        //    string filter = "Imágenes|*.bmp;*.gif;*.ico;*.jpg;*.png;*.wdp;*.tiff";

        //    if (open_file_dialog.ShowDialog(title, filter) == true)
        //    {
        //        ImageFullPath = open_file_dialog.FileName;

        //        ImageFileName = Path.GetFileName(ImageFullPath);
                
        //        var copySvc = base.GetService<IFileCopyService>();

        //        copySvc.CopyImage(ImageFullPath, App.AppProductsFolderName);
        //    }
        //}

        //#endregion

        #region Tags

        ObservableCollection<Tag> tags = new ObservableCollection<Tag>();
        public ObservableCollection<Tag> Tags
        {
            get { return tags; }
        }

        ObservableCollection<Tag> restOfTags = new ObservableCollection<Tag>();
        public ObservableCollection<Tag> RestOfTags
        {
            get { return restOfTags; }
        }

        #region Create New Tag Command

        RelayCommand ctCommand;
        public ICommand CreateNewTagCommand
        {
            get
            {
                if (ctCommand == null)
                    ctCommand = new RelayCommand(x => NewTag());
                return ctCommand;
            }
        }

        void NewTag()
        {
            var windowManager = base.GetService<IWindowManager>();

            TagViewModel cvm = new TagViewModel(appvm);

            if (windowManager.ShowDialog(cvm, this) == true)
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    AddTag(unitOfWork.TagRepository.GetById(cvm.TagId));
                }                
            }
        }

        #endregion 

        #region Add Tag Command

        Tag tagToAdd;
        public Tag TagToAdd
        {
            get { return tagToAdd; }
            set
            {
                tagToAdd = value;
                OnPropertyChanged("TagToAdd");
            }
        }

        //public string SearchTextCategory { get; set; }

        RelayCommand addTagCommand;
        public RelayCommand AddTagCommand
        {
            get
            {
                if (addTagCommand == null)
                    addTagCommand = new RelayCommand(x => AddTag(tagToAdd),
                        x => CanAddTag);
                return addTagCommand;
            }
        }

        bool CanAddTag { get { return tagToAdd != null; } }

        void AddTag(Tag t)
        {
            tags.Add(t);

            restOfTags.Remove(t);

            HasPendingChanges = true;
        }

        #endregion

        #region Remove Tag Command

        Tag tagToRemove;

        public Tag TagToRemove
        {
            get { return tagToRemove; }
            set
            {
                tagToRemove = value;
                OnPropertyChanged("TagToRemove");
            }
        }

        RelayCommand removeTagCommand;
        public ICommand RemoveTagCommand
        {
            get
            {
                if (removeTagCommand == null)
                    removeTagCommand = new RelayCommand(x => RemoveTag(tagToRemove), x => CanRemoveTag);
                return removeTagCommand;
            }
        }

        bool CanRemoveTag { get { return tagToRemove != null; } }

        void RemoveTag(Tag t)
        {
            tags.Remove(t);
            restOfTags.Add(t);

            HasPendingChanges = true;
        }

        #endregion        

        #endregion        

        ObservableCollection<OutgredientViewModel> outgredients = new ObservableCollection<OutgredientViewModel>();
        public ObservableCollection<OutgredientViewModel> Outgredients
        {
            get { return outgredients; }
        }

        public bool NoOutgredients { get { return pType == ProductType.RawMaterials && outgredients.Count == 0; } }

        #region Open Outgredient Command

        public OutgredientViewModel SelectedOutgredient { get; set; }

        RelayCommand openProductCommand;
        public RelayCommand OpenOutgredientCommand
        {
            get
            {
                if (openProductCommand == null)
                    openProductCommand = new RelayCommand(x => OpenOutgredient(SelectedOutgredient), x => CanOpenOutgredient);
                return openProductCommand;
            }
        }

        bool CanOpenOutgredient { get { return SelectedOutgredient != null; } }

        void OpenOutgredient(OutgredientViewModel og)
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase vmb) =>
            {
                if (!(vmb is ProductViewModel)) return false;

                ProductViewModel vm = (ProductViewModel)vmb;

                return vm.ProductId == og.BaseProduct.Id;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                ProductViewModel pvm = new ProductViewModel(appvm, og.BaseProduct.Id);
                windowManager.ShowChildWindow(pvm, appvm);
            }
        }

        #endregion

        #region Ingredients

        ObservableCollection<IngredientViewModel> ingredients = new ObservableCollection<IngredientViewModel>();
        public ObservableCollection<IngredientViewModel> Ingredients
        {
            get { return ingredients; }
        }

        public bool NoIngredients { get { return ingredients.Count == 0; } }

        #region Ingredientes To Add

        //ICollectionView icvIngredients;
        //public ICollectionView IngredientsView
        //{
        //    get
        //    {
        //        if (icvIngredients == null)
        //        {
        //            CollectionViewSource cvs = new CollectionViewSource();
        //            cvs.Source = appvm.ProductsOC;
        //            icvIngredients = cvs.View;

        //            icvIngredients.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

        //            icvIngredients.Filter = x =>
        //            {
        //                Product p = (Product)x;

        //                //if (!p.IsIngredient) return false;

        //                //a product cant be ingredient of itself
        //                if (p == product) return false;

        //                //dont add ingredient twice
        //                foreach (var item in Ingredients)
        //                {
        //                    if (item.Product == p) return false;
        //                }

        //                if (!creating && CheckIngredientCycle(p)) return false;

        //                return true;
        //            };
        //        }
        //        return icvIngredients;
        //    }
        //}

        bool IngredientFilter(Product p)
        {
            //a product cant be ingredient of itself
            if (p.Id == prodId) return false;

            //dont add ingredient twice
            foreach (var item in Ingredients)
            {
                if (item.ProductId == p.Id) return false;
            }

            if (!creating && CheckIngredientCycle(p)) return false;

            return true;
        }

        /// <summary>
        /// check if the current product is ingredient of p, recursively
        /// </summary>
        /// <returns></returns>
        bool CheckIngredientCycle(Product productToCheck) 
        {
            if (productToCheck.ProductType == ProductType.FinishedGoods || productToCheck.ProductType == ProductType.WorkInProcess) 
            {
                foreach (var ing in productToCheck.Ingredients)
                {
                    //look for the current product in ingredients
                    if (ing.IngredientProductId == prodId) return true;

                    //check recursively in each ingredient
                    if (CheckIngredientCycle(ing.IngredientProduct)) return true;
                }
            }
            return false;
        }

        #endregion

        #region Add Ingredient Command               

        //Product ingredientToAdd;
        //public Product IngredientToAdd
        //{
        //    get { return ingredientToAdd; }
        //    set
        //    {
        //        ingredientToAdd = value;
        //        OnPropertyChanged("IngredientToAdd");
        //    }
        //}

        //public string SearchTextIngredient { get; set; }

        RelayCommand addIngredientCommand;
        public ICommand AddIngredientCommand
        {
            get
            {
                if (addIngredientCommand == null)
                {
                    addIngredientCommand = new RelayCommand(x => ShowAddIngredientDialog());
                }
                return addIngredientCommand;
            }
        }

        //bool CanAddIngredient
        //{
        //    get
        //    {
        //        return /*!ItemToAdd.HasErrors &&*/ ingredientToAdd != null && ingredient_quantity > 0;
        //    }
        //}

        private void ShowAddIngredientDialog()
        {
            AddLineDialogViewModel addLineDialog = new AddLineDialogViewModel(appvm, IngredientFilter);

            var windowManager = base.GetService<IWindowManager>();

            if (windowManager.ShowDialog(addLineDialog, this) == true)
            {
                double qtty = addLineDialog.Quantity;
                UnitMeasure um = addLineDialog.UnitOfMeasure;
                Product ingredient = addLineDialog.SelectedProduct;

                IngredientViewModel ing = new IngredientViewModel(qtty, um, ingredient, OnIngredientModified);

                ingredients.Add(ing);
                //appvm.SaveChanges();

                UpdateIngredientsCost();

                HasPendingChanges = true;
            }

            //var baseUM = ingredientToAdd.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);          

            //ing.BaseProduct = product;
            //ing.IngredientProduct = ingredientToAdd;
            //ing.Quantity = quantityToAdd;            

            //icvIngredients.Refresh();

            //IngredientQuantity = 0;
            //IngredientToAdd = null;
            //SearchTextIngredient = "";
            //OnPropertyChanged("SearchTextIngredient");            
        }

        void OnIngredientModified()
        {
            UpdateIngredientsCost();
            HasPendingChanges = true;
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

            UpdateIngredientsCost();

            //icvIngredients.Refresh();

            HasPendingChanges = true;
        }

        #endregion

        #endregion     

        //bool hasDifferentFormats;
        //public bool HasDifferentFormats 
        //{
        //    get { return hasDifferentFormats; }
        //    set
        //    {
        //        hasDifferentFormats = value;
        //        OnPropertyChanged("HasDifferentFormats");
        //    }
        //}
   
        //RelayCommand specifyFormatsCommand;
        //public ICommand SpecifyFormatsCommand 
        //{
        //    get 
        //    {
        //        if (specifyFormatsCommand == null)
        //            specifyFormatsCommand = new RelayCommand(x => SpecifyFormats());
        //        return specifyFormatsCommand;
        //    }
        //}

        //void SpecifyFormats()         
        //{ 
        //    HasDifferentFormats = true;
        //    HasPendingChanges = true;
        //}


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
        
        //public ObservableCollection<ProductionArea> ProductionAreas 
        //{
        //    get { return appvm.ProductionAreasOC; }
        //}

        //public IEnumerable<UnitMeasure> UnitMeasures
        //{
        //    get { return appvm.Context.UnitMeasures; }
        //}

        //public IEnumerable<UMFamily> UMFamilies 
        //{
        //    get { return appvm.Context.UMFamilies; }
        //}

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

        bool CanRemove
        {
            get
            {
                if (appvm.LoggedInUser == null || !appvm.LoggedInUser.Role.CanEditProducts) return false;
                return !creating;
            }
        }

        void Remove()
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = "¿Está seguro que desea eliminar este producto?";

            if (msgBox.ShowYesNoDialog(message) == true)
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {                    
                    //remove outgredients         

                    var prod = unitOfWork.ProductRepository.GetById(prodId);

                    var temp_outgredients = prod.Outgredients.Select(x => x.BaseProduct).ToList();

                    unitOfWork.ProductRepository.Remove(prodId);                    

                    //update cost of outgredients
                    var inventorySvc = base.GetService<IInventoryService>();

                    foreach (var item in temp_outgredients)
                    {
                        inventorySvc.UpdateProductCost(item);
                    }

                    unitOfWork.SaveChanges();
                }                

                CloseMe();

                appvm.GlobalEventsManager.FireProductRemoved(prodId);
            }
        }

        //private void CloseMe()
        //{
        //    //close this window
        //    var windowManager = base.GetService<IWindowManager>();
        //    selfClosing = true;
        //    windowManager.Close(this);
        //}

        void RemoveMe(IUnitOfWork uow)
        {
            //close
            //////clear categories
            //while (toRemove.RelatedCategories.Count > 0)
            //    appvm.Context.ProductIndexes.DeleteObject(toRemove.RelatedCategories.First());

            //remove IngredientProducts
            //var related_ingredientes = from ing in appvm.Context.Ingredients
            //                           where ing.IngredientProduct.Id == prod.Id
            //                           select ing;    

            

            //List<LineItem> lineitems_toRemove = new List<LineItem>();
            //var related_lineitems = from li in appvm.Context.LineItems
            //                        where li.Product.Id == product.Id
            //                        select li;

            //foreach (var item in related_lineitems.ToList())
            //{
            //    appvm.Context.LineItems.DeleteObject(item);
            //}

            //appvm.ProductManager.VentaItems.Remove(product);
            //appvm.ProductManager.PurchasableProducts.Remove(product);
            //appvm.ProductManager.InventoryProducts.Remove(product);
            //appvm.ProductManager.RecipeProducts.Remove(product);
            //appvm.ProductManager.IngredientProducts.Remove(product);

            
            //appvm.ProductsOC.Remove(product);
            //appvm.Context.Products.DeleteObject(product);

            //appvm.SaveChanges();

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
                    saveCommand = new RelayCommand(x => SaveAndClose(), x => this.CanSave);
                return saveCommand;
            }
        }

        //bool hasErrors;

        protected override bool CanSave
        {
            get
            {
                if (appvm.LoggedInUser != null && !appvm.LoggedInUser.Role.CanEditProducts) return false;
                if(string.IsNullOrWhiteSpace(name))return false;
                if (nameNotUnique) return false;

                if (isInSales && pluNotUnique) return false;

                return true;
            }
        }

        protected override void Save() 
        {
            if (HasPendingChanges)
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    Product product;

                    //bool cost_changed_flag = false;
                    //bool umFamilyChanged = false;

                    if (creating)
                    {
                        product = new Product();

                        unitOfWork.ProductRepository.Add(product);
                    }
                    else
                    {
                        product = unitOfWork.ProductRepository.GetById(prodId);

                        //bool costChanged = CostChanged(product.CostPrice, product.CostQuantity, product.CostUnitMeasure, priceRate, arbitraryCostUM);

                        if (totalCost != product.CostPrice || arbitraryCostUMId != product.CostUMId)
                        {
                            UpdateOutgredientsCost(product);
                        }

                        //if (umFamilyId != product.UMFamilyId) umFamilyChanged = true;
                    }

                    CopyToProduct(unitOfWork, product);

                    unitOfWork.SaveChanges();
                    //bool category_changed = CategoryChanged();

                    HasPendingChanges = false;

                    if (creating)
                    {
                        prodId = product.Id;
                        appvm.GlobalEventsManager.FireProductCreated(product.Id);
                    }
                    else
                    {
                        appvm.GlobalEventsManager.FireProductModified(product.Id);
                    }

                    //when the cost of a product changes update the cost of recipes where it appears
                    //if (cost_changed_flag)
                    //{
                    //    var invSvc = base.GetService<IInventoryService>();

                    //    foreach (var outgredient in product.Outgredients)
                    //    {
                    //        if (umFamilyChanged)//update UoM in outgredients
                    //        {
                    //            outgredient.UnitMeasureId = availableUMs.First().Id;// umFamily.UnitMeasures.Single(x => x.IsFamilyBase);
                    //        }
                    //        invSvc.UpdateProductCost(outgredient.BaseProduct);
                    //    }
                    //}

                    //unitOfWork.SaveChanges();
                }                

                //just to get the new ingredients id's
                //CopyIngredientsFromProduct();

                //if (creating)
                //{
                //    creating = false;

                //    //icvIngredients.Refresh();

                //    ProductId = product.Id;

                //    //onProductCreated(product);
                //}

            }

            DialogResult = true;
        }

        private void UpdateOutgredientsCost(Product product)
        {
            foreach (var outgredient in product.Outgredients)
            {
                decimal oldCost = product.CostPrice * (decimal)(outgredient.Quantity * outgredient.UnitMeasure.ToBaseConversion / product.CostUnitMeasure.ToBaseConversion);

                UnitMeasure newOutgredientUM = outgredient.UnitMeasure;

                if (umFamilyId != product.UMFamilyId)//update UoM in outgredients
                {
                    newOutgredientUM = availableUMs.Single(x => x.IsFamilyBase);
                    outgredient.UnitMeasureId = newOutgredientUM.Id;// umFamily.UnitMeasures.Single(x => x.IsFamilyBase);
                }

                UnitMeasure newCostUM = availableUMs.Single(x => x.Id == arbitraryCostUMId);

                decimal newCost = totalCost * (decimal)(outgredient.Quantity * newOutgredientUM.ToBaseConversion / newCostUM.ToBaseConversion);

                outgredient.BaseProduct.CostPrice += newCost - oldCost;//add difference
            }
        }

        public void SaveAndClose()
        {
            Save();            

            CloseMe(); 
        }

        public bool DialogResult { get; set; }

        private void CopyIngredientsFromProduct(Product product)
        {
            ingredients.Clear();
            foreach (var item in product.Ingredients)
            {
                IngredientViewModel ic = new IngredientViewModel(item, OnIngredientModified);
                //ic.Quantity = item.Quantity;
                //ic.IngredientProduct = item.IngredientProduct;

                ingredients.Add(ic);
            }
        }

        #endregion

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
            CloseMe();
        }

        #endregion

        bool creating;

        //bool hasPendingChanges;
        //public bool HasPendingChanges
        //{
        //    get { return hasPendingChanges; }
        //    set
        //    {
        //        hasPendingChanges = value;
        //        OnPropertyChanged("HasPendingChanges");
        //    }
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

        //bool selfClosing = false;

        //bool IScreen.IsSelfClosing()
        //{
        //    return selfClosing;
        //}

        //bool IScreen.TryToClose()
        //{
        //    if (hasPendingChanges && this.CanSave)
        //    {
        //        var msgBox = base.GetService<IMessageBoxService>();
        //        if (msgBox != null)
        //        {
        //            var result = msgBox.Show("¿Desea guardar los cambios antes de salir?",
        //                "Guardar cambios",
        //                MessageBoxButton.YesNoCancel,
        //                MessageBoxImage.Question);

        //            if (result == MessageBoxResult.Cancel)
        //                return false;

        //            if (result == MessageBoxResult.Yes)
        //                this.Save();
        //        }
        //    }
        //    return true;
        //}
    }
}
