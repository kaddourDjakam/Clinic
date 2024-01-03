using Clinik.Commands;
using Clinik.Model;
using Clinik.Repository.DataContext;
using Clinik.Services;
using Clinik.View.WorkSpace.Payment;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Clinik.ViewModel.WorkSpace.Cards
{
    public class WaitingQViewModel: ViewModelBase
    {
        public Person PersonEnst { get; set; }
        public PatientModel PatientEnst { get; set; }
        public Appointment AppointmentEnst { get; set; }
        // New property to track the parent ScrollViewer
        public ScrollViewer ParentScrollViewer { get; set; }

        private decimal _Amount;

        public decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; OnPropertyChanged(nameof(Amount)); }
        }

        private decimal _PayedAmount;

        public decimal PayedAmount
        {
            get { return _PayedAmount; }
            set { _PayedAmount = value; OnPropertyChanged(nameof(PayedAmount)); }
        }

        public int Age { get; set; }
        public int Number { get; set; }
        public ICommand PaymentAppointment_Cmd { get; set; }
        public ICommand OnSelectPhotoClick { get; set; }
        public ICommand OnConfirmClick { get; set; }

        private BitmapImage _selectedImage;

        public BitmapImage SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                if (_selectedImage != value)
                {
                    _selectedImage = value;
                    OnPropertyChanged(nameof(SelectedImage));
                }
            }
        }
        public WaitingQViewModel(Person person, PatientModel patientModel, Appointment appointment,int number)
        {
            PersonEnst = person;
            PatientEnst = patientModel;
            AppointmentEnst = appointment;
            Age = (int)((DateTime.Now.Date - PatientEnst.Birthday).TotalDays / 365.25);
            Number = number;

            PaymentAppointment_Cmd = new RelayCommand(PaymentAppointmentFunc);
            OnSelectPhotoClick = new RelayCommand(OnSelectPhotoClickFunc);
            OnConfirmClick = new RelayCommand(OnConfirmClickFunc);
        }
        public PaymentWorkSpaceWindowView _paymentWorkSpaceWindowView { get; set; }
        void PaymentAppointmentFunc()
        {
            _paymentWorkSpaceWindowView = new PaymentWorkSpaceWindowView() { DataContext = this};
            _paymentWorkSpaceWindowView.ShowDialog(); 

        }
        // Helper method to close the PaymentWorkSpaceWindowView
        private void CloseWindow()
        {
            _paymentWorkSpaceWindowView.Close();
 
        }
        void OnConfirmClickFunc()
        {
           
            using (var contextDb = new ClinikEntities())
            {
                try
                {
                    var payment = new Payment() { Amount = Amount, Date = DateTime.Now };
                    var trt = new Treatment()
                    {
                        AppointmentID = AppointmentEnst.AppointmentID,
                        Payment = payment,
                        Price = PayedAmount,


                    };


                    var p = new Prescription() { Treatment = trt, PrescriptionImage = ConvertImage(SelectedImage) };

                    contextDb.Add(p);
                    contextDb.SaveChanges();

                    _updatePaymentList.Invoke(this);

                    MessageService.ShowSuccessMessage("Paiement réussi !", Application.Current.MainWindow);
                    // Use Task.Run to execute the remaining code in the background without blocking the UI thread
                    Task.Run(async () =>
                    {
                        // Wait for 2 seconds
                        await Task.Delay(2000);

                        // Run the remaining code on the UI thread
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            CloseWindow();
                        });
                        ;
                    });
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
        }
        byte[] ConvertImage(BitmapImage selectedImage)
        {
            // Convert BitmapImage to byte array and assign it to PrescriptionImage
            var bitmapImage = selectedImage;
            var encoder = new JpegBitmapEncoder(); // You can choose another encoder based on your image format
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
            
        }
        void OnSelectPhotoClickFunc()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        private Action<WaitingQViewModel> _updatePaymentList { get; set; }
        public WaitingQViewModel(Person person, PatientModel patientModel, Appointment appointment, int number, Action<WaitingQViewModel> action)
        {
            PersonEnst = person;
            PatientEnst = patientModel;
            AppointmentEnst = appointment;
            Age = (int)((DateTime.Now.Date - PatientEnst.Birthday).TotalDays / 365.25);
            Number = number;
            _updatePaymentList = action;
            PaymentAppointment_Cmd = new RelayCommand(PaymentAppointmentFunc);
            OnSelectPhotoClick = new RelayCommand(OnSelectPhotoClickFunc);
            OnConfirmClick = new RelayCommand(OnConfirmClickFunc);
        }

    }
   
}
