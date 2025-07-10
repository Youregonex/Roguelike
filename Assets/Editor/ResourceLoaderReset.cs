using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ResourceLoaderReset
{
    static ResourceLoaderReset()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            Debug.Log("Resetting ResourceLoader after exiting play mode");
            ResourceLoader.Clear();
        }
    }
}