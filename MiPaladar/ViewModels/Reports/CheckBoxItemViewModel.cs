using System;
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class CheckBoxItemViewModel :ViewModelBase
    {
        Action<int> onChecked;

        public CheckBoxItemViewModel(int id, string name, bool isChecked, Action<int> onChecked = null, int level = 0)
        {
            Id = id;
            Name = name;
            this.isChecked = isChecked;
            Level = level;
            this.onChecked = onChecked;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int Level { get; set; }

        bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {                    
                    isChecked = value;

                    if (onChecked != null) onChecked(Id);

                    OnPropertyChanged("IsChecked");
                }                
            }
        }
    }
}
