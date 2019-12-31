using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Text.RegularExpressions;

using MiPaladar.Classes;
using MiPaladar.ViewModels;

namespace MiPaladar.Services
{
    public interface IReport101Reader
    {
        Report101InfoViewModel Load101Report(string filepath, BackgroundWorker load101Worker);
    }
    public class Report101Reader : IReport101Reader
    {
        //lines of work keyed by tablenumber
        Dictionary<int, LineOfWork> activeLinesOfWork;
        List<LineOfWork> shiftLinesOfWork;

        int abrir_cajon_counter = 0;

        #region Build lines of work list

        public Report101InfoViewModel Load101Report(string filepath, BackgroundWorker load101Worker)
        {
            var fileReader = ServiceContainer.GetService<IFileReaderService>();
            List<string> lines = fileReader.ReadLines(filepath);

            //int total_facturas = HowManyInvoices(lines);
            //int loaded_facturas = 0;

            //nonuseful lines always there
            if (lines.Count <= 5) throw new Exception("El reporte está vacío.");

            //make sure first line says "Electronic Journal"
            string firstLine = lines[0];
            //string lineTitle = firstLine.Split(';')[5];

            if (!firstLine.Contains("\"Electronic Journal\"")) throw new Exception("Este no es el reporte 101.");

            activeLinesOfWork = new Dictionary<int, LineOfWork>();
            shiftLinesOfWork = new List<LineOfWork>();
            abrir_cajon_counter = 0;

            List<string> current_operation_lines = new List<string>();

            //skip 2 lines

            //first build the lines of work

            for (int i = 2; i < lines.Count; i++)
            {
                string currentLine = lines[i];

                if (currentLine.Contains("----------------------------------------"))
                {
                    ProcessOperation(current_operation_lines);

                    current_operation_lines.Clear();

                    continue;
                }
                else current_operation_lines.Add(currentLine);

                load101Worker.ReportProgress(i * 100 / lines.Count);
            }

            load101Worker.ReportProgress(0);

            //now process all the information
            Report101InfoViewModel result = new Report101InfoViewModel();
            result.FilePath = filepath;
            result.OpenDrawerCount = abrir_cajon_counter;

            int count = 0;

            foreach (var low in shiftLinesOfWork)
            {
                result.TotalSales += low.Total;
                result.TotalDiscount += low.Discount;
                result.TotalClients += low.Clients;
                result.LinesOfWork.Add(low);

                var span = low.FacturaDate - low.StartDate;

                bool recibo_found = false;

                for (int i = 0; i < low.OperationTrack.Count; i++)
                {
                    if (low.OperationTrack[i] == Report101Operation.Print)
                        recibo_found = true;

                    else if (low.OperationTrack[i] == Report101Operation.Void)
                    {
                        result.VoidCount++;
                        if (recibo_found)
                        {
                            result.CriticalVoidCount++;
                            if (string.IsNullOrEmpty(result.CriticalVoidFacturas))
                                result.CriticalVoidFacturas = low.FacturaNumber.ToString();
                            else result.CriticalVoidFacturas += ", " + low.FacturaNumber;
                        }
                         
                    }
                }

                load101Worker.ReportProgress(++count * 100 / lines.Count);
            }

            //service time data
            var groupQuery = from low in shiftLinesOfWork
                             let span = GetSpanKey(low)
                             where span >= 0
                             orderby span
                             group low by span;

            foreach (var group in groupQuery)
            {
                ServiceTimeTotal stt = new ServiceTimeTotal();

                int minutes = group.Key * 5;
                stt.ServiceTimeMinutes = minutes > 60 ? "> 60" : minutes.ToString();
                stt.Quantity = group.Count();

                result.ServiceTimeTotals.Add(stt);
            }

            BuildClientsByHourData(result);

            return result;
        }

        private void BuildClientsByHourData(Report101InfoViewModel result)
        {
            //query
            //group by flat hour
            var querySortedEnd = from low in shiftLinesOfWork
                                 where low.FacturaDate.Year > 1
                                 orderby low.FacturaDate
                                 group low by low.FacturaDate.AddMinutes(-low.FacturaDate.Minute);

            foreach (var group in querySortedEnd)
            {
                ClientsByHour cbh = new ClientsByHour();
                cbh.Hour = string.Format("{0:h:mm tt}", group.Key);
                cbh.Clients = group.Count();

                result.ClientsByHourTotals.Add(cbh);
            }
        }

        private static int GetSpanKey(LineOfWork low)
        {
            if (low.FacturaDate < low.StartDate) 
            { }
            //TimeSpan span = low.FacturaDate - low.StartDate;
            int inMinutes = low.ServiceTime;

            if (inMinutes > 60) inMinutes = 65;

            int result = inMinutes == 0 ? 1 : (inMinutes - 1) / 5 + 1;

            return result;
        }

