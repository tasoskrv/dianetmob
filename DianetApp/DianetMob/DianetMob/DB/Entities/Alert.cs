using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class Alert : Model
    {
        private DateTime alerttime;
        private string recurrence;
        private string description;
                
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

        public string Recurrence
        {
            get
            {
                return recurrence;
            }
            set
            {
                if (recurrence != value)
                {
                    recurrence = value;
                    OnPropertyChanged("Recurrence");
                }
            }
        }

        [MaxLength(500)]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public int Deleted { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Alert()
        {
            IDServer = 0;
            Deleted = 0;
            alerttime = DateTime.UtcNow;
        }        

        public override string ToString()
        {
            string str = "";

            str += "&idserver=" + IDServer.ToString();

            if (IDUser != -1)
                str += "&iduser=" + IDUser.ToString();
            if(AlertTime != null)
                str += "&alerttime=" + AlertTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (!Recurrence.Equals(""))
                str += "&recurrence=" + Recurrence.ToString();
            if (!Description.Equals(""))
                str += "&description=" + Description.ToString();
            if (InsertDate != null)
                str += "&insertdate=" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss");
            if (UpdateDate != null)
                str += "&updatedate=" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss");

            return str.Substring(1);            
        }        
    }
}
