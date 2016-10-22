using Dianet.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB
{
    public class ConnectionInfo
    {
        public User LoginUser { get; set; }
        public Plan LoginUserPlan { get; set; }
        public Weight LoginUserWeight { get; set; }

        private Settings settings;
        public Settings Settings {
            get {
                if (settings == null) {
                    this.settings = StorageManager.GetConnection().Find<Settings>(1);
                    if (this.settings.LastLoggedIn>0)
                    this.LoginUser= StorageManager.GetConnection().Find<User>(this.settings.LastLoggedIn);
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
