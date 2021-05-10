using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace ProductivityApp.AppTesting.Helpers
{
    public class MyProp<T> : INotifyPropertyChanged
    {
        private T _value;
        public event PropertyChangedEventHandler PropertyChanged;

        public T Value
        {
            get => _value;
            set
            {
                if(Equals(_value, value)) return;

                _value = value;
                OnPropertyChanged(nameof(_value));
            }
        }

        public static implicit operator T(MyProp<T> value)
        {
            return value.Value;
        }

        public static implicit operator MyProp<T>(T value)
        {
            return new MyProp<T> { Value = value };
        }

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
