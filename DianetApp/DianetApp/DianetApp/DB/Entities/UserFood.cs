using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetApp.DB.Entities
{
    public class UserFood : Model
    {
        private string name;
        private string description;

        [PrimaryKey, AutoIncrement]
        public int IDUserFood { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int IDServer { get; set; }

        [MaxLength(45)]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
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

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public UserFood()
        {
            IDServer = -1;
        }

        public override string ToString()
        {
            string str = "";

            str += "&idserver=" + IDServer.ToString();

            if (IDUser != -1)
                str += "&iduser=" + IDUser.ToString();
            if (!Name.Equals(""))
                str += "&name=" + Name.ToString();
            if (!Description.Equals(""))
                str += "&description=" + Description.ToString();
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
