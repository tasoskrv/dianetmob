using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Pages;
using DianetMob.Utils;
using DianetMob.Views;
using System;
using Xamarin.Forms;


namespace DianetMob
{
    public partial class App : Application
    {

        public static InAppViewModel ViewModel;

        public App()
        {
            InitializeComponent();
            //ViewModel = new InAppViewModel();
            // ViewModel.RestoreState(Current.Properties);
            
            ConnectionInfo info = StorageManager.GetConnectionInfo();
            Settings settings = info.Settings;
            if (settings.LastLoggedIn != 0)
                MainPage = new MainPage();
            else
                MainPage = new LoginPage();//StartPage

        }


        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
         //  ViewModel.SaveState(Current.Properties);
        }

        protected override void OnResume()
        {
           
        }
    }
}
