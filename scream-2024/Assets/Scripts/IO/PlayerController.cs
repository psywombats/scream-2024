using DG.Tweening;
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
    [SerializeField] public Collider probe;
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
    [SerializeField] private float abseilCutoff = -10f;
    [SerializeField] private float abseilAcc = 1f;
    [SerializeField] private float abseilDeacc = 10f;
    [SerializeField] private float abseilV = -2f;
    [SerializeField] private float abseilOscPeriod = 3f;
    [SerializeField] private float abseilOscSpeed = 3f;
    [Space]
    [SerializeField] private GameObject flarePrefab = null;

    private bool godMode;
    private bool isAbseiling, isAbsDown;
    private int pauseCount;
    private float currentAbsV, absTimer, oldV;
    private Vector3 targetFrameV;
    private float timeSinceGrounding;

    public GameObject OldFlare { get; private set; }

    public bool IsPaused => pauseCount > 0;

    private void Start()
    {
        Global.Instance.Avatar = this;
    }

    private void Update()
    {
        if (!IsPaused)
        {
            HandleFPC();
        }

        HandleRay();

        if (Global.Instance.Data.GetSwitch("auto_abseil"))
        {
            Global.Instance.Data.SetSwitch("auto_abseil", false);
            SetAbseil(true);
        }
        if (isAbseiling)
        {
            if (IsGrounded)
            {
                SetAbseil(false);
            }
            else
            {
                absTimer += Time.deltaTime;
                var targetAbsV = isAbsDown ? abseilV : 0f;
                if (isAbsDown)
                {
                    isAbsDown = false;
                    absTimer = 0f;
                }
                if (currentAbsV < targetAbsV)
                {
                    currentAbsV += abseilAcc * Time.deltaTime;
                }
                else if (currentAbsV > targetAbsV)
                {
                    currentAbsV -= abseilAcc * Time.deltaTime;
                }
                var absX = MathF.Sin(absTimer * 2 * Mathf.PI / abseilOscPeriod) * abseilOscSpeed;
                var absY = MathF.Sin(absTimer * 2 * Mathf.PI / abseilOscPeriod) * abseilOscSpeed * .2f;
                if (oldV < 0)
                {
                    oldV += Time.deltaTime * abseilDeacc;
                }
                else if (oldV > 0)
                {
                    oldV = 0F;
                }
                body.velocity = new Vector3(absX, oldV + currentAbsV, absY);
            }
        }
        else
        {
            body.velocity = new Vector3(targetFrameV.x, body.velocity.y, targetFrameV.z);
        }

        targetFrameV = Vector3.zero;

        if (body.velocity.y < abseilCutoff && !isAbseiling)
        {
            SetAbseil(true);
        }

        var fallSpeed = Mathf.Abs(body.velocity.y / abseilCutoff);
        if (body.velocity.y > 0) fallSpeed = 0f;
        

        timeSinceGrounding += Time.deltaTime;
    }

    public void OnEnable()
    {
        InputManager.Instance.PushListener(this);
    }

    public void OnDisable()
    {
        InputManager.Instance?.RemoveListener(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Chunk>() != null)
        {
            timeSinceGrounding = 0f;
        }
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

    public bool IsGrounded => timeSinceGrounding < .5f;

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
                        SetAbseil(false);
                        return false;
                    case InputManager.Command.Down:
                        if (isAbseiling)
                        {
                            isAbsDown = true;
                        }
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
                        ThrowFlare();
                        return false;
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
    }

    private bool TryStep(OrthoDir dir)
    {
        if (!isAbseiling)
        {
            var component = dir.Px3D() * tilesPerSecond * 1.2f;
            var c2 = Quaternion.AngleAxis(camera.transform.localEulerAngles.y, Vector3.up) * component;
            targetFrameV += c2;
        }

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
        if (IsPaused
            || MapManager.Instance.ActiveMap.lighting != LightingMode.Cave
            || !Global.Instance.Data.GetSwitch("enable_flares"))
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

    private void SetAbseil(bool on)
    {
        if (isAbseiling == on  || Global.Instance.Data.GetSwitch("abseiling_disabled"))
        {
            return;
        }
        isAbseiling = on;
        body.useGravity = !isAbseiling;
        StartCoroutine(CoUtils.RunTween(MapOverlayUI.Instance.abseilInfo.DOFade(on ? 1f : 0f, .7f)));
        if (on)
        {
            oldV = body.velocity.y;
        }
    }
}
