using Beast.Mapping;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace Beast.MapMaker.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        private Map _backingMap;
        private const string PropertyNameBackingMap = "BackingMap";
        public Map BackingMap   
        {
            get { return _backingMap; }
            set
            {
                if (value == _backingMap)
                    return;

                _backingMap = value;

                LoadTerrain();

                RaisePropertyChanged(PropertyNameBackingMap);
                RaisePropertyChanged(PropertyNameName);
                RaisePropertyChanged(PropertyNameWidth);
                RaisePropertyChanged(PropertyNameHeight);
                RaisePropertyChanged(PropertyNameMinLevel);
                RaisePropertyChanged(PropertyNameMaxLevel);
            }
        }

        public string Name
        {
            get { return BackingMap.Name; }
            set 
            {
                BackingMap.Name = value;
                RaisePropertyChanged(PropertyNameName);
            }
        }
        private const string PropertyNameName = "Name";
        

        public int Width
        {
            get { return BackingMap.Width; }
            set 
            {
                BackingMap.Width = value;
                RaisePropertyChanged(PropertyNameWidth);
            }
        }
        private const string PropertyNameWidth = "Width";

        public int Height
        {
            get { return BackingMap.Height; }
            set
            {
                BackingMap.Height = value;
                RaisePropertyChanged(PropertyNameHeight);
            }
        }
        private const string PropertyNameHeight = "Height";

        public int MinLevel
        {
            get { return BackingMap.MinLevel; }
            set 
            {
                BackingMap.MinLevel = value;
                RaisePropertyChanged(PropertyNameMinLevel);
            }
        }
        private const string PropertyNameMinLevel = "MinLevel";

        public int MaxLevel
        {
            get { return BackingMap.MaxLevel; }
            set 
            {
                BackingMap.MaxLevel = value;
                RaisePropertyChanged(PropertyNameMaxLevel);
            }
        }
        private const string PropertyNameMaxLevel = "MaxLevel";

        public ObservableCollection<TerrainViewModel> Terrain { get; set; }

        public MapViewModel()
            : this(new Map
            {
                Width = 20,
                Height = 20,
                MinLevel = 1,
                MaxLevel = 5
            })
        {
        }
        
        public MapViewModel(Map map)
        {
            Terrain = new ObservableCollection<TerrainViewModel>();
            BackingMap = map;
        }

        private void LoadTerrain()
        {
            if (BackingMap == null)
                return;

            Terrain.Clear();
            foreach (var terrain in BackingMap.Terrain)
            {
                Terrain.Add(new TerrainViewModel(terrain));
            }
        }
    }
}
