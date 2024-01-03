using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clinik.View.Rendez_vous
{
    /// <summary>
    /// Interaction logic for Rendez_vousView.xaml
    /// </summary>
    public partial class Rendez_vousView : UserControl
    {
        public Rendez_vousView()
        {
            InitializeComponent();
          
        }
        private void OpenCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (this.calendarPopup.IsOpen)
            {
                calendarPopup.IsOpen = false;
            }
            else
            {
                calendarPopup.IsOpen = true;
            }
        }
        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Close the Popup when the date is changed
            calendarPopup.IsOpen = false;
        }
    }
        
    
}
