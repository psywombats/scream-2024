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
        Source.SetFloat("_PitRad", pitRadius);
        Source.SetFloat("_PitHard", pitHardness);
        Source.SetFloat("_PitOffX", pitSlopeX);
        Source.SetFloat("_PitOffZ", pitSlopeZ);
        Source.SetFloat("_WallWeight", wallWeight);
        Source.SetFloat("_Curvature", curvature);
    }
}
