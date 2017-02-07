using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class Plan : Model
    {
        private double goal;
        private DateTime startgoal;

        [PrimaryKey, AutoIncrement]
        public int IDPlan { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int IDServer { get; set; }

        public double Goal
        {
            get
            {
                return goal;
            }
            set
            {
                if (goal != value)
                {
                    goal = value;
                    OnPropertyChanged("Goal");
                }
            }
        }
        
        public DateTime StartGoal
        {
            get
            {
                return startgoal;
            }
            set
            {
                if (startgoal != value)
                {
                    startgoal = value;
                    OnPropertyChanged("StartGoal");
                }
            }
        }
        
        public int Deleted { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Plan()
        {
            IDServer = 0;
            Deleted = 0;
        }

        public override string ToString()
        {
            string str = "";

            str += "&idserver= \"" + IDServer.ToString() + "\"";

            if (IDUser != -1)
                str += "&iduser= \"" + IDUser.ToString() + "\"";
            if (Goal != 0)
                str += "&goal= \"" + Goal.ToString() + "\"";
            if (StartGoal != null)
                str += "&startgoal= \"" + StartGoal.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (InsertDate != null)
                str += "&insertdate= \"" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (UpdateDate != null)
                str += "&updatedate= \"" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";

            return str.Substring(1);
        }


    }
}
