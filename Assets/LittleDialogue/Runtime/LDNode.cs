using System;
using System.Globalization;
using UnityEngine;

namespace LittleDialogue.Runtime
{
    [System.Serializable]
    public class LDNode
    {
        [SerializeField] private string m_guid;
        [SerializeField] private Rect m_position;

        public string TypeName;
        public string ID => m_guid;
        public Rect Position => m_position;

        public LDNode()
        {
            NewGUID();
        }

        private void NewGUID()
        {
            m_guid = Guid.NewGuid().ToString();
        }

        public void SetPosition(Rect position)
        {
            m_position = position;
        }

        public virtual string OnProcess(LDGraph currentGraph)
        {
            LDNode nextNodeInFlow = currentGraph.GetNodeFromOutput(m_guid, 0);
            if (nextNodeInFlow != null)
            {
                return nextNodeInFlow.ID;
            }
            return string.Empty;
        }
    }
}
