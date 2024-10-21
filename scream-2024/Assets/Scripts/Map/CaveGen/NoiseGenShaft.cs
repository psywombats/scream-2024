using UnityEngine;

public class NoiseGenShaft : NoiseGenerator
{
    [Space]
    [SerializeField, Range(0f, 64f)] public float pitRadius = 32f;
    [SerializeField, Range(0f, 64f)] public float pitSlopeX = .2f;
    [SerializeField, Range(0f, 64f)] public float pitSlopeZ = .4f;
    [SerializeField, Range(0f, 10f)] public float pitHardness = 1f;
    [SerializeField, Range(0f, 64f)] public float wallWeight = 16f;
    [SerializeField, Range(0f, 1f)] public float curvature = 0f;

    protected override void SetSpecificNoiseVars()
    {
        base.SetSpecificNoiseVars();
        noiseShader.SetFloat("_PitRad", pitRadius);
        noiseShader.SetFloat("_PitHard", pitHardness);
        noiseShader.SetFloat("_PitOffX", pitSlopeX);
        noiseShader.SetFloat("_PitOffZ", pitSlopeZ);
        noiseShader.SetFloat("_WallWeight", wallWeight);
        noiseShader.SetFloat("_Curvature", curvature);
    }
}
