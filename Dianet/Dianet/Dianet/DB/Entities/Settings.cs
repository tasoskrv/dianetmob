using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Settings
    {
        [PrimaryKey]
        public int IDSettings { get; set; }

        public int TrialPeriod { get; set; }

        public int RemindWeight { get; set; }

        public int LastLoggedIn { get; set; }
    }
}
