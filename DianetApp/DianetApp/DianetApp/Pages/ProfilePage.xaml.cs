using DianetApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetApp.Pages
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            FillInSettingsLoggedIn();
            BindingContext = StorageManager.GetConnectionInfo().LoginUser;
        }

        private void FillInSettingsLoggedIn()
        {
            fHelloLabel.Text = "Hello " + StorageManager.GetConnectionInfo().LoginUser.FirstName.ToString() + "!";
        }

        private void OnEditClicked(object sender, EventArgs e)
        {
            fFirstNameEntry.IsEnabled = true;
            fSurNameEntry.IsEnabled = true;
            //fEmailEntry.IsEnabled = true;
            fbirthDatePicker.IsEnabled = true;
            fSexPicker.IsEnabled = true;
            fHeightPicker.IsEnabled = true;
            fHeightEntry.IsEnabled = true;
            fWristEntry.IsEnabled = true;
        }

        private void OnSaveSettingsClicked(object sender, EventArgs e)
        {
            if (fFirstNameEntry.IsEnabled || fSurNameEntry.IsEnabled || fbirthDatePicker.IsEnabled ||
                fSexPicker.IsEnabled || fHeightPicker.IsEnabled || fHeightEntry.IsEnabled)
            {
                if (AllFieldsAreFilled())
                {
                    StorageManager.UpdateData(StorageManager.GetConnectionInfo().LoginUser);
                    RefreshPage();
                }
            }
        }

        private void RefreshPage()
        {
            FillInSettingsLoggedIn();
            fFirstNameEntry.IsEnabled = false;
            fSurNameEntry.IsEnabled = false;
            fEmailEntry.IsEnabled = false;
            fbirthDatePicker.IsEnabled = false;
            fSexPicker.IsEnabled = false;
            fHeightPicker.IsEnabled = false;
            fHeightEntry.IsEnabled = false;
            fWristEntry.IsEnabled = false;
        }

        /*private void NeedToLoginNextTime(User usr)
        {            
            if (!StorageManager.GetConnectionInfo().LoginUser.Email.Equals(usr.Email, StringComparison.Ordinal))
            {                
                StorageManager.GetConnectionInfo().Settings.LastLoggedIn = 0;
                StorageManager.GetConnectionInfo().Settings.UpdateDate = usr.UpdateDate;                                
                StorageManager.UpdateData(StorageManager.GetConnectionInfo().Settings);
            }            
        }*/

        private bool AllFieldsAreFilled()
        {
            if (fFirstNameEntry.Text == null || fSurNameEntry.Text == null || fHeightEntry.Text == null ||
                fEmailEntry.Text == "" || fFirstNameEntry.Text == "" || fSurNameEntry.Text == "" || fHeightEntry.Text == "")
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
