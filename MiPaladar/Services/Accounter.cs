using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Data;
using System.Collections.Specialized;

using MiPaladar.Classes;
using MiPaladar.ViewModels;
using MiPaladar.Entities;

namespace MiPaladar.Services
{
    public class Accounter
    {
        //MainToolVM appModel;       
        
        //ObservableCollection<TotalByIngredient> totalsIngredient = new ObservableCollection<TotalByIngredient>();                

        //public Accounter(MainToolVM appModel) 
        //{
        //    this.appModel = appModel;
        //    //UpdateTotals(date, null);
        //    //date = DateTime.Today;
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void OnPropertyChanged(String info)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(info));
        //    }
        //}

        //public static void DeleteValeFile(Vale3 v)
        //{
        //    //Vale2 v = accounter.Vales.Single<Vale2>(x => x.ID == valeID);

        //    int day = v.Day;
        //    int month = v.Month;
        //    int year = v.Year;

        //    string todaydir = App.RootDirectory + year + '\\' + month + '\\' + day;

        //    string prefix = v.ListaPrecio.Name.Substring(0, 3);

        //    string threeDigitsID = v.ID.ToString().PadLeft(3, '0');

        //    string file = todaydir + "\\" + prefix + threeDigitsID + ".xml";
        //    File.Delete(file);
        //}

        //public static string GetDateDirectory(DateTime day)
        //{
        //    return App.RootDirectory + day.Year + '\\' + day.Month + '\\' + day.Day;
        //}
        //public static string GetDateDirectory(int year, int month, int day)
        //{
        //    return App.RootDirectory + year + '\\' + month + '\\' + day;
        //}

        //public static DateTime GetLatestExistingDate()
        //{
        //    string[] dirs = Directory.GetDirectories(App.RootDirectory);

        //    //get biggest year

        //    int maxYear = 0;

        //    foreach (string dir in dirs)
        //    {
        //        try
        //        {
        //            string yearS = dir.Substring(dir.LastIndexOf("\\") + 1);
        //            int year = int.Parse(yearS);

        //            maxYear = Math.Max(maxYear, year);
        //        }
        //        catch (Exception)
        //        {
        //            continue;
        //        }
        //    }

        //    //now biggest month

        //    dirs = Directory.GetDirectories(App.RootDirectory + maxYear);

        //    //get biggest year

        //    int maxMonth = 0;

        //    foreach (string dir in dirs)
        //    {
        //        try
        //        {
        //            string monthS = dir.Substring(dir.LastIndexOf("\\") + 1);
        //            int month = int.Parse(monthS);

        //            maxMonth = Math.Max(maxMonth, month);
        //        }
        //        catch (Exception)
        //        {
        //            continue;
        //        }
        //    }

        //    //now biggest day

        //    dirs = Directory.GetDirectories(App.RootDirectory + maxYear + "\\" + maxMonth);

        //    //get biggest year

        //    int maxDay = 0;

        //    foreach (string dir in dirs)
        //    {
        //        try
        //        {
        //            string dayS = dir.Substring(dir.LastIndexOf("\\") + 1);
        //            int day = int.Parse(dayS);

        //            maxDay = Math.Max(maxDay, day);
        //        }
        //        catch (Exception)
        //        {
        //            continue;
        //        }
        //    }

        //    return new DateTime(maxYear, maxMonth, maxDay);
        //}        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="today"> totals to be calculated for today or a span of days</param>
        //public void UpdateTotals(DateTime date, ListaPrecio lp)
        //{
        //    ValeSet vs = new ValeSet(date, false);

        //    UpdateTotalsByProduct(vs, lp, true);
        //    //UpdateTotalsByIngredient(date, date);
        //    UpdateTotalsByWaiter(vs, lp);
        //}
        //public void UpdateTotals(Collection<SalesOrder> salesOrders, Collection<Purchase> purchaseOrders, DateTime fromDate, DateTime toDate)
        //{
        //    var vales = from v in salesOrders
        //                where v.RealDateTime >= fromDate && v.RealDateTime <= toDate
        //                select v;

        //    var compras = from v in purchaseOrders
        //                  where v.Date >= fromDate && v.Date <= toDate
        //                  select v;

