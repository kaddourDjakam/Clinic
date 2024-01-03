using Clinik.Model;
using Clinik.View.Rendez_vous.Cards;
using Clinik.View.WorkSpace.Cards;
using Clinik.ViewModel.Rendez_vous.Cards;
using Clinik.ViewModel.WorkSpace;
using Clinik.ViewModel.WorkSpace.Cards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clinik.View.WorkSpace
{
    /// <summary>
    /// Interaction logic for WorkSpaceView.xaml
    /// </summary>
    public partial class WorkSpaceView : UserControl
    {
      

        public WorkSpaceView()
        {
            InitializeComponent();
        }

        private void SecondScrollViewer_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ApointmentHomeCardView)))
            {
                ApointmentHomeCardView draggedCard = e.Data.GetData(typeof(ApointmentHomeCardView)) as ApointmentHomeCardView;

                if (draggedCard != null)
                {
                    (DataContext as WorkSpaceViewModel).HandleDrop(draggedCard.DataContext as WaitingQViewModel);

                    // Remove the card from the first ScrollViewer
                    var firstScrollViewer = FindVisualParent<ScrollViewer>(draggedCard);
                    if (firstScrollViewer != null)
                    {
                        var appointmentsList = firstScrollViewer.DataContext as ObservableCollection<ApointmentCardViewModel>;
                        appointmentsList?.Remove(draggedCard.DataContext as ApointmentCardViewModel);
                    }
                }
            }
        }

        private void FirstScrollViewer_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(WaitingQCardView)))
            {
                // Get the dropped ApointmentCardView
                WaitingQCardView droppedCard = e.Data.GetData(typeof(WaitingQCardView)) as WaitingQCardView;

                if (droppedCard != null)
                {
                    // Get the ScrollViewer reference from the drag-and-drop data
                    ScrollViewer sourceScrollViewer = e.Data.GetData("ParentScrollViewer") as ScrollViewer;

                    // Handle the drop in the ViewModel
                    (DataContext as WorkSpaceViewModel).HandleDropBack(droppedCard.DataContext as WaitingQViewModel);
                }
            }
        }
        //
        private void PaymentScrollViewer_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(WaitingQCardView)))
            {
                // Get the dropped ApointmentCardView
                WaitingQCardView droppedCard = e.Data.GetData(typeof(WaitingQCardView)) as WaitingQCardView;

                if (droppedCard != null)
                {
                    // Get the ScrollViewer reference from the drag-and-drop data
                    ScrollViewer sourceScrollViewer = e.Data.GetData("ParentScrollViewer") as ScrollViewer;

                    // Handle the drop in the ViewModel
                    (DataContext as WorkSpaceViewModel).HandleDropToPayment(droppedCard.DataContext as WaitingQViewModel);
                }
            }
        }

        private static T FindVisualParent<T>(Visual element) where T : Visual
        {
            while (element != null && !(element is T))
            {
                element = (Visual)VisualTreeHelper.GetParent(element);
            }

            return (T)element;
        }
    }
}
