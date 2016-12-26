using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Pages;
using Xamarin.Forms;


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