        //    UpdateTotalsByProduct(fromDate, toDate);
        //    UpdateTotalsByWaiter(vales);
        //    UpdateTotalsByMesa(vales);
        //    UpdateTotalsByCentro(vales);

        //    UpdateTotalsCompras(fromDate, toDate);
        //}
        //public void UpdateTotals(DateTime fromDate, DateTime to, PriceList lp)
        //{
        //    var vales = from v in appModel.SalesOrders
        //                where v.PriceList == lp && v.WorkingDate >= fromDate && v.WorkingDate <= to
        //                select v;

        //    var compras = from v in appModel.PurchaseOrders
        //                  where v.Date >= fromDate && v.Date <= to
        //                  select v;

        //    UpdateTotalsByProduct(fromDate, to);
        //    UpdateTotalsByWaiter(vales);
        //    UpdateTotalsByMesa(vales);
        //    UpdateTotalsByCentro(vales);

        //    UpdateTotalsCompras(fromDate, to);
        //}

        //List<TotalByProduct> totalsProduct = new List<TotalByProduct>();

        public static List<TotalByProduct> UpdateTotalsByProduct(RestaurantDBEntities context,
            PriceList centro, DateTime fromDate, DateTime toDate) 
        {
            List<TotalByProduct> totalsProduct = new List<TotalByProduct>();
            
            //bool mostrar_aparecen = fromDate == toDate;

            //var directProducts =
            //    from valeItem in orderItems
            //    where valeItem.Order.RealDateTime >= fromDate && valeItem.Order.RealDateTime <= toDate                
            //    select valeItem;
            
            //List<ValeItemExpanded> valeItems = new List<ValeItemExpanded>();

            ////ValeSet vs = new ValeSet(from, to, false);
            int centroId = centro != null ? centro.Id : 0;

            var sales = from s in context.Orders.OfType<Sale>()
                        where s.Date >= fromDate && s.Date <= toDate &&
                        (centroId != 0 ? s.Table.PriceList.Id == centroId : true)
                        select s;

            List<ProductOperation> orderItems = new List<ProductOperation>();
            //totals by product            
            foreach (var sale in sales)
            {
                foreach (var lineitem in sale.LineItems)
                {
                    var prod = lineitem.Product;

                    if (prod == null) continue;
                    
                    orderItems.Add(new ProductOperation()
                    {
                        Product = prod,
                        Total = lineitem.Quantity
                    });

                    //desglose                    
                    if (prod.IsRecipe)
                    {
                        foreach (Ingredient tbp in prod.Ingredients)
                        {
                            if (tbp.IngredientProduct == null) continue;

                            orderItems.Add(new ProductOperation()
                            {
                                Product = tbp.IngredientProduct,
                                Total = lineitem.Quantity * tbp.Quantity
                            });
                            //AddProductTo(totalsProduct, vale, tbp.Product, tbp.Total * vi.Quantity, mostrar_aparecen);
                        }
                    }
                }                
            }
            //var orderItems = from oi in context.LineItems
            //                 let order = oi.Order
            //                 where order is Sale && order.Date >= fromDate && order.Date <= toDate
            //                 select new ProductOperation()
            //                 {
            //                     Product = oi.Product,
            //                     Total = oi.Quantity
            //                 };

            

            //foreach (ProductOperation vi in orderItems)
            //{
            //    if (vi.Product == null) continue;                

            //    //AddProductTo(totalsProduct, vale, vi.Product, vi.Quantity, mostrar_aparecen);

            //    todos.Add(vi);

                
            //}

            var sum = from orderitem in orderItems
                      group orderitem by orderitem.Product into grouping                      
                      select new TotalByProduct 
                      {
                          Product = grouping.Key,
                          Total = grouping.Sum(vi => vi.Total)
                      };

            //var directProducts =
            //    from valeItem in App.Ctx.OrderItems
            //    where valeItem.Order.RealDateTime >= fromDate && valeItem.Order.RealDateTime <= toDate
            //    group valeItem by valeItem.Product into grouping
            //    select new TotalByProduct
            //    {
            //        Product = grouping.Key,
            //        Total = grouping.Sum(vi => vi.Quantity),
            //        //FoundIn = !mostrar_aparecen ? string.Empty : grouping.Aggregate(string.Empty, (workingSentence, next) => workingSentence + ", " + next.Order.Number)                    
            //    };                        

            //var ingredientProducts =
            //    from valeItem in App.Ctx.OrderItems
            //    where valeItem.Product.IsComposite && 
            //        valeItem.Product.Ingredients.Count > 0 && 
            //        valeItem.Order.WorkingDate >= fromDate && 
            //        valeItem.Order.WorkingDate <= toDate
            //    join ingredient in App.Ctx.Ingredients on valeItem.Product equals ingredient.BaseProduct
            //    group ingredient by ingredient.IngredientProduct into grouping
            //    select new TotalByProduct
            //    {
            //        Product = grouping.Key,
            //        Total = grouping.Sum(vi => vi.Quantity)
            //    };

            //var todos = from t in directProducts.Union(ingredientProducts)
            //            group t by t.Product into grouping
            //            select new TotalByProduct 
            //            {
            //                Product = grouping.Key,
            //                Total = grouping.Sum(vi => vi.Total)
            //            };

            foreach (var tbp in sum)
            {
                if (tbp.Product != null) totalsProduct.Add(tbp);
            }

            return totalsProduct;
        }

