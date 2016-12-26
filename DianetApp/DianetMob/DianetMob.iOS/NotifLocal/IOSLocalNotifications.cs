using Dianet.Notification;
using DianetMob.iOS.NotifLocal;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSLocalNotifications))]
namespace DianetMob.iOS.NotifLocal
{

    public class IOSLocalNotifications : ICrossLocalNotifications
    {
        public IOSLocalNotifications() { }
        public ILocalNotifier CreateLocalNotifier()
        {
            return new LocalNotifier();
        }
    }
}

