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

        private SaveDataBehaviour _currentDataBehaviour;
        
        private List<Type> _allTypes = new List<Type>();
        
        private Dictionary<string, bool> _foldoutProperties = new Dictionary<string, bool>();
        private Dictionary<string, SaveDataBehaviourStruct> _fieldCheckProperties = new Dictionary<string, SaveDataBehaviourStruct>();
        private List<SaveDataBehaviourStruct> _fieldCheckList = new List<SaveDataBehaviourStruct>();
        private FieldInfo[] _tempFieldsInfo;
    
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
        public static void ShowWindow(SaveDataBehaviour dataBehaviour)
        {
            if (!_window)
            {
                _window = GetWindow<SaveEditorTool>("SaveEditorTool");
                _window.Load(dataBehaviour);
            }

        }
        private void Load(SaveDataBehaviour dataBehaviour)
        {
            _currentDataBehaviour = dataBehaviour;
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
                            _tempFieldsInfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                            foreach (var f in _tempFieldsInfo)
                            {
                                bool isFieldExisting = _fieldCheckList.Exists(x => x.NameOfField == f.Name);
                                if (!isFieldExisting)
                                {
                                    _fieldCheckList.Add(new SaveDataBehaviourStruct(f.Name, true));
                                    EditorGUILayout.BeginHorizontal();
                                    //_fieldCheckProperties[f.Name] = SaveDataBehaviourStruct.SetCheck(_fieldCheckProperties[f.Name], EditorGUILayout.Toggle("", _fieldCheckProperties[f.Name].IsChecked));
                                    _fieldCheckList[^1] = SaveDataBehaviourStruct.SetCheck(_fieldCheckList[^1], EditorGUILayout.Toggle("", _fieldCheckList[^1].IsChecked));
                                    EditorGUILayout.LabelField($"{f.Name} ({f.FieldType.Name})");
                                    EditorGUILayout.EndHorizontal();
                                }
                                else
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    int i =_fieldCheckList.FindIndex(x => x.NameOfField == f.Name);
                                    _fieldCheckList[i] =SaveDataBehaviourStruct.SetCheck(_fieldCheckList[i], EditorGUILayout.Toggle("", _fieldCheckList[i].IsChecked));
                                    EditorGUILayout.LabelField($"{f.Name} ({f.FieldType.Name})");
                                    EditorGUILayout.EndHorizontal();
                                }
                                
                                // if (_fieldCheckProperties.ContainsKey(f.Name))
                                // {
                                //     EditorGUILayout.BeginHorizontal();
                                //     // SaveDataBehaviourStruct t = _fieldCheckProperties[f.Name];
                                //     // t.IsChecked = EditorGUILayout.Toggle("",_fieldCheckProperties[f.Name].IsChecked);
                                //     // _fieldCheckProperties[f.Name] = t;
                                //     _fieldCheckProperties[f.Name] = SaveDataBehaviourStruct.SetCheck(_fieldCheckProperties[f.Name], EditorGUILayout.Toggle("", _fieldCheckProperties[f.Name].IsChecked));
                                //     EditorGUILayout.LabelField($"{f.Name} ({f.FieldType.Name})");
                                //     EditorGUILayout.EndHorizontal();
                                // }
                            }
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
            return listOfAllTypes;
        }

        /**
         * UpdateSo
         * <param name="currentSo"></param>
         */
        public void UpdateSo(SaveDataBehaviour currentSo, SaveDataBehaviourTypeStruct currentTypeStruct)
        {
            int currentTypeIndexInSo = -1;
            if (currentSo.ListOfType.IndexOf(currentTypeStruct) != -1)
            {
                currentTypeIndexInSo = currentSo.ListOfType.IndexOf(currentTypeStruct);

                currentSo.ListOfType[currentTypeIndexInSo] = currentTypeStruct;
            }
            else
            {
                currentSo.ListOfType.Add(currentTypeStruct);
            }
        }
    }
}

