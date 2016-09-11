using Dianet.DB.Entities;
using Dianet.Service;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using Dianet.DB;
using System.Collections.Generic;
using Dianet.Utils;

namespace Dianet.Pages
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
        private void PerformLogin() {
            App.IsUserLoggedIn = true;
            App.Current.MainPage = new MainPage();
        } 
        public async void AreCredentialsCorrect(User user)
        {
            ICollection<User> usrs = conn.Query<User>("SELECT IdUser,FirstName,LastName,Height,Birthdate,Email,Gender,HeightType,Password FROM User WHERE Email ='" + user.Email + "' AND Password ='" + user.Password + "'");
            if (usrs.Count > 0) {
                PerformLogin();
                return;
            }
            // return true;
            //πχ χρήστης   /user/login/username=spiros.karavanis@gmail.com/password=545
            ModelService<User> srvUser = await ServiceConnector.GetServiceData<ModelService<User>>("/user/login/username='" + user.Email + "'/password='" + user.Password + "'");
            if (srvUser.totalRows > 0) {
                srvUser.InsertAllToDB();
                GenLib.FullServiceLoadAndStore();
                PerformLogin();
                return;
            }
            //serv.in

           
            await  DisplayAlert("Sorry", "the credentials you have entered are wrong", "Forgot it?");
            
            //return false;                     
        }
    } 
}
