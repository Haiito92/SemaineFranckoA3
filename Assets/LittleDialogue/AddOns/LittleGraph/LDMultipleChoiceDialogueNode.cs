using LittleGraph.Runtime;
using LittleGraph.Runtime.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleDialogue.AddOns.LittleGraph
{
#if LITTLE_GRAPH
    [LGNodeInfo("Multiple Choice Dialogue", "Little Dialogue/Multiple Choice Dialogue")]
    public class LDMultipleChoiceDialogueNode : LGNode
    {
        [ExposedProperty()]
        public GameObject DialogueBoxPrefab;
        
        protected override void ExecuteNode()
        {
            if (DialogueBoxPrefab)
            {
                GameObject.Instantiate(DialogueBoxPrefab, SceneManager.GetActiveScene());   
            }
            
            base.ExecuteNode();
        }
    }
#endif
    
}
