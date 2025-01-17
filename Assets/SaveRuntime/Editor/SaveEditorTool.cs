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
        ////Fields
        //window
        private static SaveEditorTool _window;

        //so
        private SaveDataBehaviour _currentDataBehaviour;
        private SerializedObject _serializedObject;
        
        private List<Type> _allTypes = new List<Type>();
        
        private Dictionary<string, bool> _foldoutProperties = new Dictionary<string, bool>();
        private List<SaveDataBehaviourStruct> _fieldCheckList = new List<SaveDataBehaviourStruct>();
        private FieldInfo[] _tempFieldsInfo;
    
        //Scroll Position
        private Vector2 _scrollView = Vector2.zero;
        
        ////Functions
        [MenuItem("EditorToolWindow/SaveEditorTool(do not use atm)")]
        public static void ShowWindow()
        {
            if (!_window)
            {
                _window = GetWindow<SaveEditorTool>("SaveEditorTool");
                
            }
            // TODO - SetWindowSize
        }
        
        //Current Data Behaviour Loaded on Customized Show Windows (only on SO -> see SaveDataEditor)
        public static void ShowWindow(SerializedObject serializedObject)
        {
            if (!_window)
            {
                _window = GetWindow<SaveEditorTool>("SaveEditorTool");
                //Load current Data
                _window.Load(serializedObject);
            }
            // TODO - SetWindowSize
        }
        
        private void Load(SerializedObject serializedObject)
        {
            _serializedObject = serializedObject;
            _currentDataBehaviour = (SaveDataBehaviour)serializedObject.targetObject;
        }
        
    
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
                    UpdateSo(_currentDataBehaviour, newSaveDataTypeStruct);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            
            //ScrollView
            _scrollView = EditorGUILayout.BeginScrollView(_scrollView, GUILayout.Height(800));
            
            //CreateFoldout
            if (_currentDataBehaviour.ListOfType.Count > 0)
            {
                for(int j = 0; j < _currentDataBehaviour.ListOfType.Count ; j++)
                {
                    if (_foldoutProperties.ContainsKey(_currentDataBehaviour.ListOfType[j].Name))
                    {
                        _foldoutProperties[_currentDataBehaviour.ListOfType[j].Name] = EditorGUILayout.Foldout(
                            _foldoutProperties[_currentDataBehaviour.ListOfType[j].Name], _currentDataBehaviour.ListOfType[j].Name);
                        if (_foldoutProperties[_currentDataBehaviour.ListOfType[j].Name])
                        {
                            for(int i = 0; i < _currentDataBehaviour.ListOfType[j].ListOfFields.Count; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                _currentDataBehaviour.ListOfType[j].ListOfFields[i] = SaveDataBehaviourStruct.SetCheck(_currentDataBehaviour.ListOfType[j].ListOfFields[i], EditorGUILayout.Toggle("",
                                    _currentDataBehaviour.ListOfType[j].ListOfFields[i].IsChecked));
                                EditorGUILayout.LabelField($"{_currentDataBehaviour.ListOfType[j].ListOfFields[i].NameOfField}");
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }
                    else
                    {
                        _foldoutProperties[_currentDataBehaviour.ListOfType[j].Name] = false;
                        _foldoutProperties[_currentDataBehaviour.ListOfType[j].Name] = EditorGUILayout.Foldout(_foldoutProperties[_currentDataBehaviour.ListOfType[j].Name], _currentDataBehaviour.ListOfType[j].Name);
                        if (_foldoutProperties[_currentDataBehaviour.ListOfType[j].Name])
                        {
                            EditorGUILayout.LabelField("Wow !");
                        }
                    }
                }
            }
            
            // if (_allTypes != null && _allTypes.Count > 0)
            // {
            //     foreach (var type in _allTypes)
            //     {
            //        
            //         if (_foldoutProperties.ContainsKey(type.Name))
            //         {
            //             _foldoutProperties[type.Name] = EditorGUILayout.Foldout(_foldoutProperties[type.Name], type.Name);
            //             if (_foldoutProperties[type.Name])
            //             {
            //                 _tempFieldsInfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //
            //                 foreach (var f in _tempFieldsInfo)
            //                 {
            //                     bool isFieldExisting = _fieldCheckList.Exists(x => x.NameOfField == f.Name);
            //                     if (!isFieldExisting)
            //                     {
            //                         _fieldCheckList.Add(new SaveDataBehaviourStruct(f.Name, true));
            //                         EditorGUILayout.BeginHorizontal();
            //                         //_fieldCheckProperties[f.Name] = SaveDataBehaviourStruct.SetCheck(_fieldCheckProperties[f.Name], EditorGUILayout.Toggle("", _fieldCheckProperties[f.Name].IsChecked));
            //                         _fieldCheckList[^1] = SaveDataBehaviourStruct.SetCheck(_fieldCheckList[^1], EditorGUILayout.Toggle("", _fieldCheckList[^1].IsChecked));
            //                         EditorGUILayout.LabelField($"{f.Name} ({f.FieldType.Name})");
            //                         EditorGUILayout.EndHorizontal();
            //                     }
            //                     else
            //                     {
            //                         EditorGUILayout.BeginHorizontal();
            //                         int i =_fieldCheckList.FindIndex(x => x.NameOfField == f.Name);
            //                         _fieldCheckList[i] =SaveDataBehaviourStruct.SetCheck(_fieldCheckList[i], EditorGUILayout.Toggle("", _fieldCheckList[i].IsChecked));
            //                         EditorGUILayout.LabelField($"{f.Name} ({f.FieldType.Name})");
            //                         EditorGUILayout.EndHorizontal();
            //                     }
            //                 }
            //             }
            //         }
            //         else
            //         {
            //             _foldoutProperties[type.Name] = false;
            //             _foldoutProperties[type.Name] = EditorGUILayout.Foldout(_foldoutProperties[type.Name], type.Name);
            //             if (_foldoutProperties[type.Name])
            //             {
            //                 EditorGUILayout.LabelField("Wow !");
            //             }
            //         }
            //
            //     }
            //}
            //
            
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
            List<Assembly> allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            foreach (var assembly in allAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<SavableAttribute>() != null) //Dont check for availability yet
                    {
                        listOfAllTypes.Add(type);
                    }
                }
            }
            return listOfAllTypes;
        }

        /**
         * UpdateSo
         * <param name="currentSo"></param>
         * <param name="currentTypeStruct"></param>
         */
        public void UpdateSo(SaveDataBehaviour currentSo, SaveDataBehaviourTypeStruct currentTypeStruct)
        {
            
            // if (currentSo.ListOfType.Find())
            // {
            //     var currentTypeIndexInSo = currentSo.ListOfType.IndexOf(currentTypeStruct);
            //
            //     currentSo.ListOfType[currentTypeIndexInSo] = currentTypeStruct;
            // }

            foreach (var typeStruct in currentSo.ListOfType.ToList())
            {
                if (typeStruct.Name == currentTypeStruct.Name)
                {
                    currentSo.ListOfType[typeStruct.Id] = currentTypeStruct;
                    return;
                }
            }
            currentSo.ListOfType.Add(currentTypeStruct);
            
        }
    }
}