        //ObservableCollection<TotalByProduct> totalsCompras = new ObservableCollection<TotalByProduct>();

        public static List<TotalByProduct> UpdateTotalsCompras(RestaurantDBEntities context,
            DateTime fromDate, DateTime toDate)
        {
            List<TotalByProduct> totalsCompras = new List<TotalByProduct>();

            var purchasetotales = from pi in context.LineItems
                                  where pi.Order is Purchase &&
                                    pi.Order.Date >= fromDate && pi.Order.Date <= toDate &&
                                    pi.Product != null
                                  group pi by pi.Product into grouping
                                  select grouping;
                                  //select new TotalByProduct
                                  //{
                                  //    Product = grouping.Key,
                                  //    Total = grouping.Sum(ci => ci.Quantity),
                                  //    Costo = grouping.Sum(ci => ((PurchaseLineItem)ci).Amount)
                                  //};

            foreach (var group in purchasetotales)
            {
                TotalByProduct tbp = new TotalByProduct();
                foreach (PurchaseLineItem pi in group)
                {
                    tbp.Product = group.Key;
                    tbp.Total += pi.Quantity;
                    tbp.Costo += pi.Amount;
                }
                totalsCompras.Add(tbp);
            }
            //totalsCompras = purchasetotales.ToList();

            return totalsCompras;
        }

        public static List<TotalByDependiente> UpdateTotalsByWaiter(RestaurantDBEntities context,  DateTime fromDate, DateTime toDate)
        {
            List<TotalByDependiente> totalsWaiter = new List<TotalByDependiente>();

            var prodCountQuery = from vale in context.Orders.OfType<Sale>()
                                 where vale.Date >= fromDate && vale.Date<=toDate
                                 group vale by vale.Employee into grouping
                                 select new TotalByDependiente
                                 {
                                     Dependiente = grouping.Key.Name,
                                     TotalVentas = grouping.Sum(v => v.Total),
                                     TotalMesas = grouping.Count(),
                                     ClientesAtendidos = grouping.Sum(v => v.Closed ? v.Persons : 0),
                                     ClientesSinAtender = grouping.Sum(v => !v.Closed ? v.Persons : 0)
                                 };

            TotalByDependiente tbdTotals = new TotalByDependiente();
            tbdTotals.Dependiente = "Total";

            foreach (var tbd in prodCountQuery)
            {
                totalsWaiter.Add(tbd);

                tbdTotals.TotalVentas += tbd.TotalVentas;
                tbdTotals.TotalMesas += tbd.TotalMesas;
                tbdTotals.ClientesAtendidos += tbd.ClientesAtendidos;
                tbdTotals.ClientesSinAtender += tbd.ClientesSinAtender;
            }

            totalsWaiter.Add(tbdTotals);

            return totalsWaiter;
        }

