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
    [SerializeField] private string bgmKey = null;
    [SerializeField][Range(0, 100)] private int spookiness;
    [SerializeField] public LightingMode lighting;
    [Space]
    [SerializeField] public GameObject eventLayer;

    public virtual void Start()
    {
        if (Global.Instance.Maps.ActiveMap == null)
        {
            Global.Instance.Maps.ActiveMap = this;
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

    public virtual void OnTeleportTo()
    {
        if (bgmKey != null)
        {
            AudioManager.Instance.PlayBGM(bgmKey);
        }

        if (!Global.Instance.Data.GetSwitch("no_settings"))
        {
            foreach (var setting in displayNames)
            {
                MapOverlayUI.Instance.setting.Show(setting);
            }
        }

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Spookiness", spookiness);
    }

    public virtual void OnTeleportAway(GameMap nextMap)
    {

    }
}
