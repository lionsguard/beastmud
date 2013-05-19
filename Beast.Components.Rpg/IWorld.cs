using Beast.Mapping;
using System.Collections.Generic;

namespace Beast
{
    public interface IWorld : IInitializable
    {
        Application App { get; }
        IEnumerable<Terrain> Terrain { get; }
        IEnumerable<PlaceFlag> PlaceFlags { get; }
    }
}
