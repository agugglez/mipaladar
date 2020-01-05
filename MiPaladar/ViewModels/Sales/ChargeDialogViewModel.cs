using System;
using System.Linq;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.MVVM;

using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;

namespace MiPaladar.ViewModels
{
    public class ChargeDialogViewModel : ViewModelBase
    {
        //MainWindowViewModel appvm;

        public ChargeDialogViewModel(decimal subtotal, decimal discount, decimal tax, decimal total, decimal cash, decimal tips)
        {
            //this.appvm = appvm;

            RawTotal = subtotal;
            AjusteToMoney = discount;
            TaxToMoney = tax;
            TotalPrice = total;
            this.cash = cash;
            this.change = cash - total;
            this.tips = tips;
        }

        #region ChargeCommand

        RelayCommand chargeCommand;
        public ICommand ChargeCommand
        {
            get
            {
                if (chargeCommand == null)
                {
                    chargeCommand = new RelayCommand(x => this.Charge(), x => CanCharge);
                }
                return chargeCommand;
            }
        }

        bool CanCharge { get { return Cash >= TotalPrice; } }

        private void Charge() { }

        #endregion

        public decimal RawTotal { get; set; }

        public decimal AjusteToMoney { get; set; }

        public decimal TaxToMoney { get; set; }

        public decimal TotalPrice { get; set; }

        decimal cash;
        public decimal Cash
        {
            get { return cash; }
            set
            {
                cash = value;

                Change = cash - TotalPrice;

                CommandManager.InvalidateRequerySuggested();
            }
        }

        decimal change;
        public decimal Change
        {
            get { return change; }
            set
            {
                change = value;

                OnPropertyChanged("Change");
            }
        }

        decimal tips;
        public decimal Tips
        {
            get { return tips; }
            set
            {
                tips = value;                

                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}
