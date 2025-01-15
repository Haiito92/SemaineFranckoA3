using LittleDialogue.Runtime.Attributes;
using UnityEngine;

namespace LittleDialogue.Runtime.Types
{
    [LDNodeInfo("Debug Log", "Debug/Debug Log")]
    public class LDDebugLogNode : LDNode
    {
        [ExposedProperty()]
        public string LogMessage;

        protected override void ExecuteNode()
        {
            Debug.Log(LogMessage);
            
            LDConnection connection = NodeConnections.Find(x => x.OutputPort.NodeId == ID);
            EmitFlow(connection.InputPort.NodeId);
            
            base.ExecuteNode();
        }

        public override string OnProcess(LDGraph currentGraph)
        {
            Debug.Log(LogMessage);
            
            return base.OnProcess(currentGraph);
        }
    }
}
