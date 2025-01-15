using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine.UI;
using Assembly = System.Reflection.Assembly;

namespace SaveRuntime.Editor
{
    public class SaveEditorTool : EditorWindow
    {
        private static SaveEditorTool _window;
        private List<Type> _allTypes = new List<Type>();
        
        bool _showFoldout = false;

        private Dictionary<string, bool> _foldoutProperties = new Dictionary<string, bool>();
    
        private Vector2 _scrollView = Vector2.zero;
        
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
                _allTypes = GetAssemblyClasses();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            
            _scrollView = EditorGUILayout.BeginScrollView(_scrollView, GUILayout.Height(800));
            if (_allTypes != null && _allTypes.Count > 0)
            {
                foreach (var type in _allTypes)
                {
                    if (_foldoutProperties.ContainsKey(type.Name))
                    {
                        _foldoutProperties[type.Name] = EditorGUILayout.Foldout(_foldoutProperties[type.Name], type.Name);
                        if (_foldoutProperties[type.Name])
                        {
                            
                        }
                    }
                    else
                    {
                        _foldoutProperties[type.Name] = false;
                        _foldoutProperties[type.Name] = EditorGUILayout.Foldout(_foldoutProperties[type.Name], type.Name);
                        if (_foldoutProperties[type.Name])
                        {
                            EditorGUILayout.LabelField("Wow !");
                        }
                    }

                }
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    
        /**
         * GetAssemblyClasses -> FCT
         *
         * @List<Type> ListOfAllTypes -> List with all the classes sorted and available
         * @List<Assembly> _allAssemblies -> Assemblies of all the project
         *
         * @return List<Type>
         */
        private List<Type> GetAssemblyClasses()
        {
            List<Type> listOfAllTypes = new List<Type>(); //Return Value
            List<Assembly> _allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            foreach (var assembly in _allAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<SavableAttribute>() != null) //Dont check for availability yet
                    {
                        listOfAllTypes.Add(type);
                    }
                }
            }


            //Old Version Outdated
            // foreach (var assembly in _allAssemblies)
            // {
            //     if (names.Contains(assembly.GetName().Name) && (!assembly.GetName().Name.StartsWith("Unity.") || 
            //         !assembly.GetName().Name.StartsWith("System.") ||
            //         !assembly.GetName().Name.StartsWith("Unity.") ||
            //         !assembly.GetName().Name.StartsWith("UnityEditor.") ||
            //         !assembly.GetName().Name.StartsWith("UnityEngine.")))
            //     {
            //         _assembliesSorted.Add(assembly);
            //     }
            // }
            //
            // foreach (var assembly in _assembliesSorted)
            // {
            //     foreach (var type in assembly.GetTypes())
            //     {
            //         listOfAllTypes.Add(type);
            //     }
            // }
            
            
            //Oudated
            //List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList().Where(x =>names.Contains(x.GetName().ToString())).ToList();

            
            // foreach (var assembly in assemblies)
            // {
            //     foreach (Type type in assembly.GetTypes())
            //     {
            //         listOfAllTypes.Add(type);
            //     }
            // }
            
            // foreach (Type type in this.GetType().Assembly.GetTypes())
            // {
            //     if(string.IsNullOrEmpty(type.Namespace)) continue;
            //     string[] folders = type.Namespace.Split('.');
            //     if (folders[1] == "Editor")
            //     {
            //         listOfAllTypes.Add(type);
            //     }
            // }
            
            
            // Assembly[] asss = AppDomain.CurrentDomain.GetAssemblies();
            // foreach (var assembly in asss)
            // {
            //     foreach (Type type in assembly.GetTypes())
            //     {
            //         listOfAllTypes.Add(type);
            //     }
            // }
            
            
            return listOfAllTypes;
        }
    }
}

