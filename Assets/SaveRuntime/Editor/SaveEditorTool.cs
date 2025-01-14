using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SaveEditorTool : EditorWindow
{
    private static SaveEditorTool _window;
    public List<Type> AllTypes;
    
    [MenuItem("Window/SaveEditorTool")]
    public static void ShowWindow()
    {
        if (!_window)
        {
            _window = GetWindow<SaveEditorTool>("SaveEditorTool");
        }
        // TODO - SetWindowSize
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Press To Scan All Classes", GUILayout.MinWidth(125));
        if(GUILayout.Button("SCAN"))
        {
            AllTypes = GetAssemblyClasses();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        if (AllTypes.Count > 0)
        {
            foreach (var type in AllTypes)
            {
                GUILayout.Label(type.ToString());
            }
        }
        GUILayout.EndVertical();
    }

    private List<Type> GetAssemblyClasses()
    {
        List<Type> listOfAllTypes = new List<Type>();
        Assembly[] asss = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in asss)
        {
            foreach (Type type in assembly.GetTypes())
            {
                listOfAllTypes.Add(type);
            }
        }
        return listOfAllTypes;
    }
}
