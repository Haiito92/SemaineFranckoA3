using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LittleDialogue.Runtime
{
    [CreateAssetMenu(fileName = "LDGraph", menuName = "Scriptable Objects/LDGraph")]
    public class LDGraph : ScriptableObject
    {
        [SerializeReference]
        private List<LDNode> m_nodes;

        [SerializeField] private List<LDConnection> m_connections;
        
        public List<LDNode> Nodes => m_nodes;
        public List<LDConnection> Connections => m_connections;
        
        public LDGraph()
        {
            m_nodes = new List<LDNode>();
            m_connections = new List<LDConnection>();
        }
    }
}
