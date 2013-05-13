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
        /// <param name="location"></param>
        /// <param name="item"></param>
        /// <param name="container"></param>
        public void Equip(EquipLocation location, IItem item, IItemContainer container)
        {
            var index = (int)location;
            if (index == 0)
                return;

            // Remove the item from the container
            container.Remove(item);

            // if the slot is equipped, unequp it, adding the item to the current container.
            if (!Contains(index))
            {
                Unequip(location, _items[index], container);
            }

            _items[index] = item;
        }

        /// <summary>
        /// Unequips the specified item from the specified location and adds it to the container.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="item"></param>
        /// <param name="container"></param>
        public void Unequip(EquipLocation location, IItem item, IItemContainer container)
        {
            var index = (int)location;
            if (index == 0)
                return;

            if (!Contains(index))
                return;

            try
            {
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
            try
            {
                return _items[index] != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
