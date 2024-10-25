using UnityEngine;

public class CaveMap : GameMap
{
    public enum Humidity
    {
        dry,
        humid,
    }

    [Header("Config")]
    [SerializeField][Range(0, 1f)] private float caveSize;
    [SerializeField] private Humidity humidity;
    [SerializeField] public int initRadius = 1;
    [Space]
    [Header("References")]
    [SerializeField] public NoiseGenerator noise;
    [SerializeField] public MarchingTerrain terrain;
    [SerializeField] private WebChunk webChunkPrefab;
    [SerializeField] private ComputeChunk computeChunkPrefab;

    public override void OnTeleportTo(GameMap from)
    {
        base.OnTeleportTo(from);
        Regenerate(initRadius, Global.Instance.Avatar != null);
        AudioManager.Instance.SetGlobalParam("Cave Size", caveSize);
        AudioManager.Instance.SetGlobalParam("cave_type", humidity.ToString());
    }

    public void Regenerate(int radius = 0, bool usePlayer = false)
    {
        DestroyChunks();
        StartCoroutine(terrain.EnsureChunksRoutine(radius: radius, usePlayer: usePlayer));
    }

    public void DestroyChunks()
    {
        terrain.CullAll();
    }

    public GameObject GetChunkPrefab()
    {
        return computeChunkPrefab.gameObject;
    }
}
