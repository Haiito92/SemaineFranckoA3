using System.Collections.Generic;
using System.Linq;
using LittleDialogue.Runtime.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace LittleDialogue.Runtime
{
    [CreateAssetMenu(fileName = "LDGraph", menuName = "Scriptable Objects/LDGraph")]
    public class LDGraph : ScriptableObject
    {
        [SerializeReference]
        private List<LDNode> m_nodes;

        private Dictionary<string, LDNode> m_nodeDictionary;
        
        [SerializeField] private List<LDConnection> m_connections;
        
        public List<LDNode> Nodes => m_nodes;
        public List<LDConnection> Connections => m_connections;
        
        public LDGraph()
        {
            m_nodes = new List<LDNode>();
            m_connections = new List<LDConnection>();
        }

        public void Init()
        {
            m_nodeDictionary = new Dictionary<string, LDNode>();
            foreach (LDNode node in m_nodes)
            {
                m_nodeDictionary.Add(node.ID, node);
            }
        }
        
        public LDNode GetStartNode()
        {
            LDStartNode[] startNodes = Nodes.OfType<LDStartNode>().ToArray();

            if (startNodes.Length == 0)
            {
                Debug.LogError("There is no start node in this graph");
                return null;
            }

            return startNodes[0];
        }

        public LDNode GetNode(string nodeId)
        {
            if (m_nodeDictionary.TryGetValue(nodeId, out LDNode node))
            {
                return node;
            }

            return null;
        }

        public LDNode GetNodeFromOutput(string outputNodeId, int outputPortIndex)
        {
            foreach (LDConnection connection in m_connections)
            {
                if (connection.OutputPort.NodeId == outputNodeId && connection.OutputPort.PortIndex == outputPortIndex)
                {
                    string nodeId = connection.InputPort.NodeId;
                    LDNode inputNode = m_nodeDictionary[nodeId];

                    return inputNode;
                }
            }
            return null;
        }
    }
}
