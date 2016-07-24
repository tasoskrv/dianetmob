using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Alert
    {
        [PrimaryKey]
        public int IDAlert { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public DateTime AlertTime { get; set; }

        public int Reccurence { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

    }
}
