using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditorInternal;

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

        /*
        //Code Test For CustomAttribute
        Assembly[] _asss = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in _asss)
        {
            foreach (Type TypeOfAssClasses in assembly.GetTypes())
            {
                SavableAttribute savable = TypeOfAssClasses.GetCustomAttribute<SavableAttribute>();
                if (savable != null)
                {
                    
                }
            }
        }
        */
    }
}
