using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dianet.Model;
using Dianet.DB;
using Dianet.DB.Entities;

namespace Dianet.Pages
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
            {   if (item.TargetType == typeof(StartPage))
                {
                    Settings settings = StorageManager.GetConnectionInfo().Settings;
                    settings.LastLoggedIn = 0;
                    settings.LastSyncDate = DateTime.MinValue;
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
