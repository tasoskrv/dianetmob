using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Media;
using Android.Content;
using Dianet.Droid.Services;

namespace DianetMob.Droid
{
    [Activity(Label = "DianetMob", Icon = "@drawable/logo", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance;
        public static string ACTIVITY_NOTIF = "ACTIVITY_NOTIF";
        protected override async void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            MainActivity.Instance = this;
            await CrossMedia.Current.Initialize();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            // Ask the in-app purchasing service connection's billing handler to process this request
            InAppService inAppService = App.ViewModel.TheInAppService as InAppService;
            inAppService.HandleActivityResult(requestCode, resultCode, data);
        }

        protected override void OnDestroy()
        {
            // Disconnect from the in-app purchasing service
            InAppService inAppService = App.ViewModel.TheInAppService as InAppService;
            inAppService.OnDestroy();

            base.OnDestroy();
        }
    }
}

