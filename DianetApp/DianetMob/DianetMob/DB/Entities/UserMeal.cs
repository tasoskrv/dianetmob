using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class UserMeal : Model
    {
        [PrimaryKey, AutoIncrement]
        public int IDUserMeal { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        [ForeignKey(typeof(MealUnit))]
        public int IDMealUnit { get; set; }

        [Indexed(Name = "IDServerUserMeal", Order = 1)]
        public int IDServer { get; set; }

        public int IDCategory { get; set; }

        public double Qty { get; set; }

        public DateTime MealDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public UserMeal()
        {
            IDServer = -1;
        }

        public override string ToString()
        {
            string str = "";

            str += "&idserver=" + IDServer.ToString();

            if (IDUser != -1)
                str += "&iduser=" + IDUser.ToString();
            if (IDCategory != 0)
                str += "&idcategory=" + IDCategory.ToString();
            if (IDMealUnit != 0)
                str += "&idmealunit=" + IDMealUnit.ToString();
            if (Qty != 0)
                str += "&qty=" + Qty.ToString();
            if (MealDate != null)
                str += "&mealdate=" + MealDate.ToString("yyyy-MM-dd HH:mm:ss");
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
