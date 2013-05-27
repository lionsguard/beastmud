using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Items
{
    public class Body
    {
        private IItem[] _items = new IItem[(int)EquipLocation.Count];
        public IEnumerable<IItem> Items
        {
            get { return _items; }
            set 
            {
                var array = value.ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    _items[i] = array[i];
                }
            }
        }

        public IItem this[EquipLocation location]
        {
            get { return Contains((int)location) ? _items[(int)location] : null; }
        }

        public Body()
        {
            _items.Initialize();
        }

        public bool IsEquipped(EquipLocation location)
        {
            return Contains((int)location);
        }

        /// <summary>
        /// Equips the specified item to the specified location and removes from the container.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        public void Equip(IItem item, IItemContainer container)
        {
            var index = (int)item.EquipLocation;
            if (index < 0)
                return;

            // Remove the item from the container
            if (container != null)
                container.Remove(item);

            // if the slot is equipped, unequp it, adding the item to the current container.
            if (!Contains(index))
            {
                Unequip(_items[index], container);
            }

            _items[index] = item;
        }

        /// <summary>
        /// Unequips the specified item from the specified location and adds it to the container.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        public void Unequip(IItem item, IItemContainer container)
        {
            if (item == null)
                return;

            var index = (int)item.EquipLocation;
            if (index < 0)
                return;

            if (!Contains(index))
                return;

            try
            {
                if (container != null)
                    container.Add(item);
            }
            catch (ItemContainerFullException)
            {
                return;
            }

            _items[index] = null;
        }

        public bool Contains(int index)
        {
            if (index < 0 || index > (_items.Length - 1))
                return false;

            return _items[index] != null;
        }

        public IItem Find(string alias)
        {
            return ItemContainer.FindItem(alias, _items);
        }
    }
}
