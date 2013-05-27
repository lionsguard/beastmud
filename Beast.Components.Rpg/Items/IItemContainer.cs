using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Items
{
    public interface IItemContainer : IItem
    {
        int Capacity { get; set; }
        int Count { get; }
        IEnumerable<IItem> Items { get; set; }

        void Add(IItem item);
        IItem Find(string alias);
        void Remove(IItem item);
        void Clear();
    }
}
