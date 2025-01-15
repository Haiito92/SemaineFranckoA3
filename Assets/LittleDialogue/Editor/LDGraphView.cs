using System;
using System.Collections.Generic;
using System.Linq;
using LittleDialogue.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
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

        private List<LDEditorNode> m_graphNodes;
        private Dictionary<string, LDEditorNode> m_nodeDictionary;
        private Dictionary<Edge, LDConnection> m_connectionDictionary;

        private LDWindowSearchProvider m_searchProvider;
        
        public LDGraphView(SerializedObject serializedObject, LDEditorWindow window)
        {
            m_serializedObject = serializedObject;
            m_graph = (LDGraph)serializedObject.targetObject;
            m_window = window;

            m_graphNodes = new List<LDEditorNode>();
            m_nodeDictionary = new Dictionary<string, LDEditorNode>();
            m_connectionDictionary = new Dictionary<Edge, LDConnection>();
            m_connectionDictionary = new Dictionary<Edge, LDConnection>();

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
            DrawConnections();

            graphViewChanged += OnGraphViewChangedEvent;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> allPorts = new List<Port>();
            List<Port> ports = new List<Port>();

            foreach (LDEditorNode editorNode in m_graphNodes)
            {
                allPorts.AddRange(editorNode.Ports);
            }

            foreach (Port port in allPorts)
            {
                if(port == startPort) continue;
                if(port.node == startPort.node) continue;
                if(port.direction == startPort.direction) continue;
                if (port.portType == startPort.portType)
                {
                    ports.Add(port);
                }
            }
            
            return ports;
        }

        private GraphViewChange OnGraphViewChangedEvent(GraphViewChange graphViewChange)
        {
            if (graphViewChange.movedElements != null)
            {
                Undo.RecordObject(m_serializedObject.targetObject, "Moved Elements");

                foreach (LDEditorNode editorNode in graphViewChange.movedElements.OfType<LDEditorNode>())
                {
                    editorNode.SavePosition();
                }
            }
            
            if (graphViewChange.elementsToRemove != null)
            {
                Undo.RecordObject(m_serializedObject.targetObject, "Removed Stuff From Graph");
                
                List<LDEditorNode> editorNodes = graphViewChange.elementsToRemove.OfType<LDEditorNode>().ToList();
                if (editorNodes.Count > 0)
                {
                    for (int i = editorNodes.Count - 1; i >= 0; i--)
                    {
                        RemoveNode(editorNodes[i]);
                    }
                }

                foreach (Edge edge in graphViewChange.elementsToRemove.OfType<Edge>())
                {
                    RemoveConnection(edge);
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                Undo.RecordObject(m_serializedObject.targetObject, "Added Connections");
                foreach (Edge edge in graphViewChange.edgesToCreate)
                {
                    CreateEdge(edge);
                }
            }
            
            return graphViewChange;
        }

        private void CreateEdge(Edge edge)
        {
            LDEditorNode inputNode = (LDEditorNode)edge.input.node;
            int inputIndex = inputNode.Ports.IndexOf(edge.input);
            
            LDEditorNode outputNode = (LDEditorNode)edge.output.node;
            int outputIndex = outputNode.Ports.IndexOf(edge.output);

            LDConnection connection = new LDConnection(inputNode.Node.ID, inputIndex, outputNode.Node.ID, outputIndex);
            m_graph.Connections.Add(connection);
            m_connectionDictionary.Add(edge, connection);
            
            inputNode.Node.NodeConnections.Add(connection);
            outputNode.Node.NodeConnections.Add(connection);
        }

        private void RemoveNode(LDEditorNode editorNode)
        {
            m_graph.Nodes.Remove(editorNode.Node);
            m_nodeDictionary.Remove(editorNode.Node.ID);
            m_graphNodes.Remove(editorNode);
            m_serializedObject.Update();
        }

        private void RemoveConnection(Edge edge)
        {
            if (m_connectionDictionary.TryGetValue(edge, out LDConnection connection))
            {
                m_graph.Connections.Remove(connection);
                m_connectionDictionary.Remove(edge);
                
                LDEditorNode inputNode = (LDEditorNode)edge.input.node;
                LDEditorNode outputNode = (LDEditorNode)edge.output.node;
                
                inputNode.Node.NodeConnections.Remove(connection);
                outputNode.Node.NodeConnections.Remove(connection);
                
                m_serializedObject.Update();
            }
        }
        
        private void DrawNodes()
        {
            foreach (LDNode node in m_graph.Nodes)
            {
                AddNodeToGraph(node);
            }
            
            BindObject();
        }

        private void DrawConnections()
        {
            if(m_graph.Connections == null) return;

            foreach (LDConnection connection in m_graph.Connections)
            {
                DrawConnection(connection);
            }
        }

        private void DrawConnection(LDConnection connection)
        {
            LDEditorNode inputNode = GetNode(connection.InputPort.NodeId);
            LDEditorNode outputNode = GetNode(connection.OutputPort.NodeId);
            
            if(inputNode == null || outputNode == null) return;

            Port inputPort = inputNode.Ports[connection.InputPort.PortIndex];
            Port outputPort = outputNode.Ports[connection.OutputPort.PortIndex];

            Edge edge = inputPort.ConnectTo(outputPort);
            AddElement(edge);
            m_connectionDictionary.Add(edge, connection);
        }

        private LDEditorNode GetNode(string nodeId)
        {
            LDEditorNode node = null;
            m_nodeDictionary.TryGetValue(nodeId, out node);
            return node;
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
            BindObject();
        }

        private void AddNodeToGraph(LDNode node)
        {
            node.TypeName = node.GetType().AssemblyQualifiedName;

            LDEditorNode editorNode = new LDEditorNode(node, m_serializedObject);
            editorNode.SetPosition(node.Position);
            m_graphNodes.Add(editorNode);
            m_nodeDictionary.Add(node.ID, editorNode);
            
            AddElement(editorNode);
        }

        private void BindObject()
        {
            m_serializedObject.Update();
            this.Bind(m_serializedObject);
        }
    }
}
