using DG.Tweening;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IInputListener
{
    [SerializeField] public GameObject firstPersonParent;
    [SerializeField] public Rigidbody body;
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
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private StudioEventEmitter climbEmitter;
    [SerializeField] private StudioEventEmitter abseilEmitter;
    [Space]
    [SerializeField] private GameObject flarePrefab = null;
    [Space]
    [SerializeField] private RenderTexture lapTex;
    [SerializeField] private RenderTexture transTex;

    private bool godMode;
    private bool isAbseiling, isAbsDown, isAbsUp;
    private bool isClimbing;
    private bool isClimbEmit, isAbseilEmit;
    private int pauseCount;
    private float currentAbsV, absTimer, oldV;
    private Vector3 targetFrameV;
    private float lastVelY;
    private float timeSinceGrounding, timeSinceJumping, timeSinceAbsing, timeSinceStep;

    public GameObject OldFlare { get; private set; }

    private bool selfPaused;
    public bool IsPaused => pauseCount > 0;

    public bool PastSafetyThreshold => body.velocity.y < abseilCutoff;

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

        if (targetFrameV == Vector3.zero || !IsGrounded)
        {
            timeSinceStep = 10f;
        }
        else if (timeSinceStep > .5f)
        {
            timeSinceStep = 0f;
            Global.Instance.Audio.PlaySFX("player/footsteps", gameObject);
        }
        timeSinceStep += Time.deltaTime;

        HandleRay();
        HandleAbseil();

        targetFrameV = Vector3.zero;
        
        var floorType = "Rock";
        if (Global.Instance.Maps.ActiveMap.lighting == LightingMode.Cave)
        {
            if (transform.localPosition.y < Global.Instance.Maps.ActiveMap.waterHeight)
            {
                floorType = "Puddle";
            }
        }
        else
        {
            floorType = "Other";
        }
        AudioManager.Instance.SetGlobalParam("Floor_Type", floorType);

        var fallSpeed = Mathf.Abs(body.velocity.y / abseilCutoff);
        if (body.velocity.y > 0) fallSpeed = 0f;
        AudioManager.Instance.SetGlobalParam("FallSpeed", fallSpeed);

        timeSinceGrounding += Time.deltaTime;

        //Debug.Log("lastVelY: " + lastVelY + " , " + IsGrounded);
        if (lastVelY < abseilCutoff * .1f && IsGrounded)
        {
            AudioManager.Instance.PlaySFX("player/landing", gameObject);
            lastVelY = 0f;
        }
        lastVelY = lastVelY * 3 / 4 + body.velocity.y / 4;
    }

    public void OnEnable()
    {
        InputManager.Instance.PushListener(this);
    }

    public void OnDisable()
    {
        InputManager.Instance?.RemoveListener(this);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.GetComponent<Chunk>() != null)
        {
            var contact = collision.GetContact(0);
            if ((transform.position + Vector3.up - contact.point).normalized.y > .4f)
            {
                timeSinceGrounding = 0f;
            }
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

    public bool IsGrounded => timeSinceGrounding < .1f;

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

    public IEnumerator RotateTowardRoutine(GameObject mover, GameObject target, GameObject rotater)
    {
        var targetPos = target.transform.position + new Vector3(0, 1f, 0);
        var dir = (targetPos - mover.transform.position).normalized;
        if (mover == rotater)
        {
            dir *= -1;
        }
        var lookAngles = Quaternion.LookRotation(dir).eulerAngles;
        if (mover == rotater)
        {
            lookAngles.x = 0f;
        }

        return CoUtils.RunTween(rotater.transform.DORotate(lookAngles, .5f));
    }

    public void Screenshot()
    {
        camera.enabled = false;
        camera.targetTexture = lapTex;
        camera.Render();
        camera.targetTexture = transTex;
        camera.Render();
        camera.targetTexture = null;
        camera.enabled = true;
    }

    public bool OnCommand(InputManager.Command command, InputManager.Event eventType)
    {
        if (command == InputManager.Command.Menu && eventType == InputManager.Event.Up)
        {
            if (selfPaused)
            {
                UnpauseInput();
                selfPaused = false;
            }
            else
            {
                PauseInput();
                selfPaused = true;
            }
        }
        
        if (IsPaused)
        {
            return true;
        }
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
                    case InputManager.Command.Tertiary:
                        isAbsDown = true;
                        break;
                    case InputManager.Command.Quaternary:
                        isAbsUp = true;
                        if (!isAbseiling)
                        {
                            SetAbseil(true, playAudio: true);
                        }
                        break;
                }
                break;
            case InputManager.Event.Up:
                switch (command)
                {
                    case InputManager.Command.Primary:
                    case InputManager.Command.Click:
                        SetAbseil(false, playAudio: true);
                        Interact();
                        break;
                    case InputManager.Command.Debug:
                        ToggleGodMode();
                        break;
                    case InputManager.Command.Secondary:
                        ThrowFlare();
                        break;
                    default:
                        break;
                }
                break;
        }
        return true;
    }

    private CharaEvent GetLookingChara()
    {
        var cameraCenter = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, camera.nearClipPlane));
        if (Physics.Raycast(cameraCenter, camera.transform.forward, out var hit, 1000))
        {
            var obj = hit.transform.gameObject;
            var chara = obj.GetComponentInParent<CharaEvent>();
            if (chara != null && hit.distance < chara.interactDistance)
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
            StartCoroutine(RotateTowardRoutine(firstPersonParent, chara.gameObject, camera.gameObject));
            StartCoroutine(RotateTowardRoutine(chara.gameObject, firstPersonParent, chara.gameObject));
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
        if (IsPaused || !CanFlare)
        {
            return;
        }
        OldFlare = Instantiate(flarePrefab);
        OldFlare.transform.SetParent(camera.transform);
        OldFlare.transform.localPosition = new Vector3(0f, 0f, .5f);
        OldFlare.transform.SetParent(transform.parent);
        OldFlare.GetComponent<Rigidbody>().velocity = camera.transform.forward * 10 + body.velocity;
    }

    public bool CanFlare => MapManager.Instance.ActiveMap.lighting == LightingMode.Cave
            && Global.Instance.Data.GetSwitch("enable_flares")
            && Global.Instance.Maps.ActiveMap.allowFlare;

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

    private void HandleAbseil()
    {
        isClimbing = false;
        if (Global.Instance.Data.GetSwitch("auto_abseil"))
        {
            Global.Instance.Data.SetSwitch("auto_abseil", false);
            SetAbseil(true, playAudio: false);
        }
        if (isAbseiling)
        {
            isClimbing = isAbsDown || isAbsUp;
            if (IsGrounded && timeSinceAbsing > .2f)
            {
                SetAbseil(false, playAudio: true);
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
                if (isAbsUp)
                {
                    isAbsUp = false;
                    targetAbsV = climbSpeed;
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
        if (PastSafetyThreshold && !isAbseiling)
        {
            SetAbseil(true, playAudio: true);
        }

        timeSinceJumping += Time.deltaTime;


        var playClimb = isAbseiling && isClimbing;
        var playAbs = isAbseiling && !isClimbing;
        if (playClimb && !isClimbEmit)
        {
            climbEmitter.Play();
        }
        if (!playClimb && isClimbEmit)
        {
            climbEmitter.Stop();
        }
        isClimbEmit = playClimb;

        if (playAbs && !isAbseilEmit)
        {
            abseilEmitter.Play();
        }
        if (!playAbs && isAbseilEmit)
        {
            abseilEmitter.Stop();
        }
        isAbseilEmit = playAbs;
    }

    private void SetAbseil(bool on, bool playAudio)
    {
        if (Global.Instance.Data.GetSwitch("abseiling_disabled") 
            || (Global.Instance.Maps.ActiveMap != null && !Global.Instance.Maps.ActiveMap.allowAbseil)
            || on == isAbseiling)
        {
            return;
        }
        isAbseiling = on;
        body.useGravity = !isAbseiling;
        StartCoroutine(CoUtils.RunTween(MapOverlayUI.Instance.abseilInfo.DOFade(on ? 1f : 0f, .7f)));
        if (on)
        {
            oldV = body.velocity.y;
            timeSinceAbsing = 0f;
        }

        if (playAudio)
        {
            if (on)
            {
                if (PastSafetyThreshold)
                {
                    Global.Instance.Audio.PlaySFX("player/abseiling_clipping", gameObject);
                }
                else
                {
                    if (timeSinceJumping > 1f && IsGrounded)
                    {
                        currentAbsV = climbSpeed * 2f;
                        Global.Instance.Audio.PlaySFX("player/jumping", gameObject);
                        timeSinceJumping = 0f;
                    }
                }
            }
            else
            {
                if (IsGrounded)
                {
                    // landing is handled elsewhere
                    //Global.Instance.Audio.PlaySFX("player/landing", gameObject);
                }
                else
                {
                    // player released
                }
            }
        }
    }
}
