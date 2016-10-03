using Xamarin.Forms;
using Dianet.DB;
using System;

namespace Dianet.Pages
{
    public partial class ProfilePage : ContentPage
    {                
        public ProfilePage()
        {
            InitializeComponent();
            FillInSettingsLoggedIn();
        }

        private void FillInSettingsLoggedIn()
        {
            fHelloLabel.Text = "Hello " + StorageManager.GetConnectionInfo().LoginUser.FirstName.ToString() + "!";                                    
            fFirstNameEntry.Text = StorageManager.GetConnectionInfo().LoginUser.FirstName.ToString();
            fSurNameEntry.Text = StorageManager.GetConnectionInfo().LoginUser.LastName.ToString();
            fEmailEntry.Text = StorageManager.GetConnectionInfo().LoginUser.Email.ToString();
            fbirthDatePicker.Date = Convert.ToDateTime(StorageManager.GetConnectionInfo().LoginUser.Birthdate);

            if (StorageManager.GetConnectionInfo().LoginUser.Gender == 1)
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
            }
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
            fEmailEntry.IsEnabled = true;
            fbirthDatePicker.IsEnabled = true;
            fSexPicker.IsEnabled = true;
            fHeightPicker.IsEnabled = true;
            fHeightEntry.IsEnabled = true;            
            fWristPicker.IsEnabled = true;
        }

        private void OnSaveSettingsClicked(object sender, EventArgs e)
        {

        }

    }
}
