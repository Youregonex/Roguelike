using UnityEngine;
using Yg.Battle;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SpellSO), true)]
public class SpellSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SpellSO spellSO = (SpellSO)target;

        GUILayout.Space(10);

        GUI.enabled = string.IsNullOrEmpty(spellSO.SpellId);

        if (GUILayout.Button("Generate ID"))
        {
            spellSO.GenerateId();
            EditorUtility.SetDirty(spellSO);
        }

        GUI.enabled = true;
    }
}

#endif