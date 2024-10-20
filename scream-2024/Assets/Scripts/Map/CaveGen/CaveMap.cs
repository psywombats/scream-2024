using UnityEngine;

public class CaveMap : GameMap
{
    [Header("Config")]
    [SerializeField] private NoiseGenerator noise;
    [SerializeField][Range(0, 1f)] private float caveSize;
    [SerializeField][Range(0, 1f)] private float spookiness;
    [Space]
    [Header("References")]
    [SerializeField] public MarchingTerrain terrain;

    public override void OnTeleportTo()
    {
        base.OnTeleportTo();
        Regenerate();
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Cave Size", caveSize);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Spookiness", spookiness);
    }

    public void Regenerate(int radius = 0)
    {
        DestroyChunks();
        terrain.EnsureChunks(ensureAll: true, radius: radius);
    }

    public void DestroyChunks()
    {
        terrain.CullAll();
    }
}
