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
        [Indexed(Name = "IDPlan_PK", Order = 1)]
        public int IDPlan { get; set; }

        [Indexed(Name = "IDPlan_IDUser_PK", Order = 2), ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public double Goal { get; set; }

        public DateTime GoalDate { get; set; }

        public int UserStep { get; set; }

        public int PlanType { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