        public static List<TotalByWaiterByProduct> UpdateTotalsByWaiterByProducts(RestaurantDBEntities context, DateTime fromDate, DateTime toDate)
        {
            List<TotalByWaiterByProduct> totalsByWP = new List<TotalByWaiterByProduct>();

            var orderItems = from orderitem in context.LineItems
                             where orderitem.Order is Sale && orderitem.Order.Date >= fromDate &&
                                   orderitem.Order.Date <= toDate
                             select orderitem;

            //desglosar productos compuestos
            List<ProductOperation> todos = new List<ProductOperation>();

            foreach (var item in orderItems)
            {
                if (item.Product == null) continue;
                if (item.Order.Date < fromDate || item.Order.Date > toDate) continue;
                
                Sale sale = item.Order as Sale;
                if (sale == null) continue;

                todos.Add(          
                    new ProductOperation
                    {
                        Product = item.Product,
                        Total = item.Quantity,
                        Waiter = sale.Employee
                    }
                );

                if (item.Product.IsRecipe)
                {
                    foreach (Ingredient tbp in item.Product.Ingredients)
                    {
                        if (tbp.IngredientProduct == null) continue;

                        todos.Add(new ProductOperation
                        {
                            Waiter = sale.Employee,
                            Product = tbp.IngredientProduct,
                            Total = item.Quantity * tbp.Quantity
                        });
                    }
                }                
            }

            

            //foreach (LineItem vi in orderItems)
            //{
            //    if (vi.Product == null) continue;

            //    todos.Add(vi);

            //    //vi.Waiter = (vi.Order as Sale).Waiter;

            //    if (vi.Product.IsComposite)
            //    {
            //        foreach (Ingredient tbp in vi.Product.Ingredients)
            //        {
            //            if (tbp.IngredientProduct == null) continue;

            //            todos.Add(new LineItem()
            //            {
            //                Waiter = (vi.Order as Sale).Waiter,
            //                Product = tbp.IngredientProduct,
            //                Quantity = vi.Quantity * tbp.Quantity
            //            });
            //        }
            //    }
            //}

            //a sumar
            var query = from operation in todos
                        group operation by operation.Waiter into groupingByWaiter
                        from groupingByProduct in
                            (from orderitem in groupingByWaiter
                             group orderitem by orderitem.Product)
                        select new TotalByWaiterByProduct
                        {
                            Waiter = groupingByWaiter.Key,
                            Product = groupingByProduct.Key,
                            Total = groupingByProduct.Sum(op => op.Total)
                        };

            foreach (var item in query)
            {
                totalsByWP.Add(item);
            }

            return totalsByWP;
        }

        public static List<TotalByDependiente> ShortUpdateTotalsByWaiter(IEnumerable<Sale> vales)
        {
            //by waiter
            List<TotalByDependiente> shortTotalsWaiter = new List<TotalByDependiente>();

            var prodCountQuery =
                from vale in vales
                where vale.Employee != null
                group vale by vale.Employee into grouping
                select new TotalByDependiente
                {
                    Dependiente = grouping.Key.Name,
                    //TotalVentas = grouping.Sum(v => v.TotalPrice),
                    TotalMesas = grouping.Count(),
                    ClientesAtendidos = grouping.Sum(v => v.Paid ? v.Persons : 0),
                    ClientesSinAtender = grouping.Sum(v => !v.Paid ? v.Persons : 0)
                };

            foreach (var tbd in prodCountQuery)
            {
                shortTotalsWaiter.Add(tbd);
            }

            return shortTotalsWaiter;
        }

        public static List<TotalByMesa> UpdateTotalsByMesa(IEnumerable<Sale> vales)
        {
            //by mesa

            List<TotalByMesa> totalsMesa = new List<TotalByMesa>();

            var prodCountQuery =
                from vale in vales
                where vale.Table != null
                group vale by vale.Table into grouping
                select new TotalByMesa
                {
                    Mesa = grouping.Key.Number.ToString(),
                    TotalVentas = grouping.Sum(v => v.Total),
                    TotalClientes = grouping.Sum(v => v.Persons)
                };

            TotalByMesa tbmTotals = new TotalByMesa();
            tbmTotals.Mesa = "Total";

            foreach (var tbm in prodCountQuery)
            {
                totalsMesa.Add(tbm);

                tbmTotals.TotalVentas += tbm.TotalVentas;
                tbmTotals.TotalClientes += tbm.TotalClientes;
            }

            totalsMesa.Add(tbmTotals);

            return totalsMesa;
        }

