using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace DianetMob.DB.Entities
{
    public class Alert : Model
    {
        private DateTime alerttime;
        private int mealtype;
        private int status;
               
        [PrimaryKey, AutoIncrement]
        public int IDAlert { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int IDServer { get; set; }

        public DateTime AlertTime
        {
            get
            {
                return alerttime;
            }
            set
            {
                if (alerttime != value)
                {
                    alerttime = value;
                    OnPropertyChanged("AlertTime");
                }
            }
        }

        public int MealType
        {
            get
            {
                return mealtype;
            }
            set
            {
                if (mealtype != value)
                {
                    mealtype = value;
                    OnPropertyChanged("MealType");
                }
            }
        }

        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Alert()
        {
            IDServer = 0;
            alerttime = DateTime.Now;
        }        

        public override string ToString()
        {
            string str = "";

            str += "&idserver=\"" + IDServer.ToString() + "\"";

            if (IDUser != -1)
                str += "&iduser=\"" + IDUser.ToString() + "\"";
            if(AlertTime != null)
                str += "&alerttime=\"" + AlertTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (Status != -1)
                str += "&status=\"" + Status.ToString() + "\"";
            if (MealType != -1)
                str += "&mealtype=\"" + MealType.ToString() + "\"";
            if (InsertDate != null)
                str += "&insertdate=\"" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (UpdateDate != null)
                str += "&updatedate=\"" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";

            return str.Substring(1);            
        }        
    }
}
