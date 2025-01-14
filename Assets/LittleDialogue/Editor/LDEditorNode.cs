using System;
using System.Collections.Generic;
using System.Reflection;
using LittleDialogue.Runtime;
using LittleDialogue.Runtime.Attributes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace LittleDialogue.Editor
{
    public class LDEditorNode : Node
    {
        private LDNode m_node;
        private Port m_outputPort;
        private List<Port> m_ports;

        private SerializedObject m_serializedObject;
        private SerializedProperty m_serializedProperty;
        
        public LDNode Node => m_node;
        public List<Port> Ports => m_ports;
        
        public LDEditorNode(LDNode node, SerializedObject graphObject)
        {
            this.AddToClassList("ld-node");

            m_serializedObject = graphObject;
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
            
            //So output is always index 0 (to be changed later)
            if (info.HasFlowOutput)
            {
                CreateFlowOutputPort();
            }
            
            if (info.HasFlowInput)
            {
                CreateFlowInputPort();
            }

            foreach (FieldInfo property in typeInfo.GetFields())
            {
                if (property.GetCustomAttribute<ExposedPropertyAttribute>() is ExposedPropertyAttribute exposedPropertyAttribute)
                {
                    PropertyField field = DrawProperty(property.Name);
                    //field.RegisterValueChangeCallback(OnFieldChangeCallback);
                }
            }
            
            RefreshExpandedState();
        }

        private void OnFieldChangeCallback(SerializedPropertyChangeEvent evt)
        {
            throw new NotImplementedException();
        }

        private PropertyField DrawProperty(string propertyName)
        {
            if (m_serializedProperty == null)
            {
                FetchSerializedProperty();
            }

            SerializedProperty property = m_serializedProperty.FindPropertyRelative(propertyName);
            
            PropertyField field = new PropertyField(property);
            field.bindingPath = property.propertyPath;
            extensionContainer.Add(field);
            return field;
        }

        private void FetchSerializedProperty()
        {
            //Get the nodes
            SerializedProperty nodes = m_serializedObject.FindProperty("m_nodes");
            if (nodes.isArray)
            {
                int size = nodes.arraySize;
                for (int i = 0; i < size; i++)
                {
                    SerializedProperty element = nodes.GetArrayElementAtIndex(i);
                    SerializedProperty elementId = element.FindPropertyRelative("m_guid");
                    if (elementId.stringValue == m_node.ID)
                    {
                        m_serializedProperty = element;
                    }
                }
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
