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
using DianetMob.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using Xamarin.Auth;
using DianetMob.Pages;

[assembly: ExportRenderer(typeof(LoginPageFacebook), typeof(LoginPageRenderer))]
namespace DianetMob.Droid.Renderers
{
    public class LoginPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            var accounts = AccountStore.Create().FindAccountsForService("Dianet");
            var account = accounts.FirstOrDefault();
            if (account == null)
            {
                var activity = this.Context as Activity;

                var auth = new OAuth2Authenticator(
                    clientId: "111858092648076", // your OAuth2 client id
                    scope: "email", // the scopes for the particular API you're accessing, delimited by "+" symbols
                    authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"), // the auth URL for the service
                    redirectUrl: new Uri("http://dianet.cloudocean.gr/")); // the redirect URL for the service

                auth.Completed += (sender, eventArgs) =>
                {
                    if (eventArgs.IsAuthenticated)
                    {
                        PerformLogin(eventArgs.Account);
                        AccountStore.Create().Save(eventArgs.Account, "Dianet");
                    }
                    else
                    {
                        AccountStore.Create().Delete(eventArgs.Account, "Dianet");
                    }
                };
                activity.StartActivity(auth.GetUI(activity));
            }
            else {
                PerformLogin(account);
            }
        }
        
        private void PerformLogin(Account account) {
            var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=email,first_name,last_name"), null, account);
            request.GetResponseAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                    MessagingCenter.Send<LoginPageFacebook, string>((LoginPageFacebook)App.Current.MainPage, "LoginToFacebook", "error");
                else
                {
                    string json = t.Result.GetResponseText();
                    MessagingCenter.Send<LoginPageFacebook,string>((LoginPageFacebook)App.Current.MainPage, "LoginToFacebook", json);
                }
            });

        } 
    }
}