        public static List<TotalByCentro> UpdateTotalsByCentro(IEnumerable<Sale> vales)
        {
            List<TotalByCentro> totalsCentro = new List<TotalByCentro>();

            var prodCountQuery =
                from vale in vales
                where vale.Table != null
                group vale by vale.Table.PriceList into grouping
                select new TotalByCentro
                {
                    Centro = grouping.Key,
                    TotalVentas = grouping.Sum(v => v.Total),
                    TotalClientes = grouping.Sum(v => v.Persons),
                    TotalMesas = grouping.Count()
                };

            TotalByCentro tbcTotals = new TotalByCentro();
            tbcTotals.Centro = new PriceList() { Name = "Total" };

            foreach (var tbc in prodCountQuery)
            {
                totalsCentro.Add(tbc);

                tbcTotals.TotalVentas += tbc.TotalVentas;
                tbcTotals.TotalClientes += tbc.TotalClientes;
                tbcTotals.TotalMesas += tbc.TotalMesas;
            }

            totalsCentro.Add(tbcTotals);

            return totalsCentro;
        }        

        //const int numberOfDays = 7;

        //public static List<DayData> GetDayTotals(RestaurantDBEntities context, DateTime startDate, DateTime finishDate)
        //{
        //    //ObservableCollection<Vale2> targetVales = null;

        //    List<DayData> dayTotals = new List<DayData>();

        //    //numberOfDays = (int)DateTime.Today.DayOfWeek;

        //    //if (numberOfDays == 0) numberOfDays = 7;

        //    //always do a week
        //    //DateTime firstDate = DateTime.Today.AddDays(-numberOfDays + 1);
        //    //DateTime today = DateTime.Today;

        //    //INICIO
        //    //var existences = from e in context.Existences
        //    //                 where e.Date >= startDate && e.Date <= finishDate
        //    //                 select e;

        //    //foreach (var existence in existences)
        //    //{
        //    //    if (existence.Product == null) continue;

        //    //    int i = (existence.Date - startDate).Days;

        //    //    AddProductToAlmacen(dayTotals, 
        //    //        startDate, FieldType.Inicio, 
        //    //        existence.Product, existence.Quantity);
        //    //}

        //    //COMPRAS
        //    //var queryCompras = from compra in purchaseItems
        //    //                   where compra.Purchase.Date >= firstDate && compra.Purchase.Date <= today
        //    //                   select compra;
            
        //    //var purchaseItems = from purchaseItem in context.LineItems
        //    //                    where purchaseItem.Order.Date >= startDate && purchaseItem.Order.Date <= finishDate
        //    //                    select purchaseItem;

        //    //foreach (var compra in purchaseItems)
        //    //{
        //    //    if (compra.Product == null) continue;

        //    //    //int i = (compra.Purchase.Date - firstDate).Days;

        //    //    AddProductToAlmacen(dayTotals, startDate, FieldType.Compras, compra.Product, compra.Quantity);

        //    //    if (compra.Product.IsComposite)
        //    //    {
        //    //        foreach (Ingredient tbp in compra.Product.Ingredients)
        //    //        {
        //    //            if (tbp.IngredientProduct == null) continue;

        //    //            AddProductToAlmacen(dayTotals, startDate, 
        //    //                FieldType.Compras, tbp.IngredientProduct, 
        //    //                tbp.Quantity * compra.Quantity);
        //    //        }
        //    //    }
        //    //}

        //    //VENTAS
        //    //var queryVentas = from venta in orderItems
        //    //                   where venta.Order.RealDateTime >= firstDate && 
        //    //                   venta.Order.RealDateTime <= today
        //    //                   select venta;

        //    var productOperations = from lineitem in context.LineItems
        //                            where lineitem.Product != null && lineitem.Order.Date >= startDate && lineitem.Order.Date <= finishDate
        //                            select new ProductOperation()
        //                            {
        //                                Product = lineitem.Product,
        //                                Total = lineitem.Quantity,
        //                                Date = lineitem.Order.Date,
        //                                Vale = lineitem.Order
        //                            };            

