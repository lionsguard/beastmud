using Beast.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Beast
{
    /// <summary>
    /// Provides settings for configuring the Application.
    /// </summary>
    public class ApplicationSettings : Dictionary<string, object>
    {
        /// <summary>
        /// Gets a default ApplicationSettings configuration.
        /// </summary>
        public static readonly ApplicationSettings Default = CreateDefault();

        /// <summary>
        /// Gets or sets a collection of directory paths to search in order to load external components.
        /// </summary>
        public List<string> ComponentDirectories { get; set; }

        /// <summary>
        /// Gets or sets a collection of assemblies that contain external components.
        /// </summary>
        public List<Assembly> ComponentAssemblies { get; set; }

        /// <summary>
        /// Gets or sets the root path in which to write temp files, data files, etc.
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// Gets or sets the current update interval.
        /// </summary>
        public TimeSpan UpdateInterval { get; set; }

        /// <summary>
        /// Gets or sets the timeout period for connections tracked by the application.
        /// </summary>
        public TimeSpan ConnectionTimeout { get; set; }

        /// <summary>
        /// Initializes a new instance of the ApplicationSettings class.
        /// </summary>
        public ApplicationSettings()
        {
            UpdateInterval = TimeSpan.FromSeconds(10);
            ConnectionTimeout = TimeSpan.FromMinutes(20);
            ComponentDirectories = new List<string>();
            ComponentAssemblies = new List<Assembly>();
        }

        /// <summary>
        /// Gets the value for the specified key cast to the specified type or defaultValue if the key was not present in the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of cast the result.</typeparam>
        /// <param name="key">The key to find within the dictionary.</param>
        /// <param name="defaultValue">A defaultValue to return if the key was not found.</param>
        /// <returns>A value for the specified key or defaultValue if the key was not found.</returns>
        public T GetValue<T>(string key, T defaultValue)
        {
            object val;
            if (!TryGetValue(key, out val))
                return defaultValue;

            return ValueConverter.Convert<T>(val);
        }

        private static ApplicationSettings CreateDefault()
        {
            var config = new ApplicationSettings
                {
                    RootPath = Path.GetDirectoryName(typeof(Application).Assembly.Location),
                    ComponentDirectories = new List<string>
                    {
                        "."
                    }
                };

            LoadConfigAppSettings(config);

            return config;
        }

        /// <summary>
        /// Creates an ApplicationSettings instance from configuration.
        /// </summary>
        /// <returns>An ApplicationSettings instance from configuration.</returns>
        public static ApplicationSettings FromConfig()
        {
            var section = ConfigurationManager.GetSection("beast") as BeastSection;
            if (section != null && section.Settings != null)
            {
                var settings = section.Settings;

                var config = new ApplicationSettings
                    {
                        RootPath = settings.RootPath
                    };

                TimeSpan updateInterval;
                if (TimeSpan.TryParse(settings.UpdateInterval, out updateInterval))
                    config.UpdateInterval = updateInterval;
                TimeSpan timeout;
                if (TimeSpan.TryParse(settings.ConnectionTimeout, out timeout))
                    config.ConnectionTimeout = timeout;

                foreach (AssemblyElement element in settings.ComponentAssemblies)
                {
                    try
                    {
                        var asm = Assembly.Load(AssemblyName.GetAssemblyName(element.File));
                        config.ComponentAssemblies.Add(asm);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Failed to load assembly for file '{0}': {1}", element.File, ex);
                    }
                }

                foreach (DirectoryElement dir in settings.ComponentDirectories)
                {
                    if (string.IsNullOrEmpty(dir.Path))
                        continue;

                    config.ComponentDirectories.Add(dir.Path);
                }

                // Include appSettings as well.
                LoadConfigAppSettings(config);

                return config;
            }
            return null;
        }

        /// <summary>
        /// Adds all the configuration appSettings to the specified settings instance.
        /// </summary>
        /// <param name="settings">The ApplicationSettings to load.</param>
        public static void LoadConfigAppSettings(ApplicationSettings settings)
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                settings[key] = ConfigurationManager.AppSettings[key];
            }
        }
    }
}
