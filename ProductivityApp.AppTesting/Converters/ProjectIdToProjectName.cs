using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using ProductivityApp.AppTesting.DataAccess;
using ProductivityApp.Model;
namespace ProductivityApp.AppTesting.Converters
{
    class ProjectIdToProjectName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dataAccess = new CrudOperations();

            if (!(value is Session session)) return "No Project Found";

            var projects = dataAccess.GetEntryFromDatabase<Project>("projects", session.ProjectId);
            return projects.Result.ProjectName;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        

    }
}
