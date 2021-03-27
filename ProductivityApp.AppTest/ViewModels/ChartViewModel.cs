using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;

using ProductivityApp.AppTest.Core.Models;
using ProductivityApp.AppTest.Core.Services;

namespace ProductivityApp.AppTest.ViewModels
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
