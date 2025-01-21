using SaveRuntime.Editor;
using SaveRuntime.Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace SaveRuntime.Editor
{
    [CustomEditor(typeof(SaveDataBehaviour))]
    public class SaveDataEditor : UnityEditor.Editor
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
}

