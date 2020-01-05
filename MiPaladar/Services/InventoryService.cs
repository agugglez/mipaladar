using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.ViewModels;

using System.Collections.ObjectModel;
using MiPaladar.MVVM;
using MiPaladar.Repository;

namespace MiPaladar.Services
{
    public interface IInventoryService
    {
        #region Cost

        //decimal GetCurrentCost(Product product, double quantity, UnitMeasure um);
        //decimal GetCurrentCost(double qtty, UnitMeasure um, decimal useThisCost, UnitMeasure useThisUnitMeasure);
        decimal GetProductCost(Product ingProduct, double ingQtty, UnitMeasure ingUM, bool ignoreEdiblePart = false);

        /// <summary>
        /// Checks if the product's cost is right
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        bool CheckProductCost(Product product);

        /// <summary>
        /// Corrects a product's cost
        /// </summary>
        /// <param name="product"></param>
        void UpdateProductCost(Product product);

        #endregion

        //Product Completion Percent (Helper)
        double GetInventoryCompletionPercent(IUnitOfWork unitOfWork);
        //double GetProductCompletionPercent(Product p);
        //double GetProductCompletionPercent(int prodId);

        #region Inventory Tree

        int FindProductIndex(int prodId, ObservableCollection<InventoryItemViewModel> inventoryItems);
        int FindCategoryIndex(int id, ObservableCollection<InventoryItemViewModel> inventoryTree);

        //void CreateCategoryList(ObservableCollection<CategoryRowViewModel> targetList);
        int CreateInventoryTreeLikeList(IUnitOfWork unitOfWork, ObservableCollection<InventoryItemViewModel> inventoryItems,
            Predicate<Product> filter);
        /// <summary>
        /// Creates the inventory Category->Product tree
        /// </summary>
        /// <param name="roots"></param>
        /// <param name="predicate"></param>
        /// <returns>total numbre of products in inventory</returns>
        /// 
        //int CreateInventoryTree(ObservableCollection<CategoryNode> roots, Predicate<Product> filter);
        void CreateCategoryList(IUnitOfWork unitOfWork, ObservableCollection<CategoryRowViewModel> targetList, 
            NamingModes mode = NamingModes.SimpleName, Predicate<Category> filter = null);
        //void CreateCategoryTree(ObservableCollection<TreeNodeViewModel> rootCategoriesPlaceHolder);
        //void CreateCategoryTree(ObservableCollection<TreeNodeViewModel> rootCategoriesPlaceHolder, int ignoreCategoryId);

        //void ExpandCollapseInventoryTree(ObservableCollection<CategoryNode> roots, bool expand);

        //Product Events (Create, Remove, Change Category)
        void AddProductItem(Product p, ObservableCollection<InventoryItemViewModel> inventoryTree);
        bool RemoveProductItem(int productId, ObservableCollection<InventoryItemViewModel> inventoryTree);
        //void MoveProductNode(Product p, ObservableCollection<CategoryNode> inventoryTree);

        #endregion

        /// <summary>
        /// Finds biggest possible UoM to express a quantity (>1)
        /// </summary>
        /// <param name="productUMFamily"></param>
        /// <param name="originalQttyInBase"></param>
        /// <param name="resultQtty"></param>
        /// <param name="resultUM"></param>
        
        //void FitQuantity(UMFamily productUMFamily, double originalQttyInBase, 
        //    out double resultQtty, out UnitMeasure resultUM);        

        UnitMeasure GetBestUM(UMFamily umFamily, double qttyInBase);

        /// <summary>
        /// Checks if there is no other product with the same name in Inventory
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool NameIsUnique(IUnitOfWork unitOfWork, string name, int ignoreProductId = -1);

        bool PLUIsUnique(IUnitOfWork unitOfWork, int givenPLU, int ignoreProductId = -1);

        //void CreateInventorySnapshot(DateTime workingDate);
        ////void ExecuteSellOperation(DateTime date, Product product, float quantity, UnitMeasure um);
        //void ExecuteInventoryOperation(DateTime date, Inventory inventory,
        //    Product product, double quantity, UnitMeasure um, decimal cost);
        //InventoryItem GetInventoryItem(Inventory inventoryArea, Product product);
        ///// <summary>
        ///// Checks if there is enough existence in inventory of the product. In case of a recipe
        ///// checks existence for all the ingredients and returns a list with what is missing.
        ///// </summary>
        //List<InventoryItem> CheckAvailability(Inventory inventory, Product product, double quantity);

