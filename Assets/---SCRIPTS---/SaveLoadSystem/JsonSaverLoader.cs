using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Yg.SaveLoad
{
    public class JsonSaverLoader : IDataSaverLoader
    {
        private const string SAVE_DIRECTORY_NAME = "Save Files";

        private string PersistentDataPath => Application.persistentDataPath;

        public JsonSaverLoader()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = { new Vector2IntConverter() }
            };
        }

        public void SaveData(string key, object data)
        {
            string path = BuildPath(key);
            string json = JsonConvert.SerializeObject(data, JsonConvert.DefaultSettings());

            try
            {
                using StreamWriter filestream = new(path);
                filestream.Write(json);
            }
            catch(Exception e)
            {
                Debug.LogWarning($"Saving error: {e.Message}, StackTrace: {e.StackTrace}");
            }
        }

        public T LoadData<T>(string key)
        {
            string path = BuildPath(key);

            try
            {
                using StreamReader filestream = new(path);
                string json = filestream.ReadToEnd();
                T data = JsonConvert.DeserializeObject<T>(json, JsonConvert.DefaultSettings());
                return data;
            }
            catch(Exception e)
            {
                Debug.LogWarning($"Error: {e.Message}, StackTrace: {e.StackTrace}");
                return default;
            }
        }

        private string BuildPath(string key)
        {
            EnsureDirectoryExists();
            return Path.Combine(PersistentDataPath, SAVE_DIRECTORY_NAME, key);
        }

        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(Path.Combine(PersistentDataPath, SAVE_DIRECTORY_NAME)))
                Directory.CreateDirectory(Path.Combine(PersistentDataPath, SAVE_DIRECTORY_NAME));
        }
    }
}