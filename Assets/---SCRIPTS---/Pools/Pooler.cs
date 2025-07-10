using System.Collections.Generic;
using UnityEngine;

namespace Yg.Pooling
{
    public class Pooler<T> where T : MonoBehaviour
    {
        private Transform _parent;
        private readonly Dictionary<T, BasePool<T>> _poolDictionary = new();

        public Pooler(Transform parent)
        {
            _parent = parent;
        }

        public T Dequeue(T prefab)
        {
            if(prefab is null)
            {
                Debug.Log("Prefab is null");
                return null;
            }

            if(_poolDictionary.ContainsKey(prefab))
                return _poolDictionary[prefab].Dequeue();

            _poolDictionary.Add(prefab, new BasePool<T>(prefab, _parent));
            return _poolDictionary[prefab].Dequeue();
        }

        public void Enqueue(T prefab, T pooledObject)
        {
            if(!_poolDictionary.ContainsKey(prefab))
            {
                Debug.LogError("Pool wasn't found");
                return;
            }

            _poolDictionary[prefab].Enqueue(pooledObject);
        }
    }
}
