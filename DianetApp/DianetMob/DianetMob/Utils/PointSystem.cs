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
    class PointSystem
    {
        private double weight = 0;
        private int wrist = 0;
        private int age = 0;
        private int genre = -1;
        private double goal = 0;
        private int DT;
        private double BMR = 0;
        private double Cal = 0;
        private double FinalCal = 0;

        public double Weight
        {
            get
            {   if(weight==0)
                    weight = StorageManager.GetConnection().Query<User>("select WValue from Weight where IDUser=" + StorageManager.GetConnectionInfo())[0].IDUser;
                else 
                    weight = StorageManager.GetConnectionInfo().LoginUser.Weight;
                return weight;
            }
            set
            {
                weight = value;
            }           
        }

        public int Wrist
        {
            //get
            //{
                //if (wrist == 0)
                  //  wrist = StorageManager.GetConnection().Query<User>("select WValue from Weight where IDUser=" + StorageManager.GetConnectionInfo())[0].IDUser;
                //return wrist;
            //}
            set
            {
                wrist = value;
            }
        }

        public double Height
        {
            get
            {   
                return StorageManager.GetConnectionInfo().LoginUser.Height;
            }
        }

        public int Age
        {
            get
            {
                if (age == 0)
                {
                    DT = StorageManager.GetConnection().Query<User>("select Birthdate from User where IDUser=" + StorageManager.GetConnectionInfo())[0].IDUser;
                }
                    return age;
            }
            set
            {
                age = value;
            }
        }

        public int Genre
        {
            get
            {
                if (genre == -1)
                    genre = StorageManager.GetConnectionInfo().LoginUser.Gender;
                return genre;
            }
            
        }

        public double Goal
        {
            get
            {
                if (goal == 0)
                    goal = StorageManager.GetConnection().Query<Plan>("select Goal from Weight where IDUser=" + StorageManager.GetConnectionInfo())[0].IDUser;
                else
                    goal = StorageManager.GetConnectionInfo().LoginUser.Goal;
                return goal;
            }
            set
            {
                goal = value;
            }
        }

        public PointSystem()
        {
            wrist = 0;

        }
        //----------Ypologismos BMR-------------------------
        public double BMRCalculation()
        {
            if (Genre == 1)
            {
                BMR= 66.47 + (13.75 * Weight) + (5.003 * Height) - (6.755 * Age); 
            }
            else if (Genre ==2) 
            {
                BMR = 655.1 + (9.563 * Weight) + (1.850 * Height) - (4.676 * Age);
            }
            return BMR;
        }
        
        
        //----Ypologismos thermidwn 
        //     TODO: pws eisagw entasi askisis!!!
        public double PointCalculate(int exercise)
        {
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

            if (Goal > Weight)
                FinalCal = Cal + 500;
            else if (Goal == Weight)
                FinalCal = Cal;
            else
                FinalCal = Cal - 500;


            return Math.Round(FinalCal/80,1); 
        }

        //----Ypologismos Deikti mazas swmatos
        public double DMSCalculate()
        {
            return Math.Round(Weight / Math.Pow(Height,2), 1);
        }

    }
}
