using Xamarin.Forms;
using Dianet.DB;
using System;
using System.Text.RegularExpressions;
using Dianet.DB.Entities;

namespace Dianet.Pages
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
            //fFirstNameEntry.Text = StorageManager.GetConnectionInfo().LoginUser.FirstName.ToString();
            //fSurNameEntry.Text = StorageManager.GetConnectionInfo().LoginUser.LastName.ToString();
            //fEmailEntry.Text = StorageManager.GetConnectionInfo().LoginUser.Email.ToString();
            //fbirthDatePicker.Date = Convert.ToDateTime(StorageManager.GetConnectionInfo().LoginUser.Birthdate);

            /*if (StorageManager.GetConnectionInfo().LoginUser.Gender == 1)
            {
                fSexPicker.SelectedIndex = 0;
            }
            else
            {
                fSexPicker.SelectedIndex = 1;
            }

            if (StorageManager.GetConnectionInfo().LoginUser.HeightType == 1)
            {
                fHeightPicker.SelectedIndex = 0;
            }
            else
            {
                fHeightPicker.SelectedIndex = 1;
            }*/
            fHeightEntry.Text = StorageManager.GetConnectionInfo().LoginUser.Height.ToString();

            if (StorageManager.GetConnectionInfo().LoginUser.Skeleton == 1)
            {
                fWristPicker.SelectedIndex = 0;
            }
            else if (StorageManager.GetConnectionInfo().LoginUser.Skeleton == 2)
            {
                fWristPicker.SelectedIndex = 1;
            }
            else
            {
                fWristPicker.SelectedIndex = 2;
            }            
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
            fWristPicker.IsEnabled = true;
        }

        private void OnSaveSettingsClicked(object sender, EventArgs e)
        {
            if (fFirstNameEntry.IsEnabled || fSurNameEntry.IsEnabled || fbirthDatePicker.IsEnabled ||
                fSexPicker.IsEnabled || fHeightPicker.IsEnabled || fHeightEntry.IsEnabled || fWristPicker.IsEnabled)
            {
                if (AllFieldsAreFilled())
                {
                    User user = new User();
                    user.FirstName = fFirstNameEntry.Text;
                    user.LastName = fSurNameEntry.Text;
                    user.Email = fEmailEntry.Text;
                    user.Birthdate = fbirthDatePicker.Date;
                    //  user.InsertDate = DateTime.Now;
                    //  user.UpdateDate = user.InsertDate;
                    user.Gender = fSexPicker.SelectedIndex + 1;
                    user.Height = Convert.ToDouble(fHeightEntry.Text);
                    user.Skeleton = fWristPicker.SelectedIndex + 1;
                    user.UpdateDate = DateTime.Now;
                    user.AccessToken = StorageManager.GetConnectionInfo().LoginUser.AccessToken;
                    user.HeightType = fHeightPicker.SelectedIndex + 1;
                    user.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                    user.InsertDate = StorageManager.GetConnectionInfo().LoginUser.InsertDate;
                    user.Password = StorageManager.GetConnectionInfo().LoginUser.Password;                                                            
                    StorageManager.UpdateData(user);
                    //NeedToLoginNextTime(user);
                    StorageManager.GetConnectionInfo().LoginUser = user;                    
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
            fWristPicker.IsEnabled = false;
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
                fEmailEntry.Text == ""       || fFirstNameEntry.Text == "" || fSurNameEntry.Text == ""  || fHeightEntry.Text == "" )
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
