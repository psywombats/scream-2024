using UnityEditor;
using UnityEngine;

public class DebugWindow : EditorWindow
{
    private Vector2 scrollPos;
    private string customLua;
    private string switchName;
    private string switchOut;
    private string audioKey;

    private LuaContext Lua => Global.Instance.Lua;

    [MenuItem("Window/Debug")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DebugWindow));
    }

    void OnGUI()
    {
        GUILayout.Space(4);
        GUILayout.Label("== FANCY NEW DEBUG PANEL==");
        GUILayout.Space(24);

        if (!Application.isPlaying)
        {
            GUILayout.Label("Run the game to see the full debug panel");
            return;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 50));

        HandleAudio();
        HandleLua();
        HandleSwitchQuery();

        EditorGUILayout.EndScrollView();
    }

    private void HandleAudio()
    {
        GUILayout.Label("Audio debug");
        audioKey = GUILayout.TextField(audioKey);

        if (GUILayout.Button("Play SFX with this name"))
        {
            Global.Instance.Audio.PlaySFX(audioKey);
        }
        if (GUILayout.Button("Play BGM with this name"))
        {
            Global.Instance.Audio.PlayBGM(audioKey);
        }

        GUILayout.Space(24);
    }

    private void HandleLua()
    {
        if (!Lua.IsRunning())
        {
            EditorGUILayout.LabelField("Lua debug prompt!");
        }
        else
        {
            EditorGUILayout.LabelField("Running...");
            EditorGUI.BeginDisabledGroup(true);
        }

        customLua = EditorGUILayout.TextArea(customLua, new GUILayoutOption[] { GUILayout.Height(120) });
        GUILayout.Space(12);

        if (Lua.IsRunning())
        {
            EditorGUILayout.LabelField("Running...");
            EditorGUI.EndDisabledGroup();
        }

        if (!Lua.IsRunning())
        {
            if (GUILayout.Button("Run"))
            {
                LuaScript script = new LuaScript(Lua, customLua);
                Global.Instance.StartCoroutine(script.RunRoutine(true));
            }
        }
        else
        {
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Force terminate"))
            {
                Lua.ForceTerminate();
            }
        }
        GUILayout.Space(24);
    }

    private void HandleSwitchQuery()
    {
        GUILayout.Label("Check/set switches and variables");
        switchName = GUILayout.TextField(switchName);
        if (GUILayout.Button("Read switch value"))
        {
            switchOut = $"{switchName}: {Global.Instance.Data.GetSwitch(switchName)}";
        }
        if (GUILayout.Button("Turn switch on"))
        {
            Global.Instance.Data.SetSwitch(switchName, true);
            switchOut = null;
        }
        if (GUILayout.Button("Turn switch off"))
        {
            Global.Instance.Data.SetSwitch(switchName, false);
            switchOut = null;
        }
        if (GUILayout.Button("Read string variable value"))
        {
            switchOut = $"{switchName}: {Global.Instance.Data.GetStringVariable(switchName)}";
        }
        if (switchOut != null)
        {
            GUILayout.Label(switchOut);
        }
        GUILayout.Space(24);
    }
}
