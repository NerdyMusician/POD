using POD.Models;
using POD.Toolbox;
using POD.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // Commands
        #region AddItemCard
        public ICommand AddItemCard => new RelayCommand(DoAddItemCard);
        private void DoAddItemCard(object param)
        {
            AllCards.Add(new ItemCard());
            ActiveCard = AllCards.Last();
        }
        #endregion
        #region SortCards
        public ICommand SortItemCards => new RelayCommand(DoSortItemCards);
        private void DoSortItemCards(object param)
        {
            AllCards = new(AllCards.OrderBy(c => c.Name));
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

    }
}
