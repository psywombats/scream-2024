using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeakerData", menuName = "Data/Speaker")]
public class SpeakerData : MainSchema {

    public string tag;
    public string displayName;
    public Sprite image;
    public List<Expression> expressions;

    public override string Key => tag;
}

[System.Serializable]
public class Expression {

    public Sprite sprite;
    public string tag;
}
