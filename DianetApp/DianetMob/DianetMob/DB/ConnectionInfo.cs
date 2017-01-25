using DianetMob.DB.Entities;
using System;
using System.Collections.Generic;

namespace DianetMob.DB
{
    public class ConnectionInfo
    {
        public User LoginUser { get; set; }
        public Subscription ActiveSubscription { get; set; }

        private Settings settings;
        private UserSettings userSettings;

        public bool isTrial
        {
            get
            {
                return (LoginUser.InsertDate.AddDays(settings.TrialPeriod) > DateTime.UtcNow);
            }
        }

        public Subscription LoadActiveSubscription()
        {
            ActiveSubscription = null;
            List<Subscription> subs = StorageManager.GetConnection().Query<Subscription>("SELECT * FROM subscription WHERE iduser=" + LoginUser.IDUser.ToString() + " and isactive=1 order by enddate desc limit 1");
            if (subs.Count > 0)
            {
                ActiveSubscription = subs[0];
            }
            return ActiveSubscription;
        }

        public UserSettings UserSettings
        {
            get
            {
                if (userSettings == null)
                {
                    userSettings = StorageManager.GetConnection().Find<UserSettings>(LoginUser.IDUser);
                    if (userSettings == null)
                    {
                        userSettings = new UserSettings();
                        userSettings.IDUser = LoginUser.IDUser;
                        userSettings.LastSyncDate = DateTime.MinValue;
                        StorageManager.InsertData(userSettings);
                    }
                }
                return userSettings;
            }
            set
            {
                this.userSettings = value;
            }
        }

        public Settings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = StorageManager.GetConnection().Find<Settings>(1);
                    if (settings.LastLoggedIn > 0)
                        LoginUser = StorageManager.GetConnection().Find<User>(settings.LastLoggedIn);
                }
                return settings;
            }
            set
            {
                settings = value;
            }
        }
    }
}
