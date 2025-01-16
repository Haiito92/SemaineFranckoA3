using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct LocalizationData
{
    public string Key;
    public Component TextComponent;

    public LocalizationData(Component newComponent,  string newKey = "")
    {
        Key = newKey;
        TextComponent = newComponent;      
    }

}
