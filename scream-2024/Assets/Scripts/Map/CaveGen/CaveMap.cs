using UnityEngine;

public class CaveMap : GameMap
{
    public enum Humidity
    {
        dry,
        humid,
    }

    [Header("Config")]
    [SerializeField] private NoiseGenerator noise;
    [SerializeField][Range(0, 1f)] private float caveSize;
    [SerializeField][Range(0, 1f)] private float spookiness;
    [SerializeField] private Humidity humidity;
    [Space]
    [Header("References")]
    [SerializeField] public MarchingTerrain terrain;
    [SerializeField] public int initRadius = 1;

    public override void OnTeleportTo(GameMap from)
    {
        base.OnTeleportTo(from);
        Regenerate(initRadius, Global.Instance.Avatar != null);
        AudioManager.Instance.SetGlobalParam("Cave Size", caveSize);
        AudioManager.Instance.SetGlobalParam("Spookiness", spookiness);
        AudioManager.Instance.SetGlobalParam("cave_type", humidity.ToString());
    }

    public void Regenerate(int radius = 0, bool usePlayer = false)
    {
        DestroyChunks();
        terrain.EnsureChunks(ensureAll: true, radius: radius, usePlayer: usePlayer);
    }

    public void DestroyChunks()
    {
        terrain.CullAll();
    }
}
