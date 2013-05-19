using Beast.MapMaker.Services;
using Beast.Mapping;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Beast.MapMaker.ViewModel
{
    public class TileViewModel : BackingObjectViewModel<IPlace>
    {
        public const int TileSize = 32;

        private const string PropertyNameHasPlace = "HasPlace";
        public bool HasPlace    
        {
            get { return BackingObject != null; }
        }
        
        private int _screenX;
        private const string PropertyNameScreenX = "ScreenX";
        public int ScreenX
        {   
            get { return _screenX; }
            set
            {
                if (value == _screenX)
                    return;

                _screenX = value;

                RaisePropertyChanged(PropertyNameScreenX);
            }
        }

        private int _screenY;
        private const string PropertyNameScreenY = "ScreenY";
        public int ScreenY
        {
            get { return _screenY; }
            set
            {
                if (value == _screenY)
                    return;

                _screenY = value;

                RaisePropertyChanged(PropertyNameScreenY);
            }
        }

        private const string PropertyNameTerrain = "Terrain";
        public TerrainViewModel Terrain 
        {
            get { return ServiceLocator.Current.GetInstance<ITerrainService>().GetTerrain(GetPropertyValue(p => p.Terrain)); }
            set
            {
                SetPropertyValue(p => p.Terrain, value != null ? value.Id : 0);
                RaisePropertyChanged(PropertyNameTerrain);
            }
        }
        

        private int _x;
        private const string PropertyNameX = "X";
        public int X
        {   
            get { return _x; }
            set
            {
                if (value == _x)
                    return;

                _x = value;

                RaisePropertyChanged(PropertyNameX);
            }
        }

        private int _y;
        private const string PropertyNameY = "Y";
        public int Y
        {
            get { return _y; }
            set
            {
                if (value == _y)
                    return; 

                _y = value;

                RaisePropertyChanged(PropertyNameY);
            }
        }

        private bool _isSelected;
        private const string PropertyNameIsSelected = "IsSelected";
        public bool IsSelected  
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;

                RaisePropertyChanged(PropertyNameIsSelected);
            }
        }

        public string Name
        {
            get { return GetPropertyValue(p => p.Name); }
            set
            {
                SetPropertyValue(p => p.Name, value);
                RaisePropertyChanged(PropertyNameName);
            }
        }
        private const string PropertyNameName = "Name";

        public string Description
        {
            get { return GetPropertyValue(p => p.Description); }
            set
            {
                SetPropertyValue(p => p.Description, value);
                RaisePropertyChanged(PropertyNameDescription);
            }
        }
        private const string PropertyNameDescription = "Description";

        public int Depth
        {
            get { return GetMapStartPropertyValue(o => o.Depth); }
            set
            {
                SetMapStartPropertyValue(o => o.Depth, value);
                RaisePropertyChanged(PropertyNameDepth);
            }
        }
        private const string PropertyNameDepth = "Depth";

        public int Seed
        {
            get { return GetMapStartPropertyValue(o => o.Seed); }
            set
            {
                SetMapStartPropertyValue(o => o.Seed, value);
                RaisePropertyChanged(PropertyNameSeed);
            }
        }
        private const string PropertyNameSeed = "Seed";

        public bool ZIndexUp
        {
            get { return GetMapStartPropertyValue(o => o.ZIndexUp); }
            set
            {
                SetMapStartPropertyValue(o => o.ZIndexUp, value);
                RaisePropertyChanged(PropertyNameZIndexUp);
            }
        }
        private const string PropertyNameZIndexUp = "ZIndexUp";

        public int MinLevel
        {
            get { return GetMapStartPropertyValue(o => o.MinLevel); }
            set
            {
                SetMapStartPropertyValue(o => o.MinLevel, value);
                RaisePropertyChanged(PropertyNameMinLevel);
            }
        }
        private const string PropertyNameMinLevel = "MinLevel";

        public int MaxLevel
        {
            get { return GetMapStartPropertyValue(o => o.MaxLevel); }
            set
            {
                SetMapStartPropertyValue(o => o.MaxLevel, value);
                RaisePropertyChanged(PropertyNameMaxLevel);
            }
        }
        private const string PropertyNameMaxLevel = "MaxLevel";

        public int Width
        {
            get { return GetMapStartPropertyValue(o => o.Width); }
            set
            {
                SetMapStartPropertyValue(o => o.Width, value);
                RaisePropertyChanged(PropertyNameWidth);
            }
        }
        private const string PropertyNameWidth = "Width";

        public int Height
        {
            get { return GetMapStartPropertyValue(o => o.Height); }
            set
            {
                SetMapStartPropertyValue(o => o.Height, value);
                RaisePropertyChanged(PropertyNameHeight);
            }
        }
        private const string PropertyNameHeight = "Height";
        
        private const string PropertyNameIsMapStart = "IsMapStart";
        public bool IsMapStart  
        {
            get { return BackingObject is MapStart; }
            set
            {
                if (BackingObject == null)
                    return;

                if (value)
                {
                    if (!(BackingObject is MapStart))
                    {
                        var svc = ServiceLocator.Current.GetInstance<IPlaceService>();
                        BackingObject = MapStart.FromPlace(BackingObject);
                        svc.SetPlace(BackingObject);
                    }
                }
                else
                {
                    if (BackingObject is MapStart)
                    {
                        var svc = ServiceLocator.Current.GetInstance<IPlaceService>();
                        BackingObject = (BackingObject as MapStart).ToPlace();
                        svc.SetPlace(BackingObject);
                    }
                }

                RaisePropertyChanged(PropertyNameIsMapStart);
            }
        }
        
        public ObservableCollection<bool> Exits { get; set; }

        private TileViewModel()
        {
            Exits = new ObservableCollection<bool>(new[]
            {
                false, false, false, false, false, false, false, false, false, false
            });
        }

        public TileViewModel(Unit position)
            : this()
        {
            SetPosition(position);
            SetExits(new ExitCollection());
        }

        public TileViewModel(IPlace place)
            : this()
        {
            BackingObject = place;
        }

        protected override void OnBackingObjectChanged()
        {
            RaisePropertyChanged(PropertyNameTerrain);

            var exits = BackingObject != null ? BackingObject.Exits : new ExitCollection();

            SetExits(exits);

            RaisePropertyChanged(PropertyNameHasPlace);
            RaisePropertyChanged(PropertyNameTerrain);
            RaisePropertyChanged(PropertyNameIsMapStart);
        }

        private void SetPosition(Unit position)
        {
            X = position.X;
            Y = position.Y;
            ScreenX = position.X * TileSize;
            ScreenY = position.Y * TileSize;
        }

        private void SetExits(ExitCollection exits)
        {
            for (int i = 0; i < 10; i++)
            {
                var kd = (KnownDirection)i;
                Exits[i] = exits.HasExit(kd);
            }
        }

        public void AddExit(Direction dir)
        {
            if (BackingObject == null)
                return;

            BackingObject.Exits.Add(dir);
            SetExits(BackingObject.Exits);
        }

        public override bool Equals(object obj)
        {
            var vm = obj as TileViewModel;
            if (vm == null)
                return false;
            return vm.X == X && vm.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        private T GetMapStartPropertyValue<T>(Expression<Func<MapStart, T>> expression)
        {
            if (BackingObject == null)
                return default(T);

            var start = BackingObject as MapStart;
            if (start == null)
                return default(T);

            var property = ReflectionExtensions.GetProperty(expression);
            if (property == null)
                return default(T);

            return (T)property.GetValue(start);
        }

        private void SetMapStartPropertyValue<T>(Expression<Func<MapStart, T>> expression, T value)
        {
            if (BackingObject == null)
                return;

            var start = BackingObject as MapStart;
            if (start == null)
                return;

            var property = ReflectionExtensions.GetProperty(expression);
            if (property == null)
                return;

            property.SetValue(start, value);
        }
    }
}
