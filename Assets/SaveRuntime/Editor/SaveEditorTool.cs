using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using SaveRuntime.Runtime;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine.UI;
using Assembly = System.Reflection.Assembly;

namespace SaveRuntime.Editor
{
    public class SaveEditorTool : EditorWindow
    {
        ////Fields
        //window
        private static SaveEditorTool _window;
        private static SelectSoEditorTool _windowSelection;
        
       
        private List<Type> _allTypes = new List<Type>();
        
        private Dictionary<string, bool> _foldoutProperties = new Dictionary<string, bool>();
        private FieldInfo[] _tempFieldsInfo;
    
        //Scroll Position
        private Vector2 _scrollView = Vector2.zero;
        
        ////Functions
        [MenuItem("EditorToolWindow/SaveEditorTool(do not use atm)")]
        public static void ShowWindow()
        {
            if (!_windowSelection)
            {
                _windowSelection = GetWindow<SelectSoEditorTool>("SaveEditorTool");
            }
            // TODO - SetWindowSize
        }
        
        //Current Data Behaviour Loaded on Customized Show Windows (only on SO -> see SaveDataEditor)
        public static void ShowWindow(SaveDataBehaviour saveObject)
        {
            if (!_window)
            {
                _window = GetWindow<SaveEditorTool>("SaveEditorTool");
                //Load current Data
                _window.LoadObj(saveObject);
            }
            // TODO - SetWindowSize
        }
        
