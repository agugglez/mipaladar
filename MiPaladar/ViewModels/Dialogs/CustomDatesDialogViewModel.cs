using System;

using MiPaladar.MVVM;
using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class CustomDatesDialogViewModel : ViewModelBase
    {

        public CustomDatesDialogViewModel()
        {
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
        }
        public CustomDatesDialogViewModel(DateTime fromDate, DateTime toDate) 
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        #region OK Command

        RelayCommand okCommand;
        public RelayCommand OKCommand
        {
            get
            {
                if (okCommand == null)
                    okCommand = new RelayCommand(x => { }, x => this.CanOk);

                return okCommand;
            }
        }

        bool CanOk
        {
            get { return ToDate >= FromDate; }
        }

        #endregion 
        
    }
}