        string GetFullCategoryName(Category category);        
    }

    public enum NamingModes { FullName, SimpleName}

    

    public class InventoryService : IInventoryService
    {
        #region Cost Methods

        //public decimal GetCurrentCost(Product product, double quantity, UnitMeasure um)
        //{
        //    //double UoMRate = um.ToBaseConversion / product.CostUnitMeasure.ToBaseConversion;

        //    return product.CostPrice * (decimal)quantity * (decimal)um.ToBaseConversion / (decimal)product.CostUnitMeasure.ToBaseConversion;
        //}

        //public decimal GetCurrentCost(double qtty, UnitMeasure um, decimal useThisCost, UnitMeasure useThisUnitMeasure)
        //{
        //    //double UoMRate = um.ToBaseConversion / product.CostUnitMeasure.ToBaseConversion;

        //    return useThisCost * (decimal)(qtty * um.ToBaseConversion / useThisUnitMeasure.ToBaseConversion);
        //}

        public decimal GetProductCost(Product product, double qtty, UnitMeasure um, bool ignoreEdiblePart = false)
        {
            decimal result = 0;

            if (product.ProductType == ProductType.FinishedGoods || product.ProductType == ProductType.CompraVenta)
            {
                result = product.CostPrice * (decimal)qtty;
            }
            else
            {
                result = product.CostPrice * (decimal)qtty * (decimal)um.ToBaseConversion / (decimal)product.CostUnitMeasure.ToBaseConversion;
            }

            //EDIBLE PART
            if (product.ProductType == ProductType.RawMaterials && !ignoreEdiblePart)
            {
                result /= (decimal)product.EdiblePart;
            }

            return result;
        }

        public bool CheckProductCost(Product product)
        {
            if (product.ProductType == ProductType.FinishedGoods || product.ProductType == ProductType.WorkInProcess)
            {
                //recipe UM and cost UM should match
                if (product.CostUnitMeasure != product.RecipeUnitMeasure) 
                    return false;

                //calculate ingredients cost
                decimal ingredients_cost = 0;

                foreach (var item in product.Ingredients)
                {
                    ingredients_cost += GetProductCost(item.IngredientProduct, item.Quantity, item.UnitMeasure);
                }

                //divide by recipe qtty
                ingredients_cost /= (decimal)product.RecipeQuantity;

                decimal total_cost = ingredients_cost + product.ArbitraryCost;

                if (Math.Abs(product.CostPrice - total_cost) > 0.0001M) 
                    return false;
            }
            //COMPRAVENTA && RAWMATERIALS
            else
            {
                if (product.CostPrice != product.ArbitraryCost)
                    return false;
            }

            return true;
        }

        public void UpdateProductCost(Product product)
        {
            if (product.ProductType == ProductType.FinishedGoods || product.ProductType == ProductType.WorkInProcess)
            {
                //calculate ingredients cost
                decimal ingredients_cost = 0;

                foreach (var item in product.Ingredients)
                {
                    ingredients_cost += GetProductCost(item.IngredientProduct, item.Quantity, item.UnitMeasure);
                }

                //recipe UM and cost UM should match
                if (product.CostUnitMeasure != product.RecipeUnitMeasure) product.CostUnitMeasure = product.RecipeUnitMeasure;

                //divide by recipe qtty
                ingredients_cost /= (decimal)product.RecipeQuantity;

                decimal total_cost = ingredients_cost + product.ArbitraryCost;

                //exact compare has too much precision
                if (Math.Abs(product.CostPrice - total_cost) > 0.0001M) product.CostPrice = total_cost;
            }
                //COMPRAVENTA && RAWMATERIALS
            else
            {
                if (product.CostPrice != product.ArbitraryCost) product.CostPrice = product.ArbitraryCost;
            }
        }

        bool IsRecipe(Product product)
        {
            return product.ProductType == Entities.ProductType.FinishedGoods || product.ProductType == Entities.ProductType.WorkInProcess;
        }

