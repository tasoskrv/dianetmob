using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Model;
using DianetMob.Pages;
using DianetMob.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DianetMob.Views
{
    public class InAppViewModel : ViewModelBase
    {
        ObservableCollection<InAppProduct> _products;
        ObservableCollection<InAppPurchase> _purchases;
        InAppPurchaseList _purchaseList;

        public InAppViewModel()
        {
            TheInAppService = DependencyService.Get<IInAppService>();
            TheInAppService.OnQueryInventory += OnQueryInventory;
            TheInAppService.OnPurchaseProduct += OnPurchaseProduct;
            TheInAppService.OnRestoreProducts += OnRestoreProducts;
            TheInAppService.Initialize();
            _purchases = new ObservableCollection<InAppPurchase>();
            _purchaseList = new InAppPurchaseList();

            InitializeProducts();

            QueryCommand = new Command<InAppProduct>(
                execute: (product) =>
                {
                    TheInAppService.QueryInventory();
                });

            PurchaseCommand = new Command<InAppProduct>(
                execute: (product) =>
                {
                    TheInAppService.PurchaseProduct(product.ProductId);
                });

            RestoreCommand = new Command<InAppProduct>(
                execute: (product) =>
                {
                    TheInAppService.RestoreProducts();
                });

            RefundCommand = new Command<InAppProduct>(
                execute: (product) =>
                {
                 TheInAppService.RefundProduct();
                });
        }

        public IInAppService TheInAppService { get; private set; }

        void OnQueryInventory()
        {
           // throw new System.NotImplementedException();
        }

        void OnPurchaseProduct()
        {
            //List<Package> packs = StorageManager.GetConnection().Query<Package>("SELECT * FROM package WHERE GooglePlay=" + _purchases[0].ProductId + " or AppleStore=" + _purchases[0].ProductId );
            //if (packs.Count > 0)
            //{
                Subscription sub = new Subscription();
                sub.BeginDate = DateTime.UtcNow;
                sub.EndDate = sub.BeginDate.AddDays(_products[0].ProductPackage.Duration);
                sub.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                sub.InsertDate = sub.BeginDate;
                sub.IsActive = 1;
                sub.Price = _products[0].ProductPackage.Price;
                sub.UpdateDate= sub.BeginDate;
                sub.Trncode = _purchases[0].OrderId;
                StorageManager.InsertData<Subscription>(sub);
                StorageManager.GetConnectionInfo().ActiveSubscription = sub;
                App.Current.MainPage = new MainPage();
          //  }
           // throw new System.NotImplementedException();
        }
        void OnRestoreProducts()
        {
           // throw new System.NotImplementedException();
        }

        public ObservableCollection<InAppProduct> Products
        {
            private set { SetProperty(ref _products, value); }
            get { return _products; }
        }

        public ObservableCollection<InAppPurchase> Purchases
        {
            private set { SetProperty(ref _purchases, value); }
            get { return _purchases; }
        }

        public ICommand QueryCommand { private set; get; }

        public ICommand PurchaseCommand { private set; get; }

        public ICommand RestoreCommand { private set; get; }

        public ICommand RefundCommand { private set; get; }

        public void SaveState(IDictionary<string, object> dictionary)
        {
            _purchaseList.Purchases = new List<InAppPurchase>(_purchases);

            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(InAppPurchaseList));
                ser.WriteObject(ms, _purchaseList);
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    var purchases = sr.ReadToEnd();
                    dictionary["Purchases"] = purchases;
                }
            }
        }

        public void RestoreState(IDictionary<string, object> dictionary)
        {
            string purchases = GetDictionaryEntry(dictionary, "Purchases", string.Empty);
            if (purchases != string.Empty)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(InAppPurchaseList));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(purchases)))
                {
                    InAppPurchaseList purchaseList = (InAppPurchaseList)ser.ReadObject(ms);
                    Purchases = new ObservableCollection<InAppPurchase>(purchaseList.Purchases);
                }
            }
        }

        public T GetDictionaryEntry<T>(IDictionary<string, object> dictionary,
                                        string key, T defaultValue)
        {
            if (dictionary.ContainsKey(key))
                return (T)dictionary[key];

            return defaultValue;
        }

        public void InitializeProducts()
        {
            _products = new ObservableCollection<InAppProduct>();
            SQLiteConnection conn = StorageManager.GetConnection();
            IEnumerable<Package> packs = conn.Query<Package>("SELECT * FROM package WHERE IsActive=1");

            foreach (Package pack in packs)
            {
                _products.Add(new InAppProduct
                {
                    ProductId = pack.GooglePlay,
                    Title = pack.Name,
                    Description = pack.Description,
                    Price = pack.Price.ToString(),
                    PriceCurrencyCode = "EUR",
                    ProductPackage = pack,
                    IconSource="shop.png"
                });
            }

        }
    }
}
