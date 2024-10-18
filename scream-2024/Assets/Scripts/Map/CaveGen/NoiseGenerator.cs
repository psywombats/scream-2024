using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    [SerializeField] private ComputeShader noiseShader;
    [Space]
    [SerializeField, Range(.1f, 10f)] private float noiseScale = 1f;
    [SerializeField, Range(1f, 10f)] private float amplitude = 5f;
    [SerializeField, Range(0f, 1f)] private float frequency = 0.005f;
    [SerializeField, Range(1, 10)] private int octaves = 8;
    [SerializeField, Range(0f, 1f)] float groundPercent = 0.2f;
    [SerializeField, Range(-1f, 1f)] public float isoLevel = 0.6f;
    [Space]
    [SerializeField, Range(0f, 64f)] public float pitRadius = 32f;
    [SerializeField, Range(0f, 10f)] public float pitSlopeX = .2f;
    [SerializeField, Range(0f, 10f)] public float pitSlopeZ = .4f;
    [SerializeField, Range(0f, 10f)] public float pitHardness = 1f;
    [Space]
    [SerializeField] public NoiseType noiseType = NoiseType.NOISE_OPENSIMPLEX2;
    [SerializeField] public FractalType fractalType = FractalType.FRACTAL_RIDGED;

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

    private ComputeBuffer weightsBuffer;

    private void OnDestroy() 
    {
        ReleaseBuffers();
    }

    private void CreateBuffers()
    {
        weightsBuffer = new ComputeBuffer(
            GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk, sizeof(float)
        );
    }

    private void ReleaseBuffers()
    {
        weightsBuffer.Dispose();
    }

    public float[] GenerateNoise(Vector3 pos)
    {
        if (weightsBuffer == null)
        {
            CreateBuffers();
        }

        var noise = new float[GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk];
        noiseShader.SetBuffer(0, "_Weights", weightsBuffer);

        noiseShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        noiseShader.SetFloat("_NoiseScale", noiseScale);
        noiseShader.SetFloat("_Amplitude", amplitude);
        noiseShader.SetFloat("_Frequency", frequency);
        noiseShader.SetInt("_Octaves", octaves);
        noiseShader.SetFloat("_GroundPercent", groundPercent);
        noiseShader.SetFloat("_BaseX", pos.x);
        noiseShader.SetFloat("_BaseY", pos.y);
        noiseShader.SetFloat("_BaseZ", pos.z);
        
        noiseShader.SetInt("_NoiseTypeIn", (int)noiseType);
        noiseShader.SetInt("_FractalTypeIn", (int)fractalType);

        noiseShader.SetFloat("_PitRad", pitRadius);
        noiseShader.SetFloat("_PitHard", pitHardness);
        noiseShader.SetFloat("_PitOffX", pitSlopeX);
        noiseShader.SetFloat("_PitOffZ", pitSlopeZ);

        noiseShader.Dispatch(0, 
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount, 
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount, 
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount);
        weightsBuffer.GetData(noise);
        return noise;
    }
}
