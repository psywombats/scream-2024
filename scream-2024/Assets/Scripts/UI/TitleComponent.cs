using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

class TitleComponent : MonoBehaviour, IInputListener
{
    public void Start()
    {
        InputManager.Instance.PushListener(this);
    }

    public bool OnCommand(InputManager.Command command, InputManager.Event eventType)
    {
        switch (command)
        {
            case InputManager.Command.Primary:
                Advance();
                break;
        }
        return true;
    }

    private void Advance()
    {
        InputManager.Instance.RemoveListener(this);
        Global.Instance.StartCoroutine(StartGameRoutine("TestMap", "start", OrthoDir.North));
    }

    public IEnumerator StartGameRoutine(string map, string target, OrthoDir dir)
    {
        yield return SceneManager.LoadSceneAsync("Map3D", LoadSceneMode.Single);
        yield return Global.Instance.Maps.TeleportRoutine(map, target, dir);
        yield return null;
    }
}
