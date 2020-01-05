using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Services;
using MiPaladar.Entities;
using MiPaladar.Enums;
using MiPaladar.Classes;
using MiPaladar.Extensions;
using MiPaladar.MVVM;

using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Collections;
using System.Windows;

namespace MiPaladar.ViewModels
{
    public class SalePreViewModel : ViewModelBase
    {
        public SalePreViewModel(Sale sale)
        {
            this.saleId = sale.Id;

            CopyFromSale(sale);

            //subTotal = sale.LineItems.Sum(x => ((SaleLineItem)x).Amount);
        }

        int saleId;
        public int SaleId { get { return saleId; } }
        //Sale sale;
        //public Sale WrappedSale
        //{
        //    get { return sale; }
        //}

        #region Copy Methods

        void CopyFromSale(Sale sale)
        {
            if (workingDate != sale.Date) WorkDate = sale.Date;
            if (dateCreated != sale.DateCreated) DateCreated = sale.DateCreated;
            if (waiter != sale.Employee) Waiter = sale.Employee;
            if (shift != sale.Shift) Shift = sale.Shift;

            //if (dateClosed != sale.DateClosed) DateClosed = sale.DateClosed;
            //if (datePrinted != sale.DatePrinted) DatePrinted = sale.DatePrinted;

            if (discount != sale.Discount) Discount = sale.Discount;
            if (taxInPercent != sale.TaxInPercent) TaxInPercent = sale.TaxInPercent;

            //if (cash != sale.Cash) Cash = sale.Cash;
            if (clients != sale.Persons) Clients = sale.Persons;
            //if (paid != sale.Paid) Paid = sale.Paid;
            if (number != sale.Number) Number = sale.Number;

            if (total != sale.Total) Total = sale.Total;

            //if (closed != sale.Closed) Closed = sale.Closed;
            //if (tips != sale.Tips) Tips = sale.Tips;
            //if (table != sale.Table) Table = sale.Table;
        }

        #endregion

        #region Properties

        DateTime workingDate;
        public DateTime WorkDate
        {
            get { return workingDate; }
            set
            {
                workingDate = value;
                OnPropertyChanged("WorkDate");
            }
        }

        DateTime dateCreated;
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set
            {
                dateCreated = value;
                OnPropertyChanged("DateCreated");
            }
        }

        //DateTime? datePrinted;
        //public DateTime? DatePrinted
        //{
        //    get { return datePrinted; }
        //    set 
        //    {
        //        datePrinted = value;
        //        OnPropertyChanged("DatePrinted");
        //    }
        //}

        //DateTime? dateClosed;
        //public DateTime? DateClosed
        //{
        //    get { return dateClosed; }
        //    set
        //    {
        //        dateClosed = value;
        //        OnPropertyChanged("DateClosed");
        //    }
        //}

        decimal total;
        public decimal Total
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }

        int clients;
        public int Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged("Clients");
            }
        }

        decimal discount;
        public decimal Discount
        {
            get { return discount; }
            set
            {
                discount = value;

                OnPropertyChanged("Discount");
            }
        }

        bool taxInPercent;
        public bool TaxInPercent
        {
            get { return taxInPercent; }
            set
            {
                taxInPercent = value;
                OnPropertyChanged("TaxInPercent");
            }
        }

        Employee waiter;
        public Employee Waiter
        {
            get { return waiter; }
            set
            {
                waiter = value;
                OnPropertyChanged("Waiter");
            }
        }

        Shift shift;
        public Shift Shift
        {
            get { return shift; }
            set
            {
                shift = value;
                OnPropertyChanged("Shift");
            }
        }

        int? number;
        public int? Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        //Table table;
        //public Table Table
        //{
        //    get { return table; }
        //    set
        //    {
        //        table = value;
        //        OnPropertyChanged("Table");
        //    }
        //}

        #endregion

    }
}