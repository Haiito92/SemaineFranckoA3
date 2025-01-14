using System;
using UnityEngine;

namespace LittleDialogue.Runtime
{
    public class LDGraphObject : MonoBehaviour
    {
        [SerializeField]
        private LDGraph m_graph;

        private LDGraph m_graphInstance;
        
        private void OnEnable()
        {
            m_graphInstance = Instantiate(m_graph);
            ExecuteAsset();
        }

        private void ExecuteAsset()
        {
            m_graphInstance.Init();
            
            LDNode startNode = m_graphInstance.GetStartNode();

            ProcessAndMoveToNextNode(startNode);
        }

        private void ProcessAndMoveToNextNode(LDNode currentNode)
        {
            string nextNodeId = currentNode.OnProcess(m_graphInstance);

            if (!string.IsNullOrEmpty(nextNodeId))
            {
                LDNode nextNode = m_graphInstance.GetNode(nextNodeId);
                
                ProcessAndMoveToNextNode(nextNode);
            }
        }
    }
}
