using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using LittleDialogue.Runtime.Attributes;
using NUnit.Framework;
using UnityEngine;

namespace LittleDialogue.Runtime
{
    [System.Serializable]
    public abstract class LDNode
    {
        [SerializeField] private string m_guid;
        [SerializeField] private Rect m_position;

        [SerializeField, HideInInspector] protected List<LDConnection> m_nodeConnections;
        
        public string TypeName;
        public string ID => m_guid;
        public Rect Position => m_position;

        public List<LDConnection> NodeConnections => m_nodeConnections;

        public Action ReceivedFlow;
        public Action Executed;
        public Action<string> EmittedFlow;
        
        public LDNode()
        {
            NewGUID();
            m_nodeConnections = new List<LDConnection>();
        }

        private void NewGUID()
        {
            m_guid = Guid.NewGuid().ToString();
        }

        public void SetPosition(Rect position)
        {
            m_position = position;
        }

        public virtual void ReceiveFlow()
        {
            ExecuteNode();
            ReceivedFlow?.Invoke();
        }

        protected virtual void ExecuteNode()
        {
            Executed?.Invoke();
        }
        
        public virtual void EmitFlow(string nextNodeId)
        {
            if(string.IsNullOrEmpty(nextNodeId)) return;
            EmittedFlow?.Invoke(nextNodeId);
        }
        
        public virtual string OnProcess(LDGraph currentGraph)
        {
            if(m_nodeConnections.Count <= 0) return string.Empty;
            
            LDNodeInfoAttribute nodeInfoAttribute = TypeName.GetType().GetCustomAttribute<LDNodeInfoAttribute>();
            if (nodeInfoAttribute.HasMultipleOutputs)
            {
                return string.Empty;
            }
            else
            {
                LDConnection connection = m_nodeConnections.Find(x => x.OutputPort.NodeId == ID);
                LDNode nextNodeInFlow = currentGraph.GetNodeFromOutput(m_guid, connection.OutputPort.PortIndex);
                if (nextNodeInFlow != null)
                {
                    return nextNodeInFlow.ID;
                }
            }
            // LDNode nextNodeInFlow = currentGraph.GetNodeFromOutput(m_guid, 0);
            // if (nextNodeInFlow != null)
            // {
            //     return nextNodeInFlow.ID;
            // }
            return string.Empty;
        }
    }
}