        #endregion

        #region Best Fitting Unit of Measure

        public UnitMeasure GetBestUM(UMFamily umFamily, double qttyInBase)
        {
            var unitOfMeasures = umFamily.UnitMeasures.OrderBy(x => -x.ToBaseConversion);

            foreach (var item in unitOfMeasures)
            {
                if (qttyInBase / item.ToBaseConversion >= 1) return item;
            }

            return unitOfMeasures.First();

            //resultUM = productUMFamily.UnitMeasures.Where(x => originalQttyInBase / x.ToBaseConversion >= 1).OrderBy(x => x.ToBaseConversion).Last();
            //resultQtty = originalQttyInBase / resultUM.ToBaseConversion;

            //return bestUM;
        }

        //public void FitQuantity(UMFamily productUMFamily, double originalQttyInBase, 
        //    out double resultQtty, out UnitMeasure resultUM)
        //{
        //    //UnitMeasure baseUM = umFamily.UnitMeasures.Single(x => x.IsFamilyBase);

        //    //var ordered = from um in umFamily.UnitMeasures
        //    //              orderby um.ToBaseConversion descending
        //    //              select um;

        //    //foreach (var um in ordered)
        //    //{
        //    //    double qtty_in_um = qtty / um.ToBaseConversion;

        //    //    if (qtty_in_um >= 1)
        //    //    {
        //    //        return um;
        //    //    }
        //    //}

        //    //return umFamily.UnitMeasures.Single(x => x.IsFamilyBase);

        //    if (productUMFamily.UnitMeasures.Count == 1)
        //    {
        //        resultQtty = originalQttyInBase;
        //        resultUM = productUMFamily.UnitMeasures.First();                
        //    }
        //    else
        //    {
        //        resultUM = productUMFamily.UnitMeasures.Where(x => originalQttyInBase / x.ToBaseConversion >= 1).OrderBy(x => x.ToBaseConversion).Last();
        //        resultQtty = originalQttyInBase / resultUM.ToBaseConversion;
        //    }
        //}

        #endregion        

        public string GetFullCategoryName(Category category)
        {
            return (category.ParentCategory == null ? "" : GetFullCategoryName(category.ParentCategory) + ":") + category.Name;
        }

        #region Check Repeated Name and PLU

