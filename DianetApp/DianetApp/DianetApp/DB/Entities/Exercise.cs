using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetApp.DB.Entities
{
    public class Exercise : Model
    {
        [PrimaryKey, AutoIncrement]
        public int IDExercise { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int IDServer { get; set; }

        public int Minutes { get; set; }

        public DateTime TrainDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Exercise()
        {
            IDServer = -1;
        }

        public override string ToString()
        {
            string str = "";

            str += "&idserver=" + IDServer.ToString();

            if (IDUser != -1)
                str += "&iduser=" + IDUser.ToString();
            if (Minutes != 0)
                str += "&minutes=" + Minutes;
            if (TrainDate != null)
                str += "&traindate=" + TrainDate.ToString("yyyy-MM-dd HH:mm:ss");
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
