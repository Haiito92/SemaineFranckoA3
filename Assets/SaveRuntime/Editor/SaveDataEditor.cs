using SaveRuntime.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveDataBehaviour))]
public class SaveDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Save Tool"))
        {
            SaveEditorTool.ShowWindow((SaveDataBehaviour)serializedObject.targetObject);
        }
        base.OnInspectorGUI();
    }
    
}
