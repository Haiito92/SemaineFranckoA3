using Localization.Runtime;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(LanguageManager))]
public class LanguageManagerEditor : Editor
{
    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LanguageManager languageManager = (LanguageManager)target;

        if (GUILayout.Button("Test"))
            languageManager.AddTextToList();
            
    }
}
