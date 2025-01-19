using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SaveRuntime.Editor
{
    [Serializable]
    public struct SaveDataBehaviourStruct
    {
        public string NameOfField;
        public bool IsChecked;
        
        public SaveDataBehaviourStruct(string name, bool toggleCheck)
        {
            NameOfField = name;
            IsChecked = toggleCheck;
        }

        public void SetIsChecked(bool checkNewValue)
        {
            IsChecked = checkNewValue;
        }
        
        public void SetIsChecked(SaveDataBehaviourStruct data)
        {
            IsChecked = data.IsChecked;
        }

        public static SaveDataBehaviourStruct SetCheck(SaveDataBehaviourStruct data, bool value)
        {
            data.IsChecked = value;
            return data;
        }
    }

    [Serializable]
    public struct SaveDataBehaviourTypeStruct
    {
        public string Name;
        public int Id;
        public Type TypeOfClass;
        public List<SaveDataBehaviourStruct> ListOfFields;

        public SaveDataBehaviourTypeStruct(string newName,int id, Type newTypeOfClass, List<SaveDataBehaviourStruct> newListOfSaveDataBehaviour)
        {
            Name = newName;
            TypeOfClass = newTypeOfClass;
            ListOfFields = newListOfSaveDataBehaviour;
            Id = id;
        }
    }
    
    [Serializable]
    public struct SavableInstanceOfDataStruct
    {
        public string NameOfTheObject;
        public int Id;
        public MonoBehaviour Script;
        public Type TypeOfClass;

        public List<FieldInfo> Fields;
        
        public SavableInstanceOfDataStruct(string nameOfTheObject,int id, Type newTypeOfClass, MonoBehaviour script, List<FieldInfo> fields)
        {
            NameOfTheObject = nameOfTheObject;
            TypeOfClass = newTypeOfClass;
            Id = id;
            Script = script;
            Fields = fields;
        }
    }
    
    [Serializable]
    public struct SavableStructWithoutScript
    {
        public string NameOfTheObject;
        public int Id;
        public Type TypeOfClass;

        public List<FieldInfo> Fields;
        
        public SavableStructWithoutScript(string nameOfTheObject,int id, Type newTypeOfClass, List<FieldInfo> fields)
        {
            NameOfTheObject = nameOfTheObject;
            TypeOfClass = newTypeOfClass;
            Id = id;
            Fields = fields;
        }
    }
}

