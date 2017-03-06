using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class User : Model
    {
        private string firstname;
        private string lastname;
        private string email;
        private DateTime? birthday;
        private double height;
        private int heighttype;
        private int weighttype;
        private int gender;
        private double skeleton;
        private string location;
        private double weight;
        private double goal;
        private DateTime weightdate;
        private int fertility;

        [PrimaryKey]
        public int IDUser { get; set; }

        [MaxLength(45)]
        public string AccessToken { get; set; }

        public Int64 FacebookID { get; set; }

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
                    OnPropertyChanged("Name");
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
                    OnPropertyChanged("Name");
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

        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged("Location");
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
        public int WeightType
        {
            get
            {
                return weighttype;
            }
            set
            {
                if (weighttype != value)
                {
                    weighttype = value;
                    OnPropertyChanged("WeightType");
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

        public int Fertility
        {
            get
            {
                return fertility;
            }
            set
            {
                if (fertility != value)
                {
                    fertility = value;
                    OnPropertyChanged("Fertility");
                }
            }
        }

        public DateTime? RemindDate { get; set; }

        public int AdjustDiet { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int Isactive { get; set; }

        public Byte[] ImageBefore { get; set; }

        public Byte[] ImageAfter { get; set; }

        [Ignore]
        public string Name {
            get {
                return FirstName + " " + LastName;
            }
        }

        [Ignore]
        public double Weight {
            get
            {
                if (weight == 0)
                {
                    List<Weight> whts= StorageManager.GetConnection().Query<Weight>("select WValue, WeightDate from Weight where IDUser=" + IDUser.ToString() + " order by WeightDate DESC LIMIT 1");
                    if (whts.Count > 0)
                    {
                        weight = whts[0].WValue;
                        weightdate = whts[0].WeightDate;
                    }
                }
                return weight;
            }
            set {
                weight = value;
            }
        }

        [Ignore]
        public DateTime LastWeightDate
        {
            get
            {
                return weightdate;
            }
        }

        [Ignore]
        public double Goal
        {
            get
            {
                if (goal == 0)
                {
                    List<Plan> plns = StorageManager.GetConnection().Query<Plan>("select Goal from Plan where IDUser=" + IDUser.ToString() + " order by UpdateDate DESC LIMIT 1");
                    if (plns.Count > 0)
                        goal = plns[0].Goal;
                }
                return goal;
            }
            set {
                goal = value;
            }
        }

        [Ignore]
        public int Age
        {
            get
            {
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span = DateTime.Now - (DateTime)Birthdate;
                return (zeroTime + span).Year - 1;
            }
        }


        public User()
        {
            IDUser = -1;
            AccessToken = "";
            FacebookID = -1;
            FirstName = "";
            LastName = "";
            Gender = -1;
            Email = "";
            Password = "";
            Location = "";
            Height = -1;
            HeightType = -1;
            Skeleton = 0;
            Birthdate = null;
            RemindDate = null;
            InsertDate = DateTime.MinValue;
            UpdateDate = DateTime.MinValue;
            AdjustDiet = 0;
            Isactive = 0;
            weight = 0;//TODO Weight?
            fertility = 0;
            ImageBefore = null;
            ImageAfter = null;
        }

        public override string ToString()
        {
            string str = "";
            if (IDUser != -1)
                str += "&iduser=\"" + IDUser.ToString() + "\"";
            if (!AccessToken.Equals(""))
                str += "&accesstoken=\"" + Uri.EscapeDataString(AccessToken) + "\"";
            if (FacebookID != -1)
                str += "&iduser=\"" + FacebookID.ToString() + "\"";
            if (!FirstName.Equals(""))
                str += "&firstname=\"" + Uri.EscapeDataString(FirstName) + "\"";
            if (!LastName.Equals(""))
                str += "&lastname=\"" + Uri.EscapeDataString(LastName) + "\"";
            if (Gender != -1)
                str += "&gender=\"" + Gender.ToString() + "\"";
            if (!Email.Equals(""))
                str += "&email=\"" + Uri.EscapeDataString(Email) + "\"";
            if (!Password.Equals(""))
                str += "&password=\"" + Password + "\"";
            if (!Location.Equals(""))
                str += "&location=\"" + Location + "\"";
            if (Height != -1)
                str += "&height=\"" + Height.ToString() + "\"";
            if (HeightType != -1)
                str += "&heighttype=\"" + HeightType.ToString() + "\"";
            if (Skeleton != 0)
                str += "&skeleton=\"" + Skeleton.ToString() + "\"";
            if (fertility != -1)
                str += "&fertility=\"" + Fertility.ToString() + "\"";
            if (Birthdate != null)
                str += "&birthdate=\"" + Birthdate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (RemindDate != null)
                str += "&reminddate=\"" + RemindDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (InsertDate != DateTime.MinValue)
                str += "&insertdate=\"" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (UpdateDate != DateTime.MinValue)
                str += "&updatedate=\"" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            if (AdjustDiet != -1)
                str += "&adjustdiet=\"" + AdjustDiet.ToString() + "\"";
            return str.Substring(1);

        }
    }
}
