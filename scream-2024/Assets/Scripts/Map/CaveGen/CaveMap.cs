using UnityEngine;

public class CaveMap : GameMap
{
    [Header("Config")]
    [SerializeField] private NoiseGenerator noise;
    [Space]
    [Header("References")]
    [SerializeField] private MarchingTerrain terrain;
}
