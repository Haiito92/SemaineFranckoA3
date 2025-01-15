using System.Reflection;
using LittleDialogue.Runtime.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleDialogue.Runtime.Types.Dialogue
{
    [LDNodeInfo("Multiple Choice Dialogue", "Dialogue/Multiple Choice", true, true, true)]
    public class LDMultipleChoiceDialogueNode : LDNode
    {
        [ExposedProperty()] public GameObject DialogueBoxPrefab;
        
        protected override void ExecuteNode()
        {
            if (DialogueBoxPrefab)
            {
                GameObject.Instantiate(DialogueBoxPrefab, SceneManager.GetActiveScene());   
            }
            base.ExecuteNode();
        }
    }
}
