using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Exercise
    {
        [Indexed(Name = "IDExercise_PK", Order = 1)]
        public int IDExercise { get; set; }
        
        [Indexed(Name = "IDExercise_IDUser_PK", Order = 2), ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int Minutes { get; set; }

        public DateTime TrainDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
