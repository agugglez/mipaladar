using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;
using MiPaladar.ViewModels;
using MiPaladar.MVVM;
using MiPaladar.Repository;

namespace MiPaladar.Services
{
    public class QueryLineItem
    {
        public double Quantity { get; set; }
        public UnitMeasure UnitOfMeasure { get; set; }
        public Product Product { get; set; }
        public decimal Amount { get; set; }
    }
    public class SevenDaysDataAnswer 
    {
        public decimal TotalSales { get; set; }
        public int TotalClients { get; set; }
        public decimal SpendingByClient { get; set; }
        public decimal SpendingDeviation { get; set; }

        public List<ChartItemViewModel> SevenDaysColumnData { get; set; }
        public List<ReportLineViewModel> TopProducts { get; set; }
    }

    //public class GraphDataAnswer
    //{
    //    public decimal TotalSales { get; set; }
    //    public int TotalClients { get; set; }
    //    public decimal SpendingByClient { get; set; }
    //    public decimal SpendingDeviation { get; set; }

    //    public List<SalesChartColumnViewModel> GraphData { get; set; }
    //}
    public interface IQueryService 
    {
        //DASHBOARD QUERY
        SevenDaysDataAnswer GetLastSevenDaysData(IUnitOfWork unitOfWork);
        //DateTime GetLastSaleDate();

        //REPORTS WINDOW QUERIES
        List<ReportLineViewModel> GetSalesData(IUnitOfWork unitOfWork, DateTime fromDate, DateTime toDate);
        //List<ReportLineViewModel> GetSalesDataWithCost(DateTime fromDate, DateTime toDate);
        List<ReportLineViewModel> GetSalesByItemData(IUnitOfWork unitOfWork, DateTime fromDate, DateTime toDate, bool includeIngredients);

        List<QueryLineItem> GetQueryLineItemsData(IUnitOfWork unitOfWork, DateTime fromDate, DateTime toDate, bool includeIngredients);
        //GraphDataAnswer GetGraphData(DateTime fromDate, DateTime toDate);
    }
    public class QueryService :  IQueryService
    {
        #region Dashboard Queries

