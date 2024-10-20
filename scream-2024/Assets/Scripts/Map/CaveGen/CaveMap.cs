﻿using UnityEngine;

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

    public override void OnTeleportTo()
    {
        base.OnTeleportTo();
        Regenerate();
        AudioManager.Instance.SetGlobalParam("Cave Size", caveSize);
        AudioManager.Instance.SetGlobalParam("Spookiness", spookiness);
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
