using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Clinik.Helpers
{
    public class HoursToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int selectedHour)
            {
                // Assuming that selectedHour is the hour from your ComboBox
                // You can adjust the format as needed
                return $"{selectedHour:00}:00";
            }

            return string.Empty; // or throw an exception if the conversion is not possible
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Implement this method if needed
            throw new NotImplementedException();
        }
    }
}
