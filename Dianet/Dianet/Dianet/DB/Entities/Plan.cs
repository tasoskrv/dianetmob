using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Plan: Model
    {
        [PrimaryKey, AutoIncrement]
        public int IDPlan { get; set; }

        [PrimaryKey, ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int IDServer { get; set; }

        public double Goal { get; set; }

        public DateTime GoalDate { get; set; }

        public int UserStep { get; set; }

        public int PlanType { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
