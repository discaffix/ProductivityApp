using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace ProductivityApp.AppTesting.Converters
{
    class AutoSuggestTextChangedParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = (AutoSuggestBox) value;

            return args;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
