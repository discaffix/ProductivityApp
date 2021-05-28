using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace ProductivityApp.App.Converters
{
    internal class TrackingStateToSymbol : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Symbol.Pause : Symbol.Play;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
