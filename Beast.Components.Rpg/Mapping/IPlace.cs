using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Mapping
{
    public interface IPlace : IGameObject
    {
        Unit Location { get; set; }
        ExitCollection Exits { get; set; }
        int Terrain { get; set; }
        int Flags { get; set; }

        bool HasExit(KnownDirection direction);
    }
}
