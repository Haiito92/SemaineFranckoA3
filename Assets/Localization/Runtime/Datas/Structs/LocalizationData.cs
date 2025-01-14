using System;
using UnityEngine;

[Serializable]
public struct LocalizationData
{
    public string Key;
    public string Guid;
    public string Content;

    public LocalizationData(string newContent, string newKey = "")
    {
        Key = newKey;
        Guid = System.Guid.NewGuid().ToString();
        Content = newContent;
    }

}
