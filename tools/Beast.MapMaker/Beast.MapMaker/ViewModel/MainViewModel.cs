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
    public class MainViewModel : ViewModelBase, ITerrainService, IPlaceService
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
                if (value == _selectedTile)
                    return;

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

        public ObservableCollection<TileViewModel> Tiles { get; set; }

        private IMapService _mapService;
        private IFileService _fileService;
        private IDialogService _dialogService;

        private Dispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Tiles = new ObservableCollection<TileViewModel>();

            SimpleIoc.Default.Register<ITerrainService>(() => this);
            SimpleIoc.Default.Register<IPlaceService>(() => this);

            _dispatcher = Dispatcher.CurrentDispatcher;

            _mapService = ServiceLocator.Current.GetInstance<IMapService>();
            _fileService = ServiceLocator.Current.GetInstance<IFileService>();
            _dialogService = ServiceLocator.Current.GetInstance<IDialogService>();

            NewMapCommand = new RelayCommand(() => 
            {
                FileName = string.Empty;

                var vm = new MapViewModel();
                if (_dialogService.ShowDialog<NewMapDialog>(vm) == true)
                {
                    Map = vm;
                }
            });
            OpenMapCommand = new RelayCommand(() => 
            {
                var file = _fileService.OpenFile("Open Map", "Map files|*.json");
                if (!string.IsNullOrEmpty(file))
                {
                    FileName = file;
                    Map = new MapViewModel(_mapService.GetMap(FileName));
                }
            });
            SaveMapCommand = new RelayCommand(() => 
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    FileName = _fileService.SaveFile("Save Map", "Map files|*.json", "json");
                }

                if (string.IsNullOrEmpty(FileName))
                    return;

                _mapService.SaveMap(Map.BackingMap, FileName);
            }, () => Map != null);
            ExitCommand = new RelayCommand(() => ServiceLocator.Current.GetInstance<IShutdownService>().Shutdown());

            EditTerrainCommand = new RelayCommand(() =>
            {
            }, () => Map != null);
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

                if (start.Location.DistanceTo(end.Location) > 1)
                    return;

                var dir = Direction.FromPoints(start.Location, end.Location);

                previousTile.AddExit(dir);
                _selectedTile.AddExit(dir.Counter());
            }
        }

        #region Terrain
        public TerrainViewModel GetTerrain(int id)
        {
            if (Map == null)
                return null;
            return Map.Terrain.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<TerrainViewModel> GetAllTerrain()
        {
            if (Map == null)
                return new TerrainViewModel[0];
            return Map.Terrain;
        }

        public void SaveTerrain(TerrainViewModel terrain)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        public void SetPlace(IPlace place)
        {
            if (Map == null || Map.BackingMap == null)
                return;

            Map.BackingMap[place.Location] = place;
        }
    }

    public enum EditorActionType
    {
        Select,
        Draw,
        Fill
    }
}