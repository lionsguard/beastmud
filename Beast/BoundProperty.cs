using System;

namespace Beast
{
	public class BoundProperty<T> where T : IComparable
	{
		private T _min;
		private T _max;
		private T _value;

		public T Minimum
		{
			get { return _min; }
			set 
			{ 
				_min = value;
				if (_min.CompareTo(_value) > 0)
					_value = _min;
			}
		}
		public T Maximum
		{
			get { return _max; }
			set
			{
				_max = value;
				if (_max.CompareTo(_max) <= 0)
					_min = _max;
				if (_value.CompareTo(_max) > 0)
					_value = _max;
			}
		}
		public T Value
		{
			get { return _value; }
			set
			{
				_value = value;
				if (_value.CompareTo(_min) < 0)
					_value = _min;
				if (_value.CompareTo(_max) > 0)
					_value = _max;
			}
		}

		public BoundProperty()
		{
			
		}

		public BoundProperty(T minimum, T maximim, T value)
		{
			if (minimum.CompareTo(maximim) > 0)
				throw new ArgumentOutOfRangeException("minimum");

			_min = minimum;
			_max = maximim;
			_value = value;
		}
	}
}