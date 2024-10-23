using System;
using UnityEngine;

[UnityEngine.CreateAssetMenu(fileName = "CutinIndexData", menuName = "Data/Index/Cutin")]
public class CutinIndexData : SerializableObjectIndex<CutinData>
{

}

[Serializable]
public class CutinData : GenericDataObject
{
    public Sprite sprite;
}
