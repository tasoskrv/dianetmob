using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DianetMob.Utils;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class LoginPage : ContentPage
    {
        SQLiteConnection conn = null;

        public LoginPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();

            Signup.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() => {
                        App.Current.MainPage = new SignUpPage();
                    })
                }
            );

            ForgotPassword.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() => {
                        App.Current.MainPage = new ForgotPasswordPage();
                    })
                }
            );
            usernameEntry.Completed += (object sender, EventArgs e) => { passwordEntry.Focus(); };
        }

        void OnValidateEmail(object sender, EventArgs e)
        {
            CheckValidMail();
        }

        private bool CheckValidMail()
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (usernameEntry.Text == null || !Regex.IsMatch(usernameEntry.Text, pattern))
            {
                MessageLabel.Text = Properties.LangResource.invalidemail;
                return false;
            }
            else
            {
                return true;
            }
        }

        private void OnFacebookLoginClicked(object sender, EventArgs e) {
             App.Current.MainPage = new LoginPageFacebook();
        }

        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            //call service example
            //MealService serv = await ServiceConnector.GetServiceData<MealService>("/meal/getall");
            //serv.InsertMeals();
            //call InsertMeal            
            if ((usernameEntry.Text == null) || (passwordEntry.Text == null) || (usernameEntry.Text == "") || (passwordEntry.Text == ""))
            {
                MessageLabel.Text = Properties.LangResource.entercredentials;
                return;
            }
            var user = new User
            {
                Email = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            if (CheckValidMail())
            {
                AreCredentialsCorrect(user);
            }
        }

        private async void PerformLogin(User user)
        {
            BeginLogin();
            ConnectionInfo info = StorageManager.GetConnectionInfo();
            info.LoginUser = user;
            info.LoginUser.Password = passwordEntry.Text;
            info.Settings.LastLoggedIn = user.IDUser;
            Settings settings = info.Settings;
            StorageManager.UpdateData<Settings>(settings);
            await GenLib.LoginSynch();
            if (user.Isactive == 0)
            {
                MessageLabel.Text = Properties.LangResource.notactive;
            }
            else
            {
                List<Weight> wghts = conn.Query<Weight>("SELECT IDWeight FROM Weight WHERE Deleted=0 AND IDUser=" + user.IDUser);
                List<Plan> plans = conn.Query<Plan>("SELECT IDPlan FROM Plan WHERE Deleted=0 AND IDUser=" + user.IDUser);
                if (user.HeightType == -1 || user.Height == -1 || user.Gender == -1 || wghts.Count == 0 || plans.Count == 0)
                {
                   
                    App.Current.MainPage = new LoginProcessPage();
                }
                else
                {
                    App.Current.MainPage = new MainPage();
                }                
            }
            EndLogin();
        }

        public void BeginLogin()
        {
            loader.IsRunning = true;
            btnlogin.IsEnabled = false;
            btnfblogin.IsEnabled = false;
            passwordEntry.IsEnabled = false;
            usernameEntry.IsEnabled = false;
            ForgotPassword.IsEnabled = false;
            Signup.IsEnabled = false;
        }

        public void EndLogin()
        {
            loader.IsRunning = false;
            btnlogin.IsEnabled = true;
            btnfblogin.IsEnabled = true;
            passwordEntry.IsEnabled = true;
            usernameEntry.IsEnabled = true;
            ForgotPassword.IsEnabled = true;
            Signup.IsEnabled = true;
        }

        public async void AreCredentialsCorrect(User user)
        {
            /* COMMENTED  - FIND USER LOCALLY 
            List<User> usrs = conn.Query<User>("SELECT IdUser,FirstName,LastName,Height,Birthdate,Email,Gender,HeightType,Password, AccessToken FROM User WHERE Email ='" + user.Email + "' AND Password ='" + user.Password + "'");
            if (usrs.Count > 0)
            {
                PerformLogin(usrs[0]);
                return;
            }
            */
            /* e.g. /user/login/username=spiroskaravanis2@gmail.com/password=12345 */
            try
            {
                BeginLogin();
                ModelService<User> srvUser = await ServiceConnector.GetServiceData<ModelService<User>>("/user/login/email=" + user.Email + "/password=" + user.Password);
                if (srvUser.totalRows > 0)
                {
                    srvUser.data[0].Password= user.Password;
                    srvUser.SaveAllToDB();
                    PerformLogin(srvUser.data[0]);
                    return;
                }
                EndLogin();
            }
            catch (Exception ex)
            {
                MessageLabel.Text = Properties.LangResource.error + " " + ex.Message;
                EndLogin();
            }
            MessageLabel.Text = Properties.LangResource.wrongcredentials;
        }
    }
}
