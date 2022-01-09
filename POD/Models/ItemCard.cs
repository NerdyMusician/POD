using POD.Toolbox;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace POD.Models
{
    public class ItemCard : BaseModel
    {
        // Constructors
        public ItemCard()
        {
            Name = "New Item";
            CardType = "Other";
            ItemImages = new();
            PurchaseDate = "2021.01.01";
            PurchasePrice = "0";
            CurrentPrice = "0";

        }

        // Databound Properties - Primary Values
        #region Name
        private string _Name;
        [XmlSaveMode("Single")]
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
        #region CardType
        private string _CardType;
        [XmlSaveMode("Single")]
        public string CardType
        {
            get
            {
                return _CardType;
            }
            set
            {
                _CardType = value;
                NotifyPropertyChanged();
                UpdateFormDisplays();
            }
        }
        #endregion
        #region Description
        private string _Description;
        [XmlSaveMode("Single")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                NotifyPropertyChanged();
                UpdateFormDisplays();
            }
        }
        #endregion
        #region ItemImages
        private ObservableCollection<ItemImage> _ItemImages;
        [XmlSaveMode("Enumerable")]
        public ObservableCollection<ItemImage> ItemImages
        {
            get
            {
                return _ItemImages;
            }
            set
            {
                _ItemImages = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region ActiveImage
        private ItemImage _ActiveImage;
        public ItemImage ActiveImage
        {
            get
            {
                return _ActiveImage;
            }
            set
            {
                _ActiveImage = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        // Databound Properties - Form Fields
        #region Series
        private string _Series;
        [XmlSaveMode("Single")]
        public string Series
        {
            get
            {
                return _Series;
            }
            set
            {
                _Series = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region PurchaseLocation
        private string _PurchaseLocation;
        [XmlSaveMode("Single")]
        public string PurchaseLocation
        {
            get
            {
                return _PurchaseLocation;
            }
            set
            {
                _PurchaseLocation = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region PurchaseDate
        private string _PurchaseDate;
        [XmlSaveMode("Single")]
        public string PurchaseDate
        {
            get
            {
                return _PurchaseDate;
            }
            set
            {
                _PurchaseDate = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region PurchasePrice
        private string _PurchasePrice;
        [XmlSaveMode("Single")]
        public string PurchasePrice
        {
            get
            {
                return _PurchasePrice;
            }
            set
            {
                _PurchasePrice = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region CurrentPrice
        private string _CurrentPrice;
        [XmlSaveMode("Single")]
        public string CurrentPrice
        {
            get
            {
                return _CurrentPrice;
            }
            set
            {
                _CurrentPrice = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region Hyperlink
        private string _Hyperlink;
        [XmlSaveMode("Single")]
        public string Hyperlink
        {
            get
            {
                return _Hyperlink;
            }
            set
            {
                _Hyperlink = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Brand
        private string _Brand;
        [XmlSaveMode("Single")]
        public string Brand
        {
            get
            {
                return _Brand;
            }
            set
            {
                _Brand = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Author
        private string _Author;
        [XmlSaveMode("Single")]
        public string Author
        {
            get
            {
                return _Author;
            }
            set
            {
                _Author = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region Publisher
        private string _Publisher;
        [XmlSaveMode("Single")]
        public string Publisher
        {
            get
            {
                return _Publisher;
            }
            set
            {
                _Publisher = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region ISBN
        private string _ISBN;
        [XmlSaveMode("Single")]
        public string ISBN
        {
            get
            {
                return _ISBN;
            }
            set
            {
                _ISBN = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region SerialNumber
        private string _SerialNumber;
        [XmlSaveMode("Single")]
        public string SerialNumber
        {
            get
            {
                return _SerialNumber;
            }
            set
            {
                _SerialNumber = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        // Databound Properties - Form Field Display Toggles
        #region Display_Brand
        private bool _Display_Brand;
        public bool Display_Brand
        {
            get
            {
                return _Display_Brand;
            }
            set
            {
                _Display_Brand = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region Display_Series
        private bool _Display_Series;
        public bool Display_Series
        {
            get
            {
                return _Display_Series;
            }
            set
            {
                _Display_Series = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Display_Author
        private bool _Display_Author;
        public bool Display_Author
        {
            get
            {
                return _Display_Author;
            }
            set
            {
                _Display_Author = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region Display_Publisher
        private bool _Display_Publisher;
        public bool Display_Publisher
        {
            get
            {
                return _Display_Publisher;
            }
            set
            {
                _Display_Publisher = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region Display_ISBN
        private bool _Display_ISBN;
        public bool Display_ISBN
        {
            get
            {
                return _Display_ISBN;
            }
            set
            {
                _Display_ISBN = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Display_SerialNumber
        private bool _Display_SerialNumber;
        [XmlSaveMode("Single")]
        public bool Display_SerialNumber
        {
            get
            {
                return _Display_SerialNumber;
            }
            set
            {
                _Display_SerialNumber = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        // Commands
        #region AddImage
        public ICommand AddImage => new RelayCommand(DoAddImage);
        private void DoAddImage(object param)
        {
            ItemImages.Add(new ItemImage());
            ActiveImage = ItemImages.Last();
        }
        #endregion
        #region SortImages
        public ICommand SortImages => new RelayCommand(DoSortImages);
        private void DoSortImages(object param)
        {
            ItemImages = new(ItemImages.OrderBy(i => i.Name));
        }
        #endregion
        #region RemoveCard
        public ICommand RemoveCard => new RelayCommand(DoRemoveCard);
        private void DoRemoveCard(object param)
        {
            Configuration.MainModelRef.AllCards.Remove(this);
        }
        #endregion
        #region OpenLink
        public ICommand OpenLink => new RelayCommand(DoOpenLink);
        private void DoOpenLink(object param)
        {
            try
            {
                System.Diagnostics.Process.Start(Hyperlink);
            }
            catch (Exception e)
            {
                HelperMethods.WriteToLogFile(e.Message, true);
            }
        }
        #endregion

        // Private Methods
        private void UpdateFormDisplays()
        {
            // Set all to false as default
            Display_Brand = false;
            Display_Series = false;
            Display_Author = false;
            Display_Publisher = false;
            Display_ISBN = false;
            Display_SerialNumber = false;

            // Set to true based on type
            switch (CardType)
            {
                case "Book":
                    Display_Author = true;
                    Display_Publisher = true;
                    Display_ISBN = true;
                    break;
                case "Figurine":
                    Display_Brand = true;
                    Display_Series = true;
                    break;
                case "Firearm":
                    Display_Brand = true;
                    Display_SerialNumber = true;
                    break;
                default:
                    return;
            }

        }


    }
}
