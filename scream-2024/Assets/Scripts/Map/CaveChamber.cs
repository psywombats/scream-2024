using UnityEngine;

public class CaveChamber : MonoBehaviour
{
    [SerializeField] private string caveName;
    [SerializeField] private NoiseGenerator noise;
    [Space]
    [SerializeField] private MarchingTerrain terrain;
}
