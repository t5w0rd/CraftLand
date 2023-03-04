using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Editor Settings")]
public class EditorSettings : ScriptableObject
{
    [SerializeField] private bool clickToUpdateSettings;
    [HideInInspector] [Scene] public List<string> scenePaths;

    private void OnValidate()
    {
        clickToUpdateSettings = false;
        UpdateSettings();
    }

    private void UpdateSettings()
    {
#if UNITY_EDITOR
        _ = clickToUpdateSettings;
        Debug.Log("Update EditorSettings");
        if (scenePaths == null)
        {
            scenePaths = new();
        }
        scenePaths.Clear();
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        for (int i = 0; i < scenes.Length; i++)
        {
            scenePaths.Add(scenes[i].path);
        }
#endif
    }
}
