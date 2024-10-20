using UnityEngine;

public class RotationComponent : MonoBehaviour
{
    [SerializeField] public Vector3 rot;

    public void Update()
    {
        transform.localEulerAngles += rot * Time.deltaTime;
    }
}
