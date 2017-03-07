using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Model;
using DianetMob.Service;
using Newtonsoft.Json;
using SQLite;
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
        private async void OnLoginToFacebook(LoginPageFacebook sender, string json)
        {
            if (json.Equals("error"))
            {
            }
            else
            {
                FacebookObj facebookobj = JsonConvert.DeserializeObject<FacebookObj>(json);
                //TODO DO LOGIN STAFF
                /*
                LoginPage login = new LoginPage();
                User user = new User();
                user.Email = facebookobj.email;                
                login.AreCredentialsCorrect();
                */
                try
                {
                    ModelService<User> srvUser = await ServiceConnector.GetServiceData<ModelService<User>>("/user/facebookLogin/email=" + facebookobj.email
                        + "/firstname=" + facebookobj.first_name + "/lastname=" + facebookobj.last_name + "/facebookid=" + facebookobj.ID);

                    if (srvUser.success)
                    {
                        User user = new User();
                        user.IDUser = srvUser.ID;
                        user.Email = facebookobj.email;
                        user.FirstName = facebookobj.first_name;
                        user.LastName = facebookobj.last_name;
                        StorageManager.SaveData(user);

                        SQLiteConnection conn = StorageManager.GetConnection(); ;
                        List<Weight> wghts = conn.Query<Weight>("SELECT IDWeight FROM Weight WHERE Deleted=0 AND IDUser=" + user.IDUser);
                        List<Plan> plans = conn.Query<Plan>("SELECT IDPlan FROM Plan WHERE Deleted=0 AND IDUser=" + user.IDUser);

                        ConnectionInfo info = StorageManager.GetConnectionInfo();
                        info.LoginUser = user;
                        info.Settings.LastLoggedIn = srvUser.ID;
                        Settings settings = info.Settings;
                        StorageManager.UpdateData<Settings>(settings);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (info.LoginUser.HeightType == -1 || info.LoginUser.Height == -1 || info.LoginUser.Gender == -1 || wghts.Count == 0 || plans.Count == 0)
                            {
                                App.Current.MainPage = new LoginProcessPage();
                            }
                            else
                            {
                                App.Current.MainPage = new MainPage();
                            }
                        });
                        return;                        
                        //srvUser.data[0].Password= user.Password;
                        //srvUser.SaveAllToDB();
                        //PerformLogin(srvUser.data[0]);
                    }
                    else
                    {
                        await DisplayAlert(Properties.LangResource.error, Properties.LangResource.genericerror, "OK");
                    }
                    
                }
                catch (Exception ex)
                {
                    //MessageLabel.Text = "Error " + ex.Message;
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LoginPageFacebook, string>(this, "LoginToFacebook");
        }

    }
}