        public bool NameIsUnique(IUnitOfWork unitOfWork, string givenName, int ignoreProductId = -1)
        {
            bool result = true;

            foreach (var item in unitOfWork.ProductRepository.Get())
            {
                if (item.Id == ignoreProductId) continue;

                if (string.Compare(item.Name, givenName, true) == 0)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public bool PLUIsUnique(IUnitOfWork unitOfWork, int givenPLU, int ignoreProductId = -1)
        {
            return unitOfWork.ProductRepository.Get(x => x.Id != ignoreProductId && x.Code == givenPLU).Count > 0;


            //foreach (var item in unitOfWork.ProductRepository.Get())
            //{
            //    if (item.Id == ignoreProductId) continue;

            //    if (item.Code == givenPLU)
            //    {
            //        result = false;
            //        break;
            //    }
            //}

            //return result;
        }

        #endregion

        #region Inventory Completion Percent

        public double GetInventoryCompletionPercent(IUnitOfWork unitOfWork)
        {
            double averageCompletion = 0;
            double productCount = 0;

            var allProducts = unitOfWork.ProductRepository.Get();

            productCount = allProducts.Count;

            if (productCount == 0) return 0;

            double doneCount = allProducts.Where(x => x.DoneByUser).Count();

            averageCompletion = doneCount * 100 / productCount;

            return averageCompletion;
        }

        //ITEM COMPLETION PERCENT
        //public double GetProductCompletionPercent(int prodId)
        //{
        //    double result = 0;
        //    using (var context = new RestaurantDBEntities())
        //    {
        //        result = GetProductCompletionPercent(context.Products.Single(x => x.Id == prodId));
        //    }

        //    return result;
        //}
        //public double GetProductCompletionPercent(Product p)
        //{
        //    double totalConditionsCount = 1;//category
        //    double passedConditions = 0;

        //    if (p.CategoryId != null) passedConditions++;
        //    //if (!string.IsNullOrWhiteSpace(p.Description)) passedConditions++;

        //    if (p.ProductType == ProductType.FinishedGoods)
        //    {
        //        totalConditionsCount += 3;

        //        if (p.Code > 0) passedConditions++;
        //        if (p.SalePrice > 0) passedConditions++;
        //        if (p.Ingredients.Count > 0) passedConditions++;
        //        //if (!string.IsNullOrWhiteSpace(p.ProductionProcess)) passedConditions++;
        //    }
        //    else if (p.ProductType == ProductType.WorkInProcess)
        //    {
        //        totalConditionsCount += 1;

        //        if (p.Ingredients.Count > 0) passedConditions++;
        //        //if (p.Outgredients.Count > 0) passedConditions++;
        //        //if (!string.IsNullOrWhiteSpace(p.ProductionProcess)) passedConditions++;
        //    }
        //    else if (p.ProductType == ProductType.RawMaterials)
        //    {
        //        totalConditionsCount += 2;

        //        if (p.ArbitraryCost > 0) passedConditions++;
        //        //if (p.EdiblePart < 1) passedConditions++;
        //        if (p.Outgredients.Count > 0) passedConditions++;
        //    }
        //    else if (p.ProductType == ProductType.CompraVenta)
        //    {
        //        totalConditionsCount += 3;

        //        if (p.Code > 0) passedConditions++;
        //        if (p.SalePrice > 0) passedConditions++;
        //        if (p.ArbitraryCost > 0) passedConditions++;
        //    }

        //    return passedConditions * 100 / totalConditionsCount;
        //}

        #endregion        

        #region Category List

        public void CreateCategoryList(IUnitOfWork unitOfWork, ObservableCollection<CategoryRowViewModel> targetList, 
            NamingModes mode = NamingModes.SimpleName, Predicate<Category> filter = null)
        {
            if (targetList == null) throw new ArgumentException("targetList");

            //targetList.Clear();

            //start with root categories
            var query = from c in unitOfWork.CategoryRepository.Get(x => x.ParentCategory_Id == null)
                        orderby c.Name
                        select c;

            foreach (var item in query)
            {
                if (filter != null && !filter(item)) continue;

                CategoryRowViewModel crvm = new CategoryRowViewModel();
                crvm.Id = item.Id;
                crvm.Name = item.Name;
                crvm.Level = 0;

                targetList.Add(crvm);

                AddChildrenList(targetList, filter, 1, mode, crvm, item);
            }
        }

        private void AddChildrenList(ObservableCollection<CategoryRowViewModel> targetList, Predicate<Category> filter, int deep, NamingModes mode, CategoryRowViewModel parentRow, Category source)
        {
            foreach (var item in source.ChildrenCategories.OrderBy(x => x.Name))
            {
                if (filter != null && !filter(item)) continue;

                CategoryRowViewModel crvm = new CategoryRowViewModel();
                crvm.Id = item.Id;
                crvm.Level = parentRow.Level + 1;

                switch (mode)
                {
                    case NamingModes.FullName:
                        crvm.Name = parentRow.Name + ":" + item.Name;
                        break;
                    case NamingModes.SimpleName:
                        crvm.Name = item.Name;
                        break;
                    default:
                        break;
                }

                targetList.Add(crvm);

                AddChildrenList(targetList, filter, deep + 1, mode, crvm, item);
            }
        }

        #endregion        

        #region Inventory Tree

        public int CreateInventoryTreeLikeList(IUnitOfWork unitOfWork, ObservableCollection<InventoryItemViewModel> inventoryItems, 
            Predicate<Product> filter)
        {
            int totalProductsCounter = 0;

            //start with root categories
            var query = from c in unitOfWork.CategoryRepository.Get(x => x.ParentCategory_Id == null)
                        //where c.ParentCategory == null
                        orderby c.Name
                        select c;

            foreach (var item in query)
            {
                InventoryItemViewModel crvm = new InventoryItemViewModel(item, 0);

                inventoryItems.Add(crvm);

                int childrenCount = AddInventoryItemChildren(item, 0, inventoryItems, filter);

                if (childrenCount == 0) inventoryItems.Remove(crvm);

                totalProductsCounter += childrenCount;
            }

            //add products without category
            var pQuery = from p in unitOfWork.ProductRepository.Get(x => x.CategoryId == null)
                         //where p.Category == null
                         orderby p.Name
                         select p;

            //products without category
            InventoryItemViewModel uncategorized = new InventoryItemViewModel(0, "Sin Categoría", 0, InventoryItemType.Category);

            inventoryItems.Add(uncategorized);

            int uncategorizedCounter = 0;
            foreach (var item in pQuery)
            {
                if (!filter(item)) continue;

                InventoryItemViewModel pn = new InventoryItemViewModel(item, 1);

                inventoryItems.Add(pn);

                uncategorizedCounter++;
            }

            if (uncategorizedCounter == 0)
            {
                inventoryItems.Remove(uncategorized);
            }
            else totalProductsCounter += uncategorizedCounter;

            return totalProductsCounter;
        }

        private int AddInventoryItemChildren(Category parentCategory, int parentLevel, ObservableCollection<InventoryItemViewModel> inventoryItems, Predicate<Product> filter)
        {
            int productCounter = 0;

            foreach (var item in parentCategory.ChildrenCategories.OrderBy(x => x.Name))
            {
                InventoryItemViewModel catItem = new InventoryItemViewModel(item, parentLevel + 1);

                inventoryItems.Add(catItem);

                int childrenCount = AddInventoryItemChildren(item, parentLevel + 1, inventoryItems, filter);

                if (childrenCount == 0) inventoryItems.Remove(catItem);

                productCounter += childrenCount;
            }

            foreach (var item in parentCategory.Products.OrderBy(x => x.Name))
            {
                InventoryItemViewModel prodItem = new InventoryItemViewModel(item, parentLevel + 1);

                if (filter(item))
                {
                    inventoryItems.Add(prodItem);
                    productCounter++;
                }
            }

            return productCounter;
        }

        //public int CreateInventoryTree(ObservableCollection<TreeNodeViewModel> prodTypeNodes, Predicate<Product> filter)
        //{
        //    prodTypeNodes.Clear();

        //    int count = 0;

        //    using (var context = new RestaurantDBEntities())
        //    {
        //        //start with root categories
        //        var query = from c in context.Categories
        //                    where c.ParentCategory == null
        //                    orderby c.Name
        //                    select c;

        //        foreach (var item in query)
        //        {
        //            CategoryNode cn = new CategoryNode();
        //            cn.Id = item.Id;
        //            cn.Name = item.Name;

        //            var children_count = AddChildrenTree(cn, item, filter);

        //            //don't show categories without child products
        //            if (children_count > 0)
        //            {
        //                prodTypeNodes.Add(cn);

        //                count += children_count;
        //            }
        //        }

        //        var pQuery = from p in context.Products
        //                     where p.Category == null
        //                     orderby p.Name
        //                     select p;

        //        //products without category
        //        CategoryNode uncategorized = new CategoryNode();
        //        uncategorized.Id = 0;
        //        uncategorized.Name = "Sin Categoría";

        //        foreach (var item in pQuery)
        //        {
        //            if (!filter(item)) continue;

        //            ProductNode pn = new ProductNode();
        //            pn.Id = item.Id;
        //            pn.Name = item.Name;

        //            pn.Parent = uncategorized;
        //            uncategorized.Children.Add(pn);
        //        }

        //        if (uncategorized.Children.Count > 0)
        //        {
        //            prodTypeNodes.Add(uncategorized);
        //            count += uncategorized.Children.Count;
        //        }
        //    }

        //    return count;
        //}

        //private int AddChildrenTree(CategoryNode parentNode, Category parentOriginal, Predicate<Product> filter)
        //{
        //    int count = 0;

        //    var childrenCategories = from c in parentOriginal.ChildrenCategories
        //                             orderby c.Name
        //                             select c;

        //    foreach (var item in childrenCategories)
        //    {
        //        CategoryNode cn = new CategoryNode();
        //        cn.Id = item.Id;
        //        cn.Name = item.Name;

        //        var children_count = AddChildrenTree(cn, item, filter);

        //        //don't show categories without child products
        //        if (children_count > 0)
        //        {
        //            cn.Parent = parentNode;
        //            parentNode.Children.Add(cn);

        //            count += children_count;
        //        }
        //    }

        //    var childrenProducts = from p in parentOriginal.Products
        //                           orderby p.Name
        //                           select p;

        //    foreach (var item in childrenProducts)
        //    {
        //        if (!filter(item)) continue;

        //        ProductNode pn = new ProductNode();
        //        pn.Id = item.Id;
        //        pn.Name = item.Name;

        //        pn.Parent = parentNode;
        //        parentNode.Children.Add(pn);

        //        count++;
        //    }

        //    return count;
        //}

        #endregion

        #region Add-Remove Product Items

        public void AddProductItem(Product p, ObservableCollection<InventoryItemViewModel> inventoryItems)
        {
            int parentCategoryId = p.Category == null ? 0 : p.Category.Id;

            int parentIndex = FindCategoryIndex(parentCategoryId, inventoryItems);

            if (parentIndex == -1)
            {
                parentIndex = AddNonExistentCategory(p.Category, inventoryItems);
            }

            InventoryItemViewModel proditem = new InventoryItemViewModel(p, inventoryItems[parentIndex].Level + 1);
                        
            InsertAlphabetical(proditem, inventoryItems, parentIndex);
        }

        public bool RemoveProductItem(int productId, ObservableCollection<InventoryItemViewModel> inventoryItems)
        {
            int index = FindProductIndex(productId, inventoryItems);

            if (index != -1)
            {
                int ancestorIndex = FindItemAncestorIndex(index, inventoryItems);

                inventoryItems.RemoveAt(index);

                //some categories might end without children products
                CheckChildrenlessCategory(ancestorIndex, inventoryItems);

                return true;
            }

            return false;
        }

        private int FindItemAncestorIndex(int itemIndex, ObservableCollection<InventoryItemViewModel> inventoryItems)
        {
            int itemLevel = inventoryItems[itemIndex].Level;

            for (int i = itemIndex - 1; i >= 0; i--)
            {
                if (inventoryItems[i].Level == itemLevel - 1) return i;
            }

            return -1;
        }

        private void CheckChildrenlessCategory(int categoryIndex, ObservableCollection<InventoryItemViewModel> inventoryItems)
        {
            if (!CategoryHasChildrenProducts(categoryIndex, inventoryItems))
            {
                int ancestorIndex = FindItemAncestorIndex(categoryIndex, inventoryItems);

                inventoryItems.RemoveAt(categoryIndex);

                if (ancestorIndex >= 0) CheckChildrenlessCategory(ancestorIndex, inventoryItems);
            }            
        }

        bool CategoryHasChildrenProducts(int categoryIndex, ObservableCollection<InventoryItemViewModel> inventoryItems) 
        {
            if (categoryIndex < 0) throw new ArgumentException("categoryIndex");

            int mylevel = inventoryItems[categoryIndex].Level;

            for (int i = categoryIndex+1; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].ItemType == InventoryItemType.Product && inventoryItems[i].Level == mylevel + 1)
                    return true;
            }

            return false;
        }

