using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(UniqueId))]
public class UniqueIdEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UniqueId uniqueId = (UniqueId)target;

        GUILayout.Space(10);

        GUI.enabled = string.IsNullOrEmpty(uniqueId.Id);

        if (GUILayout.Button("Generate ID"))
        {
            uniqueId.GenerateId();
            EditorUtility.SetDirty(uniqueId);
        }

        GUI.enabled = true;
    }
}

#endif