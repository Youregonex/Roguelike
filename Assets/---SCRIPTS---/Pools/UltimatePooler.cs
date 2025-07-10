using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Yg.Pooling
{
    public class UltimatePooler
    {
        private readonly Transform _pooledObjectParentTransform;
        private readonly Dictionary<Type, object> _poolersDictionary = new();

        [Inject]
        public UltimatePooler(Transform parent)
        {
            _pooledObjectParentTransform = parent;
        }

        public T Dequeue<T>(T prefab) where T : MonoBehaviour
        {
            return GetPooler<T>().Dequeue(prefab);
        }

        public void Enqueue<T>(T prefab, T poolObject, bool reParent = false) where T : MonoBehaviour
        {
            if (poolObject is null) Debug.Log("Enqueued null object");
            GetPooler<T>().Enqueue(prefab, poolObject);

            if (reParent)
                poolObject.transform.SetParent(_pooledObjectParentTransform);
        }

        private Pooler<T> GetPooler<T>() where T : MonoBehaviour
        {
            var type = typeof(T);

            if (!_poolersDictionary.TryGetValue(type, out var poolerObj))
            {
                var newPooler = new Pooler<T>(_pooledObjectParentTransform);
                _poolersDictionary[type] = newPooler;
                Debug.Log($"Created new Pooler of type {type}");
                return newPooler;
            }

            return (Pooler<T>)poolerObj;
        }
    }
}
