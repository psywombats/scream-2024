using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : SingletonBehavior
{
    public static AudioManager Instance => Global.Instance.Audio;

    private EventInstance bgmEvent;

    public const string NoBGMKey = "none";
    private const string NoChangeBGMKey = "no_change";
    private const float FadeSeconds = 0.5f;

    public float BaseVolume { get; set; } = 1.0f;
    private float bgmVolumeMult = 1.0f;

    public string CurrentBGMKey { get; private set; }

    private EventInstance sfxEvent;

    public void Start()
    {
        CurrentBGMKey = NoBGMKey;
        SetVolume();
    }

    public void PlayBGM(string key)
    {
        if (Global.Instance.Data.GetSwitch("disable_bgm"))
        {
            return;
        }
        if (key != CurrentBGMKey && key != NoChangeBGMKey && key != null && key.Length > 0)
        {
            if (bgmEvent.hasHandle())
            {
                bgmEvent.stop(STOP_MODE.ALLOWFADEOUT);
                bgmEvent.clearHandle();
            }
            BaseVolume = 1f;
            SetVolume();
            CurrentBGMKey = key;
            bgmEvent = RuntimeManager.CreateInstance($"event:/BGM/{key}");
            bgmEvent.start();
        }
    }

    public void PlaySFX(string sfxKey)
    {
        sfxEvent = RuntimeManager.CreateInstance($"event:/SFX/{sfxKey}");
        sfxEvent.start();
    }

    public void StopSFX()
    {
        sfxEvent.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void SetVolume()
    {
        var sfxBus = RuntimeManager.GetBus("bus:/SFX");
        var bgmBus = RuntimeManager.GetBus("bus:/BGM");
        sfxBus.setVolume(BaseVolume);
        bgmBus.setVolume(BaseVolume);
    }

    public IEnumerator FadeOutRoutine(float durationSeconds)
    {
        CurrentBGMKey = NoBGMKey;
        while (BaseVolume > 0.0f)
        {
            BaseVolume -= Time.deltaTime / durationSeconds;
            if (BaseVolume < 0.0f)
            {
                BaseVolume = 0.0f;
            }
            SetVolume();
            yield return null;
        }
        if (bgmEvent.hasHandle())
        {
            bgmEvent.stop(STOP_MODE.ALLOWFADEOUT);
            bgmEvent.clearHandle();
        }
        BaseVolume = 1.0f;
        SetVolume();
        PlayBGM(NoBGMKey);
    }

    public IEnumerator CrossfadeRoutine(string tag)
    {
        if (CurrentBGMKey != null && CurrentBGMKey != NoBGMKey)
        {
            yield return FadeOutRoutine(FadeSeconds);
        }
        PlayBGM(tag);
    }
}