        public SevenDaysDataAnswer GetLastSevenDaysData(IUnitOfWork unitOfWork)
        {
            decimal total_sales = 0;
            int total_clients = 0;
            int different_dates = 0;

            decimal spendingByClient = 0;
            decimal spendingDeviation = 0;

            DateTime sevenDaysDate = unitOfWork.OrderRepository.GetLastSaleDate().Add(TimeSpan.FromDays(-6));

            List<ChartItemViewModel> columnsData = new List<ChartItemViewModel>();
            List<ReportLineViewModel> topProducts = new List<ReportLineViewModel>();

            //last 7 days
            //get total sales and clients from Orders->Sales
            var daysQuery = from item in unitOfWork.OrderRepository.Get(s => s.Date >= sevenDaysDate)
                            //where item.Date >= sevenDaysDate
                            group item by item.Date into dategroup
                            orderby dategroup.Key
                            select dategroup;

            foreach (var group in daysQuery)
            {
                ChartItemViewModel scc = new ChartItemViewModel();

                scc.Date = group.Key;
                scc.X = group.Key.ToString("d/MMM"); ;

                total_sales += scc.TotalSale = Math.Round(group.Sum(x => x.Total));
                total_clients += scc.Clients = group.Sum(x => x.Persons);
                //total_orders += group.Count();
                different_dates++;

                columnsData.Add(scc);
            }

            //get cost from lineitems
            var lineitemsQuery = from li in unitOfWork.LineItemRepository.Get(x => x.Order.Date >= sevenDaysDate)
                                 //where li.Order.Date >= sevenDaysDate
                                 group li by li.Order.Date;

            var invSVC = ServiceContainer.GetService<IInventoryService>();

            foreach (var group in lineitemsQuery)
            {
                decimal cost = group.Sum(x => invSVC.GetProductCost(x.Product, x.Quantity, x.UnitMeasure));

                ChartItemViewModel scc = columnsData.Single(x => x.Date == group.Key);
                scc.TotalCost = Math.Round(cost);
                scc.Profit = scc.TotalSale - scc.TotalCost;
            }

            //calculate statistics

            if (total_clients > 0)
                spendingByClient = total_sales / total_clients;

            //SPENDING DEVIATION
            var queryLastSevenDaysWithClients = from item in unitOfWork.OrderRepository.Get(x => x.Date >= sevenDaysDate && x.Persons > 0)
                                                //where item.Date >= sevenDaysDate && item.Persons > 0
                                                select item;

            if (queryLastSevenDaysWithClients.Count() > 0)
            {
                decimal average = queryLastSevenDaysWithClients.Average(x => x.Total / x.Persons);

                spendingDeviation = (decimal)Math.Sqrt(queryLastSevenDaysWithClients.Average(x => Math.Pow((double)(x.Total / x.Persons - average), 2)));
            }

            //TOP PRODUCTS
            var lineitemsByProduct = from li in unitOfWork.LineItemRepository.Get(x => x.Order.Date >= sevenDaysDate)
                                     //where li.Order.Date >= sevenDaysDate
                                     group li by li.Product;// into prodGroups;
            //let profit = prodGroups.Sum(x => x.Amount - x.Cost)
            //orderby profit descending
            //select new { Product = prodGroups.Key, Total = profit };

            List<ReportLineViewModel> temp_list = new List<ReportLineViewModel>();

            foreach (var group in lineitemsByProduct)
            {
                ReportLineViewModel sli = new ReportLineViewModel();
                sli.Product = group.Key;
                sli.Amount = group.Sum(x => x.Amount);
                sli.Cost = group.Sum(x => invSVC.GetProductCost(x.Product, x.Quantity, x.UnitMeasure));
                //sli.Profit = sli.Amount - sli.Cost;

                temp_list.Add(sli);
            }

            temp_list = temp_list.OrderBy(x => x.Profit).ToList();
            int count = 0;
            for (int i = temp_list.Count - 1; i >= 0; i--)
            {
                if (count++ == 10) break;
                var sli = temp_list.ElementAt(i);
                sli.Quantity = count;
                topProducts.Add(sli);
            }

            //if (total_orders > 0)
            //    SpendingByOrder = total_sales / total_orders;                


            ////last four weeks
            //var tempQuery = (from item in context.Orders.OfType<Sale>()
            //                 where item.Date >= fourWeeksDate
            //                 select item).ToArray();

            //var weeksQuery = from item in tempQuery
            //                 let mondayDate = SalesChartColumnViewModel.GetWeekMonday(item.Date)
            //                 group item by mondayDate into weekgroup
            //                 orderby weekgroup.Key
            //                 select weekgroup;

            //foreach (var group in weeksQuery)
            //{
            //    SalesChartColumnViewModel scc = new SalesChartColumnViewModel();

            //    DateTime weekMonday = group.Key;
            //    scc.X = weekMonday.Day + "-" + weekMonday.AddDays(6).Day;

            //    scc.TotalSale =Math.Round(group.Sum(x => x.Total));

            //    fourWeeksItems.Add(scc);
            //}

            SevenDaysDataAnswer answer = new SevenDaysDataAnswer();
            answer.TotalSales = total_sales;
            answer.TotalClients = total_clients;
            answer.SpendingByClient = spendingByClient;
            answer.SpendingDeviation = spendingDeviation;

            answer.SevenDaysColumnData = columnsData;
            answer.TopProducts = topProducts;

            return answer;

            ////best day, week, month
            //using (var context = new RestaurantDBEntities())
            //{
            //    //best day
            //    var salesGroupedByDate = from sale in context.Orders.OfType<Sale>()
            //                             group sale by sale.Date;

            //    decimal maxTotal = 0;
            //    DateTime maxDate = DateTime.Today;

            //    foreach (var item in salesGroupedByDate)
            //    {
            //        decimal total = item.Sum(x => x.Total);
            //        if (total > maxTotal) { maxTotal = total; maxDate = item.Key; }
            //    }

            //    BestDay = maxTotal;
            //    BestDayTooltip = maxDate.ToString("dd/M/yyy");

            //    //best week
            //    var allSales = (from sale in context.Orders.OfType<Sale>()
            //                    select sale).ToArray();
            //    var salesGroupedByWeek = from item in allSales
            //                     let mondayDate = SalesChartColumnViewModel.GetWeekMonday(item.Date)
            //                     group item by mondayDate into weekgroup
            //                     orderby weekgroup.Key
            //                     select weekgroup;

            //    maxTotal = 0;

            //    foreach (var item in salesGroupedByWeek)
            //    {
            //        decimal total = item.Sum(x => x.Total);
            //        if (total > maxTotal) { maxTotal = total; maxDate = item.Key; }
            //    }

            //    BestWeek = maxTotal;
            //    BestWeekTooltip = maxDate.ToString("dd/M/yyy") + " - " + maxDate.AddDays(6).ToString("dd/M/yyy");

            //    //best month
            //    //best day
            //    var salesGroupedByMonth = from sale in context.Orders.OfType<Sale>()
            //                              group sale by sale.Date.Year into groupByYear
            //                              from monthGroup in
            //                                  (from item in groupByYear
            //                                   group item by item.Date.Month)
            //                              group monthGroup by groupByYear.Key;

            //    maxTotal = 0;

            //    foreach (var yearGroup in salesGroupedByMonth)
            //    {
            //        foreach (var monthGroup in yearGroup)
            //        {
            //            decimal total = monthGroup.Sum(x => x.Total);
            //            if (total > maxTotal)
            //            {
            //                maxTotal = total;
            //                maxDate = new DateTime(yearGroup.Key, monthGroup.Key, 1);
            //            }
            //        }
            //    }

            //    BestMonth = maxTotal;
            //    BestMonthTooltip = maxDate.ToString("MMM/yyy");
            //}
        }

