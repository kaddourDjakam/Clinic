using Clinik.Commands;
using Clinik.Helpers;
using Clinik.Model;
using Clinik.Repository.DataContext;
using Clinik.Services;
using Clinik.View.Rendez_vous.Patientoptions;
using Clinik.ViewModel.Rendez_vous.Cards;
using Clinik.ViewModel.Rendez_vous.PatientOptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
        private string _TitlePage = "Nouvelle rendez-vous";

        public string TitlePage
        {
            get { return _TitlePage; }
            set { _TitlePage = value;   OnPropertyChanged(nameof(TitlePage)); }
        }


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

        private ObservableCollection<ApointmentCardViewModel> _appointments;
        public ObservableCollection<ApointmentCardViewModel> Appointments
        {
            get { return _appointments; }
            set
            {
                if (_appointments != value)
                {
                    _appointments = value;
                    OnPropertyChanged(nameof(Appointments));
                }
            }
        }

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
        private string _selectedDateString;

        public string SelectedDateString
        {
            get { return _selectedDateString; }
            set { _selectedDateString = value; OnPropertyChanged(nameof(SelectedDateString)); }
        }

        private DateTime _SelectedDate;

        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value;
                SelectedDateString = value.ToString("dd MMMM yyyy");
                OnPropertyChanged(nameof(SelectedDate)); }
        }

        private string _SearchString ;

        public string SearchString
        {
            get { return _SearchString; }
            set {
                _SearchString = value;
                OnPropertyChanged(nameof(SearchString)); }
        }


        private DateTime _SelectedFilterDate = DateTime.Now.Date;

        public DateTime SelectedFilterDate
        {
            get { return _SelectedFilterDate; }
            set
            {
                _SelectedFilterDate = value;
                _isDateSelected = true;
                OnPropertyChanged(nameof(SelectedFilterDate));
            }
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
        /* Select Patien PopUp ViewModel*/
        public ObservableCollection<SelectPatientViewModel> Patients { get;} = new ObservableCollection<SelectPatientViewModel>();
        private bool _IsSelectPatientViewOpen = false;

        public bool IsSelectPatientViewOpen
        {
            get { return _IsSelectPatientViewOpen; }
            set { _IsSelectPatientViewOpen = value; OnPropertyChanged(nameof(IsSelectPatientViewOpen)); }
        }

        private SelectPatientViewModel _SelectPatientViewModel;

        public SelectPatientViewModel SelectPatientViewModelEnts
        {
            get { return _SelectPatientViewModel; }
            set { _SelectPatientViewModel = value; OnPropertyChanged(nameof(SelectPatientViewModelEnts)); }
        }
        private void InitializePatients()
        {
            Patients.Clear();
            using (var contextDb = new ClinikEntities())
            {
                var patients = contextDb.Persons
                                       .Where(person => (person.Patient.Barcode != null)).OrderByDescending(person => person.Patient.Barcode)
                                       .Include(x => x.Patient) // Filter persons with at least one patient
                                       ;
                foreach (var item in patients)
                {
                    Patients.Add(new SelectPatientViewModel(item, HandlePatientClick));
                }
            }

        }
        private Person _CurrentPerson;

        public Person CurrentPerson
        {
            get { return _CurrentPerson; }
            set { _CurrentPerson = value; OnPropertyChanged(nameof(CurrentPerson)); }
        }
        private PatientModel _CurrentPatient;

        public PatientModel CurrentPatient
        {
            get { return _CurrentPatient; }
            set { _CurrentPatient = value; OnPropertyChanged(nameof(CurrentPatient)); }
        }

        void HandlePatientClick(Person person)
        {
            CurrentPerson = new Person(); 
            CurrentPerson = person;
            CurrentPatient = new PatientModel();
            CurrentPatient = person?.Patient;
            IsSelectPatientViewOpen = false;
        }

        public ObservableCollection<int> Hours { get; set; }

        private int selectedHour ;

        public int SelectedHour
        {
            get { return selectedHour; }
            set
            {
                if (selectedHour != value)
                {
                    selectedHour = value;
                    OnPropertyChanged(nameof(SelectedHour));
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
        public ICommand CloseNewAppointment { get; set; }

        public ICommand ApplyFilter_Cmd { get; set; }
        public ICommand RemoveFilter_Cmd { get; set; }
        



        public Rendez_vousViewModel()
        {
            InitializeAppointmentsListView();
            Hours = new ObservableCollection<int>(Enumerable.Range(0, 24));
            CurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ApointmentNum = DateTime.Now.ToString("ddMMyyHHmmss");
            IsNewAppointmentOpen = false;
           
            NewAppointment_Cmd = new RelayCommand(NewAppointment);

            MoveToNextMonthCommand = new RelayCommand(MoveToNextMonth);
            MoveToPreviousMonthCommand = new RelayCommand(MoveToPreviousMonth);

            OpenNewPatientView = new RelayCommand(OpenNewPatientViewFun);
            OpenPatientLIstView = new RelayCommand(OpenPatientLIstViewFun);

            SubmitAppointment = new RelayCommand(SubmitAppointmentFunc);
            CloseNewAppointment = new RelayCommand(CloseNewAppointmentFunc);

            ApplyFilter_Cmd = new RelayCommand(ApplyFilterFunc);
            RemoveFilter_Cmd = new RelayCommand(RemoveFilterFunc);

            NewPatientViewViewModel = new NewPatientViewModel();
            CurrentViewModel = new NewPatientView() { DataContext = NewPatientViewViewModel };
            IsNewPatientViewOpen = false;
            IsPatientLIstViewOpen = true;
        }

        /* filter Func */
        public bool _isDateSelected { get; set; }
        void RemoveFilterFunc()
        {
            InitializeAppointmentsListView();
        }
        void ApplyFilterFunc()
        {
            if (_isDateSelected)
            {
                if (Appointments != null) Appointments.Clear();
                using (var contextDb = new ClinikEntities())
                {
                    var editAppointmentAction = EditAppointment;
                    var deleteClickedFunc = DeleteClickedFunc;
                    var appointments = contextDb.Appointments.Include(ap => ap.Patient).ThenInclude(p => p.Person)
                        .Where(ap => ((ap.Date >= SelectedFilterDate.Date) && (ap.Date < SelectedFilterDate.Date.AddDays(1))) && (ap.Patient.Person.Fullname.Contains(SearchString)))
                        .Select(a => new ApointmentCardViewModel(a.Patient.Person, a.Patient, a, editAppointmentAction, deleteClickedFunc));
                    Appointments = new ObservableCollection<ApointmentCardViewModel>(appointments);
                }
            }
            else
            {
                if (Appointments != null) Appointments.Clear();
                using (var contextDb = new ClinikEntities())
                {
                    var editAppointmentAction = EditAppointment;
                    var deleteClickedFunc = DeleteClickedFunc;
                    var appointments = contextDb.Appointments.Include(ap => ap.Patient).ThenInclude(p => p.Person).Where(ap =>  (ap.Patient.Person.Fullname.Contains(SearchString)))
                        .Select(a => new ApointmentCardViewModel(a.Patient.Person, a.Patient, a, editAppointmentAction, deleteClickedFunc));
                    Appointments = new ObservableCollection<ApointmentCardViewModel>(appointments);
                }
            }
          
        }
        void EditAppointment(ApointmentCardViewModel apointmentCardViewModel)
        {
            TitlePage = "Modifier rendez-vous";
            IsNewAppointmentOpen = true;
            CurrentPatient  = apointmentCardViewModel.PatientEnst;
            CurrentPerson   = apointmentCardViewModel.PersonEnst;
            CurrentMonth    = apointmentCardViewModel.AppointmentEnst.Date;
            CurrentYear     = apointmentCardViewModel.AppointmentEnst.Date.Year;
            ApointmentNum   = apointmentCardViewModel.AppointmentEnst.AppointmentNum;
            SelectedDate    = apointmentCardViewModel.AppointmentEnst.Date;
            SelectedHour    = apointmentCardViewModel.AppointmentEnst.Date.Hour;
            IsPatientLIstViewOpen   = false;
            IsNewPatientViewOpen    = false;
            CurrentViewModel        = new PatientLIstView();
        }
        void DeleteClickedFunc(Appointment appointment)
        {
            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce rendez-vous ?", "Supprimer le rendez-vous", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                
                using (var contextDb = new ClinikEntities())
                {
                   var context =  contextDb.Treatments.Include(tr => tr.Appointment).Where(x=> x.AppointmentID == appointment.AppointmentID).ToList().Count();
                    if (context>0)
                    {
                        MessageService.ShowErrorMessage("Vous ne pouvez pas supprimer ce rendez-vous car il est lié à d'autres traitements.", Application.Current.MainWindow);
                    }
                    else
                    {
                        try
                        {
                            contextDb.Remove(appointment);
                            contextDb.SaveChanges();
                            InitializeAppointmentsListView();
                            MessageService.ShowSuccessMessage("Rendez-vous supprimé avec succès.", Application.Current.MainWindow);
                            
                        }
                        catch (Exception ex)
                        {
                            MessageService.ShowErrorMessage(ex.ToString(), Application.Current.MainWindow);
                        }
                        

                    }
                }
            }
            else
            {
                // User clicked "No" or closed the dialog
            }
        }
        /* Initialize Appointments List*/

        /* Initialize Appointments List*/
        async Task InitializeAppointmentsListView()
        {
            SearchString = "";
            _isDateSelected = false;
            if (Appointments != null) Appointments.Clear();
            using (var contextDb = new ClinikEntities())
            {
                var editAppointmentAction = EditAppointment;
                var deleteClickedFunc = DeleteClickedFunc;
                var appointments = contextDb.Appointments.Include(ap => ap.Patient).ThenInclude(p => p.Person).Where(ap => ap.Date >= DateTime.Now.Date.AddDays(-7))
                    .Select(a => new ApointmentCardViewModel (a.Patient.Person, a.Patient, a, editAppointmentAction, deleteClickedFunc) ) ;
                 Appointments =  new ObservableCollection<ApointmentCardViewModel>(appointments);
            }
        }
      
        void OpenPatientLIstViewFun()
        {
            IsNewPatientViewOpen = true;
            IsPatientLIstViewOpen = true;
            IsSelectPatientViewOpen = true;
            InitializePatients();
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

         void SubmitAppointmentFunc()
        {
            try
            {
                if (IsPatientLIstViewOpen == false && IsNewPatientViewOpen == false)
                {
                    using (var contextDb = new ClinikEntities())
                    {
                     
                        var  toUpdateApp = contextDb.Appointments.Where(x => x.AppointmentNum == ApointmentNum).FirstOrDefault();
                        toUpdateApp.Date = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, SelectedHour, 0, 0);
                        contextDb.Appointments.Update(toUpdateApp);
                        contextDb.SaveChanges();
                    }
                    MessageService.ShowSuccessMessage("Rendez-vous soumis avec succès.", Application.Current.MainWindow);
                    // Use Task.Run to execute the remaining code in the background without blocking the UI thread
                    Task.Run(async () =>
                    {
                        // Wait for 2 seconds
                        await Task.Delay(2000);

                        // Run the remaining code on the UI thread
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            CloseNewAppointmentFunc();
                        });
                    });
                }
                else
                {
                    if (SelectedDate < DateTime.Now.Date)
                    {
                        throw new InvalidOperationException("La date sélectionnée ne peut pas être antérieure à la date actuelle.");
                    }
                    else
                    {
                        if (!IsNewPatientViewOpen)
                        {
                            var patienInfo = ((CurrentViewModel as NewPatientView).DataContext as NewPatientViewModel);

                            var paient = new PatientModel()
                            {
                                Birthday = new DateTime(patienInfo.BirthdayYear, patienInfo.BirthdayMonth, patienInfo.BirthdayDay),
                                Gender = patienInfo.SelectedGender == "Male" ? Gender.Male : Gender.Female,
                                Barcode = DateTime.Now.ToString("ddMMyyHHmmss"),

                            };
                            var person = new Person()
                            {
                                Fullname = patienInfo.Fullname,
                                Adress = patienInfo.Address,
                                Phone = patienInfo.Phone,

                                Patient = paient

                            };
                            var appointment = new Appointment()
                            {
                                AppointmentNum = ApointmentNum,
                                Date = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,SelectedHour, 0, 0),
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
                        else
                        {
                            var appointment = new Appointment()
                            {
                                AppointmentNum = ApointmentNum,
                                Date = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, (int)SelectedHour, 0, 0),
                                Description = Description,
                                PatientID = CurrentPatient.ID,

                            };
                            using (var contextDb = new ClinikEntities())
                            {
                                contextDb?.Appointments?.Add(appointment);
                                contextDb?.SaveChanges();
                            }
                        }
                        MessageService.ShowSuccessMessage("Rendez-vous soumis avec succès.", Application.Current.MainWindow);
                        // Use Task.Run to execute the remaining code in the background without blocking the UI thread
                        Task.Run(async () =>
                        {
                            // Wait for 2 seconds
                            await Task.Delay(2000);

                            // Run the remaining code on the UI thread
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                CloseNewAppointmentFunc();
                            });
                        });
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageService.ShowErrorMessage($"Error: {ex.Message}", Application.Current.MainWindow);
            }
           
        }

        async void CloseNewAppointmentFunc()
        {
            IsNewAppointmentOpen = false;
            await  InitializeDays();
            await InitializeAppointmentsListView();
        }

        void NewAppointment()
        {
            TitlePage = "Nouvelle rendez-vous";
            IsNewAppointmentOpen = true;
            IsPatientLIstViewOpen = false;
            IsNewPatientViewOpen = true;
            ApointmentNum = DateTime.Now.ToString("ddMMyyHHmmss");
        
        }
        private void MoveToNextMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
        }

        private void MoveToPreviousMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
        }
        private async Task InitializeDays()
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

                SelectedDate = date;
            
        }
    }
    public class DayViewModel : ViewModelBase
    {
        private readonly Action<DateTime> dayClickHandler;

        public DateTime Date { get; }

        private int _Appointmentnumber;

        public int Appointmentnumber
        {
            get { return _Appointmentnumber; }
            set { _Appointmentnumber = value; OnPropertyChanged(nameof(Appointmentnumber)); }
        }



        public ICommand DayClickCommand { get; }



        public bool IsEnabled
        {
            get
            {
                DateTime startOfDay = DateTime.Now.Date; // Today 00:00
                DateTime endOfDay = DateTime.Now.Date.AddDays(1).AddTicks(-1); // Today 23:59:59.9999999

                return Date >= startOfDay ;
            }
        }



        public DayViewModel(DateTime date, Action<DateTime> dayClickHandler)
        {
            using (var contextDb = new ClinikEntities()) 
            {
                var sum = contextDb.Appointments.Where(x => x.Date.Date == date.Date).ToList();
                Appointmentnumber = sum.Count;
            }
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
