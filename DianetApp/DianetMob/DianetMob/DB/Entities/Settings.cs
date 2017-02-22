using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class Settings : Model
    {
        [PrimaryKey]
        public int IDSettings { get; set; }

        public int TrialPeriod { get; set; }

        public int RemindWeight { get; set; }

        public int LastLoggedIn { get; set; }

        private int lang;
        public int Lang
        {
            get
            {
                return lang;
            }
            set
            {
                lang = value;
                if (lang == 1)
                {
                    if ((currentCulture == null) || (currentCulture.Name.ToLower().Equals("en-GB")))
                    {
                        currentCulture = new CultureInfo("el-gr");
                        CultureInfo.DefaultThreadCurrentUICulture = currentCulture;
                    }
                }
                else if (lang == 2)
                {
                    if ((currentCulture == null) || (currentCulture.Name.ToLower().Equals("el-gr")))
                    {
                        currentCulture = new CultureInfo("en-GB");
                        CultureInfo.DefaultThreadCurrentUICulture = currentCulture;
                    }

                }
            }
        }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        private CultureInfo currentCulture;

        public CultureInfo GetCurrentCulture() {
                return currentCulture;
        }

        public Settings()
        {
            Lang = 0;
            LastLoggedIn = 0;
            currentCulture = null;
        }
    }
}
