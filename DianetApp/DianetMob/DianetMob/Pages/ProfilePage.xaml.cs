using DianetMob.DB;
using DianetMob.DB.Entities;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;

using Xamarin.Forms;

namespace DianetMob.Pages
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
            //fHelloLabel.Text = "Hello " + StorageManager.GetConnectionInfo().LoginUser.FirstName.ToString() + "!";
        }
        
        private void OnProfileSubmitClicked(object sender, EventArgs e)
        {
            if (ProfileBtn.Text.ToLower().Equals("edit"))
            {
                ProfileBtn.Text = "Save";
                fFirstNameEntry.IsEnabled = true;
                fSurNameEntry.IsEnabled = true;
                //fEmailEntry.IsEnabled = true;
                fbirthDatePicker.IsEnabled = true;
                fSexPicker.IsEnabled = true;
                fHeightPicker.IsEnabled = true;
                fHeightEntry.IsEnabled = true;
                fWristEntry.IsEnabled = true;
                fLocationEntry.IsEnabled = true;
            }
            else
            {
                ProfileBtn.Text = "Edit";
                if (fFirstNameEntry.IsEnabled || fSurNameEntry.IsEnabled || fbirthDatePicker.IsEnabled ||
                    fSexPicker.IsEnabled || fHeightPicker.IsEnabled || fHeightEntry.IsEnabled || fLocationEntry.IsEnabled)
                {
                    if (AllFieldsAreFilled())
                    {
                        StorageManager.UpdateData(StorageManager.GetConnectionInfo().LoginUser);
                        RefreshPage();
                    }
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
            fLocationEntry.IsEnabled = false;
        }
        
        private void ViewPhotosBtnTap(object sender, EventArgs e)
        {
            bool isVisible = PhotoGrid.IsVisible;
            if(isVisible)
                PhotoGrid.IsVisible = false;
            else
                PhotoGrid.IsVisible = true;
            
            ProfileScrollView.ScrollToAsync(ProfileLayout, ScrollToPosition.End, true);            
        }
        
        private async void TakePhotoButtonOnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No camera available", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true
                }
            );

            if (file == null)
                return;

            PathLabel.Text = file.AlbumPath;
            bool toggled = PhotoSelect.IsToggled;
            if (toggled)
            {
                AfterImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
            else
            {
                BeforeImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
        }

        private async void PickPhotoButtonOnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Oops", "Pick photo is not supported", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
            {
                return;
            }

            PathLabel.Text = file.Path;
            bool toggled = PhotoSelect.IsToggled;
            if (toggled)
            {
                AfterImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
            else
            {
                BeforeImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
        }

        /*
        private async void TakeVideoButtonOnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await DisplayAlert("No Camera", "No camera available", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
            {
                SaveToAlbum = true,
                Quality = VideoQuality.Medium
            });

            if (file == null)
            {
                return;
            }
            PathLabel.Text = "Video path " + file.Path;
            MainImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }
        */
            
        private void NeedToLoginNextTime(User usr)
        {            
            if (!StorageManager.GetConnectionInfo().LoginUser.Email.Equals(usr.Email, StringComparison.Ordinal))
            {                
                StorageManager.GetConnectionInfo().Settings.LastLoggedIn = 0;
                StorageManager.GetConnectionInfo().Settings.UpdateDate = usr.UpdateDate;                                
                StorageManager.UpdateData(StorageManager.GetConnectionInfo().Settings);
            }            
        }

        private bool AllFieldsAreFilled()
        {
            if (fFirstNameEntry.Text == null || fSurNameEntry.Text == null || fHeightEntry.Text == null || fLocationEntry.Text == null ||
                fEmailEntry.Text.Equals("") || fFirstNameEntry.Text.Equals("") || fSurNameEntry.Text.Equals("") || fHeightEntry.Text.Equals("") || fLocationEntry.Text.Equals(""))
            {
                DisplayAlert("Please", "fill in all fields", "OK");
                return false;
            }            
            return true;            
        }

    }
}
