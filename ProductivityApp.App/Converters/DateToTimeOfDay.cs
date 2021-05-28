using System;
using Windows.UI.Xaml.Data;

namespace ProductivityApp.App.Converters
{
    internal class DateToTimeOfDay : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTimeOffset)value;

            return date.ToString("HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
