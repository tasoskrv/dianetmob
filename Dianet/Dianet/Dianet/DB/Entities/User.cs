using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class User : INotifyPropertyChanged
    {
        private string firstname;

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
        
        public override string ToString() {
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
