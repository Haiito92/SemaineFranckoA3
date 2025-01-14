using LittleDialogue.Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace LittleDialogue.Editor
{
    [CustomEditor(typeof(LDGraph))]
    public class LDGraphEditor : UnityEditor.Editor
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int index)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceId);
            if (asset.GetType() == typeof(LDGraph))
            {
                LDEditorWindow.Open((LDGraph)asset);
                return true;
            }

            return false;
        }
        
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                LDEditorWindow.Open((LDGraph)target);
            }
        }
    }
}
