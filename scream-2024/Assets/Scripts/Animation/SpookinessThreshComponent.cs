using UnityEngine;

public class SpookinessThreshComponent : MonoBehaviour
{
    [SerializeField][Range(0, 1f)] private float spookinessRequired;
    [SerializeField] private GameObject enableChild;

    public void Update()
    {
        if (Global.Instance.Maps.ActiveMap != null)
        {
            enableChild.SetActive(Global.Instance.Maps.ActiveMap.spookiness >= spookinessRequired);
        }
        
    }
}

