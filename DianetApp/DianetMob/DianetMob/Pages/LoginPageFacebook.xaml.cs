using DianetMob.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class LoginPageFacebook : ContentPage
    {
        
        public LoginPageFacebook()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Unsubscribe<LoginPageFacebook, string>(this, "LoginToFacebook");
            MessagingCenter.Subscribe<LoginPageFacebook, string>(this, "LoginToFacebook", OnLoginToFacebook);

        }
        private void OnLoginToFacebook(LoginPageFacebook sender, string json)
        {
            if (json.Equals("error"))
            {
            }
            else
            {
                FacebookObj facebookobj = JsonConvert.DeserializeObject<FacebookObj>(json);
                //TODO DO LOGIN STAFF
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage = new MainPage();
                });
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LoginPageFacebook, string>(this, "LoginToFacebook");
        }

    }
}