        //public DateTime GetLastSaleDate()
        //{
        //    DateTime result = DateTime.Today;

        //    using (var context = new RestaurantDBEntities())
        //    {
        //        if (context.Orders.OfType<Sale>().Count() > 0)
        //        {
        //            result = context.Orders.OfType<Sale>().Max(x => x.Date);
        //        }
        //    }

        //    return result;
        //}

        #endregion

        #region Reports Window Queries

        public List<ReportLineViewModel> GetSalesData(IUnitOfWork unitOfWork, DateTime fromDate, DateTime toDate)
        {
            return GetSalesData(unitOfWork, fromDate, toDate, false);
        }

        //public List<ReportLineViewModel> GetSalesDataWithCost(DateTime fromDate, DateTime toDate)
        //{
        //    return GetSalesData(fromDate, toDate, true);
        //}

        private List<ReportLineViewModel> GetSalesData(IUnitOfWork unitOfWork, DateTime fromDate, DateTime toDate, bool loadCost)
        {
            List<ReportLineViewModel> allSales = new List<ReportLineViewModel>();
            //List<SaleLineItemReportViewModel> allSalesTotalized = new List<SaleLineItemReportViewModel>();

            DateTime startDate = fromDate.Date;
            DateTime endDate = toDate.Date.AddDays(1);

            //get data
            var query = from sale in unitOfWork.OrderRepository.Get(s => s.Date >= startDate && s.Date < endDate)
                        //where sale.Date >= startDate && sale.Date < endDate
                        select sale;
            //group li by li.Product;

            //UnitMeasure unitUM = context.UnitMeasures.Single(x => x.Name == "U");

            foreach (var sale in query)
            {
                ReportLineViewModel copy = new ReportLineViewModel(sale, loadCost);

                allSales.Add(copy);

            }//foreach

            return allSales;
        }

