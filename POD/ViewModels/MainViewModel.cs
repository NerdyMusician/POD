using POD.Models;
using POD.Toolbox;
using POD.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;

namespace POD.ViewModels
{
    public class MainViewModel : BaseModel
    {
        // Constructors
        public MainViewModel()
        {
            Configuration.MainModelRef = this;
            ApplicationVersion = "POD 1.00.00 beta";
            XmlMethods.XmlToList(Configuration.ItemDataFilePath, out List<ItemCard> items);
            AllCards = new ObservableCollection<ItemCard>(items);
            AllCards.CollectionChanged += AllCards_CollectionChanged;
            CardTypes = new()
            {
                "Book",
                "Electronic",
                "Figurine",
                "Firearm",
                "Furniture",
                "Jewelry",
                "Media",
                "Other",
                "Tool",
            };
            SetSearchFilters();
            ItemSearchText = "";
        }

        

        // Databound Properties
        #region ApplicationVersion
        private string _ApplicationVersion;
        public string ApplicationVersion
        {
            get
            {
                return _ApplicationVersion;
            }
            set
            {
                _ApplicationVersion = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region AllCards
        private ObservableCollection<ItemCard> _AllCards;
        public ObservableCollection<ItemCard> AllCards
        {
            get
            {
                return _AllCards;
            }
            set
            {
                _AllCards = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region FilteredCards
        private ObservableCollection<ItemCard> _FilteredCards;
        public ObservableCollection<ItemCard> FilteredCards
        {
            get => _FilteredCards;
            set => SetAndNotify(ref _FilteredCards, value);
        }
        #endregion
        #region ActiveCard
        private ItemCard _ActiveCard;
        public ItemCard ActiveCard
        {
            get
            {
                return _ActiveCard;
            }
            set
            {
                _ActiveCard = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region CardTypes
        private List<string> _CardTypes;
        public List<string> CardTypes
        {
            get
            {
                return _CardTypes;
            }
            set
            {
                _CardTypes = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region IsFilterMenuOpen
        private bool _IsFilterMenuOpen;
        public bool IsFilterMenuOpen
        {
            get => _IsFilterMenuOpen;
            set => SetAndNotify(ref _IsFilterMenuOpen, value);
        }
        #endregion
        #region ItemTypeFilters
        private ObservableCollection<BoolOption> _ItemTypeFilters;
        public ObservableCollection<BoolOption> ItemTypeFilters
        {
            get => _ItemTypeFilters;
            set => SetAndNotify(ref _ItemTypeFilters, value);
        }
        #endregion
        #region ItemSearchText
        private string _ItemSearchText;
        public string ItemSearchText
        {
            get => _ItemSearchText;
            set
            {
                _ItemSearchText = value;
                NotifyPropertyChanged();
                UpdateFilteredItemList();
            }
        }
        #endregion
        #region CardShowCount
        private string _CardShowCount;
        public string CardShowCount
        {
            get => _CardShowCount;
            set => SetAndNotify(ref _CardShowCount, value);
        }
        #endregion

        // Commands
        #region AddItemCard
        public ICommand AddItemCard => new RelayCommand(DoAddItemCard);
        private void DoAddItemCard(object param)
        {
            ItemCard newItem = new();
            AllCards.Add(newItem);
            ActiveCard = newItem;
        }
        #endregion
        #region SortCards
        public ICommand SortItemCards => new RelayCommand(DoSortItemCards);
        private void DoSortItemCards(object param)
        {
            AllCards = new(AllCards.OrderBy(c => c.Name));
            AllCards.CollectionChanged += AllCards_CollectionChanged;
            UpdateFilteredItemList();
        }
        #endregion
        #region SaveCards
        public ICommand SaveItemCards => new RelayCommand(DoSaveItemCards);
        private void DoSaveItemCards(object param)
        {
            if (AllCards.Count() == 0)
            {
                // Prevents zero item save crash
                XDocument blankDoc = new XDocument();
                blankDoc.Add(new XElement("ItemCardSet"));
                blankDoc.Save(Configuration.ItemDataFilePath);
                return;
            }
            XDocument itemDocument = new XDocument();
            itemDocument.Add(XmlMethods.ListToXml(AllCards.ToList()));
            itemDocument.Save(Configuration.ItemDataFilePath);
            new NotificationDialog("Items Saved.").ShowDialog();
            return;
        }
        #endregion
        #region CreateReport
        public ICommand CreateReport => new RelayCommand(DoCreateReport);
        private void DoCreateReport(object param)
        {
            if (AllCards.Count() == 0) { new NotificationDialog("You have no items entered.").ShowDialog(); return; }
            string message = "POD Item Report:";
            Dictionary<string, int> itemsByType = new();
            Dictionary<string, int> itemsByName = new();
            List<string> priceErrors = new();
            List<string> valueErrors = new();
            List<string> missingImagesFor = new();
            List<string> missingIsbnFor = new();
            List<string> missingSerialNumFor = new();
            decimal totalPurchaseCost = 0m;
            decimal totalCurrentValue = 0m;
            bool hasDuplicateNames = false;

            foreach (ItemCard card in AllCards)
            {
                // Duplicate Check
                if (itemsByName.ContainsKey(card.Name))
                {
                    itemsByName[card.Name]++;
                    hasDuplicateNames = true;
                }
                else
                {
                    itemsByName.Add(card.Name, 1);
                }

                // Type Counts
                if (itemsByType.ContainsKey(card.CardType))
                {
                    itemsByType[card.CardType]++;
                }
                else
                {
                    itemsByType.Add(card.CardType, 1);
                }

                // Missing relevant important data
                if (card.CardType == "Firearm" && card.SerialNumber == "")
                {
                    missingSerialNumFor.Add(card.Name);
                }
                if (card.CardType == "Book" && card.ISBN == "")
                {
                    missingIsbnFor.Add(card.Name);
                }

                // Image check
                if (card.ItemImages.Count == 0) { missingImagesFor.Add(card.Name); }

                // Pricing totals
                bool validPurchasePrice = decimal.TryParse(card.PurchasePrice, out decimal purchasePrice);
                if (validPurchasePrice) { totalPurchaseCost += purchasePrice; } else { priceErrors.Add(card.Name); }

                bool validCurrentPrice = decimal.TryParse(card.CurrentPrice, out decimal currentPrice);
                if (validCurrentPrice) { totalCurrentValue += currentPrice; } else { valueErrors.Add(card.Name); }

            }

            message += "\n\nItems by Type";
            foreach (KeyValuePair<string, int> pair in itemsByType)
            {
                message += "\n" + pair.Key + ": " + pair.Value;
            }

            message += "\n\nTotal Cost: $" + totalPurchaseCost;
            message += "\nCurrent Value: $" + totalCurrentValue;

            if (hasDuplicateNames)
            {
                message += "\n\nDuplicate names found:";
                foreach (KeyValuePair<string, int> pair in itemsByName)
                {
                    if (pair.Value > 1) { message += "\n" + pair.Key + ": " + pair.Value; }
                }
            }

            message += GetMessageSegmentFromMissingData(missingImagesFor, "images");
            message += GetMessageSegmentFromMissingData(missingSerialNumFor, "serial number");
            message += GetMessageSegmentFromMissingData(missingIsbnFor, "ISBN");

            if (priceErrors.Count() > 0)
            {
                message += "\n\nInvalid purchase price entry for:";
                foreach (string item in priceErrors)
                {
                    message += "\n" + item;
                }
            }

            if (valueErrors.Count() > 0)
            {
                message += "\n\nInvalid current price entry for:";
                foreach (string item in valueErrors)
                {
                    message += "\n" + item;
                }
            }

            new NotificationDialog(message, "Report").ShowDialog();

        }
        #endregion
        #region ProcessKeyboardShortcut
        public ICommand ProcessKeyboardShortcut  => new RelayCommand(DoProcessKeyboardShortcut);
        private void DoProcessKeyboardShortcut(object key)
        {
            switch (key.ToString())
            {
                case "CtrlS":
                    DoSaveItemCards(null);
                    break;
                case "CtrlN":
                    DoAddItemCard(null);
                    break;
                case "CtrlR":
                    DoCreateReport(null);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region ChangeFilters
        public ICommand ChangeFilters => new RelayCommand(DoChangeFilters);
        private void DoChangeFilters(object param)
        {
            if (param == null) { return; }
            List<string> options = param.ToString().Split(',').ToList();
            if (options.Contains("ITEMTYPE"))
            {
                if (options.Contains("SELECTALL"))
                {
                    foreach (BoolOption filter in ItemTypeFilters)
                    {
                        filter.IsMarked = true;
                    }
                }
                if (options.Contains("UNSELECTALL"))
                {
                    foreach (BoolOption filter in ItemTypeFilters)
                    {
                        filter.IsMarked = false;
                    }
                }
            }
        }
        #endregion

        // Private Methods
        private string GetMessageSegmentFromMissingData(List<string> missingItems, string dataField)
        {
            if (missingItems.Count() == 0) { return ""; }
            string message = "\n\nMissing " + dataField + " for:";
            foreach (string item in missingItems)
            {
                message += "\n" + item;
            }
            return message;
        }
        private void SetSearchFilters()
        {
            ItemTypeFilters = new();
            foreach (string type in CardTypes)
            {
                ItemTypeFilters.Add(new(type, true));
                ItemTypeFilters.Last().PropertyChanged += new PropertyChangedEventHandler(ItemTypeFilter_PropertyChanged);
            }
        }
        private void ItemTypeFilter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateFilteredItemList();
        }
        private void AllCards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateFilteredItemList();
        }
        private void UpdateFilteredItemList()
        {
            ObservableCollection<ItemCard> filteredCards = new();
            foreach (ItemCard item in AllCards)
            {
                if (item.Name.ToUpper().Contains(ItemSearchText.ToUpper()) == false) { continue; }
                BoolOption filter = ItemTypeFilters.FirstOrDefault(filter => filter.Name == item.CardType);
                if (filter == null) { continue; }
                if (filter.IsMarked) { filteredCards.Add(item); }
            }
            FilteredCards = new(filteredCards.OrderBy(c => c.Name));

            CardShowCount = $"Showing {FilteredCards.Count} of {AllCards.Count}";

        }
    }
}
