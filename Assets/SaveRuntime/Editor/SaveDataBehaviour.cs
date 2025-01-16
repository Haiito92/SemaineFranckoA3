using System.Collections.Generic;
using SaveRuntime.Editor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaveRuntime.Editor
{ 
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SaveDataBehaviourSO", order = 1)] 
    public class SaveDataBehaviour : ScriptableObject
    {
        public List<SaveDataBehaviourTypeStruct> ListOfType = new List<SaveDataBehaviourTypeStruct>();
    }
}
