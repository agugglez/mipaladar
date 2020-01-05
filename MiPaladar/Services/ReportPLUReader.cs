using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.ViewModels;
using MiPaladar.MVVM;

using System.ComponentModel;
using MiPaladar.Repository;
using System.Globalization;

namespace MiPaladar.Services
{
    public interface IReportPLUReader
    {
        int LoadReport(IUnitOfWork unitOfWork, string filepath, DateTime selectedDate, int shiftId, BackgroundWorker loadReportWorker);
    }

    public class ReportPLUReader : IReportPLUReader
    {
        public int LoadReport(IUnitOfWork unitOfWork, string filepath, DateTime selectedDate, int shiftId, BackgroundWorker loadReportWorker)
        {
            var fileReader = ServiceContainer.GetService<IFileReaderService>();
            List<string> lines = fileReader.ReadLines(filepath);

            //nonuseful lines always there
            if (lines.Count <= 5) throw new Exception("El reporte está vacío.");

            //make sure first line says "INFORME X CODIGOS"
            string firstLine = lines[0];
            string lineTitle = firstLine.Split(';')[5];

            if (lineTitle != "\"INFORME X CODIGOS\"") throw new Exception("Este no es el informe por códigos.");

            //Create new sale
            Sale new_sale = new Sale();
            new_sale.DateCreated = DateTime.Now;
            new_sale.Date = selectedDate;
            new_sale.ShiftId = shiftId;

            unitOfWork.OrderRepository.Add(new_sale);
            //new_sale.WorkSession = FindOrCreateWorkSession(loadreport_dialog.SelectedDate, loadreport_dialog.SelectedShift);

            decimal total_price = 0;
            decimal total_cost = 0;
            //skip 1 line, don't read the 5 last lines
            for (int i = 1; i < lines.Count - 4; i++)
            {
                string currentLine = lines[i];

                string[] array = currentLine.Split(';');

                //#4 PLU #5 Name #7 Qty #8 Price

                int plu;
                int.TryParse(array[4], out plu);

                //skip first and last quotes
                string name = array[5].Substring(1, array[5].Length - 2);

                if (name == "*TOTAL SECCION*") continue;

                double quantity;
                double.TryParse(array[7], out quantity);

                decimal price;
                //decimal.TryParse(array[8], out price);
                decimal.TryParse(array[8], NumberStyles.Currency, CultureInfo.InvariantCulture, out price);

                Product prod = unitOfWork.ProductRepository.GetFromPLU(plu);

                if (prod == null)
                {
                    prod = new Product();
                    prod.Code = plu;
                    prod.Name = name;
                    prod.SalePrice = price;
                    //prod = CreateProduct(plu, name, price / (decimal)quantity);
                }           

                //Product targetProduct = appvm.ProductsOC.Single(x => x.CodeString == plu.ToString());
                //svm.NewLineItem(quantity, prod);
                if (quantity > 0)
                {
                    decimal cost = AddLineItemToSale(unitOfWork, new_sale, quantity, prod);

                    total_price += (decimal)quantity * prod.SalePrice;
                    total_cost += cost;
                }

                //unitOfWork.SaveChanges();

                loadReportWorker.ReportProgress(i * 100 / lines.Count);
            }

            new_sale.Total = total_price;
            new_sale.TotalCost = total_cost;

            unitOfWork.SaveChanges();

            return new_sale.Id;

            //LoadReportDialogViewModel loadreport_dialog = new LoadReportDialogViewModel(appvm);

            //if (windowManager.ShowDialog(loadreport_dialog, appvm) == true)
            //{                
            //}
            //SelectingTable = !SelectingTable;

            //ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        //private Sale CreateSale(DateTime workDate)
        //{

        //    //find a new ID for the order
        //    //int newID = GenerateId();

        //    //create the new order
        //    Sale newvale = new Sale();
        //    //newvale.Number = newID;

        //    newvale.DateCreated = DateTime.Now;
        //    newvale.Date = workDate;

        //    appvm.Context.Orders.AddObject(newvale);

        //    return newvale;
        //}

        //Product GetProductFromPLU(int plu)
        //{
        //    foreach (var item in appvm.ProductsOC)
        //    {
        //        if (item.Code == plu) return item;
        //    }
        //    return null;
        //}

        //private Product CreateProduct(int plu, string name, decimal price)
        //{
        //    Product prod = new Product();
        //    prod.Name = name;
        //    prod.Code = plu;
        //    prod.SalePrice = price;
        //    prod.UMFamily = appvm.UnitMeasureManager.Quantity;
        //    prod.CostUnitMeasure = appvm.UnitMeasureManager.Unit;
        //    //prod.CostQuantity = 1;

        //    appvm.ProductsOC.Add(prod);

        //    return prod;
        //}

        private decimal AddLineItemToSale(IUnitOfWork unitOfWork, Sale sale, double qtty_to_add, Product product_to_add)
        {
            SaleLineItem newLineItem = new SaleLineItem();
            newLineItem.Quantity = qtty_to_add;
            newLineItem.Product = product_to_add;
            newLineItem.UnitMeasure = unitOfWork.UMRepository.Unit;// appvm.UnitMeasureManager.Unit;
            newLineItem.Amount = (decimal)qtty_to_add * product_to_add.SalePrice;

            sale.LineItems.Add(newLineItem);
            sale.SaleLineItems.Add(newLineItem);

            var invSVC = ServiceContainer.GetService<IInventoryService>();

            newLineItem.Cost = invSVC.GetProductCost(product_to_add, qtty_to_add, unitOfWork.UMRepository.Unit);

            return newLineItem.Cost;

            //ExecuteSellOperation(sale.Date, product_to_add, -qtty_to_add);
        }
    }
}
