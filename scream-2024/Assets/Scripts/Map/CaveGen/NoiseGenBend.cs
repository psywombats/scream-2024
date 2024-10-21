using UnityEngine;

public class NoiseGenBend : NoiseGenerator
{
    [Space]
    [SerializeField, Range(0f, 64f)] public float pitRadius = 32f;
    [SerializeField, Range(0f, 64f)] public float noiseWeight = 6f;
    [SerializeField] public Vector3 xCoefs;
    [SerializeField] public Vector3 yCoefs;
    [SerializeField, Range(0f, 5f)] public float scaleX = 1f;
    [SerializeField, Range(0f, 5f)] public float scaleY = 1;

    protected override void SetSpecificNoiseVars()
    {
        base.SetSpecificNoiseVars();
        noiseShader.SetFloat("_PitRad", pitRadius);
        noiseShader.SetFloat("_NoiseWeight", noiseWeight);
        noiseShader.SetFloat("_ScaleX", scaleX);
        noiseShader.SetFloat("_ScaleY", scaleY);
        noiseShader.SetFloat("_X1", xCoefs.x);
        noiseShader.SetFloat("_X2", xCoefs.y);
        noiseShader.SetFloat("_X3", xCoefs.z);
        noiseShader.SetFloat("_Y1", yCoefs.x);
        noiseShader.SetFloat("_Y2", yCoefs.y);
        noiseShader.SetFloat("_Y3", yCoefs.z);
    }
}
