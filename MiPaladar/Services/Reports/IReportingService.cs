using MiPaladar.Classes;
using MiPaladar.ViewModels;
using MiPaladar.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPaladar.Services
{
    public interface IReportingService
    {
        List<ReportLineViewModel> GenerateReportData(IUnitOfWork unitOfWork, CustomizeReportOptions cro);

        bool IgnoreCategories { get; }
        bool IgnoreTags { get; }
        //ReportLineViewModel[] GenerateProjectionsReport(ReportType rType, CustomizeReportOptions cro);
        //List<ReportLineViewModel> GetGlobalSalesReport(CustomizeReportOptions cro);
        //List<ReportLineViewModel> GetSalesByItemReport(CustomizeReportOptions cro);
        //List<ReportLineViewModel> GetWIPByItemReport(CustomizeReportOptions cro);
        //List<ReportLineViewModel> GetCostByItemReport(CustomizeReportOptions cro);
        //List<ReportLineViewModel> GetConteoReport(CustomizeReportOptions cro);
        //List<ReportLineViewModel> GetSalesByCategoryReport(CustomizeReportOptions cro);
        //List<ReportLineViewModel> GetSalesBySalesPersonReport(CustomizeReportOptions cro);
        //List<ReportLineViewModel> GetDayOfWeekSalesReport(CustomizeReportOptions cro);
        //List<ReportLineViewModel> GetProductClassesReport(CustomizeReportOptions cro);
    }

    public interface IGlobalSalesRS : IReportingService
    {
    }
    public interface ISalesByItemRS : IReportingService
    {
    }
    public interface IConteoRS : IReportingService
    {
    }
    public interface ISalesByCategoryRS : IReportingService
    {
    }
    public interface ISalesPersonRS : IReportingService
    {
    }
    public interface IDayOfWeekSalesRS : IReportingService
    {
    }
    public interface IProductClassesRS : IReportingService
    {
    }
    public interface IWIPByItemRS : IReportingService
    {
    }
    public interface ICostByItemRS : IReportingService
    {
    }
    public interface ISalesProjectionsRS : IReportingService
    {
    }
    public interface IWIPProjectionsRS : IReportingService
    {
    }
    public interface ICostProjectionsRS : IReportingService
    {
    }
    public interface IServiceTimeRS : IReportingService
    {
    }
    public interface IDemandByHourRS : IReportingService
    {
    }

    //public class ReportingService : IReportingService
    //{
    //    //#region Global Sales

    //    //public ReportLineViewModel[] GenerateGlobalSalesData(CustomizeReportOptions cro)
    //    //{
    //    //    using (var context = new RestaurantDBEntities())
    //    //    {
    //    //        //get data
    //    //        var queryService = ServiceContainer.GetService<IQueryService>();

    //    //        List<ReportLineViewModel> lines_unfiltered = queryService.GetSalesData(context, cro.FromDate, cro.ToDate);

    //    //        //filter data
    //    //        List<ReportLineViewModel> lines_filtered;

    //    //        //don't filter
    //    //        if (cro.AllGood)
    //    //        {
    //    //            lines_filtered = lines_unfiltered;
    //    //        }
    //    //        else
    //    //        {
    //    //            lines_filtered = new List<ReportLineViewModel>();

    //    //            var filter = ServiceContainer.GetService<IReportLineFilterService>();

    //    //            foreach (var item in lines_unfiltered)
    //    //            {
    //    //                if (filter.CheckLine(item, ReportType.GlobalSales, cro)) 
    //    //                    lines_filtered.Add(item);
    //    //            }
    //    //        }

    //    //        //generate final report data
    //    //        List<ReportLineViewModel> lines_toshow = DoMathGS(lines_filtered);

    //    //        return lines_toshow.ToArray();
    //    //    }
    //    //}

    //    //private List<ReportLineViewModel> DoMathGS(List<ReportLineViewModel> lines_filtered)
    //    //{
    //    //    int daysCount = lines_filtered.Distinct(new SameDateComparer()).Count();
    //    //    int monthCount = lines_filtered.Distinct(new SameMonthComparer()).Count();

    //    //    List<ReportLineViewModel> lines_toshow = new List<ReportLineViewModel>();

    //    //    //group by month
    //    //    if (monthCount >= 3)
    //    //    {
    //    //        //months data
    //    //        var queryNestedGroups = from item in lines_filtered
    //    //                                group item by item.Date.Year into yearGroup
    //    //                                from monthGroup in
    //    //                                    (from item in yearGroup
    //    //                                     orderby item.Date.Year, item.Date.Month
    //    //                                     group item by item.Date.Month)
    //    //                                group monthGroup by yearGroup.Key;

    //    //        foreach (var yearGroup in queryNestedGroups)
    //    //        {
    //    //            foreach (var monthGroup in yearGroup)
    //    //            {
    //    //                ReportLineViewModel rLine = new ReportLineViewModel();
    //    //                rLine.DateSpanString = monthGroup.First().Date.ToString("MMM/yy");

    //    //                rLine.Clients = monthGroup.Sum(x => x.Clients);
    //    //                rLine.Amount = monthGroup.Sum(x => x.Amount);

    //    //                lines_toshow.Add(rLine);
    //    //            }
    //    //        }
    //    //    }
    //    //    //group by week
    //    //    else if (daysCount >= 21)
    //    //    {
    //    //        var query = from item in lines_filtered
    //    //                    group item by item.MondayDate;

    //    //        foreach (var weekGroup in query)
    //    //        {
    //    //            ReportLineViewModel rLine = new ReportLineViewModel();
    //    //            rLine.DateSpanString = weekGroup.First().WeekString;

    //    //            rLine.Clients = weekGroup.Sum(x => x.Clients);
    //    //            rLine.Amount = weekGroup.Sum(x => x.Amount);

    //    //            lines_toshow.Add(rLine);
    //    //        }
    //    //    }
    //    //    //group by date
    //    //    else
    //    //    {
    //    //        var query = from item in lines_filtered
    //    //                    group item by item.Date;

    //    //        foreach (var dateGroup in query)
    //    //        {
    //    //            ReportLineViewModel rLine = new ReportLineViewModel();
    //    //            rLine.DateSpanString = dateGroup.Key.ToString("d/MMM");

    //    //            rLine.Clients = dateGroup.Sum(x => x.Clients);
    //    //            rLine.Amount = dateGroup.Sum(x => x.Amount);

    //    //            lines_toshow.Add(rLine);
    //    //        }
    //    //    }

    //    //    return lines_toshow.ToList();
    //    //}

    //    //#endregion

    //    //#region Sales Projections

    //    //public ReportLineViewModel[] GenerateProjectionsReport(ReportType rType, CustomizeReportOptions cro)
    //    //{
    //    //    if (rType != ReportType.SalesProjectionsByItem && rType != ReportType.WIPProjectionsByItem &&
    //    //        rType != ReportType.CostProjectionsByItem) throw new ArgumentException("El reporte no es de proyección");

    //    //    using (var context = new RestaurantDBEntities())
    //    //    {
    //    //        //get data
                

    //    //        //filter data
    //    //        List<ReportLineViewModel> lines_filtered;

    //    //        //don't filter
    //    //        lines_filtered = new List<ReportLineViewModel>();

    //    //        var filter = ServiceContainer.GetService<IReportLineFilterService>();

    //    //        foreach (var item in lines_unfiltered)
    //    //        {
    //    //            if (filter.CheckLine(item, rType, cro)) lines_filtered.Add(item);
    //    //        }

    //    //        //generate final report data
    //    //        List<ReportLineViewModel> lines_toshow = DoMathSP(lines_filtered);

    //    //        return lines_toshow.ToArray();
    //    //    }
    //    //}

    //    //private List<ReportLineViewModel> DoMathSP(List<ReportLineViewModel> lines_filtered)
    //    //{
            
    //    //}

        

    //    //#endregion

    //    public List<ReportLineViewModel> GetGlobalSalesReport(CustomizeReportOptions cro)
    //    {
    //        GlobalSalesReport report = new GlobalSalesReport();
    //        return report.GenerateReportData(cro);
    //    }

    //    public List<ReportLineViewModel> GetSalesByItemReport(CustomizeReportOptions cro)
    //    {
    //        SalesByItemReport report = new SalesByItemReport();
    //        return report.GenerateReportData(cro);
    //    }

    //    public List<ReportLineViewModel> GetWIPByItemReport(CustomizeReportOptions cro)
    //    {
    //        WIPByItemReport report = new WIPByItemReport();
    //        return report.GenerateReportData(cro);
    //    }

    //    public List<ReportLineViewModel> GetCostByItemReport(CustomizeReportOptions cro)
    //    {
    //        CostByItemReport report = new CostByItemReport();
    //        return report.GenerateReportData(cro);
    //    }

    //    public List<ReportLineViewModel> GetConteoReport(CustomizeReportOptions cro)
    //    {
    //        ConteoReport report = new ConteoReport();
    //        return report.GenerateReportData(cro);
    //    }

    //    public List<ReportLineViewModel> GetSalesByCategoryReport(CustomizeReportOptions cro)
    //    {
    //        SalesByCategoryReport report = new SalesByCategoryReport();
    //        return report.GenerateReportData(cro);
    //    }

    //    public List<ReportLineViewModel> GetSalesBySalesPersonReport(CustomizeReportOptions cro)
    //    {
    //        SalesBySalesPersonReport report = new SalesBySalesPersonReport();
    //        return report.GenerateReportData(cro);
    //    }

    //    public List<ReportLineViewModel> GetDayOfWeekSalesReport(CustomizeReportOptions cro)
    //    {
    //        DayOfWeekSalesReport report = new DayOfWeekSalesReport();
    //        return report.GenerateReportData(cro);
    //    }

    //    public List<ReportLineViewModel> GetProductClassesReport(CustomizeReportOptions cro)
    //    {
    //        ProductClassesReport report = new ProductClassesReport();
    //        return report.GenerateReportData(cro);
    //    }
    //} 
}
