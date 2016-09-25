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
        public string AccessToken { get; set; }

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

        public DateTime? Birthdate { get; set; }

        public DateTime? RemindDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int AdjustDiet { get; set; }

        public User() {
            IdUser = -1;
            AccessToken = "";
            FacebookID = -1;
            FirstName = "";
            LastName = "";
            Gender = -1;
            Email = "";
            Password = "";
            Height = -1;
            HeightType = -1;
            Skeleton = -1;
            Birthdate = null;
            RemindDate = null;
            InsertDate = DateTime.MinValue;
            UpdateDate = DateTime.MinValue;
            AdjustDiet = -1;
        }

        public override string ToString() {
            string str = "";
            if (IdUser != -1)
                str += "&iduser=" + IdUser.ToString();
            if (!AccessToken.Equals(""))
                str += "&accesstoken=" + Uri.EscapeDataString(AccessToken);
            if (FacebookID != -1)
                str += "&iduser=" + FacebookID.ToString();
            if (!FirstName.Equals(""))
                str += "&firstname=" + Uri.EscapeDataString(FirstName);
            if (!LastName.Equals(""))
                str += "&lastname=" + Uri.EscapeDataString(LastName);
            if (Gender != -1)
                str += "&gender=" + Gender.ToString();
            if (!Email.Equals(""))
                str += "&email=" + Uri.EscapeDataString(Email);
            if (!Password.Equals(""))
                str += "&password=" + Password;
            if (Height != -1)
                str += "&height=" + Height.ToString();
            if (HeightType != -1)
                str += "&heighttype=" + HeightType.ToString();
            if (Skeleton != -1)
                str += "&skeleton=" + Skeleton.ToString();
            if (Birthdate != null)
                str += "&birthdate=" + Birthdate.Value.ToString("yyyyMMdd");
            if (RemindDate != null)
                str += "&reminddate=" + RemindDate.Value.ToString("yyyyMMdd");
            if (InsertDate != DateTime.MinValue)
                str += "&insertdate=" + InsertDate.ToString("yyyyMMdd");
            if (UpdateDate != DateTime.MinValue)
                str += "&updatedate=" + UpdateDate.ToString("yyyyMMdd");
            if (AdjustDiet != -1)
                str += "&adjustdiet=" + AdjustDiet.ToString();
            return str.Substring(1);

        }
    }
}