        //    List<ProductOperation> allOperations = new List<ProductOperation>(productOperations);            

        //    foreach (var po in productOperations)
        //    {
        //        //AddProductToAlmacen(dayTotals, startDate, FieldType.Ventas, lineitem.Product, lineitem.Quantity);

        //        allOperations.Add(po);

        //        if (po.Product.IsRecipe)
        //        {
        //            foreach (Ingredient tbp in po.Product.Ingredients)
        //            {
        //                if (tbp.IngredientProduct == null) continue;

        //                allOperations.Add(new ProductOperation() 
        //                {
        //                    Product = tbp.IngredientProduct,
        //                    Total = tbp.Quantity * po.Total,
        //                    Date = po.Date,
        //                    Vale = po.Vale
        //                });

        //                //AddProductToAlmacen(dayTotals, startDate, 
        //                //    FieldType.Ventas, tbp.IngredientProduct, tbp.Quantity * po.Quantity);
        //            }
        //        }
        //    }

        //    var query = from operation in allOperations
        //                group operation by operation.Date into groupingByDate
        //                from groupingByProduct in
        //                    (from orderitem in groupingByDate
        //                     group orderitem by orderitem.Product)
        //                select new DayData
        //                {
        //                    Date = groupingByDate.Key,
        //                    Product = groupingByProduct.Key,
        //                    Init = FindExistence(context, groupingByDate.Key, groupingByProduct.Key),
        //                    Purchased = groupingByProduct.Where(x => x.Vale is Purchase).Sum(oi => oi.Total),
        //                    Sales = groupingByProduct.Where(x => x.Vale is Sale).Sum(oi => oi.Total)
        //                };

        //    //for (int i = 0; firstDate <= DateTime.Today; i++)
        //    //{
        //    //    if (ProductExistence.Exists(firstDate))
        //    //    {
        //    //        ProductExistence pe = new ProductExistence(firstDate);

        //    //        foreach (TotalByProduct tbp in pe.Totals)
        //    //        {
        //    //            if (tbp.ProductOld == null) 
        //    //                continue;
        //    //            AddProductToAlmacen(FieldType.Inicio, tbp.ProductOld, tbp.Total, i);
        //    //        }
        //    //    }

        //    //    CompraSet compras = new CompraSet(firstDate, false);
        //    //    //COMPRAS
        //    //    foreach (Compra compra in compras.Vales)

        //    //        foreach (CompraItem ci in compra.ValeItems)
        //    //        {
        //    //            if (ci.Product == null) continue;

        //    //            AddProductToAlmacen(FieldType.Compras, ci.Product, ci.Quantity, i);
        //    //        }

        //    //    VENTAS
        //    //    ValeSet ventas = new ValeSet(firstDate, false);

        //    //    foreach (Vale3 vale in ventas.Vales)
        //    //    {
        //    //        foreach (ValeItem vi in vale.ValeItems)
        //    //        {
        //    //            if (vi.Product == null) continue;

        //    //            AddProductToAlmacen(FieldType.Ventas, vi.Product, vi.Quantity, i);

        //    //            //ingredients
        //    //            if (vi.Product.Composite)
        //    //            {
        //    //                foreach (TotalByProduct tbp in vi.Product.IngredientItems)
        //    //                {
        //    //                    if (tbp.ProductOld == null) continue;

        //    //                    AddProductToAlmacen(FieldType.Ventas, tbp.ProductOld, tbp.Total * vi.Quantity, i);
        //    //                }
        //    //            }
        //    //        }
        //    //    }

        //    //    firstDate = firstDate.AddDays(1);
        //    //}            

        //    //last colum, current existence
        //    //foreach (CurrentExistence tbp in context.CurrentExistences)
        //    //{
        //    //    AddProductToAlmacen(totalsAlmacen, firstDate, FieldType.Ahora, tbp.Product, tbp.Quantity, -1);
        //    //}

        //    //event handler on
        //    //foreach (var item in totalsAlmacen)
        //    //{
        //    //    InitWeekEvents(item);
        //    //}

        //    return query.ToList();
        //}

        //private static double FindExistence(RestaurantDBEntities context, DateTime dateTime, Product product)
        //{
        //    var result = from ex in context.InventoryTraces
        //                 where ex.Product.Id == product.Id && ex.Date == dateTime
        //                 select ex;

