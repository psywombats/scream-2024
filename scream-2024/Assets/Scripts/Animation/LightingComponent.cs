using UnityEngine;

public class LightingComponent : MonoBehaviour
{
    [SerializeField] public LightingMode requiredMode;
    [SerializeField] public GameObject enableChild;

    public void OnEnable()
    {
        enableChild.SetActive(MapManager.Instance.ActiveMap.lighting == requiredMode);
    }
}
