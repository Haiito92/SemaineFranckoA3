using System;
using System.Collections.Generic;
using System.Linq;
using LittleDialogue.Runtime.LittleGraphAddOn;
using LittleGraph.Runtime;
using UnityEditor;
using UnityEngine;

namespace LittleDialogue.Runtime
{
    public class DialogueController : MonoBehaviour
    {
        private List<LDMultipleChoiceDialogueNode> m_dialogueNodes;

        [SerializeField] private DialogueBox m_dialogueBox; 
        
        public void Init(List<LGGraphObject> graphObjects)
        {
            //Subscribe to all dialogue nodes
            foreach (LGGraphObject graphObject in graphObjects)
            {
                m_dialogueNodes = graphObject.GraphInstance.Nodes.OfType<LDMultipleChoiceDialogueNode>().ToList();
                foreach (LDMultipleChoiceDialogueNode dialogueNode in m_dialogueNodes)
                {
                    dialogueNode.Executed += OnDialogueNodeExecuted;
                }
            }
        }
        private void OnDialogueNodeExecuted(LGNode node)
        {
            if(!m_dialogueBox) return;

            m_dialogueBox.ShowBox();

            if (node is LDMultipleChoiceDialogueNode dialogueNode)
            {
                m_dialogueBox.UpdateText(dialogueNode.DialogueText);
            }
        }
    }
}