        public List<ReportLineViewModel> GetSalesByItemData(IUnitOfWork unitOfWork,
            DateTime fromDate, DateTime toDate, bool includeIngredients)
        {
            List<ReportLineViewModel> allSalesLines = new List<ReportLineViewModel>();
            //List<SaleLineItemReportViewModel> allSalesTotalized = new List<SaleLineItemReportViewModel>();

            DateTime startDate = fromDate.Date;
            DateTime endDate = toDate.Date.AddDays(1);

            //get data
            var query = from li in unitOfWork.LineItemRepository.Get(x => x.Order.Date >= startDate && x.Order.Date < endDate)
                        //where li.Order.Date >= startDate && li.Order.Date < endDate
                        select li;
            //group li by li.Product;

            //UnitMeasure unitUM = context.UnitMeasures.Single(x => x.Name == "U");

            foreach (var li in query)
            {
                ReportLineViewModel copy = new ReportLineViewModel(li);

                //copy.Product = li.Product;
                //copy.Quantity = li.Quantity;
                ////unit of measure will be "each"(U)
                //copy.UnitMeasure = unitUM;

                allSalesLines.Add(copy);

                //add ingredients
                if (includeIngredients && IsRecipe(copy.Product))
                    AddIngredients(copy.Product, copy.Quantity, copy.UnitMeasure, allSalesLines, li.Sale.Date, li.Sale.Shift, li.Sale.Employee);

            }//foreach

            return allSalesLines;
        }

        private void AddIngredients(Product recipeProduct, double qtty, UnitMeasure um,
            List<ReportLineViewModel> allSalesLines, DateTime date, Shift shift, Employee salesPerson)
        {
            double proportion = qtty * um.ToBaseConversion / recipeProduct.RecipeQuantity / recipeProduct.RecipeUnitMeasure.ToBaseConversion;

            foreach (var ingredient in recipeProduct.Ingredients)
            {
                Product ing_prod = ingredient.IngredientProduct;
                double ing_qtty = proportion * ingredient.Quantity;
                UnitMeasure ing_um = ingredient.UnitMeasure;

                ReportLineViewModel ing_op = new ReportLineViewModel(ing_prod, ing_qtty, ing_um, 0, date, shift, salesPerson);

                //ing_op.Product = ingredient.IngredientProduct;
                //ing_op.Quantity = proportion * ingredient.Quantity;
                //ing_op.UnitMeasure = ingredient.UnitMeasure;

                //TAGS
                //foreach (var tag in ing_op.Product.Tags)
                //{
                //    ing_op.TagIds.Add(tag.Id);
                //}
                //if (salesPerson != null)
                //{
                //    ing_op.SalesPerson = salesPerson;
                //    ing_op.SalesPersonId = salesPerson.Id;
                //}
                //if (shift != null)
                //{
                //    ing_op.Shift = shift;
                //    ing_op.ShiftId = shift.Id;
                //}

                allSalesLines.Add(ing_op);

                //add ingredients recursively
                if (IsRecipe(ingredient.IngredientProduct))
                    AddIngredients(ing_op.Product, ing_op.Quantity, ing_op.UnitMeasure, allSalesLines, date, shift, salesPerson);
            }
        }

        private bool IsRecipe(Product p)
        {
            return p.ProductType == ProductType.FinishedGoods || p.ProductType == ProductType.WorkInProcess;
        }

        #region Get Data

        public List<QueryLineItem> GetQueryLineItemsData(IUnitOfWork unitOfWork, DateTime fromDate, DateTime toDate, bool includeIngredients)
        {
            List<QueryLineItem> result = new List<QueryLineItem>();

            DateTime startDate = fromDate.Date;
            DateTime endDate = toDate.Date.AddDays(1);

            //get data
            var query = from li in unitOfWork.LineItemRepository.Get(x => x.Order.Date >= startDate && x.Order.Date < endDate)
                        //where li.Order.Date >= startDate && li.Order.Date < endDate
                        select li;
            //group li by li.Product;

            //UnitMeasure unitUM = context.UnitMeasures.Single(x => x.Name == "U");

            foreach (var li in query)
            {
                QueryLineItem copy = new QueryLineItem();
                copy.Quantity = li.Quantity;
                copy.UnitOfMeasure = li.UnitMeasure; ;
                copy.Product = li.Product;
                copy.Amount = li.Amount;

                result.Add(copy);

                //add ingredients
                if (includeIngredients && IsRecipe(copy.Product))
                    AddIngredients(copy.Product, copy.Quantity, copy.UnitOfMeasure, result);

            }//foreach

            return result;
        }

