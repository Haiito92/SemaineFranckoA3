using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LittleDialogue.Editor
{
    public class LDEditorNode : Node
    {
        public LDEditorNode()
        {
            this.AddToClassList("ld-node");
        }
    }
}
