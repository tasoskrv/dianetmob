using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Subscription: Model
    {
        [Indexed(Name = "IDSubscription_PK", Order = 1)]
        public int IDSubscription { get; set; }

        [Indexed(Name = "IDSubscription_IDUser_PK", Order = 2), ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        [ForeignKey(typeof(Package))]
        public int IDPackage { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