        //    if (result.Count() > 1) throw new Exception("more than one existence");
        //    else if (result.Count() == 0) return 0;
        //    else return result.First().Quantity;
            
        //}

        enum FieldType { Inicio, Compras, Ventas }

        //static void AddProductToAlmacen(List<DayData> totalsAlmacen, DateTime date, FieldType ft, Product prod, float count)
        //{
        //    DayData daydata = null;

        //    foreach (var item in totalsAlmacen)
        //    {
        //        if (item.Product == prod && item.Date == date)
        //        { daydata = item; break; }
        //    }

        //    if (daydata == null)
        //    {
        //        daydata = new DayData() { Product = prod };

        //        totalsAlmacen.Add(daydata);
        //    }

        //    switch (ft)
        //    {
        //        case FieldType.Inicio:
        //            daydata.Init = count;
        //            break;
        //        case FieldType.Compras:
        //            daydata.Purchased += count;
        //            break;
        //        case FieldType.Ventas:
        //            daydata.Sales += count;
        //            break;
        //        default:
        //            break;
        //    }

        //}
        //#region WeekData Handling

        ////float previous;

        
        //#endregion        

        //public static List<ProductOperation> UpdateProductOperations(RestaurantDBEntities context, DateTime fromDate, DateTime toDate)
        //{
        //    List<ProductOperation> product_operations = new List<ProductOperation>();

        //    var targetVales = from so in context.Orders.OfType<Sale>()
        //                      where so.Date >= fromDate && so.Date <= toDate
        //                      select so;

        //    foreach (Sale vale in targetVales)
        //    {
        //        foreach (LineItem vi in vale.LineItems)
        //        {
        //            if (vi.Product == null) continue;

        //            //product
        //            ProductOperation po = new ProductOperation();

        //            //po.Type = "Venta";
        //            po.Total = vi.Quantity;
        //            po.Date = vale.Date;
        //            po.Vale = vale;
        //            po.Product = vi.Product;

        //            product_operations.Add(po);

        //            if (vi.Product.IsComposite)
        //                //ingredients
        //                foreach (Ingredient tbp in vi.Product.Ingredients)
        //                {
        //                    po = new ProductOperation();

        //                    //po.Type = "Venta";
        //                    po.Total = vi.Quantity * tbp.Quantity;
        //                    po.Date = vale.Date;
        //                    po.Vale = vale;
        //                    po.Product = vi.Product;

        //                    product_operations.Add(po);
        //                }
        //        }
        //    }

        //    var cs = from p in context.Orders.OfType<Purchase>()
        //             where p.Date >= fromDate && p.Date <= toDate
        //             select p;

        //    foreach (Purchase compra in cs)

        //        foreach (LineItem ci in compra.LineItems)
        //        {
        //            if (ci.Product == null) continue;
                    
        //            ProductOperation po = new ProductOperation();

        //            po.Total = ci.Quantity;
        //            //po.Type = compra.PurchaseType.ToString();
        //            po.Date = compra.Date;
        //            po.Vale = compra;

        //            product_operations.Add(po);                    
        //        }

        //    return product_operations;
        //}

        //static float ContainsIngredient(Product product, Product ingredient)
        //{
        //    foreach (Ingredient tbp in product.Ingredients)
        //    {
        //        if (tbp.IngredientProduct == ingredient) return tbp.Quantity;
        //    }

        //    return 0;
        //}

        ///// <summary>
        ///// inicio will get modified
        ///// </summary>
        //public static void ComputeDayEnding(
        //    DateTime from,
        //    DateTime to,
        //    ObservableCollection<TotalByProduct> inicio)
        //{
        //    ValeSet target_vales = new ValeSet(from, to, false);
        //    CompraSet target_compras = new CompraSet(from, to, false);

        //    foreach (Compra compra in target_compras.Vales)

        //        foreach (CompraItem ci in compra.ValeItems)
        //        {                    
        //            if (ci.Product == null) continue;

        //            //only consider contable/ingredient products
        //            if (!ci.Product.Contable && !ci.Product.IsIngredient) continue;

