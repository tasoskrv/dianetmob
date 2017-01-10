using Android.Content;
using DianetMob.DB;
using DianetMob.DB.Entities;
using Java.IO;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class ProfilePage : ContentPage
    {
        private MediaFile _mediaFileBefore;
        private MediaFile _mediaFileAfter;

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
                _mediaFileAfter = file;
                AfterImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = _mediaFileAfter.GetStream();
                    //_mediaFile.Dispose();
                    return stream;
                });
            }
            else
            {
                _mediaFileBefore = file;
                BeforeImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = _mediaFileBefore.GetStream();                    
                    //_mediaFile.Dispose();
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
                return;
            
            PathLabel.Text = file.Path;
            bool toggled = PhotoSelect.IsToggled;
            if (toggled)
            {
                _mediaFileAfter = file;
                AfterImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = _mediaFileAfter.GetStream();
                    //_mediaFile.Dispose();
                    return stream;
                });
            }
            else
            {
                _mediaFileBefore = file;
                BeforeImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = _mediaFileBefore.GetStream();
                    //_mediaFile.Dispose();
                    return stream;
                });
            }
        }

        public async void OnUploadClicked(object sender, EventArgs e)
        {            
            if (_mediaFileBefore != null)
            {
                uploadImage(_mediaFileBefore, "before");
            }
            else
            {
                await DisplayAlert("Message", "No images to upload", "OK");
            }
            if (_mediaFileAfter != null)
            {
                uploadImage(_mediaFileAfter, "after");
            }
            else
            {
                await DisplayAlert("Message", "No images to upload", "OK");
            }
        }

        private async void uploadImage(MediaFile mediafile, string type)
        {
            byte[] bitmapData;
            var stream = new MemoryStream();
            mediafile.GetStream().CopyTo(stream);
            bitmapData = stream.ToArray();
            var fileContent = new ByteArrayContent(bitmapData);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "image",
                FileName = type + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString()
            };

            string boundary = "---8393774hhy37373773";
            MultipartFormDataContent multipartContent = new MultipartFormDataContent(boundary);
            multipartContent.Add(fileContent);
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsync("http://dianet.cloudocean.gr/api/v1/user/upload", multipartContent);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                JObject res = JObject.Parse(content);
                if (res["success"].ToString() == "True")
                {
                    await DisplayAlert("Message", "Image uploaded", "OK");
                }
                else
                {
                    await DisplayAlert("Message", "Error - " + res["error"].ToString() + " code:" + res["code"].ToString(), "OK");
                }
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
