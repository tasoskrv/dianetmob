using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace DianetMob.DB.Entities
{
    public class Exercise : Model
    {
        private int minutes;

        [PrimaryKey, AutoIncrement]
        public int IDExercise { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int IDServer { get; set; }

        public int Minutes
        {
            get
            {
                return minutes;
            }
            set
            {
                if (minutes != value)
                {
                    minutes = value;
                    OnPropertyChanged("Minutes");
                }
            }
        }

        public DateTime TrainDate { get; set; }

        public int Deleted { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Exercise()
        {
            IDServer = 0;
            Deleted = 0;
        }

        public override string ToString()
        {
            string str = "";

            str += "&idserver=\"" + IDServer.ToString() + "\"";

            if (IDUser != -1)
                str += "&iduser=\"" + IDUser.ToString() + "\"";
            if (Minutes != 0)
                str += "&minutes=\"" + Minutes + "\"";
            if (TrainDate != null)
                str += "&traindate=\"" + TrainDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (InsertDate != null)
                str += "&insertdate=\"" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (UpdateDate != null)
                str += "&updatedate=\"" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";

            return str.Substring(1);
        }


    }
}