        private void AddIngredients(Product recipeProduct, double qtty, UnitMeasure um, List<QueryLineItem> result)
        {
            double proportion = qtty * um.ToBaseConversion / recipeProduct.RecipeQuantity / recipeProduct.RecipeUnitMeasure.ToBaseConversion;

            foreach (var ingredient in recipeProduct.Ingredients)
            {
                QueryLineItem ing_op = new QueryLineItem();

                ing_op.Quantity = proportion * ingredient.Quantity;
                ing_op.UnitOfMeasure = ingredient.UnitMeasure;
                ing_op.Product = ingredient.IngredientProduct;

                result.Add(ing_op);

                //add ingredients recursively
                if (IsRecipe(ingredient.IngredientProduct))
                    AddIngredients(ing_op.Product, ing_op.Quantity, ing_op.UnitOfMeasure, result);
            }
        }

        #endregion

        //public GraphDataAnswer GetGraphData(DateTime fromDate, DateTime toDate)
        //{
        //    DateTime startDate = fromDate.Date;
        //    DateTime endDate = toDate.Date.AddDays(1);

        //    decimal total_sales = 0;
        //    int total_clients = 0;
        //    int different_dates = 0;

        //    decimal spendingByClient = 0;
        //    decimal spendingDeviation = 0;

        //    List<SalesChartColumnViewModel> columnsData = new List<SalesChartColumnViewModel>();

        //    using (var context = new RestaurantDBEntities())
        //    {
        //        //get total sales and clients from Orders->Sales
        //        var daysQuery = from item in context.Orders.OfType<Sale>()
        //                        where item.Date >= startDate && item.Date < endDate
        //                        group item by item.Date into dategroup
        //                        orderby dategroup.Key
        //                        select dategroup;


        //        foreach (var group in daysQuery)
        //        {
        //            SalesChartColumnViewModel scc = new SalesChartColumnViewModel();

        //            scc.Date = group.Key;
        //            scc.X = group.Key.ToString("d/MMM"); ;

        //            total_sales += scc.TotalSale = Math.Round(group.Sum(x => x.Total));
        //            total_clients += scc.Clients = group.Sum(x => x.Persons);
        //            //total_orders += group.Count();
        //            different_dates++;

        //            columnsData.Add(scc);
        //        }

        //        ////get cost from lineitems
        //        //var lineitemsQuery = from li in context.LineItems.OfType<SaleLineItem>()
        //        //                     where li.Order.Date >= startDate && li.Order.Date < endDate 
        //        //                     group li by li.Order.Date;

        //        //var invSVC = ServiceContainer.GetService<IInventoryService>();

        //        //foreach (var group in lineitemsQuery)
        //        //{
        //        //    decimal cost = group.Sum(x => invSVC.GetProductCost(x.Product, x.Quantity, x.UnitMeasure));

        //        //    SalesChartColumnViewModel scc = columnsData.Single(x => x.Date == group.Key);
        //        //    scc.TotalCost = Math.Round(cost);
        //        //    scc.Profit = scc.TotalSale - scc.TotalCost;
        //        //}

        //        //SPENDING BY CLIENT
        //        if (total_clients > 0)
        //            spendingByClient = total_sales / total_clients;

        //        //SPENDING DEVIATION
        //        var queryLastSevenDaysWithClients = from item in context.Orders.OfType<Sale>()
        //                                            where item.Date >= startDate && item.Date < endDate && item.Persons > 0
        //                                            select item;

        //        if (queryLastSevenDaysWithClients.Count() > 0)
        //        {
        //            decimal average = queryLastSevenDaysWithClients.Average(x => x.Total / x.Persons);

        //            spendingDeviation = (decimal)Math.Sqrt(queryLastSevenDaysWithClients.Average(x => Math.Pow((double)(x.Total / x.Persons - average), 2)));
        //        }                
        //    }

        //    GraphDataAnswer answer = new GraphDataAnswer();
        //    answer.TotalSales = total_sales;
        //    answer.TotalClients = total_clients;
        //    answer.SpendingByClient = spendingByClient;
        //    answer.SpendingDeviation = spendingDeviation;

        //    answer.GraphData = columnsData;

        //    return answer;
        //}

        #endregion

    }
}
