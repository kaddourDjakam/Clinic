using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.ViewModel.Rendez_vous.PatientOptions
{
    public class NewPatientViewModel : ViewModelBase
    {
        private string _fullname;
        private string _phone;
        private string _address;
        private int _birthdayDay;
        private int _birthdayMonth;
        private int _birthdayYear;
        private string _selectedGender;

        public NewPatientViewModel()
        {
            Genders = new ObservableCollection<string> { "Male", "Female" };
        }

        public string Fullname
        {
            get { return _fullname; }

            set { _fullname = value; OnPropertyChanged(nameof(Fullname)); }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(nameof(Address)); }
        }

        public int BirthdayDay
        {
            get { return _birthdayDay; }
            set { _birthdayDay = value; OnPropertyChanged(nameof(BirthdayDay)); }
        }

        public int BirthdayMonth
        {
            get { return _birthdayMonth; }
            set { _birthdayMonth = value; OnPropertyChanged(nameof(BirthdayMonth)); }
        }

        public int BirthdayYear
        {
            get { return _birthdayYear; }
            set { _birthdayYear = value; OnPropertyChanged(nameof(BirthdayYear)); }
        }

        public ObservableCollection<string> Genders { get; }

        public string SelectedGender
        {
            get { return _selectedGender; }
            set { _selectedGender = value; OnPropertyChanged(nameof(SelectedGender)); }
        }
    }
}
