using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetApp.DB.Entities
{
    public class UserSettings
    {
        [PrimaryKey]
        public int IDUser { get; set; }

        public DateTime LastSyncDate { get; set; }
    }
}
