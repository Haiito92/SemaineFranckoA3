using Localization.Runtime;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(LanguageManager))]
public class LanguageManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LanguageManager languageManager = (LanguageManager)target;

        if (GUILayout.Button("Fetch texts from scene"))
            languageManager.AddTextToList();

        if (GUILayout.Button("Clear list"))
            languageManager.ClearTextList();
            
    }
}
