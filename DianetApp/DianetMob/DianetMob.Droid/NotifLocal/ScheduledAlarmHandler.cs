using System;
using System.IO;
using System.Xml.Serialization;
using Android;
using Android.App;
using Android.Content;
using Android.Runtime;
using Dianet.Notification;
using Android.Media;

namespace DianetMob.Droid.NotifLocal
{
    [BroadcastReceiver]
    public class ScheduledAlarmHandler : BroadcastReceiver
    {
        public const string LocalNotificationKey = "LocalNotification";

        public override void OnReceive(Context context, Intent intent)
        {
            var extra = intent.GetStringExtra(LocalNotificationKey);
            var notification = serializeFromString(extra);

            var nativeNotification = createNativeNotification(notification);
            var manager = getNotificationManager();

            manager.Notify(notification.Id, nativeNotification);
        }

        private NotificationManager getNotificationManager()
        {
            var notificationManager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            return notificationManager;
        }

        private Notification createNativeNotification(LocalNotification notification)
        {
            var activeContext = Application.Context.ApplicationContext;

            Intent intent = new Intent(activeContext, typeof(PendingActivity))
                .PutExtra(MainActivity.ACTIVITY_NOTIF, notification.Id.ToString());

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(activeContext, pendingIntentId, intent, PendingIntentFlags.OneShot);


            var builder = new Notification.Builder(Application.Context)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Text)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetSmallIcon(Application.Context.ApplicationInfo.Icon);
            

            var nativeNotification = builder.Build();
            nativeNotification.Flags |= NotificationFlags.AutoCancel;
            return nativeNotification;
        }

        private LocalNotification serializeFromString(string notificationString)
        {
            var xmlSerializer = new XmlSerializer(typeof(LocalNotification));
            using (var stringReader = new StringReader(notificationString))
            {
                var notification = (LocalNotification)xmlSerializer.Deserialize(stringReader);
                return notification;
            }
        }
    }
}