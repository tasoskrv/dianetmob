using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetApp.DB.Entities
{
    public class Alert : Model
    {
        private DateTime alerttime;
        private string recurrence;
        private string description;
        private DateTime propertyMinimumDate;

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

        [Ignore]
        public DateTime PropertyMinimumDate
        {
            get
            {
                return DateTime.Now;
            }
            set
            {
                propertyMinimumDate = value;
            }
        }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Alert()
        {
            IDServer = -1;
        }        
    }
}
