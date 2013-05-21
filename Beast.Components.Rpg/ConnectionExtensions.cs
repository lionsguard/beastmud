using Beast.Mobiles;

namespace Beast
{
    public static class ConnectionExtensions
    {
        /// <summary>
        /// The parameter key used to set or get an ICharacter instance from an IConnection.
        /// </summary>
        public const string ParamKeyCharacter = "Character";

        /// <summary>
        /// Gets the IMobile instance associated with the current connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static T Character<T>(this IConnection connection) where T : IMobile
        {
            return connection.Get<T>(ParamKeyCharacter, default(T));
        }

        /// <summary>
        /// Sets the IMobile instance associated with the current connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="character"></param>
        public static void Character<T>(this IConnection connection, T character) where T : IMobile
        {
            connection.Set(ParamKeyCharacter, character);
        }
    }
}
