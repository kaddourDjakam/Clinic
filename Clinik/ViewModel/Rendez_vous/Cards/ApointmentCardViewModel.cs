using Clinik.Commands;
using Clinik.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clinik.ViewModel.Rendez_vous.Cards
{
    class ApointmentCardViewModel:ViewModelBase
    {
        public Person PersonEnst { get; set; } 
        public PatientModel PatientEnst { get; set; } 
        public Appointment AppointmentEnst { get; set; }
        // New property to track the parent ScrollViewer
        public ScrollViewer ParentScrollViewer { get; set; }
        public int Age { get; set; }

        public ICommand AppointmentClicked { get; set; }
        public ICommand DeleteClicked { get; set; }

        private readonly Action<ApointmentCardViewModel> EditAppointment;
        private readonly Action<Appointment> DeleteAppointment;

        public ApointmentCardViewModel(Person person, PatientModel patientModel, Appointment appointment, Action<ApointmentCardViewModel> action, Action<Appointment> deleteAppointment)
        {
            PersonEnst = person;
            PatientEnst = patientModel;

            AppointmentEnst = appointment;


            Age = (int)((DateTime.Now.Date - PatientEnst.Birthday).TotalDays / 365.25);
            AppointmentClicked = new RelayCommand(AppointemtClickedFunc);
            DeleteClicked = new RelayCommand(DeleteClickedFunc);
            EditAppointment = action;
            DeleteAppointment = deleteAppointment;
        }

        void AppointemtClickedFunc()
        {
            EditAppointment.Invoke(this);
        }

        void DeleteClickedFunc()
        {
            DeleteAppointment.Invoke(this.AppointmentEnst);
        }
    }
}
