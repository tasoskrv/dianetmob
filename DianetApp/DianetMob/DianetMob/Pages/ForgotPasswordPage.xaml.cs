using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Model;
using DianetMob.Service;
using DianetMob.Utils;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class ForgotPasswordPage : ContentPage
    {

        public ForgotPasswordPage()
        {
            InitializeComponent(); 
        }

        private string GetRecoverToken(string mail)
        {
            return (mail.Length * 9999888).ToString();
        }

        private async void OnMailSendButtonClicked(object sender, EventArgs e)
        {
            if (emailEntry.Text == "" || emailEntry.Text == null)
            {
                MessageLabel.Text = "Enter your email";
                return;
            }

            bool isValid = GenLib.CheckValidMail(emailEntry.Text);
            if (!isValid)
            {
                MessageLabel.Text = "Please enter a valid email";
            }
            else
            {                
                mailSend.IsEnabled = false;
                User user = new User();
                user.Email = emailEntry.Text;
                user.AccessToken = this.GetRecoverToken(user.Email.ToString());
                
                ModelService<User> srvRecoverUser = await ServiceConnector.InsertServiceData<ModelService<User>>("/user/recover/", user);
                if (srvRecoverUser.success == true)
                {
                    App.Current.MainPage = new LoginPage();
                }
                else if (srvRecoverUser.ErrorCode == 2)
                {
                    MessageLabel.Text = "User not found";
                }
                else
                {
                    MessageLabel.Text = "Something went wrong";
                }
                mailSend.IsEnabled = true;                
            }
        }        
    }
}
