using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A map is either a cave chamber or a 3D predefined map
/// </summary>
public abstract class GameMap : MonoBehaviour
{
    [SerializeField] private List<string> displayNames;
    [Space]
    [SerializeField] private string bgmKey = null;
    [SerializeField][Range(0, 100)] private int spookiness;
    [Space]
    [SerializeField] private GameObject eventLayer;

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

    public void OnTeleportTo()
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

    public void OnTeleportAway(GameMap nextMap)
    {

    }
}
