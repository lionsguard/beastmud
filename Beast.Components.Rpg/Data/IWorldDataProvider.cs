using Beast.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Data
{
    /// <summary>
    /// Represents an object that provides high level world data.
    /// </summary>
    public interface IWorldDataProvider
    {
        /// <summary>
        /// Gets a list of Terrain instances for the current world.
        /// </summary>
        /// <returns>A list of Terrain instances for the current world.</returns>
        IEnumerable<Terrain> GetTerrain();

        /// <summary>
        /// Saves a list of Terrain instances for the current world.
        /// </summary>
        /// <param name="terrain">A list of Terrain instances to save.</param>
        void SaveTerrain(IEnumerable<Terrain> terrain);

        /// <summary>
        /// Gets a list of flags for the current world.
        /// </summary>
        /// <returns>A list of flags for the current world.</returns>
        IEnumerable<PlaceFlag> GetPlaceFlags();

        /// <summary>
        /// Saves the flags for the current world.
        /// </summary>
        /// <param name="flags">The list of flags to save.</param>
        void SavePlaceFlags(IEnumerable<PlaceFlag> flags);
    }
}
