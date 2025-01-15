using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditorInternal;

namespace SaveRuntime.Editor
{
    public static class SaveFile
    {
        [SerializeField]
        static AssemblyDefinitionReferenceAsset _test;
        
        /**
         * Save Data content at runtime
         */
        public static void SaveContent()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "/saveDataState.utils";
            FileStream stream = new FileStream(path, FileMode.Create);
        
            //formatter.Serialize(stream, data);
            stream.Close();
        }
    }
}