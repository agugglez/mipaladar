using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.MVVM;
using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public abstract class ReportWithOpenLine : ReportsWindowViewModel
    {
        public ReportWithOpenLine(MainWindowViewModel appvm)
            : base(appvm)
        {
        }
        #region Double click Command

        public ReportLineViewModel SelectedLine { get; set; }

        RelayCommand openLineCommand;
        public RelayCommand OpenLineCommand
        {
            get
            {
                if (openLineCommand == null)
                    openLineCommand = new RelayCommand(x => OpenLine(SelectedLine), x => CanOpen);
                return openLineCommand;
            }
        }

        bool CanOpen { get { return SelectedLine != null; } }

        protected abstract void OpenLine(ReportLineViewModel sltdLine);

        protected void RemoveLine(int lineId)
        {
            var toRemove = ItemsShowing.Single(x => x.LineId == lineId);
            ItemsShowing.Remove(toRemove);

            UpdateTotals();
            UpdateGraphDataAsync();
        }

        #endregion
    }
}
