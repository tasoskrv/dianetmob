using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Weight
    {
        [PrimaryKey]
        public int IDWeight { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int WValue { get; set; }

        public DateTime InsertDate { get; set; }

        public int WeightType { get; set; }

    }
}
