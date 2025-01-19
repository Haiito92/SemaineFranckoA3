using System;

namespace LittleGraph.Runtime.Attributes
{
    public class ExposedPropertyAttribute : Attribute
    {
        private bool m_editableInGraph;

        public bool EditableInGraph => m_editableInGraph;

        public ExposedPropertyAttribute(bool editableInGraph = true)
        {
            m_editableInGraph = editableInGraph;
        }
    }
}
