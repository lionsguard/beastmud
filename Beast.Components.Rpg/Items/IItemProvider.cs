using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Items
{
    public interface IItemProvider : IInitializable
    {
        T GetItem<T>(Func<T, bool> predicate) where T : IItem;
        IEnumerable<T> GetItems<T>(Func<T, bool> predicate) where T : IItem;
        void SaveItem<T>(T item) where T : IItem;
    }
}
