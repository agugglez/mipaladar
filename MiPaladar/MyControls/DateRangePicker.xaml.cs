using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;
using System.Diagnostics;

namespace MiPaladar.MyControls
{
    /// <summary>
    /// Interaction logic for DateSelector.xaml
    /// </summary>
    public partial class DateRangePicker : UserControl, INotifyPropertyChanged
    {
        public DateRangePicker()
        {
            DataContext = this;

            InitializeComponent();
        }

        //string[] dateChoices = new string[] { "Hoy", "Semana", "Mes", "Específico" };
        //public string[] DateChoices
        //{
        //    get { return dateChoices; }
        //}

        DateChoice selectedDateChoice;
        public DateChoice SelectedDateChoice
        {
            get { return selectedDateChoice; }
            set
            {
                selectedDateChoice = value;

                switch (selectedDateChoice)
                {
                    case DateChoice.Today:
                        FromDate = DateTime.Today;
                        ToDate = DateTime.Today;
                        CanUserPickDates = false;
                        break;
                    case DateChoice.Week:
                        CanUserPickDates = false;
                        break;
                    case DateChoice.Month:
                        FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        ToDate = DateTime.Today;
                        CanUserPickDates = false;
                        break;
                    case DateChoice.Custom:
                        CanUserPickDates = true;
                        break;
                    default:
                        break;
                }
            }
        }

        bool canUserPickDates;
        public bool CanUserPickDates 
        {
            get { return canUserPickDates; }
            set 
            {
                canUserPickDates = value;
                OnPropertyChanged("CanUserPickDates");
            }
        }

        public static readonly DependencyProperty FromDateProperty =
            DependencyProperty.Register("FromDate", typeof(DateTime), typeof(DateRangePicker));

        //DateTime fromDate;
        public DateTime FromDate
        {
            get { return (DateTime)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }

        public static readonly DependencyProperty ToDateProperty =
            DependencyProperty.Register("ToDate", typeof(DateTime), typeof(DateRangePicker));

        //DateTime toDate;
        public DateTime ToDate 
        {
            get { return (DateTime)GetValue(ToDateProperty); }
            set 
            { 
                SetValue(ToDateProperty, value); 
            }
        }       
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }

    public enum DateChoice { Today, Week, Month, Custom}
}
