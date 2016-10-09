using Dianet.DB;
using Dianet.DB.Entities;
using Dianet.Service;
using System;
using System.Text.RegularExpressions;

using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            StorageManager.GetConnection();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (AllFieldsAreFilled() && CheckValidMail())
                {
                    int token = GetRegistrationToken(emailEntry.Text, passwdEntry.Text);
                    User user = new User();
                    user.FirstName = nameEntry.Text;
                    user.LastName = surnameEntry.Text;
                    user.Email = emailEntry.Text;
                    user.Password = passwdEntry.Text;
                  //  user.InsertDate = DateTime.Now;
                  //  user.UpdateDate = user.InsertDate;
                    user.Birthdate = birthDate.Date;
                    user.AccessToken = token.ToString();
                    ModelService<User> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<User>>("/user/insert/", user);
                    if ((srvNewUser.success == true) && (srvNewUser.ID > 0) && !(srvNewUser.ErrorCode > 0))
                    {
                        user.InsertDate = DateTime.UtcNow;
                        user.UpdateDate = user.InsertDate;
                        user.IdUser = srvNewUser.ID;
                        user.AccessToken = srvNewUser.AccessToken;
                        srvNewUser.InsertRecordToDB(user);
                        App.Current.MainPage = new SignUpPage2();
                        return;
                    }
                    else if (srvNewUser.ErrorCode == 2) {
                        await DisplayAlert("Warning", "O Χρήστης υπάρχει ήδη!", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Warning", srvNewUser.message, "OK");
                    }
                    
                    return;
                }
            }
            catch (Exception ex) {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private int GetRegistrationToken(string mail, string pass)
        {
            return (mail.Length * pass.Length * 9999888);
        }

        void OnValidateEmail(object sender, EventArgs e)
        {
            CheckValidMail();
        }

        private bool CheckValidMail()
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (!Regex.IsMatch(emailEntry.Text, pattern))
            {
                DisplayAlert("Please", "enter a valid email", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool AllFieldsAreFilled()
        {
            if (emailEntry.Text == null || nameEntry.Text == null || surnameEntry.Text == null || passwdEntry.Text == null ||
                emailEntry.Text == ""   || nameEntry.Text == ""   || surnameEntry.Text == ""   || passwdEntry.Text == ""   )
            {
                DisplayAlert("Please", "fill in all fields", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }        
    }
}
