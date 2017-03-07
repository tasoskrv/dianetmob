using Dianet.Notification;
using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Utils;
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
        private SQLiteConnection conn = null;
        private ChangePassword changePasswordPageDt = new ChangePassword();
        private bool isUploading=false;
        // private int total = 0;
        private User user = null;
        private int Aweighttype;

        Dictionary<string, int> heights = new Dictionary<string, int>
        {
            { "Cm", 1 }, { "In", 2 }
        };

        Dictionary<string, int> weights = new Dictionary<string, int>
        {
            { Properties.LangResource.kgs, 1 }, {  Properties.LangResource.pounds, 2 }, {  Properties.LangResource.ounces, 3 }
        };

        Dictionary<string, int> diets = new Dictionary<string, int>
        {
            { Properties.LangResource.normal, 0 }, {  Properties.LangResource.ovum, 1 }, {  Properties.LangResource.sperm, 2 }, { Properties.LangResource.nkcells, 3 }
        };

        Dictionary<string, int> genders = new Dictionary<string, int>
        {
            { Properties.LangResource.male, 1 }, { Properties.LangResource.female, 2 }
        };

        public ProfilePage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();

            foreach (string height in heights.Keys)
            {
                fHeightPicker.Items.Add(height);
            }
            foreach (string weight in weights.Keys)
            {
                fWeightPicker.Items.Add(weight);
            }
            foreach (string diet in diets.Keys)
            {
                fDietTypePicker.Items.Add(diet);
            }
            foreach (string gender in genders.Keys)
            {
                fSexPicker.Items.Add(gender);
            }
            user = StorageManager.GetConnectionInfo().LoginUser;
            BindingContext = user;
            Aweighttype = user.WeightType;
            if (user.ImageBefore != null) {
                Stream Astream = new MemoryStream(user.ImageBefore);
                BeforeImage.Source = ImageSource.FromStream(() => { return Astream; });
            }
            if (user.ImageAfter != null)
            {
                Stream Astream = new MemoryStream(user.ImageAfter);
                BeforeImage.Source = ImageSource.FromStream(() => { return Astream; });
            }
        }
        
        private async void OnProfileSubmitClicked(object sender, EventArgs e)
        {
            if (ProfileBtn.Text.Equals(Properties.LangResource.edit))
            {
                ProfileBtn.Text = Properties.LangResource.save;
                fFirstNameEntry.IsEnabled = true;
                fSurNameEntry.IsEnabled = true;
                //fEmailEntry.IsEnabled = true;
                fbirthDatePicker.IsEnabled = true;
                fSexPicker.IsEnabled = true;
                fHeightPicker.IsEnabled = true;
                fWeightPicker.IsEnabled = true;
                fDietTypePicker.IsEnabled = true;
                fHeightEntry.IsEnabled = true;
                fSkeletonEntry.IsEnabled = true;
                fLocationEntry.IsEnabled = true;
                btnTA.IsVisible = true;
                btnPA.IsVisible = true;
                btnTB.IsVisible = true;
                btnPB.IsVisible = true;
            }
            else
            {
                ProfileBtn.Text = Properties.LangResource.edit;
                if (fFirstNameEntry.IsEnabled || fSurNameEntry.IsEnabled || fbirthDatePicker.IsEnabled ||
                    fSexPicker.IsEnabled || fHeightPicker.IsEnabled || fWeightPicker.IsEnabled || fHeightEntry.IsEnabled || fLocationEntry.IsEnabled)
                {
                    if (AllFieldsAreFilled())
                    {
                        if (Aweighttype != user.WeightType)
                        {
                            var answer = await App.Current.MainPage.DisplayAlert(Properties.LangResource.changeWeightTypeTitle, Properties.LangResource.changeWeightType, Properties.LangResource.yes, Properties.LangResource.no);
                            if (answer == true)
                            {
                                IEnumerable<Weight> Wrecords = conn.Query<Weight>("select * from weight where iduser=" + user.IDUser);

                                if (Aweighttype == 1)
                                {
                                    if (user.WeightType == 2)
                                    {
                                        foreach (Weight wrecord in Wrecords)
                                        {
                                            wrecord.WValue = Math.Round(wrecord.WValue * 2.204, 2);
                                            StorageManager.UpdateData<Weight>(wrecord);
                                        }
                                    }
                                    else if (user.WeightType == 3)
                                    {
                                        foreach (Weight wrecord in Wrecords)
                                        {
                                            wrecord.WValue = Math.Round(wrecord.WValue * 35.273, 2);
                                            StorageManager.UpdateData<Weight>(wrecord);
                                        }
                                    }
                                }
                                else if (Aweighttype == 2)
                                {
                                    if (user.WeightType == 1)
                                    {
                                        foreach (Weight wrecord in Wrecords)
                                        {
                                            wrecord.WValue = Math.Round(wrecord.WValue * 0.453, 2);
                                            StorageManager.UpdateData<Weight>(wrecord);
                                        }
                                    }
                                    else if (user.WeightType == 3)
                                    {
                                        foreach (Weight wrecord in Wrecords)
                                        {
                                            wrecord.WValue = Math.Round(wrecord.WValue * 16, 2);
                                            StorageManager.UpdateData<Weight>(wrecord);
                                        }
                                    }
                                }
                                else if (Aweighttype == 3)
                                {
                                    if (user.WeightType == 1)
                                    {
                                        foreach (Weight wrecord in Wrecords)
                                        {
                                            wrecord.WValue = Math.Round(wrecord.WValue * 0.028, 2);
                                            StorageManager.UpdateData<Weight>(wrecord);
                                        }
                                    }
                                    else if (user.WeightType == 2)
                                    {
                                        foreach (Weight wrecord in Wrecords)
                                        {
                                            wrecord.WValue = Math.Round(wrecord.WValue * 0.0625, 2);
                                            StorageManager.UpdateData<Weight>(wrecord);
                                        }
                                    }
                                }
                            }
                            Aweighttype = user.WeightType;
                        }
                        //  var user = StorageManager.GetConnectionInfo().LoginUser;
                        user.UpdateDate = DateTime.UtcNow;
                      //  int heightType = heights[fHeightPicker.Items[fHeightPicker.SelectedIndex]];
                      //  int genderType = genders[fSexPicker.Items[fSexPicker.SelectedIndex]];
                      //  user.FirstName = fFirstNameEntry.Text;
                      //  user.LastName = fSurNameEntry.Text;
                      //  user.Birthdate = fbirthDatePicker.Date;
                      //  user.Gender = genderType;
                      //  user.HeightType = heightType;
                      //  user.Height = Convert.ToDouble(fHeightEntry.Text);
                      //  user.Skeleton = Convert.ToDouble(fSkeletonEntry.Text);
                      //  user.Location = fLocationEntry.Text;                        
                      StorageManager.UpdateData(user);
                      RefreshPage();
                    }
                    else
                    {
                        ProfileBtn.Text = Properties.LangResource.save;
                    }
                }
            }
        }

        private void RefreshPage()
        {
            fFirstNameEntry.IsEnabled = false;
            fSurNameEntry.IsEnabled = false;
            fEmailEntry.IsEnabled = false;
            fbirthDatePicker.IsEnabled = false;
            fSexPicker.IsEnabled = false;
            fHeightPicker.IsEnabled = false;
            fWeightPicker.IsEnabled = false;
            fDietTypePicker.IsEnabled = false;
            fHeightEntry.IsEnabled = false;
            fSkeletonEntry.IsEnabled = false;
            fLocationEntry.IsEnabled = false;
            btnTA.IsVisible = false;
            btnPA.IsVisible = false;
            btnTB.IsVisible = false;
            btnPB.IsVisible = false;
        }
                
        private void TakePhotoButtonOnClickedB(object sender, EventArgs e)
        {
            TakeImage(1);
        }

        private void TakePhotoButtonOnClickedA(object sender, EventArgs e)
        {
            TakeImage(2);
        }

        private async void TakeImage(int mode)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert(Properties.LangResource.warning, Properties.LangResource.nocamera, "OK");
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

            ResizeAndSetImg(file,mode);
        }

        private void ResizeAndSetImg(MediaFile file,int mode) {
            byte[] bitmapData;
            var stream = new MemoryStream();
            file.GetStream().CopyTo(stream);
            bitmapData = stream.ToArray();
            var resizer = DependencyService.Get<IImageResizer>();
            var resizedBytes= resizer.ResizeImage(bitmapData, resizer.FdpToPixWidth(260), resizer.FdpToPixHeight(300));
            Stream Astream = new MemoryStream(resizedBytes);
            if (mode == 1)
            {
                BeforeImage.Source = ImageSource.FromStream(() => { return Astream; });
                user.ImageBefore = resizedBytes;
            }
            else
            {
                AfterImage.Source = ImageSource.FromStream(() => { return Astream; });
                user.ImageAfter = resizedBytes;
            }
        }

        private async void PickImage(int mode) {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert(Properties.LangResource.warning, Properties.LangResource.photoerror, "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;

            ResizeAndSetImg(file, mode);
        }

        private void PickPhotoButtonOnClickedB(object sender, EventArgs e)
        {
            PickImage(1);
        }

        private void PickPhotoButtonOnClickedA(object sender, EventArgs e)
        {
            PickImage(2);
        }

        public void OnUploadClicked(object sender, EventArgs e)
        {
            if (isUploading)
            {
                DisplayAlert(Properties.LangResource.uploading, Properties.LangResource.uploadaction, "OK");
                return;
            }
            isUploading = true;

            if (user.ImageBefore != null)
            {
                uploadImage(user.ImageBefore, "before");
            }
            if (user.ImageAfter != null)
            {
                uploadImage(user.ImageAfter, "after");
            }
        }

        async void ChangePassword(object sender, EventArgs e)
        {            
            await Navigation.PushAsync(changePasswordPageDt);            
        }            

        private async void uploadImage(byte[] bitmapData, string type)
        {
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
                    var notifier = DependencyService.Get<ICrossLocalNotifications>().CreateLocalNotifier();
                    notifier.Notify(new LocalNotification()
                    {
                        Title = Properties.LangResource.profileimages,
                        Text = Properties.LangResource.photossharing,
                        Id = 10001,
                        NotifyTime = DateTime.Now,
                    });
                    isUploading = false;
                    //await DisplayAlert("Message", "Image uploaded", "OK");
                    //numMsg = numMsg + 1;
                    //UploadMsg.Text = "uploading image " + numMsg;
                }
                else
                {
                    isUploading = false;
                    await DisplayAlert(Properties.LangResource.message, Properties.LangResource.error + " - " + res["message"].ToString() + " errorcode:" + res["errorcode"].ToString(), "OK");
                }
            }
        }

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
                DisplayAlert(Properties.LangResource.please, Properties.LangResource.fillfields, "OK");
                return false;
            }            
            return true;            
        }
    }
}
