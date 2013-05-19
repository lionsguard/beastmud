using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Mapping
{
    public struct PlaceFlag
    {
        private int _value;
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public PlaceFlag(string name, int value)
        {
            _name = name;
            _value = value;
        }

        #region Known Flags
        public static PlaceFlag None
        {
            get { return new PlaceFlag("None", (int)KnownPlaceFlags.None); }
        }
        public static PlaceFlag Safe
        {
            get { return new PlaceFlag("Safe", (int)KnownPlaceFlags.Safe); }
        }
        public static PlaceFlag Dark
        {
            get { return new PlaceFlag("Dark", (int)KnownPlaceFlags.Dark); }
        }

        public static IEnumerable<PlaceFlag> All
        {
            get { return new[] { None, Safe, Dark }; }
        }
        #endregion
    }
}
