using SaveRuntime.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[CustomEditor(typeof(SaveDataBehaviour))]
public class SaveDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Open Save Tool"))
        {
            SaveEditorTool.ShowWindow((SaveDataBehaviour)target);
        }
        GUILayout.Label("Double click on Scriptable Object to open tool faster");
        EditorGUILayout.EndVertical();

        base.OnInspectorGUI();
        
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int instanceIndex)
    {
        var assetOfSo = EditorUtility.InstanceIDToObject(instanceId);
        if (assetOfSo.GetType() == typeof(SaveDataBehaviour))
        {
            SaveEditorTool.ShowWindow((SaveDataBehaviour)assetOfSo);
            return true;
        }
        return false;
    }
}
