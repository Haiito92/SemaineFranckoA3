using LittleDialogue.Runtime.Attributes;
using UnityEngine;

namespace LittleDialogue.Runtime.Types
{
    [LDNodeInfo("Start", "Process/Start", false, true)]
    public class LDStartNode : LDNode
    {
        public override string OnProcess(LDGraph currentGraph)
        {
            Debug.Log("Start Node");

            return base.OnProcess(currentGraph);
        }
    }
}
