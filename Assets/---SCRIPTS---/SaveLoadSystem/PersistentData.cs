using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Yg.SaveLoad;

namespace Yg.GameData
{
    public class PersistentData
    {
        private const string SAVE_FILE_NAME = "GameData.json";
        private IDataSaverLoader _dataSaverLoader;

        private Dictionary<string, object> _saveData;

        public PersistentData()
        {
            _dataSaverLoader = new JsonSaverLoader();
            _saveData = _dataSaverLoader.LoadData<Dictionary<string, object>>(SAVE_FILE_NAME);
            Debug.Log($"Persistent data. Loaded Json:\n {JsonConvert.SerializeObject(_saveData)}");
        }

        public void SaveData()
        {
            CaptureState();
            _dataSaverLoader.SaveData(SAVE_FILE_NAME, _saveData);
            Debug.Log("Data Saved");
        }

        public void LoadData()
        {
            RestoreState();
            Debug.Log("Data Loaded");
        }

        public void StartNewGame()
        {
            Debug.Log("Starting new game");
            _saveData = new Dictionary<string, object>();
            _dataSaverLoader.SaveData(SAVE_FILE_NAME, _saveData);
        }

        public bool SaveFileExists() => _saveData != null;
        public bool DataIsEmpty() => _saveData.Count == 0;

        public void CaptureState()
        {
            if (_saveData == null) _saveData = new Dictionary<string, object>();

            foreach (var saveableEntity in GameObject.FindObjectsOfType<SaveableEntity>())
                _saveData[saveableEntity.Id] = saveableEntity.CaptureState();
        }

        public void RestoreState()
        {
            if (_saveData == null)
            {
                Debug.LogError("Can't RestoreState, data is null!");
                return;
            }

            foreach (var saveableEntity in GameObject.FindObjectsOfType<SaveableEntity>())
                if (_saveData.TryGetValue(saveableEntity.Id, out object data))
                    saveableEntity.RestoreState(data);
        }
    }
}