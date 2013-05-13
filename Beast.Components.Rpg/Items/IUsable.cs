using Beast.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Items
{
    public interface IUsable : IItem
    {
        void Use(IMobile user);
    }
}
