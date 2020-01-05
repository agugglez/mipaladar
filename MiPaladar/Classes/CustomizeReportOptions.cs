using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;
using MiPaladar.ViewModels;
using MiPaladar.Repository;

namespace MiPaladar.Classes
{
    public class CustomizeReportOptions
    {
        public CustomizeReportOptions(IUnitOfWork unitOfWork)
        {
            AllGood = true;

            ShiftsAllGood = true;
            SalesPersonsAllGood = true;
            CategoriesAllGood = true;
            TagsAllGood = true;

            AdmitsNoShift = true;
            AdmitsNoSalesPerson = true;
            AdmitsNoCategory = true;
            AdmitsNoTag = true;

            foreach (var item in unitOfWork.ShiftRepository.Get())
            {
                slctdShifts.Add(item);
            }
            foreach (var item in unitOfWork.EmployeeRepository.Get())
            {
                slctdSPs.Add(item);
            }
            foreach (var item in unitOfWork.CategoryRepository.Get())
            {
                slctdCats.Add(item);
            }
            foreach (var item in unitOfWork.TagRepository.Get())
            {
                slctdTags.Add(item);
            }

            ProjectionPercent = 5;            
        }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        //Nothing to filter
        public bool AllGood { get; set; }        

        //SHIFTS
        List<Shift> slctdShifts = new List<Shift>();
        public List<Shift> SelectedShifts
        {
            get { return slctdShifts; }
        }

        public bool AdmitsNoShift { get; set; }
        public bool ShiftsAllGood { get; set; }

        //SALESPERSONS
        List<Employee> slctdSPs = new List<Employee>();
        public List<Employee> SelectedSalesPersons
        {
            get { return slctdSPs; }
        }
        public bool AdmitsNoSalesPerson { get; set; }
        public bool SalesPersonsAllGood { get; set; }

        //CATEGORIES
        List<Category> slctdCats = new List<Category>();
        public List<Category> SelectedCategories
        {
            get { return slctdCats; }
        }

        public bool AdmitsNoCategory { get; set; }
        public bool CategoriesAllGood { get; set; }

        //TAGS
        List<Tag> slctdTags = new List<Tag>();
        public List<Tag> SelectedTags
        {
            get { return slctdTags; }
        }
        public bool AdmitsNoTag { get; set; }
        public bool TagsAllGood { get; set; }

        public double ProjectionPercent { get; set; }
    }
}
