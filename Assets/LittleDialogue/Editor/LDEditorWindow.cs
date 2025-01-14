using System;
using LittleDialogue.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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

        private void OnEnable()
        {
            if (m_currentGraph != null)
            {
                DrawGraph();
            }
        }

        private void OnGUI()
        {
            if (m_currentGraph != null)
            {
                if (EditorUtility.IsDirty(m_currentGraph))
                {
                    this.hasUnsavedChanges = true;
                }
                else
                {
                    this.hasUnsavedChanges = false;
                }
            }
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
            m_currentView.graphViewChanged += OnChange;
            rootVisualElement.Add(m_currentView);
        }

        private GraphViewChange OnChange(GraphViewChange graphviewchange)
        {
            EditorUtility.SetDirty(m_currentGraph);
            return graphviewchange;
        }

    }
}
