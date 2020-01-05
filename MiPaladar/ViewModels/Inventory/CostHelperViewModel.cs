using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.MVVM;
using MiPaladar.Entities;

namespace MiPaladar.ViewModels
{
    public class CostHelperViewModel : ViewModelBase
    {
        public CostHelperViewModel(int umId, decimal price)
        {
            this.umId = umId;
            this.resultUMid = umId;
            this.price = price;

            UpdateCostResult();
        }

        double qtty = 1;
        public double Quantity
        {
            get { return qtty; }
            set
            {
                qtty = value;
                UpdateCostResult();
            }
        }

        int umId;
        public int UMId
        {
            get { return umId; }
            set
            {
                umId = value;
                UpdateCostResult();
            }
        }

        decimal price;
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                UpdateCostResult();
            }
        }

        int resultUMid;
        public int ResultUMId
        {
            get { return resultUMid; }
            set
            {
                resultUMid = value;
                UpdateCostResult();
            }
        }

        decimal costResult;
        public decimal CostResult
        {
            get { return costResult; }
            set
            {
                costResult = value;
                OnPropertyChanged("CostResult");
            }
        }

        void UpdateCostResult()
        {
            UnitMeasure qttyUM = AllUMs.Single(x => x.Id == umId);
            UnitMeasure resultUM = AllUMs.Single(x => x.Id == resultUMid);

            CostResult = price / (decimal)qtty / (decimal)(qttyUM.ToBaseConversion / resultUM.ToBaseConversion);
        }

        public List<UnitMeasure> AllUMs
        {
            get
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    UnitMeasure umObject = unitOfWork.UMRepository.GetById(umId);
                    return umObject.UMFamily.UnitMeasures.ToList();
                }
            }
        }
    }
}
