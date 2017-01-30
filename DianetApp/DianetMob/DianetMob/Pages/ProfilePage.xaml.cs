using Android.Content;
using DianetMob.DB;
using DianetMob.DB.Entities;
using Java.IO;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
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
        private int numMsg = 0;        
        private SQLiteConnection conn = null;
        private int total = 0;
        User user = null;

        Dictionary<string, int> heights = new Dictionary<string, int>
        {
            { "Cm", 1 }, { "In", 2 }
        };

        Dictionary<string, int> genders = new Dictionary<string, int>
        {
            { "Male", 1 }, { "Female", 2 }
        };

        public ProfilePage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();

            foreach (string height in heights.Keys)
            {
                fHeightPicker.Items.Add(height);
            }
            foreach (string gender in genders.Keys)
            {
                fSexPicker.Items.Add(gender);
            }

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
                fSkeletonEntry.IsEnabled = true;
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
                        var user = StorageManager.GetConnectionInfo().LoginUser;
                        int heightType = heights[fHeightPicker.Items[fHeightPicker.SelectedIndex]];
                        int genderType = genders[fSexPicker.Items[fSexPicker.SelectedIndex]];
                        user.FirstName = fFirstNameEntry.Text;
                        user.LastName = fSurNameEntry.Text;
                        user.Birthdate = fbirthDatePicker.Date;
                        user.Gender = genderType;
                        user.HeightType = heightType;
                        user.Height = Convert.ToDouble(fHeightEntry.Text);
                        user.Skeleton = Convert.ToDouble(fSkeletonEntry.Text);
                        user.Location = fLocationEntry.Text;                        
                        StorageManager.UpdateData(user);
                        RefreshPage();
                    }
                    else
                    {
                        ProfileBtn.Text = "Save";
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
            fSkeletonEntry.IsEnabled = false;
            fLocationEntry.IsEnabled = false;
        }
        
        
        private async void TakePhotoButtonOnClickedB(object sender, EventArgs e)
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

            //PathLabel.Text = file.AlbumPath;

            _mediaFileBefore = file;
            BeforeImage.Source = ImageSource.FromStream(() =>
            {
                var stream = _mediaFileBefore.GetStream();
                    //_mediaFile.Dispose();
                    return stream;
            });


        }

        private async void TakePhotoButtonOnClickedA(object sender, EventArgs e)
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

            //PathLabel.Text = file.AlbumPath;
            _mediaFileAfter = file;
            AfterImage.Source = ImageSource.FromStream(() =>
            {
                var stream = _mediaFileAfter.GetStream();
                    //_mediaFile.Dispose();
                    return stream;
            });

        }

        private async void PickPhotoButtonOnClickedB(object sender, EventArgs e)
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

            //PathLabel.Text = file.Path;

            _mediaFileBefore = file;
            BeforeImage.Source = ImageSource.FromStream(() =>
            {
                var stream = _mediaFileBefore.GetStream();
                    //_mediaFile.Dispose();
                    return stream;
            });

        }

        private async void PickPhotoButtonOnClickedA(object sender, EventArgs e)
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

            //PathLabel.Text = file.Path;

            _mediaFileAfter = file;
            AfterImage.Source = ImageSource.FromStream(() =>
            {
                var stream = _mediaFileAfter.GetStream();
                    //_mediaFile.Dispose();
                    return stream;
            });

        }

        public void OnUploadClicked(object sender, EventArgs e)
        {
            if (_mediaFileBefore != null && _mediaFileAfter != null)
            {
                total = 2;
            }
            else if (_mediaFileBefore != null || _mediaFileAfter != null)
            {
                total = 1;
            }                                           
            if (_mediaFileBefore != null)
            {
                uploadImage(_mediaFileBefore, "before");
            }
            if (_mediaFileAfter != null)
            {
                uploadImage(_mediaFileAfter, "after");
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

                    numMsg = numMsg + 1;
                    UploadMsg.Text = "uploaded " + numMsg;

                    //await DisplayAlert("Message", "Image uploaded", "OK");
                    //numMsg = numMsg + 1;
                    //UploadMsg.Text = "uploading image " + numMsg;
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
            if (fEmailEntry.Text == null || fFirstNameEntry.Text == null || fSurNameEntry.Text == null || fHeightEntry.Text == null ||
                fEmailEntry.Text.Equals("") || fFirstNameEntry.Text.Equals("") || fSurNameEntry.Text.Equals("") || fHeightEntry.Text.Equals(""))
            {
                DisplayAlert("Please", "Fill all fields", "OK");
                return false;
            }            
            return true;            
        }
    }
}
