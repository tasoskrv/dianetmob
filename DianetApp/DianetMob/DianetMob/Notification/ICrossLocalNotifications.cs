using System;

namespace Dianet.Notification
{
  /// <summary>
  /// Cross platform LocalNotifications implementation
  /// </summary>
  public interface ICrossLocalNotifications
  {

       ILocalNotifier CreateLocalNotifier();
   
   }
}
