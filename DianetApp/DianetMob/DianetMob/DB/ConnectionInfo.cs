using DianetMob.DB.Entities;
using SQLite;
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
        private SQLiteConnection conn;

        public ConnectionInfo() {
            conn=StorageManager.GetConnection();
            isTrialShown = false;
        }

        public bool isTrial
        {
            get
            {
                return (LoginUser.InsertDate.AddDays(settings.TrialPeriod) > DateTime.UtcNow);
            }
        }
        public bool isTrialShown { get; set; }

        public Subscription LoadActiveSubscription()
        {
            ActiveSubscription = null;
            List<Subscription> subs = conn.Query<Subscription>("SELECT * FROM subscription WHERE iduser=" + LoginUser.IDUser.ToString() + " and isactive=1 order by enddate desc limit 1");
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
                    userSettings = conn.Find<UserSettings>(LoginUser.IDUser);
                    if (userSettings == null)
                    {
                        userSettings = new UserSettings();
                        userSettings.IDUser = LoginUser.IDUser;
                        userSettings.LastSyncDate = DateTime.MinValue;
                        StorageManager.InsertData<UserSettings>(userSettings);
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
                    settings = conn.Find<Settings>(1);
                    if (settings.LastLoggedIn > 0)
                        LoginUser = conn.Find<User>(settings.LastLoggedIn);
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
