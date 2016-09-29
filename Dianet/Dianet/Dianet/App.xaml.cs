using System.Collections.Generic;
using Xamarin.Forms;
using Dianet.Pages;
using Dianet.DB;
using Dianet.DB.Entities;
using SQLite;

namespace Dianet
{
    public partial class App : Application
    {
        public App()
        {
            Settings settings = StorageManager.GetConnectionInfo().Settings;            
            if (settings.LastLoggedIn != 0)
                MainPage = new MainPage();
            else
                MainPage = new StartPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
