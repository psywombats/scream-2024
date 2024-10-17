using MoonSharp.Interpreter;

/// <summary>
/// represents an eval-able piece of Lua, usually from an event field
/// </summary>
public class LuaCondition
{
    private LuaContext context;
    private DynValue function;

    public LuaCondition(LuaContext context, DynValue scriptFunction)
    {
        this.function = scriptFunction;
        this.context = context;
    }

    public DynValue Evaluate()
    {
        return context.Evaluate(function);
    }
}
