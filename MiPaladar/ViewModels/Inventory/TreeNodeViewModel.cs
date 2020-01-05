using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.MVVM;

using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public enum NodeType { Category, Product, ProductType}

    public class TreeNodeViewModel : ViewModelBase
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public TreeNodeViewModel Parent { get; set; }

        public NodeType Type { get; set; }

        //level in tree (height)
        public int Level { get; set; }

        bool isNodeExpanded;
        public bool IsNodeExpanded
        {
            get { return isNodeExpanded; }
            set
            {
                if (isNodeExpanded != value)
                {
                    isNodeExpanded = value;
                    OnPropertyChanged("IsNodeExpanded");
                }                
            }
        }

        ObservableCollection<TreeNodeViewModel> children = new ObservableCollection<TreeNodeViewModel>();
        public ObservableCollection<TreeNodeViewModel> Children
        {
            get { return children; }
        }                
    }
}
