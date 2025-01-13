using LittleDialogue.Runtime;
using UnityEditor;
using UnityEngine;

namespace LittleDialogue.Editor
{
    public class LDEditorWindow : EditorWindow
    {
        private LDGraph m_currentGraph;
        private SerializedObject m_serializedObject;
        private LDGraphView m_currentView;

        public LDGraph CurrentGraph => m_currentGraph;
        
        
        // [MenuItem("Tools/LD Graph")]
        // public static void Open()
        // {
        //     LDEditorWindow window = GetWindow<LDEditorWindow>();
        //     window.titleContent = new GUIContent("Little Dialogue Graph");
        // }
        
        public static void Open(LDGraph target)
        {
            LDEditorWindow[] windows = Resources.FindObjectsOfTypeAll<LDEditorWindow>();
            foreach (LDEditorWindow window in windows)
            {
                if (window.CurrentGraph == target)
                {
                    window.Focus();
                    return;
                }
            }

            LDEditorWindow newWindow = CreateWindow<LDEditorWindow>(typeof(LDEditorWindow), typeof(SceneView));
            newWindow.titleContent = new GUIContent($"{target.name}");
            newWindow.Load(target);
        }

        private void Load(LDGraph target)
        {
            m_currentGraph = target;
            DrawGraph();
        }

        private void DrawGraph()
        {
            m_serializedObject = new SerializedObject(m_currentGraph);
            m_currentView = new LDGraphView(m_serializedObject, this);
            rootVisualElement.Add(m_currentView);
        }
    }
}
