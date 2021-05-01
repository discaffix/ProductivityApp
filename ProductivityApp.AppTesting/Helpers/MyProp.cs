using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityApp.AppTesting.Helpers
{
    public class MyProp<T>
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if(Equals(_value, value)) return;

                _value = value;
            }
        }
    }
}
