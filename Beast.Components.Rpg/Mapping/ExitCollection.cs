using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Beast.Mapping
{
	public class ExitCollection : ICollection<Exit>
	{
        private readonly Dictionary<KnownDirection, bool> _exits = new Dictionary<KnownDirection, bool>();

        public bool this[KnownDirection direction]
        {
            get
            {
                bool val;
                _exits.TryGetValue(direction, out val);
                return val;
            }
            set
            {
                _exits[direction] = value;
            }
        }

        public bool HasExit(KnownDirection direction)
		{
            if (!_exits.ContainsKey(direction))
                return false;
			return this[direction];
		}

        public void Add(KnownDirection direction)
        {
            this[direction] = true;
        }

        public void Add(Direction direction)
        {
            this[direction.Value] = true;
        }

        public void Add(Exit item)
        {
            _exits[item.Direction] = item.HasExit;
        }

        public void Clear()
        {
            _exits.Clear();
        }

        public bool Contains(Exit item)
        {
            return _exits.ContainsKey(item.Direction);
        }

        public void CopyTo(Exit[] array, int arrayIndex)
        {
            var items = _exits.Select(kvp => new Exit { Direction = kvp.Key, HasExit = kvp.Value }).ToList();

            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _exits.Values.Count(e => e); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Exit item)
        {
            return _exits.Remove(item.Direction);
        }

        public IEnumerator<Exit> GetEnumerator()
        {
            return _exits.Select(kvp => new Exit { Direction = kvp.Key, HasExit = kvp.Value }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}