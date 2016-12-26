using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;


using DianetMob.Model;
using Dianet.Notification;

namespace DianetMob
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ConnectionInfo info = StorageManager.GetConnectionInfo();
            Settings settings = info.Settings;
            if (settings.LastLoggedIn != 0)
                MainPage = new MainPage();
            else
                MainPage = new LoginPage();//StartPage
        }

        protected override void OnStart()
        {
            
            var notifier = DependencyService.Get<ICrossLocalNotifications>().CreateLocalNotifier();
            notifier.Notify(new LocalNotification()
            {
                Title = "Title",
                Text = "Text",
                Id = 1,
                NotifyTime = DateTime.Now.AddSeconds(10),
            });
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
