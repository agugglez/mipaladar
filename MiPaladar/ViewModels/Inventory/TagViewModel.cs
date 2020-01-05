using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class TagViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        bool creating;

        //for filtering in reports
        public TagViewModel(int id, string name)
        {
            this.tagId = id;
            this.name = name;
        }

        //creating a new one
        public TagViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            creating = true;

            HasPendingChanges = true;
        }

        public TagViewModel(MainWindowViewModel appvm, Tag tag) 
        {
            this.appvm = appvm;

            this.tagId = tag.Id;
            this.name = tag.Name;
        }

        public override string DisplayName
        {
            get
            {
                return creating ? "Nueva Etiqueta" : "Etiquetas: " + name;
            }
        }

        //the real category
        int tagId;
        public int TagId { get { return tagId; } }

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
                HasPendingChanges = true;
            }
        }

        bool hasPendingChanges;
        public bool HasPendingChanges
        {
            get { return hasPendingChanges; }
            set
            {
                hasPendingChanges = value;
                OnPropertyChanged("HasPendingChanges");
            }
        }

        #region Save Command

        RelayCommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(x => Save(), x => this.CanSave);
                return saveCommand;
            }
        }

        //bool hasErrors;

        bool CanSave
        {
            get { return !string.IsNullOrWhiteSpace(name); }
        }

        void Save()
        {
            if (hasPendingChanges)
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    Tag tag;
                    if (creating)
                    {
                        tag = new Tag();

                        unitOfWork.TagRepository.Add(tag);
                    }
                    else
                    {
                        tag = unitOfWork.TagRepository.GetById(tagId);
                    }

                    if (tag.Name != name) tag.Name = name;

                    unitOfWork.SaveChanges();

                    HasPendingChanges = false;

                    if (creating)
                    {
                        tagId = tag.Id;
                        appvm.GlobalEventsManager.FireTagCreated(tag.Id);
                    }
                    else appvm.GlobalEventsManager.FireTagModified(tagId);
                }                
            }

            //var windowManager = base.GetService<IWindowManager>();
            //windowManager.Close(this);
        }

        #endregion

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                    removeCommand = new RelayCommand(x => this.Remove());
                return removeCommand;
            }
        }

        bool CanRemove { get { return !creating; } }

        void Remove()
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = string.Format("¿Está seguro que desea eliminar la etiqueta '{0}'?", Name);

            if (msgBox.ShowYesNoDialog(message) == true)
            {
                var unitOfWork = base.GetNewUnitOfWork();
                unitOfWork.TagRepository.Remove(tagId);
                //appvm.TagsOC.Remove(tag);

                //appvm.Context.Tags.DeleteObject(tag);

                unitOfWork.SaveChanges();

                //let everybody know
                appvm.GlobalEventsManager.FireTagRemoved(tagId);

                //close window
                var windowManager = base.GetService<IWindowManager>();
                windowManager.Close(this);
            }
        }

        #endregion 

    }
}