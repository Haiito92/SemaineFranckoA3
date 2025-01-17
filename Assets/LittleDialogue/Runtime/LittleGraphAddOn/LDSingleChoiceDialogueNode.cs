using LittleGraph.Runtime;
using LittleGraph.Runtime.Attributes;
using UnityEngine;

namespace LittleDialogue.Runtime.LittleGraphAddOn
{
#if LITTLE_GRAPH
    [LGNodeInfo("Single Choice Dialogue", "Little Dialogue/Single Choice Dialogue")]
    public class LDSingleChoiceDialogueNode : LDDialogueNode
    {
        protected override void ExecuteNode()
        {
            base.ExecuteNode();
        }
        
    }
#endif
}
