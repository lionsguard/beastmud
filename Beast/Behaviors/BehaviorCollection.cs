using System;
using System.Collections;
using System.Collections.Generic;

namespace Beast.Behaviors
{
	public class BehaviorCollection : ICollection<Behavior>
	{
		private readonly Dictionary<Type, Behavior> _behaviors = new Dictionary<Type, Behavior>();

		public GameObject Owner { get; private set; }

		public BehaviorCollection(GameObject owner)
		{
			Owner = owner;
		}

		public Behavior Find(Type type)
		{
			Behavior item;
			_behaviors.TryGetValue(type, out item);
			return item;
		}
		public T Find<T>() where T : Behavior
		{
			return (T)Find(typeof(T));
		}

		#region Implementation of IEnumerable

		public IEnumerator<Behavior> GetEnumerator()
		{
			return _behaviors.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of ICollection<Behavior>

		public void Add(Behavior item)
		{
			var key = item.GetType();
			if (_behaviors.ContainsKey(key))
			{
				var existing = _behaviors[key];
				existing.Detach();
				_behaviors.Remove(key);
			}

			item.Attach(Owner);
			_behaviors.Add(key, item);
		}

		public void Clear()
		{
			_behaviors.Clear();
		}

		public bool Contains(Behavior item)
		{
			return _behaviors.ContainsKey(item.GetType());
		}

		public void CopyTo(Behavior[] array, int arrayIndex)
		{
			_behaviors.Values.CopyTo(array, arrayIndex);
		}

		public bool Remove(Behavior item)
		{
			item.Detach();
			return _behaviors.Remove(item.GetType());
		}

		public int Count
		{
			get { return _behaviors.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		#endregion
	}
}