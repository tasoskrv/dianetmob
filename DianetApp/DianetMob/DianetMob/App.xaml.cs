using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;


using DianetMob.Model;
namespace DianetMob
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

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
