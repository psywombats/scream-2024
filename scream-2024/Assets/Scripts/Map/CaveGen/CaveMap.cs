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
    [SerializeField] public MarchingTerrain terrain;
    [SerializeField] public NoiseGenerator noise;
    [SerializeField] public ChunkHolder chunker;

    public ChunkHolder Chunker => chunker;

    public bool awaitingUnpause;

    public override void OnTeleportTo(GameMap from)
    {
        base.OnTeleportTo(from);
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (Global.Instance.Avatar != null)
            {
                Global.Instance.Avatar.PauseInput();
                Global.Instance.Avatar.body.useGravity = false;
                awaitingUnpause = true;
            }
            Regenerate(0, Global.Instance.Avatar != null);
        }
        else
        {
            Regenerate(initRadius, Global.Instance.Avatar != null);
        }
        
        AudioManager.Instance.SetGlobalParam("Cave Size", caveSize);
        AudioManager.Instance.SetGlobalParam("cave_type", humidity.ToString());
    }

    public void Regenerate(int radius = 0, bool usePlayer = false)
    {
        DestroyChunks();
        terrain.EnsureChunks(radius, usePlayer: usePlayer);
    }

    public void DestroyChunks()
    {
        terrain.CullAll();
    }

    public GameObject GetChunkPrefab() => Chunker.GetChunk();
}
