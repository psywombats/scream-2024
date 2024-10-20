using System.Collections;
using UnityEngine;

public class PlayBGMComponent : MonoBehaviour
{
    [SerializeField] private AudioManager.Bank bank = AudioManager.Bank.BGM;
    [SerializeField] private string bgmKey = null;
    [SerializeField] private GameObject source = null;

    public void Start()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        yield return null;
        var source = this.source;
        if (bank == AudioManager.Bank.ENV && source == null)
        {
            source = Global.Instance.Avatar.gameObject;
        }
        Global.Instance.Audio.PlayBGM(bgmKey, bank, source);

    }
}