        void ProcessOperation(List<string> lines)
        {
            string firstLine = lines[0];
            string date_line = lines[lines.Count - 2]; //date appears in the line before last 

            if (firstLine.Contains("*** FACTURA ***"))
            {
                int tableNumber = GetTable(lines[1]); //second line shows table

                LineOfWork low = activeLinesOfWork[tableNumber];

                int facturaNumber = GetFacturaNumber(firstLine);
                low.FacturaNumber = facturaNumber;

                low.OperationTrack.Add(Report101Operation.Factura);

                //Clients
                string thirdLine = lines[2];
                //sometimes this line doesn't appear
                if (thirdLine.Contains("CLIENTE #")) 
                {
                    low.Clients = int.Parse(thirdLine.Substring(thirdLine.Length - 5, 4));
                }               

                //Discount
                string discount_line = lines[lines.Count - 8];
                if (discount_line.Contains("DESCONTADO %"))
                {
                    string last_piece = discount_line.Split(';').Last();
                    low.Discount = decimal.Parse(last_piece.Substring(last_piece.Length - 7, 6));
                }

                //Total
                //int qtty; string name; decimal price;
                //ParseLineItem(lines[lines.Count - 7], out qtty, out name, out price);
                low.Total = GetFacturaTotal(lines);

                //and set the factura date
                low.FacturaDate = ParseDate(date_line);

                //remove line of work from active list (it will stay in shift list)
                activeLinesOfWork.Remove(low.TableNumber);
            }
            else if (firstLine.Contains("** RECIBO **"))
            {
                int tableNumber = GetTable(lines[1]); //second line shows table

                int qtty; string name; decimal price;

                //Transfer Table
                if (lines[2].Contains("Transfer Table"))
                {
                    int tableSource, tableTarget = 0;

                    GetTablesInTransfer(lines, out tableSource, out tableTarget);

                    if (tableSource != tableTarget)
                    {
                        //change table number in line of work for future operations
                        LineOfWork low = activeLinesOfWork[tableSource];
                        low.TableNumber = tableTarget;

                        if (!activeLinesOfWork.ContainsKey(tableTarget))
                        {
                            activeLinesOfWork[tableTarget] = low;
                        }
                        else
                        {
                            shiftLinesOfWork.Remove(low);
                        }

                        activeLinesOfWork.Remove(tableSource);                        
                    }
                }
                //Split Table
                else if (lines[2].Contains("Split Table"))
                {
                    //3rd line bottom up show destination table
                    int tableDestination = GetTable(lines[lines.Count - 3]);

                    //create a new LoW if it didn't exist
                    if (!activeLinesOfWork.ContainsKey(tableDestination))
                    {
                        LineOfWork temp = CreateLineOfWork(date_line, tableDestination);
                    }

                }
                //VOID
                else if (lines[2].Contains("VOID"))
                {
                    //add a void operation to the line of work
                    LineOfWork low = activeLinesOfWork[tableNumber];

                    low.OperationTrack.Add(Report101Operation.Void);

                }
                //Print Ticket                
                else if (ParseLineItem(lines[lines.Count - 3], out qtty, out name, out price) && name == "TOTAL")
                {
                    //add new print operation
                    LineOfWork low = activeLinesOfWork[tableNumber];

                    low.OperationTrack.Add(Report101Operation.Print);

                    //and set the print date
                    low.PrintTicketDate = ParseDate(date_line);
                }
                else if (!activeLinesOfWork.ContainsKey(tableNumber))
                {
                    if (tableNumber == 8)
                    { }
                    LineOfWork temp = CreateLineOfWork(date_line, tableNumber);
                }
            }
            //ABRIR CAJON
            else { abrir_cajon_counter++; }
        }

        int GetFacturaNumber(string line)
        {
            return int.Parse(line.Substring(line.Length - 6, 5));
        }

        int GetTable(string line)
        {
            return int.Parse(line.Substring(line.Length - 5, 4));
        }

        private LineOfWork CreateLineOfWork(string date_line, int tableNumber)
        {
            LineOfWork low = new LineOfWork();

            low.TableNumber = tableNumber;

            low.StartDate = ParseDate(date_line);

            activeLinesOfWork[tableNumber] = low;
            shiftLinesOfWork.Add(low);

            return low;
        }

        //private bool IsFirstOperation(int tableNumber)
        //{
        //    foreach (var item in activeLinesOfWork)
        //    {
        //        if (item.TableNumber == tableNumber) return false;
        //    }

        //    return true;
        //}

        bool ParseLineItem(string line, out int qtty, out string product_name, out decimal price)
        {
            qtty = 0; product_name = ""; price = 0;

            string pattern = @"(\d+)(.*)\s(\d+\.\d+)";

            Regex rgx = new Regex(pattern);

            string last_piece = line.Split(';').Last();

            MatchCollection matches = rgx.Matches(last_piece);

            if (matches.Count == 0) return false;

            Match m = matches[0];

            qtty = int.Parse(m.Groups[1].Value);

            product_name = m.Groups[2].Value.Trim();

            price = decimal.Parse(m.Groups[3].Value);

            return true;
        }

        decimal GetFacturaTotal(List<string> lines) 
        {
            foreach (var line in lines)
            {
                if (!line.Contains("TOTAL")) continue;
                //Total
                int qtty; string name; decimal price;
                ParseLineItem(line, out qtty, out name, out price);

                if (name == "TOTAL") return price;
            }

            return 0;
        }

        void GetTablesInTransfer(List<string> lines, out int tableSource, out int tableTarget)
        {
            tableSource = GetTable(lines[1]);
            tableTarget = GetTable(lines[3]);
        }

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

        DateTime ParseDate(string line)
        {
            //DATE
            string last_piece = line.Split(';').Last().Trim('"');

            string date_pattern = @"(.+)-(.+)-(.+)\s\d+\s+(\d+):(\d+)(A|P)";
            Regex date_rgx = new Regex(date_pattern);

            MatchCollection date_matches = date_rgx.Matches(last_piece);

            Match date_m = date_matches[0];

            int day = int.Parse(date_m.Groups[1].Value);
            int month = int.Parse(date_m.Groups[2].Value);
            int year = int.Parse(date_m.Groups[3].Value) + 2000;

            bool pm = date_m.Groups[6].Value == "P";

            int hour = int.Parse(date_m.Groups[4].Value);
            if (pm && hour != 12) hour += 12;
            if (!pm && hour == 12) hour = 0;

            int minutes = int.Parse(date_m.Groups[5].Value);

            return new DateTime(year, month, day, hour, minutes, 0);
        }

        #endregion
    }
}
