using LittleDialogue.Runtime.Attributes;
using UnityEngine;

namespace LittleDialogue.Runtime.Types
{
    [LDNodeInfo("Debug Log", "Debug/Debug Log")]
    public class LDDebugLogNode : LDNode
    {
        [ExposedProperty()]
        public string LogMessage;
        
        public override string OnProcess(LDGraph currentGraph)
        {
            Debug.Log(LogMessage);
            
            return base.OnProcess(currentGraph);
        }
    }
}
