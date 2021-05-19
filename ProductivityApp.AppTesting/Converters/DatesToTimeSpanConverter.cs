using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using ProductivityApp.Model;
using static System.String;

namespace ProductivityApp.AppTesting.Converters
{
    public class DatesToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return "";
            
            var selectedSession = (Session) value;
            var span = (selectedSession.EndTime - selectedSession.StartTime);

            return $@"{span:hh\:mm\:ss}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
