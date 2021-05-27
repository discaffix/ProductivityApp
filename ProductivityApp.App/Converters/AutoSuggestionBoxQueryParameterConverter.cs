using ProductivityApp.Model;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace ProductivityApp.App.Converters
{
    class AutoSuggestionBoxQueryParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = (AutoSuggestBoxQuerySubmittedEventArgs)value;

            return (Project)args.ChosenSuggestion;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
