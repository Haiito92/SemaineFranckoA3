using System;
using UnityEngine;

namespace LittleDialogue.Runtime.Attributes
{
    public class LDNodeInfoAttribute : Attribute
    {
        private string m_nodeTitle;
        private string m_menuItem;

        public string Title => m_nodeTitle;
        public string MenuItem => m_menuItem;

        public LDNodeInfoAttribute(string title, string menuItem = "")
        {
            m_nodeTitle = title;
            m_menuItem = menuItem;
        }
    }
}
