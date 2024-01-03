using Clinik.Commands;
using Clinik.Model;
using Clinik.Repository.DataContext;
using Clinik.View.Rendez_vous;
using Clinik.View.WorkSpace;
using Clinik.ViewModel.Rendez_vous;
using Clinik.ViewModel.WorkSpace;
using Clinik.ViewModel.WorkSpace.Cards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Clinik.ViewModel.MainWindow
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Color DiabledColor { get; set; }

        private bool _homePage_isEnabled = false;

        public bool HomePage_IsEnabled
        {
            get { return _homePage_isEnabled; }
            set { _homePage_isEnabled = value; OnPropertyChanged(nameof(HomePage_IsEnabled)); }
        }

        private bool _Appointment_isEnabled = true;

        public bool Appointment_IsEnabled
        {
            get { return _Appointment_isEnabled; }
            set { _Appointment_isEnabled = value; OnPropertyChanged(nameof(Appointment_IsEnabled)); }
        }
        private Color _homePageBrdColor = Colors.Transparent;

        public Color HomePageBrdColor
        {
            get { return _homePageBrdColor; }
            set
            {
                _homePageBrdColor = value;
                OnPropertyChanged(nameof(HomePageBrdColor));
            }
        }

        private Color _AppointmentColor = Colors.Transparent;

        public Color AppointmentBrdColor
        {
            get { return _AppointmentColor; }
            set
            {
                _AppointmentColor = value;
                OnPropertyChanged(nameof(AppointmentBrdColor));
            }
        }
        private string _user_Name;

        public string User_Name
        {
            get { return _user_Name; }
            set { _user_Name = value; OnPropertyChanged(nameof(User_Name)); }
        }

        private object _currentController;
        public object CurrentController
        {
            get { return _currentController; }
            set
            {
                _currentController = value;
                OnPropertyChanged(nameof(CurrentController));
            }
        }
        private WorkSpaceView _currentHomePage;
        public WorkSpaceView CurrentHomePage
        {
            get { return _currentHomePage; }
            set
            {
                _currentHomePage = value;
                OnPropertyChanged(nameof(CurrentHomePage));
            }
        }
        public ICommand Home_page_Cmd { get; set; }
        public ICommand Appointment_Cmd { get; set; }
        public MainWindowViewModel()
        {
            User_Name = LoginViewModel.CurrentUser?.Username;
            DiabledColor = (Color)ColorConverter.ConvertFromString("#5D4FFF") ;
            HomePageBrdColor = DiabledColor;

            Home_page_Cmd = new RelayCommand(HomePageClicked);
            Appointment_Cmd = new RelayCommand(AppointmentClicked);


            CurrentController = new WorkSpaceView() {DataContext = new WorkSpaceViewModel() };
            CurrentHomePage = CurrentController as WorkSpaceView ;

        }

        private void HomePageClicked()
        {
            EnableDisableBtns("Home_page_Cmd");
        }
        private void AppointmentClicked()
        {
            EnableDisableBtns("Appointment_Cmd");
        }

        private void EnableDisableBtns(string btn_name)
        {
            ResetBtns();

            switch (btn_name)
            {
                case "Home_page_Cmd":
                    HomePage_IsEnabled = false;
                    HomePageBrdColor = DiabledColor;
                    CurrentController = CurrentHomePage;
                    using (var contextDb = new ClinikEntities())
                    {
                        int count = 0;
                        /*var appointments = contextDb.Appointments.Include(ap => ap.Patient).ThenInclude(p => p.Person).Where(ap => ap.Date.Date == DateTime.Now.Date).ToList()
                            .Select((a, index) => new WaitingQViewModel(a.Patient.Person, a.Patient, a, index + 1));*/
                       /* ((CurrentController as WorkSpaceView).DataContext as WorkSpaceViewModel).Appointments.Clear();
                        ((CurrentController as WorkSpaceView).DataContext as WorkSpaceViewModel).Appointments = new System.Collections.ObjectModel.ObservableCollection<WorkSpace.Cards.WaitingQViewModel>(appointments);*/
                        ((CurrentController as WorkSpaceView).DataContext as WorkSpaceViewModel).InitializeAppointmentsListView();
                    }
                   
                   /* using (var entityContext = new hardwareStoreEntities())
                    {
                        ((CurrentController as Home_Page).DataContext as HomePage_ViewModel).GetStockData(entityContext);
                    }
                    ((CurrentController as Home_Page).DataContext as HomePage_ViewModel).ReGenerateView(((CurrentController as Home_Page).DataContext as HomePage_ViewModel).StockExtratDataCollection);
                    ((CurrentController as Home_Page).DataContext as HomePage_ViewModel).InitializeCategoriesUI();*/
                    break;

                case "Appointment_Cmd":
                    Appointment_IsEnabled = false;
                    AppointmentBrdColor = DiabledColor;
                    CurrentController = new Rendez_vousView() { DataContext = new Rendez_vousViewModel() };
                    break;

                default:
                    HomePage_IsEnabled = false;
                    HomePageBrdColor = DiabledColor;
                    break;
            }
        }

        private void ResetBtns()
        {
            HomePage_IsEnabled = true;
            HomePageBrdColor = Colors.Transparent;

            Appointment_IsEnabled = true;
            AppointmentBrdColor = Colors.Transparent;

        }
    }
}
