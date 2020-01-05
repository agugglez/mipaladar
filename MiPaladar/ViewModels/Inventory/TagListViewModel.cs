using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class TagListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public TagListViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
        }

        public override string DisplayName
        {
            get { return "ETIQUETAS"; }
        }

        Tag selectedTag;
        public Tag SelectedTag
        {
            get { return selectedTag; }
            set
            {
                selectedTag = value;
                OnPropertyChanged("SelectedTag");
            }
        }

        ObservableCollection<Tag> alltags;
        public ObservableCollection<Tag> AllTags
        {
            get
            {
                if (alltags == null)
                {
                    SetUpEvents();
                    alltags = new ObservableCollection<Tag>(base.GetNewUnitOfWork().TagRepository.Get());
                }
                return alltags ;
            }
        }

        void SetUpEvents()
        {
            appvm.GlobalEventsManager.TagCreated += GlobalEventsManager_TagCreated;
            appvm.GlobalEventsManager.TagModified += GlobalEventsManager_TagModified;
            appvm.GlobalEventsManager.TagRemoved += GlobalEventsManager_TagRemoved;
        }

        void GlobalEventsManager_TagCreated(object sender, TagInfoEventArgs e)
        {
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                alltags.Add(unitOfWork.TagRepository.GetById(e.TagId));
            }
        }

        void GlobalEventsManager_TagModified(object sender, TagInfoEventArgs e)
        {
            var targetTag = alltags.Single(x => x.Id == e.TagId);

            alltags.Remove(targetTag);

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                alltags.Add(unitOfWork.TagRepository.GetById(e.TagId));
            }
        }

        void GlobalEventsManager_TagRemoved(object sender, TagInfoEventArgs e)
        {
            var targetTag = alltags.Single(x => x.Id == e.TagId);
            alltags.Remove(targetTag);
        }

        #region New Tag Command

        RelayCommand newTagCommand;
        public ICommand NewTagCommand
        {
            get
            {
                if (newTagCommand == null)
                    newTagCommand = new RelayCommand(x => NewTag());
                return newTagCommand;
            }
        }

        void NewTag()
        {
            var windowManager = base.GetService<IWindowManager>();

            TagViewModel cvm = new TagViewModel(appvm);

            windowManager.ShowDialog(cvm, appvm);
        }
        #endregion        

        #region Expand Command

        //public ProductRowViewModel SelectedItem { get; set; }

        RelayCommand expandCommand;
        public ICommand ExpandCommand
        {
            get
            {
                if (expandCommand == null)
                    expandCommand = new RelayCommand(x => ExpandTag(selectedTag), x => this.CanExpand);
                return expandCommand;
            }
        }

        bool CanExpand { get { return selectedTag != null; } }

        void ExpandTag(Tag original)
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase vmb) =>
            {
                if (!(vmb is TagViewModel)) return false;

                TagViewModel vm = (TagViewModel)vmb;

                return vm.TagId == original.Id;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                TagViewModel cvm = new TagViewModel(appvm, selectedTag);

                windowManager.ShowDialog(cvm, appvm);                
            }
        }

        #endregion        

        //void category_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "Name") 
        //    {
        //        appvm.SaveChanges();
        //    }
        //}

    }
}