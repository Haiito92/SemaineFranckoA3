using System;
using UnityEngine;

namespace LittleDialogue.Runtime.Attributes
{
    public class LDNodeInfoAttribute : Attribute
    {
        private string m_nodeTitle;
        private string m_menuItem;
        private bool m_hasFlowInput;
        private bool m_hasFlowOutput;
        private bool m_hasMultipleOutputs;

        public string Title => m_nodeTitle;
        public string MenuItem => m_menuItem;
        public bool HasFlowInput => m_hasFlowInput;
        public bool HasFlowOutput => m_hasFlowOutput;
        public bool HasMultipleOutputs => m_hasMultipleOutputs;

        public LDNodeInfoAttribute(string title, string menuItem = "", bool hasFlowInput = true, bool hasFlowOutput = true, bool hasMultipleOutputs = false)
        {
            m_nodeTitle = title;
            m_menuItem = menuItem;
            m_hasFlowInput = hasFlowInput;
            m_hasFlowOutput = hasFlowOutput;
            m_hasMultipleOutputs = hasMultipleOutputs;
        }
    }
}
