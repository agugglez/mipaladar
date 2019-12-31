using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public class ListsTabViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public ListsTabViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
        }

        public override string DisplayName
        {
            get { return "LISTAS"; }
        }

        protected override void OnDispose()
        {
            if (lists != null)
                foreach (var item in lists)
                {
                    item.Dispose();
                }
        }

        ObservableCollection<ViewModelBase> lists;
        public ObservableCollection<ViewModelBase> Lists
        {
            get 
            {
                if (lists == null)
                {
                    lists = new ObservableCollection<ViewModelBase>();
                    lists.Add(new CategoriesListViewModel(appvm));
                    lists.Add(new InventoryAreasListViewModel(appvm));
                    lists.Add(new ProductionAreasListViewModel(appvm));
                    lists.Add(new SaleAreasListViewModel(appvm));
                    lists.Add(new TablesListViewModel(appvm));

                    SelectedList = lists[0];
                }
                return lists; 
            }
        }

        ViewModelBase selectedList;
        public ViewModelBase SelectedList
        {
            get { return selectedList; }
            set
            {
                selectedList = value;
                OnPropertyChanged("SelectedList");
            }
        }
    }
}
