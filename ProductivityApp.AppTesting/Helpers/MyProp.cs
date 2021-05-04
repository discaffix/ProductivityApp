using GalaSoft.MvvmLight;

namespace ProductivityApp.AppTesting.Helpers
{
    public class MyProp<T> : ObservableObject
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if(Equals(_value, value)) return;

                _value = value;
                RaisePropertyChanged();
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
    }
}
