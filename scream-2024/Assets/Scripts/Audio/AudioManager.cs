using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : SingletonBehavior
{
    public enum Bank
    {
        Testing,
        BGM,
        ENV,
        SFX,
        UI,
    }

    public static AudioManager Instance => Global.Instance.Audio;

    [SerializeField] private StudioEventEmitter nightmareSnap;

    private EventInstance bgmEvent;
    private EventInstance sfxEvent;
    private Dictionary<string, EventInstance> envEvents = new();
    private Dictionary<string, string> envParams = new();

    public const string NoBGMKey = "none";
    private const string NoChangeBGMKey = "no_change";
    private const float FadeSeconds = 0.5f;

    public float BaseVolume { get; set; } = 1.0f;

    public string CurrentBGMKey { get; private set; }

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
            if (key.StartsWith(NoBGMKey))
            {
                return;
            }
            foreach (var ev in envEvents.Values)
            {
                if (ev.hasHandle())
                {
                    ev.stop(STOP_MODE.ALLOWFADEOUT);
                    ev.clearHandle();
                }
            }
            envEvents.Clear();
            BaseVolume = 1f;
            SetVolume();
            CurrentBGMKey = key;
            bgmEvent = RuntimeManager.CreateInstance($"event:/BGM/{key}");
            bgmEvent.start();
        }
    }

    public void PlaySFX(string sfxKey, GameObject src = null, Bank bank = Bank.SFX)
    {
        //var banksy = bank == Bank.BGM ? "BGM" : "Other/" + bank;
        sfxEvent = RuntimeManager.CreateInstance($"event:/{bank}/{sfxKey}");
        if (src != null)
        {
            RuntimeManager.AttachInstanceToGameObject(sfxEvent, src.transform);
        }
        sfxEvent.start();
    }

    public void PlayENV(string envKey, GameObject src = null)
    {
        if (envEvents.ContainsKey(envKey))
        {
            return;
        }
        var envEvent = RuntimeManager.CreateInstance($"event:/ENV/{envKey}");
        if (src != null || Global.Instance.Avatar != null)
        {
            RuntimeManager.AttachInstanceToGameObject(envEvent, src == null ? Global.Instance.Avatar.transform : src.transform);
        }
        foreach (var param in envParams)
        {
            envEvent.setParameterByNameWithLabel(param.Key, param.Value);
        }
        envEvent.start();
        envEvents[envKey] = envEvent;
    }

    public void SetENVParam(string key, string value)
    {
        envParams[key] = value;
    }

    public void SetGlobalParam(string key, float value)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(key, value);
    }

    public void SetGlobalParam(string key, string value)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel(key, value);
    }

    public void StopSFX()
    {
        sfxEvent.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void StopENV()
    {
        foreach (var envEvent in envEvents.Values)
        {
            if (envEvent.hasHandle())
            {
                envEvent.stop(STOP_MODE.ALLOWFADEOUT);
                envEvent.clearHandle();
            } 
        }
        envEvents.Clear();
    }

    public void SetVolume()
    {
        //var sfxBus = RuntimeManager.GetBus("bus:/SFX");
        var bgmBus = RuntimeManager.GetBus("bus:/BGM");
        //var envBus = RuntimeManager.GetBus("bus:/UI");
        //var uiBus = RuntimeManager.GetBus("bus:/ENV");
        //sfxBus.setVolume(BaseVolume);
        bgmBus.setVolume(BaseVolume);
        //uiBus.setVolume(BaseVolume);
        //envBus.setVolume(BaseVolume);
    }

    public void SetNightmare(bool isNightmare)
    {
        if (!nightmareSnap.IsPlaying() && isNightmare)
        {
            nightmareSnap.Play();
        } 
        else if (nightmareSnap.IsPlaying() && !isNightmare)
        {
            nightmareSnap.Stop();
        }
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
        StopENV();

        BaseVolume = 1.0f;
        SetVolume();
        PlayBGM(NoBGMKey);
    }

    public IEnumerator CrossfadeRoutine(string tag, Bank bank = Bank.BGM)
    {
        if (CurrentBGMKey != null && CurrentBGMKey != NoBGMKey)
        {
            yield return FadeOutRoutine(FadeSeconds);
        }
        PlayBGM(tag);
    }
}
