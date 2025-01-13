using System.Collections.Generic;
using UnityEngine;

namespace LittleDialogue.Runtime
{
    [CreateAssetMenu(fileName = "LDGraph", menuName = "Scriptable Objects/LDGraph")]
    public class LDGraph : ScriptableObject
    {
        [SerializeReference]
        public List<LDNode> m_nodes;
    }
}
