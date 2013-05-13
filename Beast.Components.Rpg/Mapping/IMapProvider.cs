
namespace Beast.Mapping
{
    /// <summary>
    /// Defines a provider the save and retrieve maps.
    /// </summary>
    public interface IMapProvider : IInitializable
    {
        /// <summary>
        /// Gets the specified map by name.
        /// </summary>
        /// <param name="name">The name of the map to find.</param>
        /// <returns>A Map instance for the specified name if found; otherwise null.</returns>
        Map GetMap(string name);

        /// <summary>
        /// Saves the specified map.
        /// </summary>
        /// <param name="map">The Map instance to save.</param>
        void SaveMap(Map map);
    }
}
