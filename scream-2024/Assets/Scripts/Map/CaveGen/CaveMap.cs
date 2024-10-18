using UnityEngine;

public class CaveMap : GameMap
{
    [Header("Config")]
    [SerializeField] private NoiseGenerator noise;
    [Space]
    [Header("References")]
    [SerializeField] private MarchingTerrain terrain;

    public override void OnTeleportTo()
    {
        base.OnTeleportTo();
        Regenerate();
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
