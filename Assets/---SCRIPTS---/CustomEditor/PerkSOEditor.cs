using UnityEngine;
using Yg.GameData.Perks;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PerkSO), true)]
public class PerkSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PerkSO perkSO = (PerkSO)target;

        GUILayout.Space(10);

        GUI.enabled = string.IsNullOrEmpty(perkSO.PerkId);

        if (GUILayout.Button("Generate ID"))
        {
            perkSO.GenerateId();
            EditorUtility.SetDirty(perkSO);
        }

        GUI.enabled = true;
    }
}

#endif