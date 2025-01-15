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
            RegisterEvents();
            ExecuteAsset();
        }

        private void OnDisable()
        {
            UnRegisterEvents();
        }

        private void RegisterEvents()
        {
            foreach (LDNode node in m_graphInstance.Nodes)
            {
                node.ReceivedFlow += OnReceivedFlowAction;
                node.Executed += OnExecutedAction;
                node.EmittedFlow += OnEmittedFlowAction;
            }
        }

        private void OnReceivedFlowAction()
        {
            //Doing nothing for now
        }

        private void OnExecutedAction()
        {
            //Doing nothing for now
        }

        private void OnEmittedFlowAction(string nextNodeId)
        {
            m_graphInstance.GetNode(nextNodeId).ReceiveFlow();
        }

        private void UnRegisterEvents()
        {
            foreach (LDNode node in m_graphInstance.Nodes)
            {
                node.ReceivedFlow -= OnReceivedFlowAction;
                node.Executed -= OnExecutedAction;
                node.EmittedFlow -= OnEmittedFlowAction;
            }
        }
        
        private void ExecuteAsset()
        {
            m_graphInstance.Init();
            
            LDNode startNode = m_graphInstance.GetStartNode();

            startNode.ReceiveFlow();
            
            //ProcessAndMoveToNextNode(startNode);
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
