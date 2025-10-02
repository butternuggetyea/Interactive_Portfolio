using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace LightingBox.Effects
{
    public static class ConfigUtility
    {
        private static string filePath = "Assets/LightingBox2/Settings/SaveData.txt";

        /// <summary>
        /// Reads all configurations from the file.
        /// </summary>
        /// <returns>A list of configurations as (sceneName, configPath) tuples.</returns>
        public static List<(string sceneName, string configPath)> GetAllConfigs()
        {
            EnsureFileExists();
            var configs = new List<(string sceneName, string configPath)>();
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2)
                {
                    configs.Add((parts[0].Trim(), parts[1].Trim()));  // Trim to avoid spaces
                }
            }
            return configs;
        }

        /// <summary>
        /// Gets the config path for a specific scene.
        /// </summary>
        /// <param name="sceneName">The name of the scene to search for.</param>
        /// <returns>The config path if found; null otherwise.</returns>
        public static string GetConfig(string sceneName)
        {
            EnsureFileExists();
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2 && parts[0].Trim() == sceneName.Trim())  // Trim spaces before comparison
                {
                    return parts[1].Trim(); // Return the configPath if found
                }
            }

            Debug.LogWarning($"Config for scene '{sceneName}' not found.");  // Debug log for missing scene
            return null; // Return null if not found
        }

        /// <summary>
        /// Adds a new configuration to the file.
        /// </summary>
        public static void AddConfig(string sceneName, string configPath)
        {
            EnsureFileExists();
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{sceneName}|{configPath}");
            }
        }

        /// <summary>
        /// Updates an existing configuration.
        /// </summary>
        public static void UpdateConfig(string sceneName, string newConfigPath)
        {
            EnsureFileExists();
            var configs = GetAllConfigs();

            for (int i = 0; i < configs.Count; i++)
            {
                if (configs[i].sceneName == sceneName)
                {
                    configs[i] = (sceneName, newConfigPath);
                    break;
                }
            }

            SaveAllConfigs(configs);
        }

        /// <summary>
        /// Removes a configuration from the file.
        /// </summary>
        public static void RemoveConfig(string sceneName)
        {
            EnsureFileExists();
            var configs = GetAllConfigs();

            configs.RemoveAll(c => c.sceneName == sceneName);

            SaveAllConfigs(configs);
        }

        private static void EnsureFileExists()
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, string.Empty);
            }
        }

        private static void SaveAllConfigs(List<(string sceneName, string configPath)> configs)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var config in configs)
                {
                    writer.WriteLine($"{config.sceneName}|{config.configPath}");
                }
            }
        }
    }
}