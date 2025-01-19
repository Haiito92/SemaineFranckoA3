using UnityEngine;

[Savable(typeof(Test2),CanBeModified = true)]
[System.Serializable]
public class Test2 : MonoBehaviour
{
    public bool bool1 = true;

    private string string1 = "string1";

    protected int int1 = 14;
    
}
