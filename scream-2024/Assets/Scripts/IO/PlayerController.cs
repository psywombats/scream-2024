using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IInputListener
{
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Rigidbody body;
    [Space]
    [SerializeField] private float degreesPerSecond = 120;
    [SerializeField] [Range(0f, 9f)] float mouseRotateSensitivity = 2f;
    [SerializeField] Vector2 RotationYBounds = new Vector2(-70, 70);
    [SerializeField] private float rayRange = 2.5f;
    [SerializeField] private float tilesPerSecond = 6f;
    [Space]
    [SerializeField] private GameObject flarePrefab = null;

    private int pauseCount;
    private Vector3 velocityThisFrame;

    public GameObject OldFlare { get; private set; }

    private void Start()
    {
        Global.Instance.Avatar = this;
    }

    private void Update()
    {
        HandleFPC();

        if (velocityThisFrame != Vector3.zero)
        {
            var xcom = new Vector3(velocityThisFrame.x, 0, 0);
            var ycom = new Vector3(0, 0, velocityThisFrame.z);
        }

        body.velocity = new Vector3(velocityThisFrame.x, body.velocity.y, velocityThisFrame.z);
        velocityThisFrame = Vector3.zero;
    }

    public void OnEnable()
    {
        InputManager.Instance.PushListener(this);
    }

    public void OnDisable()
    {
        InputManager.Instance.RemoveListener(this);
    }

    public void PauseInput()
    {
        pauseCount += 1;
    }

    public void UnpauseInput()
    {
        if (pauseCount > 0)
        {
            pauseCount -= 1;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    private Vector2Int GetMouse()
    {
        var pos = Mouse.current.position;
        return new Vector2Int((int)pos.x.ReadValue(), (int)pos.y.ReadValue());
    }

    public bool OnCommand(InputManager.Command command, InputManager.Event eventType)
    {
        switch (eventType)
        {
            case InputManager.Event.Hold:
                switch (command)
                {
                    case InputManager.Command.Up:
                        TryStep(OrthoDir.North);
                        return false;
                    case InputManager.Command.Down:
                        TryStep(OrthoDir.South);
                        return false;
                    case InputManager.Command.Right:
                        TryStep(OrthoDir.East);
                        return false;
                    case InputManager.Command.Left:
                        TryStep(OrthoDir.West);
                        return false;
                    default:
                        return false;
                }
            case InputManager.Event.Up:
                switch (command)
                {
                    case InputManager.Command.Primary:
                        //ThrowFlare();
                        return false;
                    case InputManager.Command.Secondary:
                    case InputManager.Command.Menu:
                        //ShowMenu();
                        return false;
                    default:
                        return false;
                }
            default:
                return false;
        }
    }

    private bool TryStep(OrthoDir dir)
    {
        var component = dir.Px3D() * tilesPerSecond * 1.2f;
        var c2 = Quaternion.AngleAxis(fpsCam.transform.localEulerAngles.y, Vector3.up) * component;
        velocityThisFrame += c2;

        return true;
    }

    private void HandleFPC()
    {
        var mouse = Mouse.current.delta;
        var inX = mouse.x.ReadValue();
        var inY = mouse.y.ReadValue();

        var trans = fpsCam.transform;

        trans.rotation *= Quaternion.AngleAxis(inY * mouseRotateSensitivity, Vector3.left);

        var ang = trans.eulerAngles.x;
        while (ang > 180) ang -= 360;
        while (ang < -180) ang += 360;

        var xcom = inX * mouseRotateSensitivity;

        trans.rotation = Quaternion.Euler(
            Mathf.Clamp(ang, RotationYBounds.x, RotationYBounds.y),
            trans.eulerAngles.y + xcom,
            0
        );
    }

    private void ThrowFlare()
    {
        Destroy(OldFlare);
        OldFlare = GameObject.Instantiate(flarePrefab);
        OldFlare.transform.SetParent(fpsCam.transform);
        OldFlare.transform.localPosition = new Vector3(0f, -.2f, 0f);
        OldFlare.transform.SetParent(transform.parent);
        OldFlare.GetComponent<Rigidbody>().velocity = fpsCam.transform.forward * 10;
    }
}
