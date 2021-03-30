using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;

using ProductivityApp.AppTesting.Core.Models;
using ProductivityApp.AppTesting.Core.Services;

namespace ProductivityApp.AppTesting.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        public ObservableCollection<DataPoint> Source { get; } = new ObservableCollection<DataPoint>();

        public ChartViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await SampleDataService.GetChartDataAsync();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
