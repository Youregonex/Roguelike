using System.Collections.Generic;
using UnityEngine;
using Yg.Factories;

namespace Yg.Pooling
{
    public class BasePool<T> where T : MonoBehaviour
    {
        private Queue<T> _poolQueue;
        private BaseFactory<T> _factory;

        private Transform _parent;

        public BasePool(T prefab, Transform parent = null, int createInitialObjectsAmount = 0)
        {
            _parent = parent;

            _factory = new(prefab, _parent);
            _poolQueue = new();

            if (createInitialObjectsAmount > 0)
                CreateInitialObjects(createInitialObjectsAmount);
        }

        public void Enqueue(T pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
            _poolQueue.Enqueue(pooledObject);
        }

        public T Dequeue()
        {
            T pooledObject;
            if (_poolQueue.Count > 0)
            {
                pooledObject = _poolQueue.Dequeue();
                pooledObject.gameObject.SetActive(true);
            }
            else
                pooledObject = _factory.Create();
            
            return pooledObject;
        }

        private void CreateInitialObjects(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                T poolObject = _factory.Create();
                Enqueue(poolObject);
            }
        }
    }
}
