using POD.Toolbox;
using System;
using System.Windows.Input;

namespace POD.Models
{
    public class ItemImage : BaseModel
    {
        // Constructors
        public ItemImage()
        {

        }
        public ItemImage(string name)
        {
            FileName = name;
        }

        // Databound Properties
        private string _FileName;
        [XmlSaveMode(XSME.Single)]
        public string FileName
        {
            get => _FileName;
            set
            {
                _FileName = value;
                NotifyPropertyChanged();
                if (!string.IsNullOrEmpty(value))
                {
                    UpdateFullFilePath();
                }
            }
        }

        private string _FullFilePath;
        public string FullFilePath
        {
            get => _FullFilePath;
            set => SetAndNotify(ref _FullFilePath, value);
        }

        // Commands
        #region RemoveImage
        public ICommand RemoveImage => new RelayCommand(DoRemoveImage);
        private void DoRemoveImage(object param)
        {
            Configuration.MainModelRef.ActiveCard.ItemImages.Remove(this);
        }
        #endregion

        // Private Methods
        private void UpdateFullFilePath()
        {
            FullFilePath = Environment.CurrentDirectory + "\\Data\\Images\\" + FileName;
        }

    }

}
