using LittleDialogue.Runtime;
using UnityEditor;
using UnityEngine;

namespace LittleDialogue.Editor
{
    [CustomEditor(typeof(LDGraph))]
    public class LDGraphEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                LDEditorWindow.Open((LDGraph)target);
            }
        }
    }
}
