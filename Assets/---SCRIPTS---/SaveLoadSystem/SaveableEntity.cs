using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Yg.SaveLoad
{
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string _inspectorDisplayId;
        [SerializeField, HideInInspector] private string _id;

        public string Id => _id;

        public void GenerateId() => _id = Guid.NewGuid().ToString();

        public object CaptureState()
        {
            var data = new Dictionary<string, object>();

            foreach (var saveable in GetComponentsInChildren<ISaveable>())
            {
                data[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return data;
        }

        public void RestoreState(object state)
        {
            var data = state as Dictionary<string, object>
                ?? ((state is JObject jObject) ? jObject.ToObject<Dictionary<string, object>>() : null);

            if (data == null)
            {
                Debug.LogError("Data for state restoration in SaveableEntity is null!");
                return;
            }

            foreach (var saveable in GetComponentsInChildren<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();
                
                if (data.TryGetValue(typeName, out object loadedData))
                    saveable.RestoreState(loadedData);
            }
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
            {
                GenerateId();
                Debug.Log($"Generated new Id during validation for {gameObject.name}: {_id}");
            }

            _inspectorDisplayId = _id;
        }
    }
}