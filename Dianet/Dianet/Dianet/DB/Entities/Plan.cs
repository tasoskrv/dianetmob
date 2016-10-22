﻿using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace Dianet.DB.Entities
{
    public class Plan: Model
    {
        private double goal;
        private DateTime goaldate;

        [PrimaryKey, AutoIncrement]
        public int IDPlan { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        public int IDServer { get; set; }

        public double Goal
        {
            get
            {
                return goal;
            }
            set
            {
                if (goal != value)
                {
                    goal = value;
                    OnPropertyChanged("Goal");
                }
            }
        }

        public DateTime GoalDate
        {
            get
            {
                return goaldate;
            }
            set
            {
                if (goaldate != value)
                {
                    goaldate = value;                    
                    OnPropertyChanged("GoalDate");
                }
            }
        }

        public int UserStep { get; set; }

        public int PlanType { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Plan() {
            IDServer = -1;
        }
    }
}
