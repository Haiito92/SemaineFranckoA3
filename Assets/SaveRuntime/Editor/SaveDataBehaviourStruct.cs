using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaveRuntime.Editor
{
    [System.Serializable]
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

    [System.Serializable]
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
}

