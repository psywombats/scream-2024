using UnityEngine;

public class StopEnvComponent : MonoBehaviour
{
    public void OnEnable()
    {
        AudioManager.Instance.StopENV();
    }
}
