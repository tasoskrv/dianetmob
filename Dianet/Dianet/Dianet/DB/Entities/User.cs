using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class User
    {
        [PrimaryKey]
        public int IdUser { get; set; }

        [MaxLength(45)]
        public double AccessToken { get; set; }

        public double FacebookID { get; set; }

        [MaxLength(45)]
        public string FirstName { get; set; }

        [MaxLength(45)]
        public string LastName { get; set; }

        public int Gender { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(45)]
        public string Password { get; set; }

        public double Height { get; set; }

        public int HeightType { get; set; }

        public double Skeleton { get; set; }

        public DateTime Birthdate { get; set; }

        public DateTime RemindDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int AdjustDiet { get; set; }
    }
}
