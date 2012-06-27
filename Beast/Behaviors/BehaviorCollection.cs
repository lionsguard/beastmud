using System;
using System.Collections;
using System.Collections.Generic;

namespace Beast.Behaviors
{
	public class BehaviorCollection : ICollection<IBehavior>
	{
		private readonly Dictionary<Type, IBehavior> _behaviors = new Dictionary<Type, IBehavior>();

		public IGameObject Owner { get; private set; }

		public BehaviorCollection(IGameObject owner)
		{
			Owner = owner;
		}

		public IBehavior Find(Type type)
		{
			IBehavior item;
			_behaviors.TryGetValue(type, out item);
			return item;
		}
		public T Find<T>() where T : IBehavior
		{
			return (T)Find(typeof(T));
		}

		#region Implementation of IEnumerable

		public IEnumerator<IBehavior> GetEnumerator()
		{
			return _behaviors.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of ICollection<Behavior>

		public void Add(IBehavior item)
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

		public bool Contains(IBehavior item)
		{
			return _behaviors.ContainsKey(item.GetType());
		}

		public void CopyTo(IBehavior[] array, int arrayIndex)
		{
			_behaviors.Values.CopyTo(array, arrayIndex);
		}

		public bool Remove(IBehavior item)
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