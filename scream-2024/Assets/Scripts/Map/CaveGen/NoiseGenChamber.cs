using UnityEngine;

public class NoiseGenChamber : NoiseGenerator
{
    [Space]
    [SerializeField, Range(0f, 512f)] public float chamberR = 32f;
    [SerializeField, Range(0f, 32f)] public float chamberOblX = 1f;
    [SerializeField, Range(0f, 32f)] public float chamberOblZ = 1f;
    [SerializeField, Range(0f, 32f)] public float ceilHard = 1f;
    [SerializeField, Range(0f, 64f)] public float floorHard = 1f;
    [SerializeField, Range(0f, 32f)] public float wallHard = 1f;
    [SerializeField, Range(0f, 5f)] public float chamberWeight = 1f;
    [SerializeField, Range(0f, 5f)] public float noiseWeight = 1f;

    protected override void SetSpecificNoiseVars()
    {
        base.SetSpecificNoiseVars();
        Source.SetFloat("_ChamberR", chamberR);
        Source.SetFloat("_ChamberOblX", chamberOblX);
        Source.SetFloat("_ChamberOblZ", chamberOblZ);
        Source.SetFloat("_CeilHardness", floorHard);
        Source.SetFloat("_FloorHardness", ceilHard);
        Source.SetFloat("_WallHardness", wallHard);
        Source.SetFloat("_ChamberWeight", chamberWeight);
        Source.SetFloat("_NoiseWeight", noiseWeight);
    }
}
