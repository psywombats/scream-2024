using System;
using UnityEngine;

[UnityEngine.CreateAssetMenu(fileName = "SpeakerIndexData", menuName = "Data/Index/Speaker")]
public class SpeakerIndexData : SerializableObjectIndex<SpeakerData>
{

}

[Serializable]
public class SpeakerData : GenericDataObject
{
    public string displayName;
    public Sprite sprite;
}
