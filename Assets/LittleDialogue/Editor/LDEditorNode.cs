using System;
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
        
        public LDEditorNode(LDNode node)
        {
            this.AddToClassList("ld-node");

            m_node = node;
            Type typeInfo = node.GetType();
            LDNodeInfoAttribute info = typeInfo.GetCustomAttribute<LDNodeInfoAttribute>();

            title = info.Title;
            
            string[] depths = info.MenuItem.Split('/');
            foreach (string depth in depths)
            {
                this.AddToClassList(depth.ToLower().Replace(' ', '-'));
            }
            
            this.name = typeInfo.Name;
        }
    }
}
