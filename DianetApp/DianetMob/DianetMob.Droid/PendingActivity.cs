using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using DianetMob.Pages;

namespace DianetMob.Droid
{
    [Activity(Label = "PendingActivity")]
    public class PendingActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string extraData = Intent.GetStringExtra(MainActivity.ACTIVITY_NOTIF);
            if (extraData != null)
                MessagingCenter.Send<MainPage, string>((MainPage)App.Current.MainPage, "NotificationAction", extraData);
            Finish();
        }
    }
}