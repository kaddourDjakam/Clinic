using System;
using System.Globalization;
using System.Windows.Data;

namespace Clinik.Helpers
{
    public class DatetimeHoursConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                // Extract only hours and minutes as a TimeSpan
                TimeSpan time = dateTime.TimeOfDay;

                // Format the TimeSpan as "HH:mm"
                return time.ToString("HH:mm", culture);
            }

            return value; // or return string.Empty; depending on your use case
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