        private void LoadObj(SaveDataBehaviour saveObject)
        {
            SaveFile._currentDataBehaviour = saveObject;
        }
        
    
        /**
         * OnGUI -> FCT
         *
         * All there is to see on the window
         * 
         * @void
         */
        private void OnGUI()
        {
            //Main Layout
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Press to get all scripts : ", GUILayout.MinWidth(125));
            if(GUILayout.Button("Press Here"))
            {
                _allTypes = GetAssemblyClasses(); //Get Types

                //Update SO
                int i = -1;
                foreach (var type in _allTypes)
                {
                    i++;
                    List<SaveDataBehaviourStruct> newFieldForTypeT = new List<SaveDataBehaviourStruct>();
                    foreach (var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        newFieldForTypeT.Add(new SaveDataBehaviourStruct(fieldInfo.Name, false));
                    }
                    SaveDataBehaviourTypeStruct newSaveDataTypeStruct = new SaveDataBehaviourTypeStruct(type.Name,i, type, newFieldForTypeT);
                    if (SaveFile._currentDataBehaviour.ListOfType.Any())
                    {
                        UpdateSo(SaveFile._currentDataBehaviour, newSaveDataTypeStruct);
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            
            //ScrollView
            _scrollView = EditorGUILayout.BeginScrollView(_scrollView, GUILayout.Height(300));
            
            //CreateFoldout
            if (SaveFile._currentDataBehaviour.ListOfType.Count > 0)
            {
                for(int j = 0; j < SaveFile._currentDataBehaviour.ListOfType.Count ; j++)
                {
                    if (_foldoutProperties.ContainsKey(SaveFile._currentDataBehaviour.ListOfType[j].Name))
                    {
                        _foldoutProperties[SaveFile._currentDataBehaviour.ListOfType[j].Name] = EditorGUILayout.Foldout(
                            _foldoutProperties[SaveFile._currentDataBehaviour.ListOfType[j].Name], SaveFile._currentDataBehaviour.ListOfType[j].Name);
                        if (_foldoutProperties[SaveFile._currentDataBehaviour.ListOfType[j].Name])
                        {
                            for(int i = 0; i < SaveFile._currentDataBehaviour.ListOfType[j].ListOfFields.Count; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                SaveFile._currentDataBehaviour.ListOfType[j].ListOfFields[i] = SaveDataBehaviourStruct.SetCheck(SaveFile._currentDataBehaviour.ListOfType[j].ListOfFields[i], EditorGUILayout.Toggle("",
                                    SaveFile._currentDataBehaviour.ListOfType[j].ListOfFields[i].IsChecked));
                                EditorGUILayout.LabelField($"{SaveFile._currentDataBehaviour.ListOfType[j].ListOfFields[i].NameOfField}");
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }
                    else
                    {
                        _foldoutProperties[SaveFile._currentDataBehaviour.ListOfType[j].Name] = false;
                        _foldoutProperties[SaveFile._currentDataBehaviour.ListOfType[j].Name] = EditorGUILayout.Foldout(_foldoutProperties[SaveFile._currentDataBehaviour.ListOfType[j].Name], SaveFile._currentDataBehaviour.ListOfType[j].Name);
                        if (_foldoutProperties[SaveFile._currentDataBehaviour.ListOfType[j].Name])
                        {
                            EditorGUILayout.LabelField("Wow !");
                        }
                    }
                }
            }
            
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
            //Save All Presset
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Data"))
            {
                SaveFile.Save();
            }
            GUILayout.EndHorizontal();
            
            //Load Button
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Load Data"))
            {
                SaveFile.Load();
            }
            GUILayout.EndHorizontal();
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
            List<Assembly> allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            foreach (var assembly in allAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    SaveDataBehaviour data = CreateInstance<SaveDataBehaviour>();
                    if (type == data.GetType())
                    {
                        Debug.Log("WTFFF LA TEAM");
                    }
                    if (type.GetCustomAttribute<SavableAttribute>() != null) //Dont check for availability yet
                    {
                        listOfAllTypes.Add(type);

                    }
                }
            }
            return listOfAllTypes;
        }

        /**
         * UpdateSo -> FCT
         * <param name="currentSo"></param>
         * <param name="currentTypeStruct"></param>
         */
        public void UpdateSo(SaveDataBehaviour currentSo, SaveDataBehaviourTypeStruct currentTypeStruct)
        {
            foreach (var typeStruct in currentSo.ListOfType.ToList())
            {
                if (typeStruct.Name == currentTypeStruct.Name)
                {
                    var fieldsOfAType = currentSo.ListOfType[typeStruct.Id].ListOfFields;
                    List<SaveDataBehaviourStruct> newFieldsOfAType = new List<SaveDataBehaviourStruct>();
                    for (int i = 0; i < currentTypeStruct.ListOfFields.Count; i++)
                    {
                        for (int j = 0; j < fieldsOfAType.Count; j++)
                        {
                            if (currentTypeStruct.ListOfFields[i].NameOfField == fieldsOfAType[j].NameOfField)
                            {
                                var saveDataBehaviourStructFieldTemp = currentTypeStruct.ListOfFields[i];
                                saveDataBehaviourStructFieldTemp.IsChecked = fieldsOfAType[j].IsChecked;
                                newFieldsOfAType.Add(saveDataBehaviourStructFieldTemp);
                            }
                        }
                    }
                    SaveDataBehaviourTypeStruct newTypeStruct = new SaveDataBehaviourTypeStruct(typeStruct.Name, typeStruct.Id, typeStruct.TypeOfClass, newFieldsOfAType);
                    currentSo.ListOfType[typeStruct.Id] = newTypeStruct;
                    return;
                }
            }
            currentSo.ListOfType.Add(currentTypeStruct);
        }

        
    }

    public class SelectSoEditorTool : EditorWindow
    {
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select the right Data Scriptable Object : ", GUILayout.MinWidth(125));
            GUILayout.EndHorizontal();
            //Do Not Use
            // GUILayout.BeginVertical();
            // foreach (var soClass in GetSOClasses())
            // {
            //     if(GUILayout.Button("SelectSO"))
            //     {
            //         //SaveEditorTool.ShowWindow(soClass);
            //     }    
            // }
            // GUILayout.EndVertical();

        }
        
        private List<Type> GetSOClasses()
        {
            List<Type> listOfAllTypes = new List<Type>(); //Return Value
            List<Assembly> allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            foreach (var assembly in allAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    SaveDataBehaviour data = CreateInstance<SaveDataBehaviour>();
                    if (type == data.GetType())
                    {
                        listOfAllTypes.Add(type);
                    }
                }
            }
            return listOfAllTypes;
        }

    }
}

