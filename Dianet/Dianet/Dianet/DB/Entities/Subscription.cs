using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Subscription
    {
        [PrimaryKey]
        public int IDSubscription { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        [ForeignKey(typeof(Package))]
        public int IDPackage { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
