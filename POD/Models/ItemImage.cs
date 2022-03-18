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
        public ItemImage()
        {
            Name = "New Image";
        }
        public ItemImage(string path)
        {
            FullFilePath = path;
        }

        // Databound Properties
        #region Name
        private string _Name;
        [XmlSaveMode(XSME.Single)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region FilePath
        private string _FileName;
        [XmlSaveMode(XSME.Single)]
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
                NotifyPropertyChanged();
                UpdateFullFilePath();
            }
        }
        #endregion
        #region FullFilePath
        private string _FullFilePath;
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
        #region SelectImage
        public ICommand SelectImage => new RelayCommand(DoSelectImage);
        private void DoSelectImage(object param)
        {
            OpenFileDialog openWindow = new OpenFileDialog
            {
                Filter = Configuration.ImageFileFilter
            };
            if (openWindow.ShowDialog() == true)
            {
                string noteDirectory = Environment.CurrentDirectory + "/Data/";
                if (File.Exists(noteDirectory + openWindow.SafeFileName))
                {
                    YesNoDialog question = new YesNoDialog(openWindow.SafeFileName + " already exists in the data directory, overwrite?");
                    if (question.ShowDialog() == true)
                    {
                        if (question.Answer == false)
                        {
                            YesNoDialog linkQuestion = new YesNoDialog("Link existing file to this entry?");
                            if (linkQuestion.ShowDialog() == true)
                            {
                                if (linkQuestion.Answer == true)
                                {
                                    FileName = openWindow.SafeFileName;
                                }
                            }
                            return;
                        }
                    }
                    else { return; }
                }
                File.Copy(openWindow.FileName, noteDirectory + openWindow.SafeFileName, true);
                FileName = openWindow.SafeFileName;
            }
        }
        #endregion
        #region OpenImage
        public ICommand OpenImage => new RelayCommand(DoOpenImage);
        private void DoOpenImage(object param)
        {
            try
            {
                System.Diagnostics.Process.Start(FullFilePath);
            }
            catch (Exception e)
            {
                YesNoDialog question = new YesNoDialog(e.Message + "\nUnlink?");
                question.ShowDialog();
                if (question.Answer == true)
                {
                    FileName = "";
                }
            }
        }
        #endregion
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
            if (FileName == null || FileName == "") { FullFilePath = ""; return; }
            FullFilePath = Environment.CurrentDirectory + "/Data/" + FileName;
        }

    }

}
