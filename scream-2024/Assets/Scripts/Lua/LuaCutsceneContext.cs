using System.Collections;
using System;
using MoonSharp.Interpreter;

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
        Lua.Globals["cs_fade"] = (Action<DynValue>)Fade;

        Lua.Globals["cs_enterNVL"] = (Action<DynValue>)EnterNVL;
        Lua.Globals["cs_exitNVL"] = (Action)ExitNVL;
        Lua.Globals["cs_enter"] = (Action<DynValue, DynValue, DynValue>)Enter;
        Lua.Globals["cs_exit"] = (Action<DynValue>)Exit;
        Lua.Globals["cs_speak"] = (Action<DynValue, DynValue>)Speak;
        Lua.Globals["cs_radio"] = (Action<DynValue, DynValue>)Radio;
        Lua.Globals["clear"] = (Action)ClearNVL;
        Lua.Globals["hideRadio"] = (Action)HideRadio;

        Lua.Globals["cs_rotateTo"] = (Action<DynValue>)RotateToward;
        Lua.Globals["setting"] = (Action<DynValue>)Setting;
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
        //RunRoutineFromLua(Global.Instance.Maps.TeleportRoutine(mapName.String, targetEventName.String, facing, raw));
    }

    private void FadeOutBGM(DynValue seconds)
    {
        RunRoutineFromLua(Global.Instance.Audio.FadeOutRoutine((float)seconds.Number));
    }

    private void Fade(DynValue type)
    {
        //
    }

    public void EnterNVL(DynValue lightLua)
    {
        var lightMode = !lightLua.IsNil() && lightLua.Boolean;
        RunRoutineFromLua(EnterNVLRoutine(lightMode));
    }
    private IEnumerator EnterNVLRoutine(bool lightMode)
    {
        yield return MapOverlayUI.Instance.nvl.ShowRoutine(lightMode);
        yield break;
    }

    public void ExitNVL()
    {
        RunRoutineFromLua(ExitNVLRoutine());
    }
    private IEnumerator ExitNVLRoutine()
    {
        yield return MapOverlayUI.Instance.nvl.HideRoutine();
        yield break;
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
        yield break;
    }

    public void Exit(DynValue speakerNameLua)
    {
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerNameLua.String);
        RunRoutineFromLua(ExitRoutine(speaker));
    }
    private IEnumerator ExitRoutine(SpeakerData speaker)
    {
        yield return MapOverlayUI.Instance.nvl.ExitRoutine(speaker);
        yield break;
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
        yield break;
    }

    private void Setting(DynValue textVal)
    {
        //MapOverlayUI.Instance.setting.Show(textVal.String);
    }

    private void RotateToward(DynValue eventName)
    {
        //var @event = MapManager.Instance.ActiveMap.GetEventNamed(eventName.String);
        //RunRoutineFromLua(AvatarEvent.Instance.RotateTowardRoutine(@event));
    }

    private void Radio(DynValue speakerLua, DynValue textLua)
    {
        RunRoutineFromLua(RadioRoutine(speakerLua.String, textLua.String));
    }
    private IEnumerator RadioRoutine(string speakerKey, string message)
    {
        yield return MapOverlayUI.Instance.radio.SpeakRoutine(speakerKey, message);
        yield break;
    }

    private void HideRadio()
    {
        Global.Instance.StartCoroutine(MapOverlayUI.Instance.radio.HideRoutine());
    }
}
