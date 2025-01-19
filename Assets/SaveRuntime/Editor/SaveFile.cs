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

namespace SaveRuntime.Editor
{
    public static class SaveFile
    {
        public static List<SavableInstanceOfDataStruct> DataPersistentObjNonSorted;
        public static List<SavableInstanceOfDataStruct> DataPersistentObjSorted;
        
        /**
         * Save Data content at runtime
         */
        public static void SaveContent(List<SavableInstanceOfDataStruct> listOfDataStruct)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "/saveDataState.utils";
            FileStream stream = new FileStream(path, FileMode.Create);

            List<SavableStructWithoutScript> finalSavePoint = new List<SavableStructWithoutScript>();
            foreach (var savableInstanceOfDataStruct in listOfDataStruct)
            {
                finalSavePoint.Add(new SavableStructWithoutScript(savableInstanceOfDataStruct.NameOfTheObject, savableInstanceOfDataStruct.Id,savableInstanceOfDataStruct.TypeOfClass, savableInstanceOfDataStruct.Fields));
            }
            formatter.Serialize(stream, finalSavePoint);
            stream.Close();
        }

        // public static void LoadContent()
        // {
        //     SavableInstanceOfDataStruct
        // }

        /**
         * Find All instances for savable files
         */
        public static List<SavableInstanceOfDataStruct> FindAllInstanceSavable()
        {
            var instanceOfType = MonoBehaviour.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
            List<SavableInstanceOfDataStruct> finalListOfSavableDataStruct = new List<SavableInstanceOfDataStruct>();
            foreach (var obj in instanceOfType)
            {
                SavableAttribute currentSavableAttribute = obj.GetType().GetCustomAttribute<SavableAttribute>();
                if (currentSavableAttribute != null)
                {
                    //Debug.Log(obj.name + " Id : " + obj.GetInstanceID() + "Type : " + currentSavableAttribute.type.Name);
                    foreach (var fieldInfo in currentSavableAttribute.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        //Debug.Log(fieldInfo.GetValue(obj) + " Name : " + fieldInfo.Name);
                    }
                    finalListOfSavableDataStruct.Add(new SavableInstanceOfDataStruct(obj.name, obj.GetInstanceID(), currentSavableAttribute.type, obj, new List<FieldInfo>(currentSavableAttribute.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))));
                }
            }
            return finalListOfSavableDataStruct;
        }
    }
}