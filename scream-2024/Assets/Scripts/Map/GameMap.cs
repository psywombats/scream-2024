using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A map is either a cave chamber or a 3D predefined map
/// </summary>
public abstract class GameMap : MonoBehaviour
{
    public const string ResourcePath = "Maps/";

    [SerializeField] private List<string> displayNames;
    [Space]
    [SerializeField] public LightingMode lighting;
    [SerializeField] public bool allowAbseil = true;
    [SerializeField] public bool allowFlare = true;
    [SerializeField] public float waterHeight = -1000;
    [Space]
    [SerializeField] public GameObject eventLayer;

    public virtual void Start()
    {
        if (Global.Instance.Maps.ActiveMap == null)
        {
            Global.Instance.Maps.ActiveMap = this;
            OnTeleportTo(null);
        }
    }

    public MapEvent GetEventNamed(string eventName, bool includeDisabled = false)
    {
        foreach (MapEvent mapEvent in eventLayer.GetComponentsInChildren<MapEvent>(includeInactive: includeDisabled))
        {
            if (mapEvent.name == eventName && (mapEvent.IsSwitchEnabled || includeDisabled))
            {
                return mapEvent;
            }
        }
        return null;
    }

    public virtual void OnTeleportTo(GameMap from)
    {
        if (MapOverlayUI.Instance != null)
        {
            if (!Global.Instance.Data.GetSwitch("no_settings") && from != this)
            {
                foreach (var setting in displayNames)
                {
                    MapOverlayUI.Instance.setting.Show(setting);
                }
            }
            MapOverlayUI.Instance.ascendInfo.alpha = allowAbseil ? 1f : 0f;
            MapOverlayUI.Instance.flareInfo.alpha = Global.Instance.Avatar.CanFlare ? 1f : 0f;
        }
    }

    public virtual void OnTeleportAway(GameMap nextMap)
    {

    }
}
