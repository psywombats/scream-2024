using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    [SerializeField] private ChunkHolder chunker;
    [Space]
    [SerializeField, Range(.1f, 10f)] private float noiseScale = 1f;
    [SerializeField, Range(1f, 10f)] private float amplitude = 5f;
    [SerializeField, Range(0f, 1f)] private float frequency = 0.005f;
    [SerializeField, Range(1, 10)] private int octaves = 8;
    [SerializeField, Range(-1f, 1f)] public float isoLevel = 0.6f;
    [Space]
    [SerializeField] public NoiseType noiseType = NoiseType.NOISE_OPENSIMPLEX2;
    [SerializeField] public FractalType fractalType = FractalType.FRACTAL_RIDGED;

    protected NoiseSource Source => chunker.NoiseSource;

    public enum NoiseType
    {
        NOISE_OPENSIMPLEX2 = 0,
        NOISE_OPENSIMPLEX2S = 1,
        NOISE_CELLULAR = 2,
        NOISE_PERLIN = 3,
        NOISE_VALUE_CUBIC = 4,
        NOISE_VALUE = 5,
    }

    public enum FractalType
    {
        FRACTAL_NONE = 0,
        FRACTAL_FBM = 1,
        FRACTAL_RIDGED = 2,
        FRACTAL_PINGPONG = 3,
        FRACTAL_DOMAIN_WARP_PROGRESSIVE = 4,
        FRACTAL_DOMAIN_WARP_INDEPENDENT = 5,
    }

    public void Start()
    {
        // Source = computeSource;
    }

    public float[] GenerateNoise(Vector3 pos)
    {
        Source.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        Source.SetFloat("_NoiseScale", noiseScale);
        Source.SetFloat("_Amplitude", amplitude);
        Source.SetFloat("_Frequency", frequency);
        Source.SetInt("_Octaves", octaves);
        Source.SetFloat("_BaseX", pos.x);
        Source.SetFloat("_BaseY", pos.y);
        Source.SetFloat("_BaseZ", pos.z);

        Source.SetInt("_NoiseTypeIn", (int)noiseType);
        Source.SetInt("_FractalTypeIn", (int)fractalType);

        SetSpecificNoiseVars();

        var noise = new float[GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk];
        Source.GenerateNoise(noise);
        return noise;
    }

    protected virtual void SetSpecificNoiseVars() { }
}
