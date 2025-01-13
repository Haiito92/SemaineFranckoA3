using LittleDialogue.Runtime;
using UnityEditor;
using UnityEngine;

namespace LittleDialogue.Editor
{
    public class LDEditorWindow : EditorWindow
    {
        [MenuItem("Tools/LD Graph")]
        public static void Open()
        {
            LDEditorWindow window = GetWindow<LDEditorWindow>();
            window.titleContent = new GUIContent("Little Dialogue Graph");
        }
        
        public static void Open(LDGraph target)
        {
            // LDEditorWindow window = CreateWindow<LDEditorWindow>()
        }
    }
}