        //adds a category item not currently present in inventory tree cause it didn't have children to show

        private int AddNonExistentCategory(Category category, ObservableCollection<InventoryItemViewModel> inventoryItems)
        {
            //"No Category" category
            if (category == null)
            {
                InventoryItemViewModel categoryRowToAdd = new InventoryItemViewModel(0, "Sin Categoría", 0, InventoryItemType.Category);
                return InsertAlphabetical(categoryRowToAdd, inventoryItems);
            }
            else
            {
                Category parentCategory = category.ParentCategory;

                InventoryItemViewModel categoryRowToAdd;

                //IS ROOT CATEGORY
                if (parentCategory == null)
                {
                    categoryRowToAdd = new InventoryItemViewModel(category, 0);
                    return InsertAlphabetical(categoryRowToAdd, inventoryItems);
                }
                else
                {
                    //CHECK IF PARENT EXISTS
                    int parentCategoryIndex = FindCategoryIndex(parentCategory.Id, inventoryItems);

                    if (parentCategoryIndex == -1)
                    {
                        parentCategoryIndex = AddNonExistentCategory(parentCategory, inventoryItems);
                    }

                    categoryRowToAdd = new InventoryItemViewModel(category, inventoryItems[parentCategoryIndex].Level + 1);
                    return InsertAlphabetical(categoryRowToAdd, inventoryItems, parentCategoryIndex);
                }
            }            
        }

