using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Model;
using DianetMob.Pages;
using DianetMob.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<MainPage, string>(this, "NotificationAction", OnNotificationMessage);
            masterPage.ListView.ItemSelected += OnItemSelected;
            masterPage.ButtonProfile.Clicked += OnBtnProfileCliked;
          //  if (Device.OS == TargetPlatform.Windows)
          // {
          //      Master.Icon = "swap.png";
          //  }
        }

        private void OnNotificationMessage(MainPage sender, string id)
        {
            if (Convert.ToInt32(id) == 33)
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MyWeight)));
            else if (Convert.ToInt32(id) > 20000 && Convert.ToInt32(id) < 20100) {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MyDay)));
            }
        }

        void OnBtnProfileCliked(object sender, EventArgs e)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ProfilePage)));
            masterPage.ListView.SelectedItem = null;
            IsPresented = false;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuElement;
            if (item != null)
            {
                if (item.TargetType == typeof(LoginPage))
                {
                    Settings settings = StorageManager.GetConnectionInfo().Settings;
                    settings.LastLoggedIn = 0;
                    StorageManager.UpdateData<Settings>(settings);
                    StorageManager.GetConnectionInfo().UserSettings = null;
                    StorageManager.GetConnectionInfo().ActiveSubscription = null;
                    App.Current.MainPage = new LoginPage();
                }
                else if (item.TargetType == typeof(ShopPage))
                {
                    if ((StorageManager.GetConnectionInfo().ActiveSubscription==null) || (StorageManager.GetConnectionInfo().ActiveSubscription.EndDate < DateTime.UtcNow))
                    {
                        Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                    }
                    else
                    {
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(PurchasesPage)));
                    }
                    masterPage.ListView.SelectedItem = null;
                    IsPresented = false;

                }
                else
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                    masterPage.ListView.SelectedItem = null;
                    IsPresented = false;
                }
            }
        }
    }
}
