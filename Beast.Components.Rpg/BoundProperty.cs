using System;

namespace Beast
{
    public class BoundProperty<T> where T : IComparable
    {
        private T _minimum;
        public T Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;

                if (_minimum.CompareTo(_maximum) > 0)
                    throw new ArgumentOutOfRangeException("Minimum", "The minimum value must be less than the maximum.");

                ClampValue();
            }
        }

        private T _maximum;
        public T Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;

                if (_maximum.CompareTo(_minimum) < 0)
                    throw new ArgumentOutOfRangeException("Maximum", "The minimum value must be less than the maximum.");

                ClampValue();
            }
        }

        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ClampValue();
            }
        }

        public BoundProperty()
        {
        }
        public BoundProperty(T min, T max, T value)
        {
            _minimum = min;
            _maximum = max;
            _value = value;
            ClampValue();
        }

        private void ClampValue()
        {
            if (_value.CompareTo(_minimum) < 0)
                _value = _minimum;
            if (_value.CompareTo(_maximum) > 0)
                _value = _maximum;
        }
    }
}
