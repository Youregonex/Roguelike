using System;
using UnityEngine;

public class UniqueId : MonoBehaviour
{
    [SerializeField] private string _inspectorDisplayId;
    [SerializeField, HideInInspector] private string _id;

    public string Id => _id;

    public void GenerateId() => _id = Guid.NewGuid().ToString();

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
