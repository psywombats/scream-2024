using UnityEngine;
using System.Collections.Generic;

public abstract class SerializableObjectIndex<T> : ScriptableObjectIndexParent, IDataIndex<T> where T : IKeyedDataObject
{
    [SerializeField] protected List<T> dataObjects = null;

    private Dictionary<string, T> tagToDataObject;
    protected Dictionary<string, T> TagToDataObject
    {
        get
        {
            if ( tagToDataObject == null )
            {
                if (dataObjects == null)
                {
                    return new Dictionary<string, T>();
                }
                tagToDataObject = new Dictionary<string, T>();
                foreach (T dataObject in dataObjects)
                {
                    if (dataObject == null || dataObject.Key == null) continue;
                    TagToDataObject[dataObject.Key.ToLower()] = dataObject;
                }
            }
            return tagToDataObject;
        }
    }

    public T GetData(string key)
    {
        if (!TagToDataObject.ContainsKey(key.ToLower()))
        {
            Debug.LogError("Index " + GetType().Name + " does not contain key\"" + key + "\"");
            return default;
        }
        return TagToDataObject[key.ToLower()];
    }

    public T GetDataOrNull(string tag)
    {
        if (TagToDataObject.ContainsKey(tag.ToLower()))
        {
            return GetData(tag);
        }
        else
        {
            return default;
        }
    }

    public List<T> GetAll()
    {
        return dataObjects;
    }
}
