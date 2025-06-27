using UnityEngine;

namespace Yg.Factories
{
    public class BaseFactory<T> where T : MonoBehaviour
    {
        private T _prefab;
        private Transform _parent;

        public BaseFactory(T prefab, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public T Create()
        {
            T createdObject = GameObject.Instantiate(_prefab);

            if(_parent is not null)
                createdObject.transform.SetParent(_parent);

            return createdObject;
        }
    }
}