        //            TotalByProduct pt = null;

        //            foreach (TotalByProduct item in inicio)
        //            {
        //                if (item.ProductOld == ci.Product)
        //                { pt = item; break; }
        //            }

        //            //if it didnt exist, create a new one
        //            if (pt == null)
        //            {
        //                if (ci.Product != null)
        //                {
        //                    pt = new TotalByProduct();
        //                    pt.ProductOld = ci.Product;

        //                    inicio.Add(pt);
        //                }
        //            }

        //            if (pt != null)
        //            {
        //                pt.Total += ci.Quantity;
        //                //if (mostrar_apareceen) pt.FoundIn += compra.ID + ", ";
        //            }
        //        }

        //    foreach (Vale3 vale in target_vales.Vales)

        //        foreach (ValeItem vi in vale.ValeItems)
        //        {
        //            if (vi.Product == null) continue;

        //            if (vi.Product.Contable || vi.Product.IsIngredient)
        //                RemoveProductFrom(inicio, vale, vi.Product, vi.Quantity);

        //            if (vi.Product.Composite)
        //            {
        //                foreach (TotalByProduct tbp in vi.Product.IngredientItems)
        //                {
        //                    RemoveProductFrom(inicio, vale, tbp.ProductOld, tbp.Total * vi.Quantity);
        //                }
        //            }
        //        }

        //    //return result;
        //}

        //static void RemoveProductFrom(ObservableCollection<TotalByProduct> targetTotals, Vale3 vale, Product3 p2, float count)
        //{
        //    TotalByProduct pt = null; 

        //    foreach (TotalByProduct item in targetTotals)
        //    {
        //        if (item.ProductOld == p2)
        //        { pt = item; break; }
        //    }

        //    //if it didnt exist, create a new one
        //    if (pt == null)
        //    {
        //        if (p2 != null)
        //        {
        //            pt = new TotalByProduct();
        //            pt.ProductOld = p2;

        //            targetTotals.Add(pt);
        //        }
        //    }

        //    if (pt != null)
        //    {
        //        pt.Total -= count;
        //    }
        //}        

        //float ContainsIngredient(Product product, Product ingredient)
        //{
        //    foreach (Ingredient tbp in product.Ingredients)
        //    {
        //        if (tbp.IngredientProduct == ingredient) return tbp.Quantity;
        //    }

        //    return 0;
        //}

        //public ObservableCollection<TotalByIngredient> TotalsByIngredient 
        //{
        //    get { return totalsIngredient; }
        //}
        //public ObservableCollection<TotalByDependiente> TotalsByDependiente 
        //{
        //    get { return totalsWaiter; }
        //}
        //public ObservableCollection<TotalByDependiente> ShortTotalsByWaiter
        //{
        //    get { return shortTotalsWaiter; }
        //}
        //public ObservableCollection<TotalByProduct> TotalsByProduct
        //{
        //    get { return totalsProduct; }
        //}

        //public ObservableCollection<TotalByMesa> TotalsByMesa
        //{
        //    get { return totalsMesa; }
        //}

        //public ObservableCollection<TotalByCentro> TotalsByCentro
        //{
        //    get { return totalsCentro; }
        //}

        //public ObservableCollection<TotalByProduct> TotalsCompras
        //{
        //    get { return totalsCompras; }
        //}

        //public ObservableCollection<WeekData> TotalsAlmacen
        //{
        //    get { return totalsAlmacen; }
        //}

        //public ObservableCollection<ProductOperation> ProductOperations
        //{
        //    get { return product_operations; }
        //}

        //DateTime date;
        //public DateTime WorkingDate 
        //{
        //    get 
        //    {
        //        return date;
        //    }
        //    set 
        //    {
        //        date = value;
        //        OnPropertyChanged("WorkingDate");
        //        OnPropertyChanged("ShortWorkingDate");
        //    }
        //}

        //public string ShortWorkingDate 
        //{
        //    get 
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append(((Dias)date.DayOfWeek).ToString());
        //        sb.Append(", "+ date.Day);
        //        sb.Append(" " + ((Meses)date.Month - 1).ToString());
        //        sb.Append("/" + date.Year);

        //        return sb.ToString(); 
        //    }
        //}
    }
}
