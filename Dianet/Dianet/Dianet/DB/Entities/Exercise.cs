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
        [PrimaryKey]
        public int IDExercise { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int Minutes { get; set; }

        public DateTime TrainDate { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
