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
        private DateTime weightDate;
        private DateTime propertyMinimumDate;

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

        public DateTime WeightDate
        {
            get
            {
                return weightDate;
            }
            set
            {
                if (weightDate != value)
                {
                    weightDate = value;
                    OnPropertyChanged("WeightDate");
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

        public DateTime InsertDate { get;set; }

        public DateTime UpdateDate { get; set; }

        public Weight()
        {
            IDServer = -1;
        }

    }
}
