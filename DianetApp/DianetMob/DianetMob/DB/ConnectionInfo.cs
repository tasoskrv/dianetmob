using DianetMob.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    this.userSettings = StorageManager.GetConnection().Find<UserSettings>(LoginUser.IDUser);
                    if (userSettings == null)
                    {
                        userSettings = new UserSettings();
                        userSettings.IDUser = LoginUser.IDUser;
                        userSettings.LastSyncDate = DateTime.MinValue;
                        StorageManager.InsertData(userSettings);
                    }
                }
                return this.userSettings;
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
                    this.settings = StorageManager.GetConnection().Find<Settings>(1);
                    if (this.settings.LastLoggedIn > 0)
                        this.LoginUser = StorageManager.GetConnection().Find<User>(this.settings.LastLoggedIn);
                }
                return this.settings;
            }
            set
            {
                this.settings = value;
            }
        }
    }
}
