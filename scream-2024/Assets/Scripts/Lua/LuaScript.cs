using MoonSharp.Interpreter;
using System.Collections;
using UnityEngine;
using Coroutine = MoonSharp.Interpreter.Coroutine;

/// <summary>
/// represents an runnable piece of Lua, usually from an event field
/// </summary>
public class LuaScript
{
    protected LuaContext context;

    public Coroutine ScriptRoutine { get; private set; }
    public bool Done { get; private set; }

    private string name;

    public LuaScript(LuaContext context, string scriptString)
    {
        this.context = context;

        string fullScript = "return function()\n" + scriptString + "\nend";
        ScriptRoutine = context.CreateScript(fullScript);
        name = scriptString.Substring(0, Mathf.Min(80, scriptString.Length));
    }

    public LuaScript(LuaContext context, DynValue function)
    {
        this.context = context;
        ScriptRoutine = context.Lua.CreateCoroutine(function).Coroutine;
        name = function.ToString();
    }

    public IEnumerator RunRoutine(bool canBlock)
    {
        Done = false;
        yield return context.RunRoutine(this, canBlock);
        Done = true;
    }

    public override string ToString()
    {
        return name != null ? name : base.ToString();
    }
}
