using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Mobiles
{
    public interface IMobileProvider : IInitializable
    {
        T GetMobile<T>(Func<T, bool> predicate) where T : IMobile;
        IEnumerable<T> GetMobiles<T>(Func<T, bool> predicate) where T : IMobile;
        void SaveMobile<T>(T mobile) where T : IMobile;
    }
}
