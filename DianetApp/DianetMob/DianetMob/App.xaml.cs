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

                SQLiteConnection conn = StorageManager.GetConnection(); ;
                List<Weight> wghts = conn.Query<Weight>("SELECT IDWeight FROM Weight WHERE IDUser=" + info.LoginUser.IDUser);
                List<Plan> plans = conn.Query<Plan>("SELECT IDPlan FROM Plan WHERE IDUser=" + info.LoginUser.IDUser);

                if (info.LoginUser.HeightType == -1 || info.LoginUser.Height == -1 || info.LoginUser.Gender == -1 || wghts.Count == 0 || plans.Count == 0)
                {
                    MainPage = new LoginProcessPage();
                }
                else
                {
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
