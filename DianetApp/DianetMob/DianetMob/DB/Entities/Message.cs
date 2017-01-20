using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class Message : Model
    {
        [PrimaryKey, AutoIncrement]
        public int IDMessage { get; set; }

        public int IDServer { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public string Title { get; set; }

        public string MessageText { get; set; }

        public int seen { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
        
        public Message()
        {
            IDServer = 0;
        }

        public override string ToString()
        {
            string str = "";

            str += "&idserver=\"" + IDServer.ToString() + "\"";

            if (IDUser != -1)
                str += "&iduser=\"" + IDUser.ToString() + "\"";
            if (!Title.Equals(""))
                str += "&title=\"" + Title + "\"";
            if (!MessageText.Equals(""))
                str += "&messagetext=\"" + MessageText + "\"";
            str += "&seen=" + seen.ToString() + "\"";
            if (InsertDate != null)
                str += "&insertdate=\"" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (UpdateDate != null)
                str += "&updatedate=\"" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";

            return str.Substring(1);
        }


    }
}
