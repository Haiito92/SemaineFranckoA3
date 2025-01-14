using System;
using System.Collections.Generic;
using System.Reflection;
using LittleDialogue.Runtime;
using LittleDialogue.Runtime.Attributes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LittleDialogue.Editor
{
    public class LDEditorNode : Node
    {
        private LDNode m_node;
        private Port m_outputPort;
        private List<Port> m_ports;
        
        public LDNode Node => m_node;
        public List<Port> Ports => m_ports;
        
        public LDEditorNode(LDNode node)
        {
            this.AddToClassList("ld-node");

            m_node = node;
            Type typeInfo = node.GetType();
            LDNodeInfoAttribute info = typeInfo.GetCustomAttribute<LDNodeInfoAttribute>();

            title = info.Title;

            m_ports = new List<Port>();
            
            string[] depths = info.MenuItem.Split('/');
            foreach (string depth in depths)
            {
                this.AddToClassList(depth.ToLower().Replace(' ', '-'));
            }
            
            this.name = typeInfo.Name;

            if (info.HasFlowInput)
            {
                CreateFlowInputPort();
            }

            if (info.HasFlowOutput)
            {
                CreateFlowOutputPort();
            }
        }

        private void CreateFlowInputPort()
        {
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                typeof(PortTypes.FlowPort));
            inputPort.portName = "In";
            inputPort.tooltip = "Flow input";
            m_ports.Add(inputPort);
            inputContainer.Add(inputPort); 
        }

        private void CreateFlowOutputPort()
        {
            m_outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                typeof(PortTypes.FlowPort));
            m_outputPort.portName = "Out";
            m_outputPort.tooltip = "Flow output";
            m_ports.Add(m_outputPort);
            outputContainer.Add(m_outputPort);
        }

        public void SavePosition()
        {
            m_node.SetPosition(GetPosition());
        }
    }
}
