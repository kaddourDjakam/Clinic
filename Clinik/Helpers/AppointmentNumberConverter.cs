using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Clinik.Helpers
{
    public class AppointmentNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int appointmentNumber)
            {
                // Format the appointment number as "N°{0}"
                return string.Format(culture, "N°{0}", appointmentNumber);
            }

            return value; // or return string.Empty; depending on your use case
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
