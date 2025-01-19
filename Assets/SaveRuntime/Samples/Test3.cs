using UnityEngine;

[Savable(typeof(Test3),CanBeModified = true)]
[System.Serializable]
public class Test3 : MonoBehaviour
{
    public bool bool1 = true;

    private string string1 = "string1";

    protected int int1 = 10;
    
}
