using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRelativeComponent : MonoBehaviour
{
    public Vector3 rel;

    // Update is called once per frame
    public void Update()
    {
        transform.position = Global.Instance.Avatar.transform.position + rel;
    }
}
