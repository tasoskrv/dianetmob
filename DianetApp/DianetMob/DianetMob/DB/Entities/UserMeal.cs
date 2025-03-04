﻿using SQLite;
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
        private int idservermealunit = 0;

        [PrimaryKey, AutoIncrement]
        public int IDUserMeal { get; set; }

        public int IDServer { get; set; }

        [ForeignKey(typeof(MealUnit))]
        public int IDMealUnit { get; set; }

        [Ignore]
        public int IDServerMealUnit
        {
            get
            {
                if (idservermealunit == 0)
                    idservermealunit = StorageManager.GetConnection().Query<MealUnit>("select idserver from mealunit where idmealunit=" + IDMealUnit.ToString())[0].IDServer;
                return idservermealunit;
            }
            set
            {
                idservermealunit = value;
                if (IDMealUnit == 0)
                    IDMealUnit = StorageManager.GetConnection().Query<MealUnit>("select idmealunit from mealunit where idserver=" + idservermealunit.ToString())[0].IDMealUnit;
            }
        }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }
                
        public int IDCategory { get; set; }

        public double Qty { get; set; }

        public DateTime MealDate { get; set; }

        public int Deleted { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public UserMeal()
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
            if (IDCategory != 0)
                str += "&idcategory=\"" + IDCategory.ToString() + "\"";
            if (IDMealUnit != 0)
                str += "&idmealunit=\"" + IDServerMealUnit.ToString() + "\"";
            if (Qty != 0)
                str += "&qty=\"" + Qty.ToString() + "\"";
            if (MealDate != null)
                str += "&mealdate=\"" + MealDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (InsertDate != null)
                str += "&insertdate=\"" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (UpdateDate != null)
                str += "&updatedate=\"" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";

            return str.Substring(1);
        }

    }
}
