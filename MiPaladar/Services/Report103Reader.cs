using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Text.RegularExpressions;

using MiPaladar.Classes;
using MiPaladar.ViewModels;
using MiPaladar.MVVM;
using MiPaladar.Entities;
using MiPaladar.Repository;
using System.Globalization;

namespace MiPaladar.Services
{
    public interface IReport103Reader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="filepath"></param>
        /// <param name="workDate"></param>
        /// <param name="shiftId"></param>
        /// <param name="load101Worker"></param>
        /// <returns>the id's of the newly created sales</returns>
        List<int> LoadReport103(IUnitOfWork unitOfWork, string filepath, DateTime workDate,
            int shiftId, BackgroundWorker load101Worker);
    }

    public class Report103Answer
    {
    }

    public class Report103Reader : IReport103Reader
    {
        public List<int> LoadReport103(IUnitOfWork unitOfWork, string filepath, DateTime workDate, 
            int shiftId, BackgroundWorker load101Worker)
        {
            var fileReader = ServiceContainer.GetService<IFileReaderService>();
            List<string> lines = fileReader.ReadLines(filepath);

            //at least 5 lines always there
            if (lines.Count <= 5) throw new Exception("El reporte está vacío.");

            //make sure first line says "Electronic Journal"
            string firstLine = lines[0];

            if (!firstLine.Contains("\"Electronic Journal\"")) throw new Exception("Este no es el reporte 103.");

            //make sure 3rd line starts with "103"
            string thirdLine = lines[2];

            Report103Line test;
            if (!TryParseReport103Line(thirdLine, out test) || test.EJournalId != 103) throw new Exception("El reporte no es el 103 o está vacío");

            var activeLinesOfWork = new Dictionary<int, LineOfWork>();

            List<int> result = new List<int>();

            List<Report103Line> current_operation_lines = new List<Report103Line>();

            int current_receipt = test.ReceiptNumber;
                        
            for (int i = 0; i < lines.Count; i++)
            {
                //Report103Line's start with 103
                if (!lines[i].StartsWith("103")) continue;

                Report103Line line;
                if (!TryParseReport103Line(lines[i], out line)) throw new Exception("Hay bateo con este reporte: línea " + i);

                if (line.ReceiptNumber != current_receipt)
                {
                    ProcessReceipt(unitOfWork, activeLinesOfWork, workDate, shiftId, current_operation_lines, result);

                    current_operation_lines.Clear();

                    current_receipt = line.ReceiptNumber;
                }

                current_operation_lines.Add(line);

                load101Worker.ReportProgress(i * 100 / lines.Count);
            }

            //last receipt
            if (current_operation_lines.Count > 0) 
            {
                ProcessReceipt(unitOfWork, activeLinesOfWork, workDate, shiftId, current_operation_lines, result);
                load101Worker.ReportProgress(100);
            }

            return result;
        }

        bool TryParseReport103Line(string line, out Report103Line result)
        {
            result = new Report103Line();

            string[]  pieces = line.Split(';');

            //each line has 16 values
            if (pieces.Length != 16) return false;

            int eJournalId;
            if (!int.TryParse(pieces[0], out eJournalId)) return false;
            result.EJournalId = eJournalId;

            DateTime dateAndTime;
            if (!TryParseDateAndTime(pieces[3], pieces[4], out dateAndTime)) return false;
            result.DateAndTime = dateAndTime;

            int receiptNumber;
            if (!int.TryParse(pieces[5], out receiptNumber)) return false;
            result.ReceiptNumber = receiptNumber;

            int salesPerson;
            if (!string.IsNullOrWhiteSpace(pieces[6]))
            {
                if (!int.TryParse(pieces[6], out salesPerson)) return false;
                result.SalesPerson = salesPerson;
            }

            int clerkNumber;
            if (!int.TryParse(pieces[10], out clerkNumber)) return false;
            result.ClerkNumber = clerkNumber;

            int functionType;
            if (!int.TryParse(pieces[11], out functionType)) return false;
            result.FunctionType = functionType;

            int functionNumber;
            if (!int.TryParse(pieces[12], out functionNumber)) return false;
            result.FunctionNumber = functionNumber;

            string functionText = pieces[13];
            result.FunctionText = functionText;

            int quantity;
            if (!string.IsNullOrWhiteSpace(pieces[14]))
            {
                if (!int.TryParse(pieces[14], out quantity)) return false;
                result.Quantity = quantity;
            }            

            decimal amount;
            if (!string.IsNullOrWhiteSpace(pieces[15]))
            {
                if (!decimal.TryParse(pieces[15], NumberStyles.Currency, CultureInfo.InvariantCulture, out amount)) return false;
                //if (!decimal.TryParse(pieces[15], out amount)) return false;
                result.Amount = amount;
            }            

            return true;
        }

        bool TryParseDateAndTime(string datePart, string timePart, out DateTime result) 
        {
            result = new DateTime();

            //DATE
            string date_pattern = @"(.+)-(.+)-(.+)";            

            Regex date_rgx = new Regex(date_pattern);

            Match date_match = date_rgx.Match(datePart);

            if (!date_match.Success) return false;

            int day = int.Parse(date_match.Groups[1].Value);
            int month = int.Parse(date_match.Groups[2].Value);
            int year = int.Parse(date_match.Groups[3].Value);

            //TIME
            string time_pattern = @"(\d+):(\d+):(\d+)\s(AM|PM)";

            Regex time_rgx = new Regex(time_pattern);

            Match time_match = time_rgx.Match(timePart);

            if (!time_match.Success) return false;

            int hour = int.Parse(time_match.Groups[1].Value);
            int minutes = int.Parse(time_match.Groups[2].Value);
            int seconds = int.Parse(time_match.Groups[3].Value);            

            bool pm = time_match.Groups[4].Value == "PM";

            if (pm && hour != 12) hour += 12;
            if (!pm && hour == 12) hour = 0;

            result = new DateTime(year, month, day, hour, minutes, seconds);
            
            return true;
        }

        void ProcessReceipt(IUnitOfWork unitOfWork, Dictionary<int, LineOfWork> activeLinesOfWork,
            DateTime workDate, int shiftId, List<Report103Line> receiptLines, List<int> result)
        {
            Report103Line firstLine = receiptLines[0];
            DateTime receiptDate = firstLine.DateAndTime; //date appears in the line before last 

            if (firstLine.FunctionText.Contains("*** FACTURA ***"))
            {
                Sale newSale = new Sale();

                //FACTURA #
                newSale.Number = firstLine.Quantity;//first line shows factura #

                //MESA #
                int tableNumber = receiptLines[1].Quantity;
                LineOfWork low = activeLinesOfWork[tableNumber];

                //work date
                newSale.Date = workDate;
                //get date created from LineOfWork
                newSale.DateCreated = low.StartDate;
                //print ticket to client date 
                newSale.DatePrinted = low.PrintTicketDate;
                //FACTURA DATE
                newSale.DateClosed = receiptDate;

                //TURNO
                newSale.ShiftId = shiftId;

                //CLIENTE #
                //sometimes this line doesn't appear
                if (receiptLines[2].FunctionText.Contains("CLIENTE #"))
                {
                    newSale.Persons = receiptLines[2].Quantity;
                }

                //LINEITEMS
                for (int i = 3; i < receiptLines.Count; i++)
                {
                    Report103Line line = receiptLines[i];

                    //los lineitems tienen 10000 ahí, no preguntes pq
                    if (line.FunctionType == 10000)
                    {
                        //check for new product
                        int plu = line.FunctionNumber;
                        Product prod = unitOfWork.ProductRepository.GetFromPLU(plu);

                        if (prod == null)
                        {
                            prod = new Product();
                            prod.Code = plu; 
                            prod.Name = line.FunctionText.Trim('"'); 
                            prod.SalePrice = line.Amount / line.Quantity;
                            prod.ProductType = ProductType.FinishedGoods;//default

                            unitOfWork.ProductRepository.Add(prod);
                            //prod = new MainWindowViewModel().CreateProduct(plu, line.FunctionText.Trim('"'), line.Amount / line.Quantity);
                        }

                        SaleLineItem sli = new SaleLineItem();

                        sli.Quantity = line.Quantity;
                        sli.UnitMeasure = unitOfWork.UMRepository.GetById(1);// appvm.UnitMeasureManager.Unit;
                        sli.Amount = line.Amount;

                        //get product from PLU or create a new one

                        sli.Product = prod;

                        var invSVC = ServiceContainer.GetService<IInventoryService>();
                        //update total cost
                        newSale.TotalCost += sli.Cost = invSVC.GetProductCost(prod, sli.Quantity, sli.UnitMeasure);

                        newSale.LineItems.Add(sli);
                        newSale.SaleLineItems.Add(sli);
                    }
                    //DESCONTADO %
                    else if (line.FunctionType == 1000)
                    {
                        System.Diagnostics.Debug.Assert(line.FunctionText.Contains("DESCONTADO %"));

                        newSale.Discount = line.Amount;
                    }
                    //TOTAL
                    else if (line.FunctionNumber == 99)
                    {
                        System.Diagnostics.Debug.Assert(line.FunctionText.Contains("TOTAL"));

                        newSale.Total = line.Amount;
                        newSale.SubTotal = newSale.Total + newSale.Discount;
                    }
                }

                //DESCONTADO %
                //Report103Line discount_line = receiptLines[receiptLines.Count - 6];
                //if (discount_line.FunctionText.Contains("DESCONTADO %"))
                //{
                //    newSale.Discount = discount_line.Amount;
                //}

                //TOTAL
                //Report103Line total_line = receiptLines[receiptLines.Count - 5];
                //if (total_line.FunctionText.Contains("TOTAL"))
                //{                    
                //    newSale.Total = total_line.Amount;
                //    newSale.SubTotal = newSale.Total + newSale.Discount;
                //}

                //VOIDS
                newSale.Voids = low.VoidCount;
                newSale.VoidsAfterReceipt = low.CriticalVoidCount;

                //save changes
                //appvm.SaveChanges();
                unitOfWork.OrderRepository.Add(newSale);
                unitOfWork.SaveChanges();

                result.Add(newSale.Id);

                //remove line of work from active list
                activeLinesOfWork.Remove(low.TableNumber);
            }
            else if (firstLine.FunctionText.Contains("** RECIBO **"))
            {
                int tableNumber = receiptLines[1].Quantity;

                //Transfer Table
                if (receiptLines[2].FunctionText.Contains("Transfer Table"))
                {
                    int tableSource = receiptLines[1].Quantity;
                    int tableTarget = receiptLines[3].Quantity;

                    //GetTablesInTransfer(receiptLines, out tableSource, out tableTarget);

                    if (tableSource != tableTarget)
                    {
                        //change table number in line of work for future operations
                        LineOfWork low = activeLinesOfWork[tableSource];
                        low.TableNumber = tableTarget;

                        activeLinesOfWork.Remove(tableSource);

                        activeLinesOfWork[tableTarget] = low;
                    }
                }
                //Split Table
                else if (receiptLines[2].FunctionText.Contains("Split Table"))
                {
                    //3rd line bottom up show destination table
                    int tableDestination = receiptLines[receiptLines.Count - 3].Quantity;

                    //create a new LoW if it didn't exist
                    if (!activeLinesOfWork.ContainsKey(tableDestination))
                    {
                        LineOfWork temp = CreateLineOfWork(activeLinesOfWork, receiptDate, tableDestination);
                    }

                }
                //VOID
                else if (receiptLines[2].FunctionText.Contains("VOID"))
                {
                    //add a void operation to the line of work
                    LineOfWork low = activeLinesOfWork[tableNumber];

                    low.VoidCount++;
                    if (low.TicketPrinted) low.CriticalVoidCount++;
                }
                //Print Ticket                
                else if (receiptLines[receiptLines.Count - 1].FunctionText.Contains("TOTAL"))
                {
                    LineOfWork low = activeLinesOfWork[tableNumber];

                    if (!low.TicketPrinted)
                    {
                        //set the print date
                        low.PrintTicketDate = receiptDate;
                        low.TicketPrinted = true;
                    }
                }
                else if (!activeLinesOfWork.ContainsKey(tableNumber))
                {
                    LineOfWork temp = CreateLineOfWork(activeLinesOfWork, receiptDate, tableNumber);
                }
            }
            //ABRIR CAJON
            else
            {
            }
        }

        //int GetFacturaNumber(string line)
        //{
        //    return int.Parse(line.Substring(line.Length - 6, 5));
        //}

        //int GetTable(string line)
        //{
        //    return int.Parse(line.Substring(line.Length - 5, 4));
        //}

        private LineOfWork CreateLineOfWork(Dictionary<int,LineOfWork> activeLinesOfWork, DateTime startDate, int tableNumber)
        {
            LineOfWork low = new LineOfWork();

            low.TableNumber = tableNumber;

            low.StartDate = startDate;

            activeLinesOfWork[tableNumber] = low;

            return low;
        }

        //Product GetProductFromPLU(int plu)
        //{
        //    foreach (var item in appvm.ProductsOC)
        //    {
        //        if (item.Code == plu) return item;
        //    }
        //    return null;
        //}        

        //private bool IsFirstOperation(int tableNumber)
        //{
        //    foreach (var item in activeLinesOfWork)
        //    {
        //        if (item.TableNumber == tableNumber) return false;
        //    }

        //    return true;
        //}

        //bool ParseLineItem(string line, out int qtty, out string product_name, out decimal price)
        //{
        //    qtty = 0; product_name = ""; price = 0;

        //    string pattern = @"(\d+)(.*)\s(\d+\.\d+)";

        //    Regex rgx = new Regex(pattern);

        //    string last_piece = line.Split(';').Last();

        //    MatchCollection matches = rgx.Matches(last_piece);

        //    if (matches.Count == 0) return false;

        //    Match m = matches[0];

        //    qtty = int.Parse(m.Groups[1].Value);

        //    product_name = m.Groups[2].Value.Trim();

        //    price = decimal.Parse(m.Groups[3].Value);

        //    return true;
        //}

        //decimal GetFacturaTotal(List<string> lines) 
        //{
        //    foreach (var line in lines)
        //    {
        //        if (!line.Contains("TOTAL")) continue;
        //        //Total
        //        int qtty; string name; decimal price;
        //        ParseLineItem(line, out qtty, out name, out price);

        //        if (name == "TOTAL") return price;
        //    }

        //    return 0;
        //}

        //void GetTablesInTransfer(List<string> lines, out int tableSource, out int tableTarget)
        //{
        //    tableSource = GetTable(lines[1]);
        //    tableTarget = GetTable(lines[3]);
        //}

        //LineOfWork GetLineOfWorkFromTableNumber(int tableNumber)
        //{
        //    foreach (var item in activeLinesOfWork)
        //    {
        //        if (item.TableNumber == tableNumber)
        //        {
        //            return item;
        //        }
        //    }

        //    return null;
        //}

        //DateTime ParseDate(string line)
        //{
        //    //DATE
        //    string last_piece = line.Split(';').Last().Trim('"');

        //    string date_pattern = @"(.+)-(.+)-(.+)\s\d+\s+(\d+):(\d+)(A|P)";
        //    Regex date_rgx = new Regex(date_pattern);

        //    MatchCollection date_matches = date_rgx.Matches(last_piece);

        //    Match date_m = date_matches[0];

        //    int day = int.Parse(date_m.Groups[1].Value);
        //    int month = int.Parse(date_m.Groups[2].Value);
        //    int year = int.Parse(date_m.Groups[3].Value) + 2000;

        //    bool pm = date_m.Groups[6].Value == "P";

        //    int hour = int.Parse(date_m.Groups[4].Value);
        //    if (pm && hour != 12) hour += 12;
        //    if (!pm && hour == 12) hour = 0;

        //    int minutes = int.Parse(date_m.Groups[5].Value);

        //    return new DateTime(year, month, day, hour, minutes, 0);
        //}
    }
}
