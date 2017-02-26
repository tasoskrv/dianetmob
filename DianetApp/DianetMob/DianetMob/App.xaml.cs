using Dianet.Notification;
using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Pages;
using DianetMob.Utils;
using DianetMob.Views;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace DianetMob
{
    public partial class App : Application
    {

        public static InAppViewModel ViewModel;

        public App()
        {
            InitializeComponent();
            // ViewModel.RestoreState(Current.Properties);

            App.ViewModel = new InAppViewModel();

            ConnectionInfo info = StorageManager.GetConnectionInfo();
            Settings settings = info.Settings;
            if (settings.Lang == 0)
            {
                MainPage = new LanguagesPage();
            }
            else if (settings.LastLoggedIn != 0)
            {//TODO weight, goal
                
                if (info.LoginUser.HeightType == -1 || info.LoginUser.Height == -1 || info.LoginUser.Gender == -1 || info.LoginUser.Weight == 0 || info.LoginUser.Goal == 0)
                {
                    MainPage = new LoginProcessPage();
                }
                else
                {
                    int days = (int)(info.LoginUser.LastWeightDate - DateTime.Now).TotalDays;
                    if (settings.RemindWeight < days)
                    {
                        var notifier = DependencyService.Get<ICrossLocalNotifications>().CreateLocalNotifier();
                        notifier.Notify(new LocalNotification()
                        {
                            Title = "Weight Reminder",
                            Text = "It has been "+ days+" days since you last entered your weight.",
                            Id = 33,
                            NotifyTime = DateTime.Now,
                        });
                    }
                    MainPage = new MainPage();
                }                
            }
            else
            {
                MainPage = new LoginPage();//StartPage
            }
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
