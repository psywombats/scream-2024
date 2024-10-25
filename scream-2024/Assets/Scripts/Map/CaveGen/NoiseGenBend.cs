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
        Source.SetFloat("_PitRad", pitRadius);
        Source.SetFloat("_NoiseWeight", noiseWeight);
        Source.SetFloat("_ScaleX", scaleX);
        Source.SetFloat("_ScaleY", scaleY);
        Source.SetFloat("_X1", xCoefs.x);
        Source.SetFloat("_X2", xCoefs.y);
        Source.SetFloat("_X3", xCoefs.z);
        Source.SetFloat("_Y1", yCoefs.x);
        Source.SetFloat("_Y2", yCoefs.y);
        Source.SetFloat("_Y3", yCoefs.z);
    }
}
