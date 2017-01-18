using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class Subscription : Model
    {
        [PrimaryKey, AutoIncrement]
        public int IDSubscription { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

       // [ForeignKey(typeof(Package))]
       // public int IDPackage { get; set; }

        public int IDServer { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Price { get; set; }

        public int IsActive { get; set; }

        public string Trncode { get; set; }

        public int Deleted { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Subscription()
        {
            IDServer = 0;
            Deleted = 0;
        }

        public override string ToString()
        {
            string str = "";

            str += "&idserver=" + IDServer.ToString();

            if (IDUser != -1)
                str += "&iduser=" + IDUser.ToString();
            if (BeginDate != null)
                str += "&begindate='" + BeginDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            if (EndDate != null)
                str += "&enddate='" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            if (Price != 0)
                str += "&price=" + Price.ToString();
            if (IsActive != 0)
                str += "&isactive=" + IsActive.ToString();
            if (InsertDate != null)
                str += "&insertdate='" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            if (UpdateDate != null)
                str += "&updatedate='" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            return str.Substring(1);
        }

    }
}