        private int InsertAlphabetical(InventoryItemViewModel invItem, ObservableCollection<InventoryItemViewModel> inventoryItems, int parentIndex = -1)
        {
            int mylevel = parentIndex == -1 ? 0 : inventoryItems[parentIndex].Level + 1;

            int i;
            for (i = parentIndex + 1; i < inventoryItems.Count; i++)
            {
                InventoryItemViewModel currentItem = inventoryItems[i];

                //ignore items deeper in the tree
                if (currentItem.Level > mylevel) continue;

                //category children ends
                if (currentItem.Level < mylevel) break;

                //insert categories before products
                if (invItem.ItemType == InventoryItemType.Category && currentItem.ItemType == InventoryItemType.Product) break;

                //and products after categories
                if (invItem.ItemType == InventoryItemType.Product && currentItem.ItemType == InventoryItemType.Category) continue;

                if (currentItem.Name.CompareTo(invItem.Name) > 0) break;
            }

            if (i == inventoryItems.Count) inventoryItems.Add(invItem);
            else
            {
                inventoryItems.Insert(i, invItem);
            }

            return i;
        }

        //private void InsertAlphabetical(CategoryNode toInsertNode, ObservableCollection<CategoryNode> inventoryTree)
        //{
        //    int index = GetIndexToInsert(toInsertNode, inventoryTree);

