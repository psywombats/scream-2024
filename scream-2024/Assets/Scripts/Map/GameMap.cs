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
    [SerializeField] public bool allowCompass = false;
    [SerializeField] public float waterHeight = -1000;
    [SerializeField] [Range(0, 1f)] public float spookiness;
    [SerializeField] public bool isNightmare;
    [Space]
    [SerializeField] public GameObject eventLayer;

    public virtual void Start()
    {
        if (Application.isPlaying)
        {
            if (Global.Instance.Maps.ActiveMap == null)
            {
                Global.Instance.Maps.ActiveMap = this;
                OnTeleportTo(null);
            }
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
            if (Global.Instance.Avatar != null)
            {
                MapOverlayUI.Instance.flareInfo.gameObject.SetActive(Global.Instance.Avatar.CanFlare);
            }
            MapOverlayUI.Instance.ascendInfo.gameObject.SetActive(allowAbseil);
            MapOverlayUI.Instance.compass.gameObject.SetActive(allowCompass);
        }
        AudioManager.Instance.SetGlobalParam("Spookiness", spookiness);
        AudioManager.Instance.SetNightmare(isNightmare);
    }

    public virtual void OnTeleportAway(GameMap nextMap)
    {

    }
}
