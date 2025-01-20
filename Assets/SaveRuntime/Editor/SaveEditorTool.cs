using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
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
        
        private List<Type> _allTypes = new List<Type>();
        
        private Dictionary<string, bool> _foldoutProperties = new Dictionary<string, bool>();
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
        public static void ShowWindow(SaveDataBehaviour saveObject)
        {
            if (!_window)
            {
                _window = GetWindow<SaveEditorTool>("SaveEditorTool");
                //Load current Data
                _window.Load(saveObject);
            }
            // TODO - SetWindowSize
        }
        
        private void Load(SaveDataBehaviour saveObject)
        {
            _currentDataBehaviour = saveObject;
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
                    UpdateSo(_currentDataBehaviour, newSaveDataTypeStruct);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            
            //ScrollView
            _scrollView = EditorGUILayout.BeginScrollView(_scrollView, GUILayout.Height(300));
            
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
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
            //Save All Presset
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Data"))
            {
                SaveFile.DataPersistentObjNonSorted = SaveFile.FindAllInstanceSavable();
                SaveFile.DataPersistentObjSorted =
                    GetSortedListOfSavableDataStruct(SaveFile.DataPersistentObjNonSorted);
                SaveFile.SaveContent(SaveFile.DataPersistentObjSorted);
            }
            GUILayout.EndHorizontal();
            
            //Load Button
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Load Data"))
            {
                List<SavableStructWithoutScript> Content = SaveFile.LoadContent();
                foreach (var savableInstanceOfDataStruct in Content)
                {
                    Debug.Log(savableInstanceOfDataStruct.NameOfTheObject + " Id : " + savableInstanceOfDataStruct.Id + "Type : " + savableInstanceOfDataStruct.TypeOfClass);
                    foreach (var fieldInfo in savableInstanceOfDataStruct.Fields)
                    {
                        Debug.Log(fieldInfo.Name + " ");
                    }
                }
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

        public List<SavableInstanceOfDataStruct> GetSortedListOfSavableDataStruct(
            List<SavableInstanceOfDataStruct> nonSortedSavableDataStruct)
        {
            List<SavableInstanceOfDataStruct> finalListOfObjWithNiceFields = new List<SavableInstanceOfDataStruct>();
            foreach (var savableInstanceOfDataStruct in nonSortedSavableDataStruct) //For each of OBJ
            {
                List<FieldInfo> newFieldsInfo = new List<FieldInfo>();
                //Find a SO type equal to the type of the instance of Script
                SaveDataBehaviourTypeStruct tempTypeStruct =
                    _currentDataBehaviour.ListOfType.Find(x =>
                        x.Name == savableInstanceOfDataStruct.TypeOfClass.Name);
                
                
                if (tempTypeStruct.ListOfFields.Any()) //Check if we found a common type
                {
                    foreach (var fieldInfoOfSavableAsset in savableInstanceOfDataStruct.Fields)
                    {
                        SaveDataBehaviourStruct currentSoField = tempTypeStruct.ListOfFields.Find(x => x.NameOfField == fieldInfoOfSavableAsset.Name);
                        if (!String.IsNullOrEmpty(currentSoField.NameOfField)) //Check if the field was found
                        {
                            var newVar = fieldInfoOfSavableAsset.GetValue(savableInstanceOfDataStruct.Script);
                            if (currentSoField.IsChecked) //If the SO Field is check, we add it
                            {
                                newFieldsInfo.Add(fieldInfoOfSavableAsset); 
                                Debug.Log("Name of type :  " + savableInstanceOfDataStruct.NameOfTheObject + "Name : " + fieldInfoOfSavableAsset.Name + " " + "Value : " + newVar);
                            }
                        }
                    }
                    
                    finalListOfObjWithNiceFields.Add(new SavableInstanceOfDataStruct(savableInstanceOfDataStruct.NameOfTheObject, savableInstanceOfDataStruct.Id, savableInstanceOfDataStruct.TypeOfClass, savableInstanceOfDataStruct.Script, newFieldsInfo));
                }
            }
            return finalListOfObjWithNiceFields;
        }
    }
}

