using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.Utils
{
    public class PointSystem
    {

        private User user;

        public PointSystem()
        {

            user = StorageManager.GetConnectionInfo().LoginUser;

        }
        //----------Ypologismos BMR-------------------------
        public static double BMRCalculation(User user)
        {
            if (user.Gender == 1)
            {
                return 66.47 + (13.75 * user.Weight) + (5.003 * user.Height) - (6.755 * user.Age);
            }    
            return 655.1 + (9.563 * user.Weight) + (1.850 * user.Height) - (4.676 * user.Age);
        }


        public static double PointCalculate(double cal)
        {
            return Math.Round(cal / 80, 1);
        }


        //----Ypologismos thermidwn 
        //     TODO: pws eisagw entasi askisis!!!
        public static double DayPointCalculate(int exercise=1)
        {
            User user = StorageManager.GetConnectionInfo().LoginUser;
            double Cal = 0;
            double BMR = BMRCalculation(user);
            switch (exercise)
            {
                case 1:
                    Cal = BMR * 1.2;
                    break;
                case 2:
                    Cal = BMR * 1.3;
                    break;
                case 3:
                    Cal = BMR * 1.375;
                    break;
                case 4:
                    Cal = BMR * 1.55;
                    break;
                case 5:
                    Cal = BMR * 1.725;
                    break;
                case 6:
                    Cal = BMR * 1.9;
                    break;

            }

            if (user.Goal > user.Weight)
                Cal = Cal + 500;
            else if (user.Goal < user.Weight)
                Cal = Cal - 500;


            return PointCalculate(Cal); 
        }



        //----Ypologismos Deikti mazas swmatos
        public static double DMSCalculate(User user)
        {
            return Math.Round(user.Weight / Math.Pow(user.Height, 2), 1);
        }

    }
}
