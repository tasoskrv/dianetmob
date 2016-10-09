using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class UserFood
    {
        [Indexed(Name = "IDUserFood_PK", Order = 1)]
        public int IDUserFood { get; set; }

        [Indexed(Name = "IDUserFood_IDUser_PK", Order = 2), ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        [MaxLength(45)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
