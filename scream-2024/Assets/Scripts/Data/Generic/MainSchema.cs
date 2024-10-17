using UnityEngine;

public class MainSchema : ScriptableObject, IKeyedDataObject
{
    public virtual string Key => name;
}
