using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetApp.DB.Entities
{
    public class Weight : Model
    {
        private int weight;
        private DateTime? insertDate;

        [PrimaryKey, AutoIncrement]
        public int IDWeight { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int IDServer { get; set; }

        public int WValue
        {
            get
            {
                return weight;
            }
            set
            {
                if (weight != value)
                {
                    weight = value;
                    OnPropertyChanged("WValue");
                }
            }
        }

        public DateTime? InsertDate
        {
            get
            {
                if (insertDate == null)
                    insertDate = DateTime.Now;
                return insertDate;
            }
            set
            {
                if (insertDate != value)
                {
                    insertDate = value;
                    OnPropertyChanged("InsertDate");
                }
            }
        }

        public DateTime UpdateDate { get; set; }

        public Weight()
        {
            IDServer = -1;
        }

    }
}
