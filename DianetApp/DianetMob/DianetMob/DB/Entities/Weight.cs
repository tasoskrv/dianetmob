using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class Weight : Model
    {
        private int weight;
        private DateTime weightDate;

        [PrimaryKey, AutoIncrement]
        public int IDWeight { get; set; }

        [MaxLength(45)]
        public string AccessToken { get; set; }

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

        public int Deleted { get; set; }

        public DateTime InsertDate { get;set; }

        public DateTime UpdateDate { get; set; }

        public Weight()
        {
            IDServer = 0;
            Deleted = 0;
            weightDate = DateTime.UtcNow;
        }
        
        public override string ToString()
        {
            string str = "";

            str += "&idserver=" + IDServer.ToString();

            if (IDUser != -1)
                str += "&iduser=" + IDUser.ToString();
            if(WValue != 0)
                str += "&weight=" + WValue.ToString();
            if(WeightDate != null)
                str += "&weightdate=" + WeightDate.ToString("yyyy-MM-dd HH:mm:ss");
            if (InsertDate != null)
                str += "&insertdate=" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss");
            if (UpdateDate != null)
                str += "&updatedate=" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss");

            return str.Substring(1);            
        }
        

    }
}
