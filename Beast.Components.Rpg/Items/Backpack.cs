using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Items
{
    public class Backpack : ItemContainer
    {
        public const int DefaultBackpackCapacity = 16;

        public Backpack()
            : base(DefaultBackpackCapacity)
        {
            Name = "Backpack";
            EquipLocation = Beast.Items.EquipLocation.Back;
        }
    }
}
