using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Clinik.Helpers
{
    public class DateTimeToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.TimeOfDay;
            }

            return TimeSpan.Zero; // or throw an exception if the conversion is not possible
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                // Assuming you have a reference date (e.g., DateTime.Today) to combine with the time
                return DateTime.Today + timeSpan;
            }

            return DateTime.MinValue; // or throw an exception if the conversion is not possible
        }
    }
}
