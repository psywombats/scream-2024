using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEnvComponent : MonoBehaviour
{
    [SerializeField] private GameObject src = null;
    [SerializeField] private List<string> envNames;
    [SerializeField] private bool killOtherEnvs;

    public void OnEnable()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        yield return null;
        if (killOtherEnvs)
        {
            AudioManager.Instance.StopENV();
        }
        foreach (var key in envNames)
        {
            AudioManager.Instance.PlayENV(key, src);
        }
    }
}
