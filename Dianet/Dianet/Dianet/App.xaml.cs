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
        public static bool IsUserLoggedIn { get; set; }
        public App()
        {            
            SQLiteConnection conn = StorageManager.GetConnection();            
            List<Settings> sett = conn.Query<Settings>("SELECT IDSettings,TrialPeriod,RemindWeight,LastLoggedIn FROM Settings");
            if ((sett.Count > 0) && (sett[0].LastLoggedIn != 0))
            {
                IsUserLoggedIn = true;
                MainPage = new MainPage();
            }
            else
            {
                IsUserLoggedIn = false;
                MainPage = new StartPage();
            }            
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