        //    if (index == inventoryTree.Count) inventoryTree.Add(toInsertNode);
        //    else
        //    {
        //        inventoryTree.Insert(index, toInsertNode);
        //    }
        //}

        //private int GetIndexToInsert(TreeNodeViewModel tNode, TreeNodeViewModel parentNode)
        //{
        //    int count = parentNode.Children.Count;

        //    for (int i = 0; i < count; i++)
        //    {
        //        TreeNodeViewModel currentNode = parentNode.Children[i];

        //        //insert categories before products
        //        if (tNode.Type == NodeType.Category && currentNode.Type == NodeType.Product) return i;

        //        //and products after categories
        //        if (tNode.Type == NodeType.Product && currentNode.Type == NodeType.Category) continue;

        //        if (currentNode.Name.CompareTo(tNode.Name) > 0) return i;
        //    }

        //    return count;
        //}

        //private int GetIndexToInsert(CategoryNode tNode, ObservableCollection<CategoryNode> inventoryTree)
        //{
        //    int count = inventoryTree.Count;

        //    for (int i = 0; i < count; i++)
        //    {
        //        TreeNodeViewModel currentNode = inventoryTree[i];

        //        //always before uncategorized
        //        if (currentNode.Id == 0) return i;

        //        if (currentNode.Name.CompareTo(tNode.Name) > 0) return i;
        //    }

        //    return count;
        //}

        #endregion        

        #region Add-Remove Category Items
        #endregion

        //#region Expand, Collapse

        //public void ExpandCollapseInventoryTree(ObservableCollection<CategoryNode> roots, bool expand)
        //{
        //    foreach (var item in roots)
        //    {
        //        ExpandCollapseSubTree(item, expand);
        //    }
        //}

        //void ExpandCollapseSubTree(TreeNodeViewModel node, bool expand)
        //{
        //    node.IsNodeExpanded = expand;

        //    if (node is ProductNode) return;

        //    CategoryNode cn = (CategoryNode)node;

        //    foreach (var item in cn.Children)
        //    {
        //        ExpandCollapseSubTree(item, expand);
        //    }
        //}

        //#endregion

        #region Find Node

