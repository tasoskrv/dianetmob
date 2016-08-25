using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Dianet.Pages;
using Dianet.DB;

namespace Dianet
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        //public Page EntryPage { get; set; }
        public App()
        {
            //EntryPage = new EntryPage();
            if (!IsUserLoggedIn)
            {
                MainPage = new MainPage();
                //MainPage = new NavigationPage(new MasterPage());
            }
            else
            {
                MainPage = new MainPage();
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
