using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Alert: Model
    { 
        [PrimaryKey, AutoIncrement]
        public int IDAlert { get; set; }

        [PrimaryKey, ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int IDServer { get; set; }

        public DateTime AlertTime { get; set; }

        public int Reccurence { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
