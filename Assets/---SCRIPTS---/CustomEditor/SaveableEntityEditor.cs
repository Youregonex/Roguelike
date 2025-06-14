using UnityEngine;
using Yg.SaveLoad;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SaveableEntity))]
public class SaveableEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SaveableEntity saveableEntity = (SaveableEntity)target;

        GUILayout.Space(10);

        GUI.enabled = string.IsNullOrEmpty(saveableEntity.Id);

        if (GUILayout.Button("Generate ID"))
        {
            saveableEntity.GenerateId();
            EditorUtility.SetDirty(saveableEntity);
        }

        GUI.enabled = true;
    }
}
#endif