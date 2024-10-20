using UnityEngine;

public class NoiseGenChamber : NoiseGenerator
{
    [Space]
    [SerializeField, Range(0f, 64f)] public float chamberR = 32f;
    [SerializeField, Range(0f, 5f)] public float chamberOblX = 1f;
    [SerializeField, Range(0f, 5f)] public float chamberOblZ = 1f;
    [SerializeField, Range(0f, 5f)] public float ceilHard = 1f;
    [SerializeField, Range(0f, 5f)] public float floorHard = 1f;
    [SerializeField, Range(0f, 5f)] public float chamberWeight = 1f;
    [SerializeField, Range(0f, 5f)] public float noiseWeight = 1f;

    protected override void SetSpecificNoiseVars()
    {
        base.SetSpecificNoiseVars();
        noiseShader.SetFloat("_ChamberR", chamberR);
        noiseShader.SetFloat("_ChamberOblX", chamberOblX);
        noiseShader.SetFloat("_ChamberOblZ", chamberOblZ);
        noiseShader.SetFloat("_CeilHardness", floorHard);
        noiseShader.SetFloat("_FloorHardness", ceilHard);
        noiseShader.SetFloat("_ChamberWeight", chamberWeight);
        noiseShader.SetFloat("_NoiseWeight", noiseWeight);
    }
}