        public int FindProductIndex(int prodId, ObservableCollection<InventoryItemViewModel> inventoryItems)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].Id == prodId && inventoryItems[i].ItemType == InventoryItemType.Product) return i;
            }

            return -1;
        }


        public int FindCategoryIndex(int id, ObservableCollection<InventoryItemViewModel> inventoryTree)
        {
            for (int i = 0; i < inventoryTree.Count; i++)
            {
                if (inventoryTree[i].Id == id && inventoryTree[i].ItemType == InventoryItemType.Category)
                    return i;
            }

            return -1;
        }

        #endregion

        //MainWindowViewModel appvm;
        //public InventoryService(MainWindowViewModel appvm)
        //{
        //    this.appvm = appvm;
        //}

        //#region Snapshot

        //public void CreateInventorySnapshot(DateTime workingDate)
        //{
        //    if (workingDate != DateTime.Today) return;

        //    var query = from e in appvm.Context.InventoryTraces
        //                where e.Date == workingDate
        //                select e;

        //    if (query.Count() > 0) return; //it was initialized

        //    foreach (var item in appvm.Context.InventoryItems)
        //    {
        //        if (!item.Product.IsStorable) continue;

        //        InventoryTrace newE = new InventoryTrace();
        //        newE.Product = item.Product;
        //        newE.Quantity = item.Quantity;
        //        newE.Date = workingDate;
        //        newE.Inventory = item.Inventory;

        //        appvm.Context.InventoryTraces.AddObject(newE);
        //    }

        //    appvm.SaveChanges();
        //}

        //#endregion

        //#region Inventory Operations

        //public void ExecuteInventoryOperation(DateTime date, Inventory inventory,
        //    Product product, double quantity, UnitMeasure um, decimal cost)
        //{
        //    if (product == null || quantity == 0 || !product.IsStorable || inventory == null) return;

        //    //if (product.IsStorable)
        //    //{
        //    //work with current existence
        //    //if (date == DateTime.Today)
        //    {
        //        var query = from e in appvm.InventoryItemsOC
        //                    where e.Product.Id == product.Id && e.Inventory.Id == inventory.Id
        //                    select e;

        //        InventoryItem inventoryItem;// = currentExistence.First();
        //        if (query.Count() > 0)
        //        {
        //            inventoryItem = query.First();
        //        }
        //        else
        //        {
        //            inventoryItem = new InventoryItem();
        //            inventoryItem.Product = product;
        //            inventoryItem.Inventory = inventory;

        //            appvm.Context.InventoryItems.AddObject(inventoryItem);
        //            appvm.InventoryItemsOC.Add(inventoryItem);
        //        }

        //        inventoryItem.Quantity += quantity * um.ToBaseConversion;
        //        inventoryItem.Cost += cost;
        //    }

        //    DateTime tomorrow = date.Date.Date.AddDays(1);

        //    //work with historic existence
        //    var queryResult = from e in appvm.Context.InventoryTraces
        //                      where e.Date >= tomorrow && e.Product.Id == product.Id && e.Inventory.Id == inventory.Id
        //                      select e;

        //    foreach (var existence in queryResult)
        //    {
        //        existence.Quantity += quantity * um.ToBaseConversion;
        //    }

        //    //appvm.SaveChanges();
        //}

        //#endregion

        //#region Availability

        //public List<InventoryItem> CheckAvailability(Inventory inventory, Product product, double quantity)
        //{
        //    List<InventoryItem> answer = new List<InventoryItem>();

        //    CheckAvailability(inventory, product, quantity, answer);

        //    return answer;
        //}
        //private void CheckAvailability(Inventory inventory, Product product, double quantity, List<InventoryItem> unavailableProducts)
        //{
        //    if (product.IsStorable)
        //    {
        //        InventoryItem inventoryItem = GetInventoryItem(inventory, product);
        //        //add to unavailable products
        //        if (inventoryItem != null && inventoryItem.Quantity < quantity) unavailableProducts.Add(inventoryItem);
        //    }
        //    else if (product.IsRecipe)
        //    {
        //        foreach (var item in product.Ingredients)
        //        {
        //            if (item.IngredientProduct.IsStorable)
        //                CheckAvailability(inventory, item.IngredientProduct, item.Quantity * quantity, unavailableProducts);
        //        }
        //    }
        //}

        //public InventoryItem GetInventoryItem(Inventory inventoryArea, Product product)
        //{
        //    if (inventoryArea == null) return null;

        //    var query = from inventoryItem in product.CurrentExistence
        //                where inventoryItem.Inventory.Id == inventoryArea.Id
        //                select inventoryItem;

        //    if (query.Count() == 0) return null;

        //    return query.First();
        //}

        //#endregion
        
    }
}
