using Clinik.Model;
using Clinik.Repository.DataContext;
using Clinik.ViewModel.Rendez_vous.Cards;
using Clinik.ViewModel.WorkSpace.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Clinik.ViewModel.WorkSpace
{
    class WorkSpaceViewModel : ViewModelBase
    {
    private ObservableCollection<WaitingQViewModel> _appointments;
    public ObservableCollection<WaitingQViewModel> Appointments
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

    private ObservableCollection<WaitingQViewModel> _secondScrollViewItems;
    public ObservableCollection<WaitingQViewModel> SecondScrollViewItems
    {
        get { return _secondScrollViewItems; }
        set
        {
            if (_secondScrollViewItems != value)
            {
                _secondScrollViewItems = value;
                OnPropertyChanged(nameof(SecondScrollViewItems));
            }
        }
    }
        private ObservableCollection<WaitingQViewModel> _PaymentScrollViewItems;
        public ObservableCollection<WaitingQViewModel> PaymentScrollViewItems
        {
            get { return _PaymentScrollViewItems; }
            set
            {
                if (_PaymentScrollViewItems != value)
                {
                    _PaymentScrollViewItems = value;
                    OnPropertyChanged(nameof(PaymentScrollViewItems));
                }
            }
        }

        public WorkSpaceViewModel()
        {
            InitializeAppointmentsListView();
            InitializeSecondScrollView();
        }

        void InitializeSecondScrollView()
        {
            // Initialize the SecondScrollViewItems collection
            SecondScrollViewItems = new ObservableCollection<WaitingQViewModel>();
            PaymentScrollViewItems = new ObservableCollection<WaitingQViewModel>();
        }

        public void HandleDrop(WaitingQViewModel draggedAppointment)
        {
            try
            {
                if (draggedAppointment != null)
                {
                    // Create a new SecondScrollViewModel with a reference to the dropped ApointmentCardViewModel

                    var secondScrollItem = SecondScrollViewItems.Contains(draggedAppointment);
                    if (!secondScrollItem)
                    {
                        // Add the item to the SecondScrollViewItems collection
                        SecondScrollViewItems.Add(draggedAppointment);

                        // Remove the card from the first ScrollViewer
                        Appointments.Remove(draggedAppointment);
                    }
                    else
                    {
                        // The appointment already exists, you can show a message or handle it accordingly
                        MessageBox.Show("The appointment already exists.");
                    }
                }

            } catch (Exception ex) { MessageBox.Show("The appointment already exists."); }
           

               
        }
        public void HandleDropBack(WaitingQViewModel draggedAppointment)
        {
            if (draggedAppointment != null)
            {
                // Check if the appointment already exists in the Appointments collection

                bool appointmentExists = Appointments.Contains(draggedAppointment);

                if (!appointmentExists)
                {
                    // Add the item to the Appointments collection

                    Appointments.Add(draggedAppointment);
                    Appointments = new ObservableCollection<WaitingQViewModel>(Appointments.OrderBy(a => a.Number)) ;

                    // Remove the card from the SecondScrollViewItems collection

                    SecondScrollViewItems.Remove(draggedAppointment);
                }
                else
                {
                    // The appointment already exists, you can show a message or handle it accordingly
                    MessageBox.Show("The appointment already exists.");
                }
            }
           
        }
        void s(WaitingQViewModel waitingQViewModel)
        {
            PaymentScrollViewItems.Remove(waitingQViewModel);
        }
        public void HandleDropToPayment(WaitingQViewModel draggedAppointment)
        {
            if (draggedAppointment != null)
            {
                var test = new WaitingQViewModel(draggedAppointment.PersonEnst, draggedAppointment.PatientEnst, draggedAppointment.AppointmentEnst, draggedAppointment.Number, s);

                bool appointmentExists = PaymentScrollViewItems.Contains(test);

                if (!appointmentExists)
                {
                    // Add the item to the Appointments collection

                    PaymentScrollViewItems.Add(test);
                  /*  PaymentScrollViewItems = new ObservableCollection<WaitingQViewModel>(Appointments.OrderBy(a => a.Number));*/

                    // Remove the card from the SecondScrollViewItems collection

                    SecondScrollViewItems.Remove(draggedAppointment);
                }
                else
                {
                    // The appointment already exists, you can show a message or handle it accordingly
                    MessageBox.Show("The appointment already exists.");
                }
            }

        }

        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            while (parentObject != null && !(parentObject is T))
            {
                parentObject = VisualTreeHelper.GetParent(parentObject);
            }

            return (T)parentObject;
        }

       public void InitializeAppointmentsListView()
        {
            if (Appointments != null) Appointments.Clear();
            using (var contextDb = new ClinikEntities())
            {
                int count = 0;
                var appointments = contextDb.Appointments.Include(ap => ap.Patient).ThenInclude(p => p.Person).Where(ap => ap.Date.Date == DateTime.Now.Date).ToList()
                    .Select((a, index) => new WaitingQViewModel(a.Patient.Person, a.Patient, a, index + 1));
                var s = contextDb.Treatments?.Include(x => x.Appointment).ThenInclude(pt => pt.Patient).ThenInclude(p => p .Person).Where(x => x.Appointment.Date.Date == DateTime.Now.Date).ToList()
                    .Select((a, index) => new WaitingQViewModel(a.Appointment.Patient.Person, a.Appointment.Patient, a.Appointment, index + 1));
                Appointments = new ObservableCollection<WaitingQViewModel>(appointments);
                Appointments = RemoveItemsFromAppointments(Appointments, PaymentScrollViewItems);
                Appointments = RemoveItemsFromAppointments(Appointments, new ObservableCollection<WaitingQViewModel>(s));

            }
        }
        public ObservableCollection<WaitingQViewModel> RemoveItemsFromAppointments(ObservableCollection<WaitingQViewModel> appointments, ObservableCollection<WaitingQViewModel> paymentScrollViewItems)
        {
            if (appointments != null && paymentScrollViewItems != null)
            {
                // Create a comparer based on the PersonEnst property
                var personEnstComparer = new WaitingQViewModelComparer();

                // Get the items that have the same PersonEnst property in both collections
                var commonItems = appointments.Intersect(paymentScrollViewItems, personEnstComparer).ToList();

                // Remove common items from the Appointments collection
              foreach (var item in commonItems)
                {
                    appointments.Remove(item);
                } 
            }
            return appointments;
        }
    }

    public class WaitingQViewModelComparer : IEqualityComparer<WaitingQViewModel>
    {
        public bool Equals(WaitingQViewModel x, WaitingQViewModel y)
        {
            // Check if PersonEnst properties are equal
            return x?.AppointmentEnst.AppointmentNum == y?.AppointmentEnst.AppointmentNum;
        }

        public int GetHashCode(WaitingQViewModel obj)
        {
            // Use the hash code of the PersonEnst property for hashing
            return obj?.AppointmentEnst.AppointmentNum?.GetHashCode() ?? 0;
        }
    }
}
