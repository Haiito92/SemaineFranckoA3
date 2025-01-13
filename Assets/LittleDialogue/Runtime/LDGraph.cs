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

        public List<LDNode> Nodes => m_nodes;

        public LDGraph()
        {
            m_nodes = new List<LDNode>();
        }
    }
}
