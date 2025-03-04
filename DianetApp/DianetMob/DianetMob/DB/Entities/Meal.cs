﻿using DianetMob.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class Meal : Model
    {
        private string name;
        private string description;

        [PrimaryKey, AutoIncrement]
        public int IDMeal { get; set; }

        public int IDServer { get; set; }

        [MaxLength(100)]
        public string Name {

            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    NormalizedName=GenLib.NormalizeGreek(name);
                    OnPropertyChanged("Name");
                }
            }
        }

        [MaxLength(100)]
        public string NormalizedName { get; set; }

        [MaxLength(800)]
        public string Description {
            get
            {
                return description;
            }
            set {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        public int IDLang { get; set; }

        public int IDUser { get; set; }

        [MaxLength(20)]
        public string Fertility { get; set; }

        public string Identifier { get; set; }

        public int IsActive { get; set; }

        public int Deleted { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Meal()
        {
            IDServer = 0;
            Deleted = 0;
            IDUser = 0;
            IsActive = 1;
        }

        public override string ToString()
        {
            string str = "";

            str += "&idserver=\"" + IDServer.ToString() + "\"";

            if (Name != null)
                str += "&name=\"" + Name + "\"";
            if (!Description.Equals(""))
                str += "&description=\"" + Description + "\"";
            if (IDLang != -1)
                str += "&idlang=\"" + IDLang.ToString() + "\"";
            if (IDUser != 0)
                str += "&iduser=\"" + IDUser.ToString() + "\"";
            if (IsActive != -1)
                str += "&isActive=\"" + IsActive.ToString() + "\"";
            if (Deleted != 0)
                str += "&deleted=\"" + Deleted.ToString() + "\"";
            if (InsertDate != null)
                str += "&insertdate=\"" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (UpdateDate != null)
                str += "&updatedate=\"" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";

            return str.Substring(1);
        }

    }
}
