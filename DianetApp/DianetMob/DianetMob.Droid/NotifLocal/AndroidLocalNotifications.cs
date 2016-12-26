using Dianet.Notification;
using DianetMob;
using DianetMob.Droid.NotifLocal;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidLocalNotifications))]
namespace DianetMob.Droid.NotifLocal
{
    /// <summary>
    /// Cross platform LocalNotifications implementation
    /// </summary>
    public class AndroidLocalNotifications : ICrossLocalNotifications
    {
        public AndroidLocalNotifications() { }
        public ILocalNotifier CreateLocalNotifier()
        {
            return new LocalNotifier();
        }
    }
}
