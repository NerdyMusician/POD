using Microsoft.Win32;
using POD.Toolbox;
using POD.Windows;
using System;
using System.IO;
using System.Windows.Input;

namespace POD.Models
{
    public class ItemImage : BaseModel
    {
        // Constructors
        public ItemImage(string path)
        {
            FullFilePath = path;
        }

        // Databound Properties
        #region FullFilePath
        private string _FullFilePath;
        [XmlSaveMode(XSME.Single)]
        public string FullFilePath
        {
            get
            {
                return _FullFilePath;
            }
            set
            {
                _FullFilePath = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        // Commands
        #region RemoveImage
        public ICommand RemoveImage => new RelayCommand(DoRemoveImage);
        private void DoRemoveImage(object param)
        {
            Configuration.MainModelRef.ActiveCard.ItemImages.Remove(this);
        }
        #endregion

        // Private Methods

    }

}
