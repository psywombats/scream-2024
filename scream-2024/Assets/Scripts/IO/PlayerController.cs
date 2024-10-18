using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IInputListener
{
    [SerializeField] private GameObject firstPersonParent;
    [SerializeField] private Rigidbody body;
    [SerializeField] public new Collider collider;
    [SerializeField] public new Camera camera;
    [Space]
    [SerializeField] private List<GameObject> godObjects;
    [SerializeField] private List<GameObject> nonGodObjects;
    [SerializeField] private List<GameObject> caveObjects;
    [Space]
    [SerializeField] [Range(0f, 9f)] float mouseRotateSensitivity = 2f;
    [SerializeField] Vector2 RotationYBounds = new Vector2(-70, 70);
    //[SerializeField] private float rayRange = 2.5f;
    [SerializeField] private float tilesPerSecond = 6f;
    [Space]
    [SerializeField] private GameObject flarePrefab = null;

    private bool godMode;
    private int pauseCount;
    private Vector3 velocityThisFrame;

    public GameObject OldFlare { get; private set; }

    public bool IsPaused => pauseCount > 0;

    private void Start()
    {
        Global.Instance.Avatar = this;
    }

    private void Update()
    {
        if ( !IsPaused )
        {
            HandleFPC();
        }

        HandleRay();

        body.velocity = new Vector3(velocityThisFrame.x, body.velocity.y, velocityThisFrame.z);
        velocityThisFrame = Vector3.zero;
    }

    public void OnEnable()
    {
        InputManager.Instance.PushListener(this);
    }

    public void OnDisable()
    {
        InputManager.Instance?.RemoveListener(this);
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

    public void SetFacing(OrthoDir dirr)
    {
        var targetPos = firstPersonParent.transform.position + dirr.Px3D();
        var dir = (targetPos - firstPersonParent.transform.position).normalized;
        var lookAngles = Quaternion.LookRotation(dir);
        camera.transform.localRotation = lookAngles;
    }

    public void SetFacing(Vector3 localEulers)
    {
        camera.transform.localEulerAngles = localEulers;
    }

    private Vector2Int GetMouse()
    {
        var pos = Mouse.current.position;
        return new Vector2Int((int)pos.x.ReadValue(), (int)pos.y.ReadValue());
    }

    public void OnTeleport()
    {
        foreach (var obj in caveObjects)
        {
            obj.SetActive(MapManager.Instance.ActiveMap.lighting == LightingMode.Cave);
        }
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
                        Interact();
                        return false;
                    case InputManager.Command.Debug:
                        ToggleGodMode();
                        return false;
                    case InputManager.Command.Secondary:
                    case InputManager.Command.Menu:
                        if (pauseCount > 0)
                        {
                            UnpauseInput();
                        } 
                        else
                        {
                            PauseInput();
                        }
                        return false;
                    default:
                        return false;
                }
            default:
                return false;
        }
    }

    private CharaEvent GetLookingChara()
    {
        var cameraCenter = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, camera.nearClipPlane));
        if (Physics.Raycast(cameraCenter, camera.transform.forward, out var hit, 1000))
        {
            var obj = hit.transform.gameObject;
            var chara = obj.GetComponentInParent<CharaEvent>();
            if (chara != null && hit.distance < 3f)
            {
                return chara;
            }
        }
        return null;
    }

    private void HandleRay()
    {
        var chara = GetLookingChara();
        if (chara != null && !IsPaused)
        {
            chara.HandleRay();
        }
    }

    private void Interact()
    {
        if (IsPaused)
        {
            return;
        }
        var chara = GetLookingChara();
        if (chara != null)
        {
            chara.Interact();
        }
        else
        {
            ThrowFlare();
        }
    }

    private bool TryStep(OrthoDir dir)
    {
        var component = dir.Px3D() * tilesPerSecond * 1.2f;
        var c2 = Quaternion.AngleAxis(camera.transform.localEulerAngles.y, Vector3.up) * component;
        velocityThisFrame += c2;

        return true;
    }

    private void HandleFPC()
    {
        var mouse = Mouse.current.delta;
        var inX = mouse.x.ReadValue();
        var inY = mouse.y.ReadValue();

        var trans = camera.transform;

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
        if (godMode || MapManager.Instance.ActiveMap.lighting != LightingMode.Cave)
        {
            return;
        }
        OldFlare = Instantiate(flarePrefab);
        OldFlare.transform.SetParent(camera.transform);
        OldFlare.transform.localPosition = new Vector3(0f, 0f, .5f);
        OldFlare.transform.SetParent(transform.parent);
        OldFlare.GetComponent<Rigidbody>().velocity = camera.transform.forward * 10;
    }

    private void ToggleGodMode()
    {
        godMode = !godMode;
        body.useGravity = !godMode;
        foreach (var obj in godObjects)
        {
            obj.SetActive(godMode);
        }
        foreach (var obj in nonGodObjects)
        {
            obj.SetActive(!godMode);
        }
    }
}
