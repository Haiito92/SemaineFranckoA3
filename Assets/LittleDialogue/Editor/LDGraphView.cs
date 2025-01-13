using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace LittleDialogue.Editor
{
    public class LDGraphView : GraphView
    {
        private SerializedObject m_serializedObject;
        
        public LDGraphView(SerializedObject serializedObject)
        {
            m_serializedObject = serializedObject;

            StyleSheet style =
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/LittleDialogue/Editor/USS/LDEditor.uss");
            styleSheets.Add(style);
                
            GridBackground background = new GridBackground();
            background.name = "Grid";
            Add(background);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
        }
    }
}
