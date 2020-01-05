//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MiPaladar.Services
//{
//    public interface IReportGenerator
//    {
//        ReportModel GenerateSalesByItemReport(DateTime fromDate, DateTime toDate);
//    }

//    public class ReportModel
//    {
//        List<string> _headers = new List<string>();
//        public List<string> Headers { get { return _headers; } }

//        List<ReportRow> _rows = new List<ReportRow>();
//        public List<ReportRow> Rows { get { return _rows; } }

//        int textSearchIndex = -1;
//        public int TextSearchIndex
//        {
//            get { return textSearchIndex; }
//            set { textSearchIndex = value; }
//        }
//    }

//    public class ReportRow
//    {
//        List<ReportCell> _cells = new List<ReportCell>();
//        public List<ReportCell> Cells { get { return _cells; } }
//    }

//    public class ReportCell
//    {
//        public object Value { get; set; }
//        public string Format { get; set; }
//    }

//    public class ReportDefinitionGenerator : IReportGenerator
//    {
//        public ReportModel GenerateSalesByItemReport(DateTime fromDate, DateTime toDate) 
//        {
//            ReportModel rd = new ReportModel();

//            rd.Headers.Add("Producto");
//            rd.Headers.Add("Cantidad");
//            rd.Headers.Add("Importe");

//            rd.TextSearchIndex = 0;

//            IQueryService querySvc = new QueryService();

//            List<QueryLineItem> lineItemData = querySvc.GetQueryLineItemsData(fromDate, toDate, false);

//            var groupByProductQuery = from li in lineItemData
//                                      group li by li.Product;

//            foreach (var group in groupByProductQuery.OrderBy(x => x.Key.Name))
//            {
//                ReportRow reportRow = new ReportRow();

//                ReportCell productCell = new ReportCell() { Value = group.Key.Name };
//                reportRow.Cells.Add(productCell);

//                ReportCell qttyCell = new ReportCell() { Value = group.Sum(x => x.Quantity) };
//                reportRow.Cells.Add(qttyCell);

//                ReportCell amountCell = new ReportCell() { Value = group.Sum(x => x.Amount), Format = "{0:c}" };
//                reportRow.Cells.Add(amountCell);

//                rd.Rows.Add(reportRow);
//            }

//            return rd;
//        }
//    }
//}
