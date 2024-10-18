using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Coroutine = MoonSharp.Interpreter.Coroutine;
using System;
using System.Collections.Generic;

/// <summary>
///  A wrapper around Script that represents an environment where a script can execute.
/// </summary>
public class LuaContext
{
    private const string DefinesPath = "Lua/Defines/GlobalDefines";
    private const string ScenesPath = "Lua/Scenes";

    private static string defines;

    private Script lua;
    public Script Lua => lua ??= new Script();

    private Stack<LuaScript> activeScripts = new Stack<LuaScript>();
    private bool forceKilled;

    public virtual void Initialize()
    {
        LoadDefines(DefinesPath);
        AssignGlobals();
    }

    // make sure the luaobject has been registered via [MoonSharpUserData]
    public void SetGlobal(string key, object luaObject)
    {
        Lua.Globals[key] = luaObject;
    }

    public bool IsRunning()
    {
        return activeScripts.Count > 0;
    }

    public DynValue CreateObject()
    {
        return Lua.DoString("return {}");
    }

    public DynValue Marshal(object toMarshal)
    {
        return DynValue.FromObject(Lua, toMarshal);
    }

    public Coroutine CreateScript(string fullScript)
    {
        try
        {
            DynValue scriptFunction = Lua.DoString(fullScript);
            return Lua.CreateCoroutine(scriptFunction).Coroutine;
        }
        catch (SyntaxErrorException e)
        {
            Debug.LogError("bad script: " + fullScript + "\n\nerror:\n" + e.DecoratedMessage);
            throw e;
        }
    }

    // all coroutines that are meant to block execution of the script should go through here
    public virtual void RunRoutineFromLua(IEnumerator routine)
    {
        if (forceKilled)
        {
            // leave the old instance infinitely suspended
            return;
        }
        Global.Instance.StartCoroutine(CoUtils.RunWithCallback(routine, () =>
        {
            if (activeScripts.Count > 0 && !forceKilled)
            {
                ResumeAwaitedScript();
            }
        }));
    }

    // meant to be evaluated synchronously
    public LuaCondition CreateCondition(string luaScript)
    {
        return new LuaCondition(this, Lua.LoadString(luaScript));
    }

    // evaluates a lua function in the global context
    public DynValue Evaluate(DynValue function)
    {
        return Lua.Call(function);
    }

    // hang on to a chunk of lua to run later
    public DynValue Load(string luaChunk)
    {
        return Lua.LoadString(luaChunk);
    }

    // kills the current script, useful for debug only
    public void ForceTerminate()
    {
        forceKilled = true;
    }

    public IEnumerator RunRoutine(string luaString, bool canBlock)
    {
        LuaScript script = new LuaScript(this, luaString);
        yield return RunRoutine(script, canBlock);
    }

    public virtual IEnumerator RunRoutine(LuaScript script, bool canBlock)
    {
        activeScripts.Push(script);
        forceKilled = false;
        try
        {
            script.ScriptRoutine.Resume();
        }
        catch (Exception)
        {
            Debug.Log("Exception during script: " + script + "\n context: " + this);
            throw;
        }
        while (script.ScriptRoutine.State != CoroutineState.Dead && !forceKilled)
        {
            yield return null;
        }
        if (MapOverlayUI.Instance.nvl.IsShown)
        {
            yield return MapOverlayUI.Instance.nvl.HideRoutine();
        }
        activeScripts.Pop();
    }

    public IEnumerator RunRoutineFromFile(string filename, bool canBlock = true)
    {
        if (filename.Contains("."))
        {
            filename = filename.Substring(0, filename.IndexOf('.'));
        }
        var asset = Resources.Load<LuaSerializedScript>("Lua/" + filename);
        if (asset == null)
        {
            throw new ArgumentException($"Can't find {filename}");
        }
        yield return RunRoutine(asset.luaString, canBlock);
    }

    protected void ResumeAwaitedScript()
    {
        activeScripts.Peek().ScriptRoutine.Resume();
    }

    protected virtual void AssignGlobals()
    {
        Lua.Globals["log"] = (Action<DynValue>)DebugLog;
        Lua.Globals["playSFX"] = (Action<DynValue>)PlaySFX;
        Lua.Globals["cs_wait"] = (Action<DynValue>)Wait;
        Lua.Globals["cs_play"] = (Action<DynValue, DynValue>)Play;
        Lua.Globals["getSwitch"] = (Func<DynValue, DynValue>)GetSwitch;
        Lua.Globals["setSwitch"] = (Action<DynValue, DynValue>)SetSwitch;
    }

    protected void LoadDefines(string path)
    {
        LuaSerializedScript script = Resources.Load<LuaSerializedScript>(path);
        Lua.DoString(script.luaString);
    }

    // === LUA CALLABLE ============================================================================

    protected DynValue GetSwitch(DynValue switchName)
    {
        bool value = Global.Instance.Data.GetSwitch(switchName.String);
        return Marshal(value);
    }

    protected void SetSwitch(DynValue switchName, DynValue value)
    {
        Global.Instance.Data.SetSwitch(switchName.String, value.Boolean);
    }

    protected void DebugLog(DynValue message)
    {
        Debug.Log(message.CastToString());
    }

    protected void Wait(DynValue seconds)
    {
        RunRoutineFromLua(CoUtils.Wait((float)seconds.Number));
    }

    protected void PlaySFX(DynValue sfxKey)
    {
        Global.Instance.Audio.PlaySFX(sfxKey.String);
    }

    protected void Play(DynValue filename, DynValue delay) => Play(filename, delay, false);
    protected void Play(DynValue filename, DynValue delay, bool blocks = true)
    {
        if (delay.IsNil())
        {
            RunRoutineFromLua(RunRoutineFromFile(filename.String, blocks));
        }
        else
        {
            RunRoutineFromLua(CoUtils.Wait(0.01f));
            activeScripts.Peek().ScriptRoutine.Resume();
            Global.Instance.StartCoroutine(CoUtils.RunAfterDelay((float)delay.Number, () =>
            {
                Global.Instance.StartCoroutine(RunRoutineFromFile(filename.String));
            }));
        }
    }
}
