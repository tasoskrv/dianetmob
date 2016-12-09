using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Model;
using DianetMob.Pages;
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

            masterPage.ListView.ItemSelected += OnItemSelected;

            if (Device.OS == TargetPlatform.Windows)
            {
                Master.Icon = "swap.png";
            }
        }
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuElement;
            if (item != null)
            {
                if (item.TargetType == typeof(StartPage))
                {
                    Settings settings = StorageManager.GetConnectionInfo().Settings;
                    settings.LastLoggedIn = 0;
                    StorageManager.UpdateData<Settings>(settings);
                    App.Current.MainPage = new StartPage();
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
