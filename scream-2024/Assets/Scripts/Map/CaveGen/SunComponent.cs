using UnityEngine;

public class SunComponent : MonoBehaviour
{
    [SerializeField] private float height = 50;
    [SerializeField] private float powerRate = -1;
    [SerializeField] private float powerFix = 500;
    [SerializeField] private new Light light;

    public void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            Global.Instance.Avatar.transform.position.y + height,
            transform.position.z);
        light.intensity = powerFix + Global.Instance.Avatar.transform.position.y * powerRate;
    }
}
