using DianetApp.DB;
using DianetApp.DB.Entities;
using DianetApp.Service;
using DianetApp.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        SQLiteConnection conn = null;
        public LoginPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        private void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new SignUpPage();
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
                DisplayAlert("Please", "enter a valid email", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            //Παράδειγμα κλήσης service     
            //MealService serv = await ServiceConnector.GetServiceData<MealService>("/meal/getall");
            //serv.InsertMeals();
            //Κληση InsertMeal            

            if ((usernameEntry.Text == null) || (passwordEntry.Text == null) || (usernameEntry.Text == "") || (passwordEntry.Text == ""))
            {
                DisplayAlert("Please", "enter your credentials", "OK");
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

        private void PerformLogin(User user)
        {
            StorageManager.GetConnectionInfo().LoginUser = user;
            StorageManager.GetConnectionInfo().LoginUser.Password = passwordEntry.Text;
            StorageManager.GetConnectionInfo().Settings.LastLoggedIn = user.IDUser;
            StorageManager.UpdateData<Settings>(StorageManager.GetConnectionInfo().Settings);
            App.Current.MainPage = new MainPage();
        }

        public async void AreCredentialsCorrect(User user)
        {
            List<User> usrs = conn.Query<User>("SELECT IdUser,FirstName,LastName,Height,Birthdate,Email,Gender,HeightType,Password, AccessToken FROM User WHERE Email ='" + user.Email + "' AND Password ='" + user.Password + "'");
            if (usrs.Count > 0)
            {
                PerformLogin(usrs[0]);
                GenLib.FullServiceLoadAndStore();
                return;
            }
            //πχ χρήστης   /user/login/username=spiroskaravanis2@gmail.com/password=12345
            try
            {
                ModelService<User> srvUser = await ServiceConnector.GetServiceData<ModelService<User>>("/user/login/email=" + user.Email + "/password=" + user.Password);
                if (srvUser.totalRows > 0)
                {
                    srvUser.SaveAllToDB();
                    PerformLogin(srvUser.data[0]);
                    GenLib.FullServiceLoadAndStore();
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            await DisplayAlert("Sorry", "the credentials you have entered are wrong", "Forgot it?");
        }
    }
}
