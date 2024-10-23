using System;
using System.Collections.Generic;
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
    public Sprite glow;
    public Sprite radioSprite;
    public Sprite moonSprite;
    public List<Expression> exprs;
}

[Serializable]
public struct Expression
{
    public string key;
    public Sprite sprite;
}
