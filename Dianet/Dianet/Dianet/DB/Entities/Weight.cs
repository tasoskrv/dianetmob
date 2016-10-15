using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Weight: Model
    {
        [Indexed(Name = "IDWeight_PK", Order = 1)]
        public int IDWeight { get; set; }

        [Indexed(Name = "IDWeight_IDUser_PK", Order = 2), ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int WValue { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
