﻿using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class UserFood
    {
        [PrimaryKey]
        public int IDUserFood { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        [MaxLength(45)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
