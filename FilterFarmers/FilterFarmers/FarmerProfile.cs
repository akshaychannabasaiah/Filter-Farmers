using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FilterFarmers
{
    public class FarmerProfile : INotifyPropertyChanged
    {
        private string firstName;
        private List<string> animals;
        private string lastName;
        private int farmSize;
        private bool f1Match = true;
        private bool f2Match = true;
        private string fullName;

        private BitmapImage profileImage;
        private string email;
        private string phone;
        internal int matchCount = 0;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("EMail"));
            }
        }

        public int MatchCount
        {
            get { return matchCount; }
            set
            {
                matchCount = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MatchCount"));
            }
        }
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Phone"));
            }
        }

        public BitmapImage ProfileImage
        {
            get { return profileImage; }
            set
            {
                profileImage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProfileImage"));
            }
        }


        private BitmapImage animalsImage;
        public BitmapImage AnimalsImage
        {
            get { return animalsImage; }
            set
            {
                animalsImage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AnimalsImage"));
            }
        }

        private BitmapImage facilityImage;
        public BitmapImage FacilityImage
        {
            get { return facilityImage; }
            set
            {
                facilityImage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FacilityImage"));
            }
        }

        private BitmapImage grainImage;
        public BitmapImage GrainImage
        {
            get { return grainImage; }
            set
            {
                grainImage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GrainImage"));
            }
        }

        private BitmapImage locationImage;
        public BitmapImage LocationImage
        {
            get { return locationImage; }
            set
            {
                locationImage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LocationImage"));
            }
        }

        private BitmapImage machineryImage;
        public BitmapImage MachineryImage
        {
            get { return machineryImage; }
            set
            {
                machineryImage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MachineryImage"));
            }
        }

        private BitmapImage projectImage;
        public BitmapImage ProjectImage
        {
            get { return projectImage; }
            set
            {
                projectImage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProjectImage"));
            }
        }

        private BitmapImage sizeImage;
        private List<string> machinery;
        private List<string> grain;
        private List<string> facility;
        private List<string> project;
        private string country;
        private string state;

        public BitmapImage SizeImage
        {
            get { return sizeImage; }
            set
            {
                sizeImage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SizeImage"));
            }
        }


        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }


        public bool F2Match
        {
            get { return f2Match; }
            set
            {
                f2Match = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("F2Match"));
            }
        }

        public bool F1Match
        {
            get { return f1Match; }
            set
            {
                f1Match = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("F1Match"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                fullName = firstName + " " + lastName;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FullName"));
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                fullName = firstName + " " + lastName;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FullName"));
            }
        }

        public int FarmSize
        {
            get { return farmSize; }
            set
            {
                farmSize = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FarmSize"));
            }
        }

        public List<string> Animals
        {
            get { return animals; }
            set
            {
                animals = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Animals"));
            }
        }
        public List<string> Machinery
        {
            get { return machinery; }
            set
            {
                machinery = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Machinery"));
            }
        }
        public List<string> Grain
        {
            get { return grain; }
            set
            {
                grain = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Grain"));
            }
        }
        public List<string> Facility
        {
            get { return facility; }
            set
            {
                facility = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Facility"));
            }
        }
        public List<string> Project
        {
            get { return project; }
            set
            {
                project = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Project"));
            }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
