using LittleDialogue.Runtime.Attributes;
using UnityEngine;

namespace LittleDialogue.Runtime.Types
{
    [LDNodeInfo("Start", "Process/Start", false, true)]
    public class LDStartNode : LDNode
    {
        protected override void ExecuteNode()
        {
            Debug.Log("Execute Start Node");

            Debug.Log( m_nodeConnections.Count);
            LDConnection connection = m_nodeConnections.Find(x => x.OutputPort.NodeId == ID);
            EmitFlow(connection.InputPort.NodeId);
            base.ExecuteNode();
        }

        public override string OnProcess(LDGraph currentGraph)
        {
            Debug.Log("Start Node");

            return base.OnProcess(currentGraph);
        }
    }
}
