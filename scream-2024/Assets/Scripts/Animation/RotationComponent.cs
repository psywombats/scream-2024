using UnityEngine;

public class RotationComponent : MonoBehaviour
{
    [SerializeField] public Vector3 rot;
    [SerializeField] public Vector3 mot;

    public void Update()
    {
        transform.localEulerAngles += rot * Time.deltaTime;
        transform.localPosition += mot * Time.deltaTime;
    }
}
