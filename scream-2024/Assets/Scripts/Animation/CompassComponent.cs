using UnityEngine;

public class CompassComponent : MonoBehaviour
{
    public void Update()
    {
        if (Global.Instance.Avatar != null)
        {
            transform.localEulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                Global.Instance.Avatar.camera.transform.eulerAngles.y);
        }
    }
}