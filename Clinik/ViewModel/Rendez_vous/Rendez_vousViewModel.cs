using Clinik.Commands;
using Clinik.Model;
using Clinik.Repository.DataContext;
using Clinik.View.Rendez_vous.Patientoptions;
using Clinik.ViewModel.Rendez_vous.PatientOptions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Clinik.ViewModel.Rendez_vous
{
    class Rendez_vousViewModel : ViewModelBase
    {
        private bool _IsPatientLIstViewOpen = false;

        public bool IsPatientLIstViewOpen
        {
            get { return _IsPatientLIstViewOpen; }
            set { _IsPatientLIstViewOpen = value; OnPropertyChanged(nameof(IsPatientLIstViewOpen)); }
        }

        private bool _IsNewPatientViewOpen = false;

        public bool IsNewPatientViewOpen
        {
            get { return _IsNewPatientViewOpen; }   
            set { _IsNewPatientViewOpen = value;OnPropertyChanged(nameof(IsNewPatientViewOpen)); }
        }


        private string _ApointmentNum;

        public string ApointmentNum
        {
            get { return _ApointmentNum; }
            set { _ApointmentNum = value; OnPropertyChanged(nameof(ApointmentNum)); }
        }

        private NewPatientViewModel _NewPatientViewViewModel;

        public NewPatientViewModel NewPatientViewViewModel
        {
            get { return _NewPatientViewViewModel; }
            set { _NewPatientViewViewModel = value; OnPropertyChanged(nameof(NewPatientViewViewModel)); }
        }

        public ObservableCollection<Person> People { get; set; }

        private bool _IsNewAppointmentOpen;

        public bool IsNewAppointmentOpen
        {
            get { return _IsNewAppointmentOpen; }
            set { _IsNewAppointmentOpen = value; OnPropertyChanged(nameof(IsNewAppointmentOpen)); }
        }

        private DateTime currentMonth;

        public DateTime CurrentMonth
        {
            get { return currentMonth; }
            set
            {
                if (currentMonth != value)
                {
                    currentMonth = value;
                    InitializeDays();
                }
                OnPropertyChanged(nameof(CurrentMonth));
            }
        }

        public int CurrentYear
        {
            get { return currentMonth.Year; }
            set
            {
                CurrentMonth = new DateTime(value, currentMonth.Month, 1);
            }
        }
        private DateTime _SelectedDate;

        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value; OnPropertyChanged(nameof(SelectedDate)); }
        }
        private object _currentViewModel;
        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }

        public ObservableCollection<DayViewModel> Days { get; } = new ObservableCollection<DayViewModel>();

        public ICommand MoveToNextMonthCommand { get; }
        public ICommand MoveToPreviousMonthCommand { get; }
        public ICommand NewAppointment_Cmd { get; set; }
        public ICommand OpenPatientLIstView { get; set; }
        public ICommand OpenNewPatientView { get; set; }
        public ICommand SubmitAppointment { get; set; }


        public Rendez_vousViewModel()
        {
            CurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ApointmentNum = DateTime.Now.ToString("ddMMyyHHmmss");
            IsNewAppointmentOpen = false;
            // Initialize the collection and populate it with data
            People = new ObservableCollection<Person>
            {
                new Person { Fullname = "John", Phone = "25" },
                new Person { Fullname = "Alice", Phone = "50" },
                // Add more data as needed
            };
            NewAppointment_Cmd = new RelayCommand(NewAppointment);

            MoveToNextMonthCommand = new RelayCommand(MoveToNextMonth);
            MoveToPreviousMonthCommand = new RelayCommand(MoveToPreviousMonth);

            OpenNewPatientView = new RelayCommand(OpenNewPatientViewFun);
            OpenPatientLIstView = new RelayCommand(OpenPatientLIstViewFun);

            SubmitAppointment = new RelayCommand(SubmitAppointmentFunc);


            NewPatientViewViewModel = new NewPatientViewModel();
            CurrentViewModel = new NewPatientView() { DataContext = NewPatientViewViewModel };
            IsNewPatientViewOpen = false;
            IsPatientLIstViewOpen = true;
        }
        void OpenPatientLIstViewFun()
        {
            IsNewPatientViewOpen = true;
            IsPatientLIstViewOpen = false;
            CurrentViewModel = new PatientLIstView();
        }
        void OpenNewPatientViewFun()
        {
            IsNewPatientViewOpen = false;
            IsPatientLIstViewOpen = true;


            NewPatientViewViewModel = new NewPatientViewModel();
            CurrentViewModel = new NewPatientView() { DataContext = NewPatientViewViewModel };
            
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value;OnPropertyChanged(nameof(Description)); }
        }

        async void SubmitAppointmentFunc()
        {
            var patienInfo = ((CurrentViewModel as NewPatientView).DataContext as NewPatientViewModel);
            
            var paient = new PatientModel()
            {
                Birthday = new DateTime(patienInfo.BirthdayYear, patienInfo.BirthdayMonth, patienInfo.BirthdayDay),
                Gender = patienInfo.SelectedGender == "Male" ? Gender.Male : Gender.Female ,
                
                
            };
            var person = new Person()
            {
                Fullname = patienInfo.Fullname,
                Adress = patienInfo.Address,
                Phone = patienInfo.Phone,
                Patients = new List<PatientModel>() { paient }
                
            };
            var appointment = new Appointment()
            {
                AppointmentNum = ApointmentNum,
                Date = SelectedDate,
                Description = Description,
                Patient = paient,

            };
            using (var contextDb = new ClinikEntities())
            {
                contextDb?.Persons?.Add(person);
                contextDb?.Appointments?.Add(appointment);
                contextDb?.SaveChanges();
            }
        }


        void NewAppointment()
        {
            IsNewAppointmentOpen = true;
        }
        private void MoveToNextMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
        }

        private void MoveToPreviousMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
        }
        private void InitializeDays()
        {
            Days.Clear();

            int daysInMonth = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);

            for (int i = 1; i <= daysInMonth; i++)
            {
                DateTime date = new DateTime(CurrentMonth.Year, CurrentMonth.Month, i);
                Days.Add(new DayViewModel(date, HandleDayClick));
            }
        }

        private void HandleDayClick(DateTime date)
        {
            // Handle the click event for the selected date
            // You can add your logic here
            // For example, show a message box with the selected date
            /*System.Windows.MessageBox.Show($"Clicked on {date.ToShortDateString()}");*/

            // Define an array of acceptable date formats
            string[] dateFormats = { "yyyy-MM-dd", "MM/dd/yyyy", "dd-MM-yyyy"}; // Add more formats as needed

            DateTime parsedDate;

            // Try parsing using each format in the array
            bool parsingSuccess = DateTime.TryParseExact(
                date.ToString("dd/MM/yyyy"),
                dateFormats,
                CultureInfo.CurrentUICulture,
                DateTimeStyles.None,
                out parsedDate
            );

            if (parsingSuccess)
            {
                SelectedDate = parsedDate;
            }
            else
            {
                
                SelectedDate = date;
            }
        }
    }
    public class DayViewModel : ViewModelBase
    {
        private readonly Action<DateTime> dayClickHandler;

        public DateTime Date { get; }

        public ICommand DayClickCommand { get; }

        public bool IsEnabled => true; // You can implement your logic here

        public DayViewModel(DateTime date, Action<DateTime> dayClickHandler)
        {
            Date = date;
            this.dayClickHandler = dayClickHandler;
            DayClickCommand = new RelayCommand(OnDayClick, () => IsEnabled);
        }

        private void OnDayClick()
        {
            dayClickHandler?.Invoke(Date);
        }
    }
}
