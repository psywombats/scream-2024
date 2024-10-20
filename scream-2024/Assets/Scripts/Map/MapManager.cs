using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class MapManager : SingletonBehavior
{
    [SerializeField] private PlayerController avatarPrefab;

    public static MapManager Instance => Global.Instance.Maps;

    public PlayerController Avatar => Global.Instance.Avatar;

    public GameMap ActiveMap { get; set; }

    private new Camera camera;
    public Camera Camera
    {
        get
        {
            if (camera == null)
            {
                camera = Global.Instance.Avatar.camera;
            }
            return camera;
        }
    }

    public void Teleport(string mapName, string targetEventName)
    {
        StartCoroutine(TeleportRoutine(mapName, targetEventName, isRaw: true));
    }

    public IEnumerator TeleportRoutine(string mapName, string targetEventName, OrthoDir? facing = null, bool isRaw = false)
    {
        var avatarExists = Avatar != null;
        if (avatarExists)
        {
            Avatar.PauseInput();
        }
        if (!isRaw)
        {
            yield return CoUtils.RunTween(MapOverlayUI.Instance.fader.DOFade(1f, .5f));
        }

        RawTeleport(mapName, targetEventName, facing);

        if (!isRaw)
        {
            yield return CoUtils.RunTween(MapOverlayUI.Instance.fader.DOFade(0f, .5f));
        }
        if (avatarExists)
        {
            Avatar.UnpauseInput();
        }
    }

    private void RawTeleport(string mapName, string targetName, OrthoDir? facing = null)
    {
        GameMap newMapInstance;
        if (ActiveMap != null && mapName == ActiveMap.name)
        {
            newMapInstance = ActiveMap;
        }
        else
        {
            newMapInstance = InstantiateMap(mapName);
            if (ActiveMap != null)
            {
                ActiveMap.OnTeleportAway(newMapInstance);
            }
        }
        var target = newMapInstance.GetEventNamed(targetName);

        if (target == null)
        {
            Debug.LogError("Couldn't find target " + target + " on " + mapName + " from " + ActiveMap?.name);
        }

        if (newMapInstance != ActiveMap)
        {
            if (ActiveMap != null)
            {
                Destroy(ActiveMap.gameObject);
            }
        }

        ActiveMap = newMapInstance;
        AddInitialAvatar();
        Avatar.transform.position = target.transform.position;
        if (facing.HasValue)
        {
            Avatar.SetFacing(facing.Value);
        }
        else
        {
            Avatar.SetFacing(target.transform.localEulerAngles);
        }

        Avatar.OnTeleport();
        ActiveMap.OnTeleportTo();
    }

    private GameMap InstantiateMap(string mapName)
    {
        var newMapObject = Resources.Load<GameObject>("Maps/" + mapName);
        Assert.IsNotNull(newMapObject);
        var obj = Instantiate(newMapObject);
        obj.name = newMapObject.name;
        obj.transform.position = Vector3.zero;
        return obj.GetComponent<GameMap>();
    }

    private void AddInitialAvatar()
    {
        if (Avatar == null)
        {
            var av = Instantiate(avatarPrefab.gameObject);
            Global.Instance.Avatar = av.GetComponent<PlayerController>();
        }
        Avatar.gameObject.name = "hero";
        Avatar.transform.SetParent(ActiveMap.eventLayer.transform, false);
    }
}
