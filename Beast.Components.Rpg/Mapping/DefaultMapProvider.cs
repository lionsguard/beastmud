﻿using Beast.Data;
using System.Collections.Generic;
using System.Linq;

namespace Beast.Mapping
{
    public class DefaultMapProvider : DefaultDataProvider, IMapProvider
    {
        public const string MapsDirectoryName = "maps";

        public virtual Map GetMap(string name)
        {
            return GetMapFromFile(GetFileName(name));
        }
        public Map GetMapFromFile(string fileName)
        {
            var file = GetObject<MapFile>(fileName);
            if (file == null)
                return null;
            return file.GetMap();
        }

        public virtual void SaveMap(Map map)
        {
            SaveMapToFile(map, GetFileName(map.Name));
        }
        public void SaveMapToFile(Map map, string fileName)
        {
            var file = new MapFile(map);
            SaveObject(file, fileName);
        }

        private string GetFileName(string name)
        {
            return GetPath(MapsDirectoryName, string.Concat(name, ".json"));
        }

        private class MapFile
        {
            public string Name { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public Unit Start { get; set; }
            public Map MapData { get; set; }

            public MapFile()
            {
            }

            public MapFile(Map map)
            {
                Name = map.Name;
                Width = map.Width;
                Height = map.Height;
                Start = map.Start != null ? map.Start.Location : Unit.Empty;
                MapData = map;
            }

            public Map GetMap()
            {
                var map = MapData;
                map.Name = Name;
                map.Width = Width;
                map.Height = Height;
                map.Start = map[Start];
                return map;
            }
        }
    }
}
