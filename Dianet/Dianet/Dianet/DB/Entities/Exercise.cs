using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Exercise: Model
    {
        [PrimaryKey, AutoIncrement]
        public int IDExercise { get; set; }
        
        [PrimaryKey, ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int IDServer { get; set; }

        public int Minutes { get; set; }

        public DateTime TrainDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
