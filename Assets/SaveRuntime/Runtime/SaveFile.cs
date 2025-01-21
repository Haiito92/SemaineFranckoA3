using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Properties;
using UnityEditorInternal;
using Object = UnityEngine.Object;
using SaveRuntime.Runtime;
using UnityEditor;


namespace SaveRuntime.Runtime
{
    public static class SaveFile
    {
        public static List<SavableInstanceOfDataStruct> DataPersistentObjNonSorted;
        public static List<SavableInstanceOfDataStruct> DataPersistentObjSorted;
        
        public static SaveDataBehaviour _currentDataBehaviour;
        
        /**
         * Save Data content at runtime
         */
        private static void SaveContent(List<SavableInstanceOfDataStruct> listOfDataStruct)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "/saveDataState.utils";
            FileStream stream = new FileStream(path, FileMode.Create);

            List<SavableStructWithoutScript> finalSavePoint = new List<SavableStructWithoutScript>();
            foreach (var savableInstanceOfDataStruct in listOfDataStruct)
            {
                Dictionary<FieldInfo, object> dico = new Dictionary<FieldInfo, object>();
                foreach (var fieldInfo in savableInstanceOfDataStruct.Fields)
                {
                    dico.Add(fieldInfo, fieldInfo.GetValue(savableInstanceOfDataStruct.Script));
                }
                finalSavePoint.Add(new SavableStructWithoutScript(savableInstanceOfDataStruct.NameOfTheObject, savableInstanceOfDataStruct.Id,savableInstanceOfDataStruct.TypeOfClass, savableInstanceOfDataStruct.Fields, dico));
            }
            formatter.Serialize(stream, finalSavePoint);
            stream.Close();
        }

        private static List<SavableStructWithoutScript> LoadContent()
        {
            string path = Application.persistentDataPath + "/saveDataState.utils";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                List<SavableStructWithoutScript> deserializeData = formatter.Deserialize(stream) as List<SavableStructWithoutScript>;
                stream.Close();

                return deserializeData;
            }
            else
            {
                Debug.Log("Not Found" + path);
                return null;
            }
        }

        /**
         * Find All instances for savable files
         */
        private static List<SavableInstanceOfDataStruct> FindAllInstanceSavable()
        {
            var instanceOfType = MonoBehaviour.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
            List<SavableInstanceOfDataStruct> finalListOfSavableDataStruct = new List<SavableInstanceOfDataStruct>();
            foreach (var obj in instanceOfType)
            {
                SavableAttribute currentSavableAttribute = obj.GetType().GetCustomAttribute<SavableAttribute>();
                if (currentSavableAttribute != null)
                {
                    //Debug.Log(obj.name + " Id : " + obj.GetInstanceID() + "Type : " + currentSavableAttribute.type.Name);
                    // foreach (var fieldInfo in currentSavableAttribute.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    // {
                    //     Debug.Log(fieldInfo.GetValue(obj) + " Name : " + fieldInfo.Name);
                    // }
                    finalListOfSavableDataStruct.Add(new SavableInstanceOfDataStruct(obj.name, obj.GetInstanceID(), currentSavableAttribute.type, obj, new List<FieldInfo>(currentSavableAttribute.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))));
                }
            }
            return finalListOfSavableDataStruct;
        }
        
        /**
         * Find All Instance of DATA STRUCT
         */
        public static List<SavableInstanceOfDataStruct> GetSortedListOfSavableDataStruct(
            List<SavableInstanceOfDataStruct> nonSortedSavableDataStruct)
        {
            List<SavableInstanceOfDataStruct> finalListOfObjWithNiceFields = new List<SavableInstanceOfDataStruct>();
            foreach (var savableInstanceOfDataStruct in nonSortedSavableDataStruct) //For each of OBJ
            {
                List<FieldInfo> newFieldsInfo = new List<FieldInfo>();
                //Find a SO type equal to the type of the instance of Script
                SaveDataBehaviourTypeStruct tempTypeStruct = _currentDataBehaviour.ListOfType.Find(x =>
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
                                //Debug.Log("Name of type :  " + savableInstanceOfDataStruct.NameOfTheObject + "Name : " + fieldInfoOfSavableAsset.Name + " " + "Value : " + newVar);
                            }
                        }
                    }
                    
                    finalListOfObjWithNiceFields.Add(new SavableInstanceOfDataStruct(savableInstanceOfDataStruct.NameOfTheObject, savableInstanceOfDataStruct.Id, savableInstanceOfDataStruct.TypeOfClass, savableInstanceOfDataStruct.Script, newFieldsInfo));
                }
            }
            return finalListOfObjWithNiceFields;
        }


        public static void Save()
        {
            //Get Data Sorted
            Debug.Log("qsd");
            var path = AssetDatabase.FindAssets("t:SaveDataBehaviour")[0];
            var finalpath = AssetDatabase.GUIDToAssetPath(path);
            _currentDataBehaviour = AssetDatabase.LoadAssetAtPath<SaveDataBehaviour>(finalpath);
            DataPersistentObjNonSorted = FindAllInstanceSavable();
            DataPersistentObjSorted =
                GetSortedListOfSavableDataStruct(DataPersistentObjNonSorted);
            SaveContent(DataPersistentObjSorted);
        }

        public static void Load()
        {
            var path = AssetDatabase.FindAssets("t:SaveDataBehaviour")[0];
            var finalpath = AssetDatabase.GUIDToAssetPath(path);
            _currentDataBehaviour = AssetDatabase.LoadAssetAtPath<SaveDataBehaviour>(finalpath);
            List<SavableStructWithoutScript> Content = LoadContent();
            // foreach (var savableInstanceOfDataStruct in Content)
            // {
            //     Debug.Log(savableInstanceOfDataStruct.NameOfTheObject + " Id : " + savableInstanceOfDataStruct.Id + "Type : " + savableInstanceOfDataStruct.TypeOfClass);
            //     foreach (var keys in savableInstanceOfDataStruct.DicoFieldsForValue)
            //     {
            //         Debug.Log("Field info name : " + keys.Key.Name + " " + "Value : " + keys.Value);
            //     }
            // }
            //Get Data Sorted
            DataPersistentObjNonSorted = FindAllInstanceSavable();
            DataPersistentObjSorted =
                GetSortedListOfSavableDataStruct(DataPersistentObjNonSorted);
            foreach (var currentInstanceInGame in DataPersistentObjSorted)
            {
                SavableStructWithoutScript goodOne =
                    Content.Find(x => x.NameOfTheObject == currentInstanceInGame.NameOfTheObject);
                if (goodOne.Fields.Any())
                {
                    foreach (var fieldInfoOfCurrentObj in currentInstanceInGame.Fields)
                    {
                        foreach (var fieldInfoOfSavedObj in goodOne.Fields)
                        {
                            if (fieldInfoOfCurrentObj.Name == fieldInfoOfSavedObj.Name)
                            {
                                fieldInfoOfCurrentObj.SetValue(currentInstanceInGame.Script,goodOne.DicoFieldsForValue[fieldInfoOfSavedObj]);
                            }
                        }
                    }
                }
            }
        }
    }
}