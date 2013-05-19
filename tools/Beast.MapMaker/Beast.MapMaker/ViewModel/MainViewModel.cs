using Beast.MapMaker.Dialogs;
using Beast.MapMaker.Services;
using Beast.Mapping;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Beast.MapMaker.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IPlaceService, ITerrainService
    {
        private MapViewModel _map;
        private const string PropertyNameMap = "Map";
        public MapViewModel Map
        {   
            get { return _map; }
            set
            {
                if (value == _map)
                    return;

                _map = value;

                CreateTiles();

                RaisePropertyChanged(PropertyNameMap);
            }
        }

        private string _fileName;
        private const string PropertyNameFileName = "FileName";
        public string FileName
        {   
            get { return _fileName; }
            set
            {
                if (value == _fileName)
                    return;

                _fileName = value;

                RaisePropertyChanged(PropertyNameFileName);
            }
        }

        private int _currentZ;
        private const string PropertyNameCurrentZ = "CurrentZ";
        public int CurrentZ
        {   
            get { return _currentZ; }
            set
            {
                if (value == _currentZ)
                    return;

                _currentZ = value;

                LoadMapLevel();

                RaisePropertyChanged(PropertyNameCurrentZ);

            }
        }

        private TileViewModel _selectedTile;
        private const string PropertyNameSelectedTile = "SelectedTile";
        public TileViewModel SelectedTile   
        {
            get { return _selectedTile; }
            set
            {
                TileViewModel previousTile = null;
                if (_selectedTile != null)
                {
                    previousTile = _selectedTile;
                    _selectedTile.IsSelected = false;
                }

                _selectedTile = value;

                if (_selectedTile != null)
                    _selectedTile.IsSelected = true;

                OnTileSelectionChanged(previousTile);

                RaisePropertyChanged(PropertyNameSelectedTile);
            }
        }

        private TerrainViewModel _selectedTerrain;
        private const string PropertyNameSelectedTerrain = "SelectedTerrain";
        public TerrainViewModel SelectedTerrain 
        {
            get { return _selectedTerrain; }
            set
            {
                if (value == _selectedTerrain)
                    return;

                _selectedTerrain = value;

                RaisePropertyChanged(PropertyNameSelectedTerrain);
            }
        }
        
        private EditorActionType _editorAction = EditorActionType.Select;
        private const string PropertyNameEditorAction = "EditorAction";
        public EditorActionType EditorAction    
        {
            get { return _editorAction; }
            set
            {
                if (value == _editorAction)
                    return;

                _editorAction = value;

                RaisePropertyChanged(PropertyNameEditorAction);
                RaisePropertyChanged(PropertyNameIsEditorActionSelect);
                RaisePropertyChanged(PropertyNameIsEditorActionDraw);
                RaisePropertyChanged(PropertyNameIsEditorActionFill);
            }
        }

        private const string PropertyNameIsEditorActionSelect = "IsEditorActionSelect";
        public bool IsEditorActionSelect
        {
            get { return _editorAction == EditorActionType.Select; }
            set 
            {
                if (value)
                    EditorAction = EditorActionType.Select;
            }
        }

        private const string PropertyNameIsEditorActionDraw = "IsEditorActionDraw";
        public bool IsEditorActionDraw
        {
            get { return _editorAction == EditorActionType.Draw; }
            set
            {
                if (value)
                    EditorAction = EditorActionType.Draw;
            }
        }

        private const string PropertyNameIsEditorActionFill = "IsEditorActionFill";
        public bool IsEditorActionFill
        {
            get { return _editorAction == EditorActionType.Fill; }
            set
            {
                if (value)
                    EditorAction = EditorActionType.Fill;
            }
        }

        private bool _isTunneling;
        private const string PropertyNameIsTunneling = "IsTunneling";
        public bool IsTunneling 
        {
            get { return _isTunneling; }
            set
            {
                if (value == _isTunneling)
                    return;

                _isTunneling = value;

                RaisePropertyChanged(PropertyNameIsTunneling);
            }
        }

        public ICommand NewMapCommand { get; set; }
        public ICommand OpenMapCommand { get; set; }
        public ICommand SaveMapCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand EditTerrainCommand { get; set; }

        public ICommand MoveCommand { get; set; }

        public ObservableCollection<TileViewModel> Tiles { get; set; }
        public ObservableCollection<TerrainViewModel> Terrain { get; set; }
        public ObservableCollection<PlaceFlagViewModel> PlaceFlags { get; set; }

        private IMapService _mapService;
        private IFileService _fileService;
        private IDialogService _dialogService;
        private IWorldDataService _worldDataService;

        private Dispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Tiles = new ObservableCollection<TileViewModel>();
            Terrain = new ObservableCollection<TerrainViewModel>();
            PlaceFlags = new ObservableCollection<PlaceFlagViewModel>();

            foreach (var terrain in DependencyResolver.Resolve<IWorld>().Terrain.OrderBy(t => t.Name))
            {
                Terrain.Add(new TerrainViewModel(terrain));
            }
            foreach (var flag in DependencyResolver.Resolve<IWorld>().PlaceFlags.Where(f => f.Value > 0).OrderBy(f => f.Value))
            {
                PlaceFlags.Add(new PlaceFlagViewModel
                {
                    Name = string.Format("{0} [{1}]", flag.Name, flag.Value),
                    Value = flag.Value
                });
            }

            SimpleIoc.Default.Register<IPlaceService>(() => this);
            SimpleIoc.Default.Register<ITerrainService>(() => this);

            _dispatcher = Dispatcher.CurrentDispatcher;

            _mapService = ServiceLocator.Current.GetInstance<IMapService>();
            _fileService = ServiceLocator.Current.GetInstance<IFileService>();
            _dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
            _worldDataService = ServiceLocator.Current.GetInstance<IWorldDataService>();

            NewMapCommand = new RelayCommand(() => 
            {
                FileName = string.Empty;
                _currentZ = 0;
                
                var vm = new MapViewModel();
                if (_dialogService.ShowDialog<NewMapDialog>(vm) == true)
                {
                    Map = vm;
                }
            });
            OpenMapCommand = new RelayCommand(() =>
            {
                _currentZ = 0;
                Map = new MapViewModel(_mapService.GetMap());
            });
            SaveMapCommand = new RelayCommand(() => 
            {
                _mapService.SaveMap(Map.BackingMap);
            }, () => Map != null);
            ExitCommand = new RelayCommand(() => ServiceLocator.Current.GetInstance<IShutdownService>().Shutdown());

            EditTerrainCommand = new RelayCommand(() =>
            {
            }, () => Map != null);

            MoveCommand = new RelayCommand<string>(s =>
                {
                    KnownDirection dir;
                    if (Enum.TryParse(s, true, out dir))
                    {
                        TryMoveOnZ(Direction.FromKnownDirection(dir));
                    }
                }, s => Map != null);
        }

        private void CreateTiles()
        {
            if (Map == null)
                return;

            Tiles.Clear();

            Task.Run(() =>
                {
                    try
                    {
                        var tiles = new List<TileViewModel>();
                        for (var y = 0; y < Map.Height; y++)
                        {
                            for (var x = 0; x < Map.Width; x++)
                            {
                                tiles.Add(new TileViewModel(new Unit(x, y, CurrentZ)));
                            }
                        }
                        _dispatcher.BeginInvoke(new Action<List<TileViewModel>>(list =>
                            {
                                foreach (var tile in list)
                                {
                                    Tiles.Add(tile);
                                }
                                LoadMapLevel();
                            }), DispatcherPriority.Background, tiles);
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessageBox("ERROR", ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                });
        }

        private void LoadMapLevel()
        {
            if (Map == null)
                return;

            var places = Map.BackingMap.Where(p => p.Location.Z == CurrentZ).ToDictionary(p => p.Location, p => p);

            for (var y = 0; y < Map.Height; y++)
            {
                for (var x = 0; x < Map.Width; x++)
                {
                    var pos = new Unit(x, y, CurrentZ);

                    IPlace place;
                    places.TryGetValue(pos, out place);

                    var tile = Tiles.FirstOrDefault(t => t.X == pos.X && t.Y == pos.Y);
                    if (tile == null)
                        return;

                    tile.BackingObject = place;
                }
            }
        }

        private void OnTileSelectionChanged(TileViewModel previousTile)
        {
            if (_selectedTile == null)
                return;

            // Flags
            foreach (var flag in PlaceFlags)
            {
                flag.BackingObject = _selectedTile.BackingObject;
            }

            var terrain = SelectedTerrain;

            switch (EditorAction)
            {   
                case EditorActionType.Select:
                    break;
                case EditorActionType.Draw:
                    var place = _selectedTile.BackingObject;
                    if (place == null)
                    {
                        place = new Place
                        {
                            Location = new Unit(_selectedTile.X, _selectedTile.Y, CurrentZ),
                            Terrain = terrain != null ? terrain.Id : 0
                        };
                        _selectedTile.BackingObject = place;
                        Map.BackingMap.Add(place);

                        if (Map.BackingMap.Start == null)
                            Map.BackingMap.Start = place;
                    }
                    else
                    {
                        _selectedTile.Terrain = terrain;
                    }
                    break;
                case EditorActionType.Fill:
                    break;
            }

            if (IsTunneling && previousTile != null)
            {
                var start = previousTile.BackingObject;
                var end = _selectedTile.BackingObject;

                if (start == null || end == null)
                    return;

                if (start.Location.Z != end.Location.Z)
                {
                    if (Math.Abs(start.Location.Z - end.Location.Z) == 1)
                    {
                        TunnelExit(Direction.FromUnit(end.Location), previousTile, _selectedTile);
                    }
                    return;
                }

                if (start.Location.DistanceTo(end.Location) > 1)
                    return;

                var dir = Direction.FromPoints(start.Location, end.Location);

                TunnelExit(dir, previousTile, _selectedTile);
            }
        }

        private void TunnelExit(Direction direction, TileViewModel start, TileViewModel end)
        {
            start.AddExit(direction);
            end.AddExit(direction.Counter());
        }

        private void TryMoveOnZ(Direction direction)
        {
            var currentTile = SelectedTile ?? new TileViewModel(Unit.Zero);

            if (direction.Value == KnownDirection.Up || direction.Value == KnownDirection.Down)
            {
                var x = currentTile.X;
                var y = currentTile.Y;

                var z = CurrentZ + direction.Unit.Z;

                // See if a place exists in the given direction.
                var pos = new Unit(x, y, z);
                if (IsTunneling)
                {
                    var dest = Map.BackingMap[pos];
                    if (dest == null)
                    {
                        // Create new place
                        dest = new Place
                        {
                            Location = pos
                        };
                        Map.BackingMap.Add(dest);
                    }
                }

                var currentPlace = Map.BackingMap[new Unit(x, y, CurrentZ)];
                CurrentZ = z; // Reloads the map

                var wasTunneling = IsTunneling;

                // Disable tunneling before selection
                IsTunneling = false;
                SelectedTile = Tiles.FirstOrDefault(t => t.X == pos.X && t.Y == pos.Y);
                IsTunneling = wasTunneling;

                if (IsTunneling && SelectedTile != null && currentPlace != null)
                {
                    IsTunneling = false;

                    currentPlace.Exits[direction.Value] = true;
                    SelectedTile.AddExit(direction.Counter());

                    IsTunneling = true;
                }
            }
        }

        public void SetPlace(IPlace place)
        {
            if (Map == null || Map.BackingMap == null)
                return;

            Map.BackingMap[place.Location] = place;
        }

        public TerrainViewModel GetTerrain(int id)
        {
            return Terrain.FirstOrDefault(t => t.Id == id);
        }
    }

    public enum EditorActionType
    {
        Select,
        Draw,
        Fill
    }
}