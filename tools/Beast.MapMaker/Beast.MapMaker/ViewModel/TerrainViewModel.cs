using Beast.Mapping;

namespace Beast.MapMaker.ViewModel
{
    public class TerrainViewModel : BackingObjectViewModel<Terrain>
    {
        public int Id
        {
            get { return GetPropertyValue(o => o.Id); }
            set
            {
                SetPropertyValue(o => o.Id, value);
                RaisePropertyChanged(PropertyNameId);
            }
        }
        private const string PropertyNameId = "Id";

        public string Name
        {
            get { return GetPropertyValue(o => o.Name); }
            set
            {
                SetPropertyValue(o => o.Name, value);
                RaisePropertyChanged(PropertyNameName);
            }
        }
        private const string PropertyNameName = "Name";

        public string Color
        {
            get { return GetPropertyValue(o => o.Color); }
            set
            {
                SetPropertyValue(o => o.Color, value);
                RaisePropertyChanged(PropertyNameColor);
            }
        }
        private const string PropertyNameColor = "Color";
        
        public TerrainViewModel(Terrain terrain)
            : base(terrain)
        {
        }
    }
}
