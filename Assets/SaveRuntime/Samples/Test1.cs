using UnityEngine;

[Savable(typeof(Test1), CanBeModified = true)]
[System.Serializable]
public class Test1 : MonoBehaviour
{
    public bool bool1 = true;

    public string string1 = "string1";

    protected int int1 = 15;

    private Vector3 position;

}
