using System.Collections.Generic;

namespace Beast.Items
{
    public class ItemContainer : Item, IItemContainer
    {
        public const int DefaultCapacity = 8;

        public int Capacity { get; set; }

        public int Count
        {
            get { return _items.Count; }
        }

        private List<IItem> _items;
        public IEnumerable<IItem> Items
        {
            get { return _items; }
            set { _items = new List<IItem>(value); }
        }

        public ItemContainer()
            : this(DefaultCapacity)
        {
        }

        public ItemContainer(int capacity)
        {
            Capacity = capacity;
            _items = new List<IItem>();
        }

        public void Add(IItem item)
        {
            if (Capacity == _items.Count)
                throw new ItemContainerFullException();
            _items.Add(item);
        }

        public IItem Find(string alias)
        {
            return FindItem(alias, _items);
        }

        public void Remove(IItem item)
        {
            _items.Remove(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        internal static IItem FindItem(string alias, IEnumerable<IItem> items)
        {
            foreach (var item in items)
            {
                if (item == null)
                    continue;

                if (item is IItemContainer)
                {
                    var childItem = (item as IItemContainer).Find(alias);
                    if (childItem != null)
                        return childItem;
                }

                if (item.Equals(alias))
                    return item;
            }
            return null;
        }
    }
}
