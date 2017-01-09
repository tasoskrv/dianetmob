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
        private DateTime goaldate;

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

        public DateTime GoalDate
        {
            get
            {
                return goaldate;
            }
            set
            {
                if (goaldate != value)
                {
                    goaldate = value;
                    OnPropertyChanged("GoalDate");
                }
            }
        }
       
        public int UserStep { get; set; }

        public int PlanType { get; set; }

        public int Deleted { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Plan()
        {
            IDServer = 0;
            Deleted = 0;
            goaldate = DateTime.UtcNow;
        }

        public override string ToString()
        {
            string str = "";

            str += "&idserver=" + IDServer.ToString();

            if (IDUser != -1)
                str += "&iduser=" + IDUser.ToString();
            if (Goal != 0)
                str += "&goal=" + Goal.ToString();
            if (GoalDate != null)
                str += "&goaldate=" + GoalDate.ToString("yyyy-MM-dd HH:mm:ss");
            if (InsertDate != null)
                str += "&insertdate=" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss");
            if (UpdateDate != null)
                str += "&updatedate=" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss");

            /*
            if (!AccessToken.Equals(""))
                str += "&accesstoken=" + Uri.EscapeDataString(AccessToken);
            */
            return str.Substring(1);
        }


    }
}
