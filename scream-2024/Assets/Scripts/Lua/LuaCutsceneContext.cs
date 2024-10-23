using System.Collections;
using System;
using MoonSharp.Interpreter;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LuaCutsceneContext : LuaContext
{
    private static readonly string DefinesPath = "Lua/Defines/CutsceneDefines";

    public override IEnumerator RunRoutine(LuaScript script, bool canBlock)
    {
        if (canBlock && Global.Instance.Avatar != null)
        {
            Global.Instance.Avatar.PauseInput();
        }
        yield return base.RunRoutine(script, canBlock);
        if (canBlock && Global.Instance.Avatar != null)
        {
            Global.Instance.Avatar.UnpauseInput();
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        LoadDefines(DefinesPath);
    }

    public override void RunRoutineFromLua(IEnumerator routine)
    {
        base.RunRoutineFromLua(routine);
    }

    public void RunTextboxRoutineFromLua(IEnumerator routine)
    {
        base.RunRoutineFromLua(routine);
    }

    protected void ResumeNextFrame()
    {
        Global.Instance.StartCoroutine(ResumeRoutine());
    }
    protected IEnumerator ResumeRoutine()
    {
        yield return null;
        ResumeAwaitedScript();
    }

    protected override void AssignGlobals()
    {
        base.AssignGlobals();
        Lua.Globals["playBGM"] = (Action<DynValue>)PlayBGM;
        Lua.Globals["playSFX"] = (Action<DynValue>)PlaySound;
        Lua.Globals["sceneSwitch"] = (Action<DynValue, DynValue>)SetSwitch;
        Lua.Globals["cs_teleport"] = (Action<DynValue, DynValue, DynValue, DynValue>)TargetTeleport;
        Lua.Globals["cs_fadeOutBGM"] = (Action<DynValue>)FadeOutBGM;
        Lua.Globals["cs_fade"] = (Action<DynValue, DynValue>)Fade;

        Lua.Globals["cs_enterNVL"] = (Action<DynValue>)EnterNVL;
        Lua.Globals["cs_exitNVL"] = (Action)ExitNVL;
        Lua.Globals["cs_enter"] = (Action<DynValue, DynValue, DynValue>)Enter;
        Lua.Globals["cs_exit"] = (Action<DynValue>)Exit;
        Lua.Globals["cs_speak"] = (Action<DynValue, DynValue>)Speak;
        Lua.Globals["cs_radio"] = (Action<DynValue, DynValue, DynValue>)Radio;
        Lua.Globals["cs_expr"] = (Action<DynValue, DynValue>)Express;
        Lua.Globals["clear"] = (Action)ClearNVL;
        Lua.Globals["hideRadio"] = (Action)HideRadio;
        Lua.Globals["exitRadio"] = (Action)HideRadio;

        Lua.Globals["cs_rotateTo"] = (Action<DynValue>)RotateToward;
        Lua.Globals["setting"] = (Action<DynValue>)Setting;
        Lua.Globals["showFlares"] = (Action)ShowFlares;
        Lua.Globals["endGame"] = (Action)EndGame;
        Lua.Globals["endGame2"] = (Action)EndGame2;
    }

    // === LUA CALLABLE ============================================================================


    private void PlayBGM(DynValue bgmKey)
    {
        Global.Instance.Audio.PlayBGM(bgmKey.String);
    }

    private void PlaySound(DynValue soundKey)
    {
        Global.Instance.Audio.PlaySFX(soundKey.String);
    }

    private void TargetTeleport(DynValue mapName, DynValue targetEventName, DynValue facingLua, DynValue rawLua)
    {
        OrthoDir? facing = null;
        if (!facingLua.IsNil()) facing = OrthoDirExtensions.Parse(facingLua.String);
        var raw = rawLua.IsNil() ? false : rawLua.Boolean;
        RunRoutineFromLua(Global.Instance.Maps.TeleportRoutine(mapName.String, targetEventName.String, facing, raw));
    }

    private void FadeOutBGM(DynValue seconds)
    {
        RunRoutineFromLua(Global.Instance.Audio.FadeOutRoutine((float)seconds.Number));
    }

    private void Fade(DynValue type, DynValue dur)
    {
        var str = type.String;
        RunRoutineFromLua(FadeRoutine(str, dur.IsNil() ? .8f : (float)dur.Number));
    }
    private IEnumerator FadeRoutine(string str, float dur)
    {
        var targetAlpha = str == "black" ? 1f : 0f;
        yield return CoUtils.RunTween(MapOverlayUI.Instance.fader.DOFade(targetAlpha, dur));
    }

    public void EnterNVL(DynValue hideBackersLua)
    {
        var hideBackers = !hideBackersLua.IsNil() && hideBackersLua.Boolean;
        RunRoutineFromLua(EnterNVLRoutine(hideBackers));
    }
    private IEnumerator EnterNVLRoutine(bool hideBackers)
    {
        yield return MapOverlayUI.Instance.nvl.ShowRoutine(hideBackers);
    }

    public void ExitNVL()
    {
        RunRoutineFromLua(ExitNVLRoutine());
    }
    private IEnumerator ExitNVLRoutine()
    {
        yield return MapOverlayUI.Instance.nvl.HideRoutine();
    }

    public void Enter(DynValue speakerNameLua, DynValue slotLua, DynValue exprLua)
    {
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerNameLua.String);
        var slot = slotLua.String;
        var alt = exprLua.String;
        RunRoutineFromLua(EnterRoutine(speaker, slot, alt));
    }
    private IEnumerator EnterRoutine(SpeakerData speaker, string slot, string expr = null)
    {
        yield return MapOverlayUI.Instance.nvl.EnterRoutine(speaker, slot, expr);
    }

    public void Express(DynValue charaLu, DynValue expr)
    {
        RunRoutineFromLua(ExpressRoutine(charaLu.String, expr.String));
    }
    private IEnumerator ExpressRoutine(string charaTag, string expr)
    {
        var adv = MapOverlayUI.Instance.nvl;
        var speaker = IndexDatabase.Instance.Speakers.GetData(charaTag);
        return adv.GetPortrait(speaker).ExpressRoutine(expr);
    }

    public void Exit(DynValue speakerNameLua)
    {
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerNameLua.String);
        RunRoutineFromLua(ExitRoutine(speaker));
    }
    private IEnumerator ExitRoutine(SpeakerData speaker)
    {
        yield return MapOverlayUI.Instance.nvl.ExitRoutine(speaker);
    }

    public void ClearNVL()
    {
        MapOverlayUI.Instance.nvl.Wipe();
    }

    public void Speak(DynValue speakerNameLua, DynValue messageLua)
    {
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerNameLua.String);
        RunRoutineFromLua(SpeakRoutine(speaker, messageLua.String));
    }
    private IEnumerator SpeakRoutine(SpeakerData speaker, string message)
    {
        yield return MapOverlayUI.Instance.nvl.SpeakRoutine(speaker, message);
    }

    private void Setting(DynValue textVal)
    {
        //MapOverlayUI.Instance.setting.Show(textVal.String);
    }

    private void RotateToward(DynValue eventName)
    {
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(eventName.String);
        RunRoutineFromLua(Global.Instance.Avatar.RotateTowardRoutine(
            Global.Instance.Avatar.firstPersonParent, 
            @event.gameObject,
            Global.Instance.Avatar.camera.gameObject));
    }

    private void Radio(DynValue speakerLua, DynValue textLua, DynValue qualLua)
    {
        var qual = RadioUIComponent.RadioQual.good;
        if (!qualLua.IsNil())
        {
            qual = Enum.Parse<RadioUIComponent.RadioQual>(qualLua.String);
        }
        RunRoutineFromLua(RadioRoutine(speakerLua.String, textLua.String, qual));
    }
    private IEnumerator RadioRoutine(string speakerKey, string message, RadioUIComponent.RadioQual qual)
    {
        yield return MapOverlayUI.Instance.radio.SpeakRoutine(speakerKey, message, qual);
    }

    private void HideRadio()
    {
        Global.Instance.StartCoroutine(MapOverlayUI.Instance.radio.HideRoutine());
    }

    private void ShowFlares()
    {
        MapOverlayUI.Instance.flareInfo.alpha = 1f;
    }

    private void EndGame()
    {
        Global.Instance.StartCoroutine(EndGameCommand());
    }
    private IEnumerator EndGameCommand()
    {
        Global.Instance.Avatar.PauseInput();
        Global.Instance.Avatar.Screenshot();
        yield return null;
        UnityEngine.Object.FindObjectOfType<EndGameComponent>(includeInactive: true).gameObject.SetActive(true);
    }

    private void EndGame2()
    {
        Global.Instance.StartCoroutine(UnityEngine.Object.FindObjectOfType<EndGameComponent>().EndGame2Routine());
    }
}
