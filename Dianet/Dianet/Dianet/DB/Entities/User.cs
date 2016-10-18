using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;

namespace Dianet.DB.Entities
{
    public class User : Model
    {
        private string firstname;
        private string lastname;
        private string email;
        private DateTime? birthday;               
        private double height;
        private int heighttype;
        private int gender;
        private double skeleton;

        [PrimaryKey]
        public int IDUser { get; set; }

        [MaxLength(45)]
        public string AccessToken { get; set; }

        public double FacebookID { get; set; }

        [MaxLength(45)]
        public string FirstName
        {
            get
            {
                return firstname;
            }
            set
            {
                if (firstname != value)
                {
                    firstname = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        [MaxLength(45)]
        public string LastName
        {
            get
            {
                return lastname;
            }
            set
            {   
                if (lastname != value)
                {
                    lastname = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        public int Gender
        {
            get
            {
                return gender;
            }
            set
            {
                if (gender != value)
                {
                    gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        [MaxLength(100)]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        [MaxLength(45)]
        public string Password { get; set; }

        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                if (height != value)
                {
                    height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        public int HeightType
        {
            get
            {
                return heighttype;
            }
            set
            {
                if (heighttype != value)
                {
                    heighttype = value;
                    OnPropertyChanged("HeightType");
                }
            }
        }

        public double Skeleton 
        {
            get
            {
                return skeleton;
            }
            set
            {
                if (skeleton != value)
                {
                    skeleton = value;
                    OnPropertyChanged("Skeleton");
                }
            }
        }

        public DateTime? Birthdate
        {
            get
            {
                return birthday;
            }
            set
            {
                if (birthday != value)
                {
                    birthday = value;
                    OnPropertyChanged("Birthdate");
                }
            }
        }

        public DateTime? RemindDate { get; set; }

        public int AdjustDiet { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public User() {
            IDUser = -1;
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

        public override string ToString()
        {
            string str = "";
            if (IDUser != -1)
                str += "&iduser=" + IDUser.ToString();
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
                str += "&birthdate=" + Birthdate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            if (RemindDate != null)
                str += "&reminddate=" + RemindDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            if (InsertDate != DateTime.MinValue)
                str += "&insertdate=" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss");
            if (UpdateDate != DateTime.MinValue)
                str += "&updatedate=" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss");
            if (AdjustDiet != -1)
                str += "&adjustdiet=" + AdjustDiet.ToString();
            return str.Substring(1);

        }
    }

}
