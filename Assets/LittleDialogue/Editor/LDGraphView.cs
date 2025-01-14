using System;
using System.Collections.Generic;
using LittleDialogue.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace LittleDialogue.Editor
{
    public class LDGraphView : GraphView
    {
        private LDGraph m_graph;
        private SerializedObject m_serializedObject;
        private LDEditorWindow m_window;
        
        public LDEditorWindow Window => m_window;

        public List<LDEditorNode> GraphNodes { get; set; }
        public Dictionary<string, LDEditorNode> NodeDictionary { get; set; }

        private LDWindowSearchProvider m_searchProvider;
        
        public LDGraphView(SerializedObject serializedObject, LDEditorWindow window)
        {
            m_serializedObject = serializedObject;
            m_graph = (LDGraph)serializedObject.targetObject;
            m_window = window;

            GraphNodes = new List<LDEditorNode>();
            NodeDictionary = new Dictionary<string, LDEditorNode>();

            m_searchProvider = ScriptableObject.CreateInstance<LDWindowSearchProvider>();
            m_searchProvider.GraphView = this;
            this.nodeCreationRequest = ShowSearchWindow;
            
            StyleSheet style =
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/LittleDialogue/Editor/USS/LDEditor.uss");
            styleSheets.Add(style);
                
            GridBackground background = new GridBackground();
            background.name = "Grid";
            Add(background);
            background.SendToBack();
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            DrawNodes();
        }

        private void DrawNodes()
        {
            foreach (LDNode node in m_graph.Nodes)
            {
                AddNodeToGraph(node);
            }
        }

        private void ShowSearchWindow(NodeCreationContext obj)
        {
            m_searchProvider.Target = (VisualElement)focusController.focusedElement;
            SearchWindow.Open(new SearchWindowContext(obj.screenMousePosition), m_searchProvider);
        }

        public void Add(LDNode node)
        {
            Undo.RecordObject(m_serializedObject.targetObject, "Added Node");
            m_graph.Nodes.Add(node);
            m_serializedObject.Update();

            AddNodeToGraph(node);
        }

        private void AddNodeToGraph(LDNode node)
        {
            node.TypeName = node.GetType().AssemblyQualifiedName;

            LDEditorNode editorNode = new LDEditorNode(node);
            editorNode.SetPosition(node.Position);
            GraphNodes.Add(editorNode);
            NodeDictionary.Add(node.ID, editorNode);
            
            AddElement(editorNode);
        }
    }
}
