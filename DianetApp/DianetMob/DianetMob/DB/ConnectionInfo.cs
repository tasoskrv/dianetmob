using DianetMob.DB.Entities;
using System;

namespace DianetMob.DB
{
    public class ConnectionInfo
    {
        public User LoginUser { get; set; }

        private Settings settings;
        private UserSettings userSettings;

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
