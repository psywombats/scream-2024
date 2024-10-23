using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class MapEvent : MonoBehaviour
{
    private const string PropertyCondition = "show";
    private const string PropertyInteract = "onInteract";
    private const string PropertyCollide = "onCollide";
    private const string PropertyEnter = "onEnter";

    [Header("Lua scripting")]
    [SerializeField] [TextArea(3, 6)] public string luaCondition;
    [SerializeField] [TextArea(3, 6)] public string luaOnInteract;
    [SerializeField] [TextArea(3, 6)] public string luaOnCollide;
    [SerializeField] [TextArea(3, 6)] public string luaOnEnter;
    [Space]
    [SerializeField] public GameObject enableChild;
    [SerializeField] public new Collider collider;

    public event Action<bool> OnSwitchChanged;
    public event Action OnInteract;
    public event Action OnCollide;

    private GameMap map;
    public GameMap Map
    {
        get
        {
            // this is wiped in update but we'll cache it across frames anyway
            if (map != null)
            {
                return map;
            }
            map = GetComponentInParent<GameMap>();
            return map;
        }
    }

    private bool isSwitchEnabled = true;
    public bool IsSwitchEnabled
    {
        get => isSwitchEnabled;
        set
        {
            if (enableChild != null)
            {
                enableChild.SetActive(value);
            }
            if (collider != null)
            {
                collider.enabled = value;
            }
            if (value != isSwitchEnabled)
            {
                OnSwitchChanged?.Invoke(value);
            }
            isSwitchEnabled = value;
        }
    }

    public LuaMapEvent LuaObject { get; private set; }

    private float lastCollided;

    public void Awake()
    {
        LuaObject = new LuaMapEvent(this);
    }

    public void Start()
    {
        if (Application.isPlaying)
        {
            LuaObject.Set(PropertyCollide, luaOnCollide);
            LuaObject.Set(PropertyInteract, luaOnInteract);
            LuaObject.Set(PropertyCondition, luaCondition);
            LuaObject.Set(PropertyEnter, luaOnEnter);

            CheckEnabled();
        }
    }

    private bool autod;
    public virtual void Update()
    {
        if (Application.IsPlaying(this))
        {
            CheckEnabled();
        }

        if (IsSwitchEnabled && Global.Instance.Avatar != null && !Global.Instance.Avatar.IsPaused)
        {
            if (!autod && luaOnEnter != null && luaOnEnter.Length > 0)
            {
                autod = true;
                LuaObject.Run(PropertyEnter);
            }
        }
    }

    public void CheckEnabled()
    {
        if (luaCondition != null && luaCondition.Length > 0)
        {
            IsSwitchEnabled = LuaObject.EvaluateBool(PropertyCondition, true);
        }
    }

    public OrthoDir DirectionTo(MapEvent other)
    {
        return DirectionTo(other.transform.position);
    }
    public OrthoDir DirectionTo(Vector3 position)
    {
        return OrthoDirExtensions.DirectionOf3D(position - transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var avatar = Global.Instance.Avatar;
        if (collision.collider != avatar.collider)
        {
            return;
        }
        if (avatar.IsPaused)
        {
            return;
        }
        if (luaOnCollide.Length == 0)
        {
            return;
        }
        if (Time.time - lastCollided < .5f)
        {
            return;
        }
        lastCollided = Time.time;
        LuaObject.Run(PropertyCollide);
        OnCollide?.Invoke();
    }

    // called when the avatar stumbles into us
    // facing us if impassable, on top of us if passable
    public void Interact()
    {
        LuaObject.Run(PropertyInteract);
        OnInteract?.Invoke();
    }

    private LuaScript ParseScript(string lua)
    {
        if (lua == null || lua.Length == 0)
        {
            return null;
        }
        else
        {
            return new LuaScript(GetComponent<LuaContext>(), lua);
        }
    }

    private LuaCondition ParseCondition(string lua)
    {
        if (lua == null || lua.Length == 0)
        {
            return null;
        }
        else
        {
            return GetComponent<LuaContext>().CreateCondition(lua);
        }
    }
}
