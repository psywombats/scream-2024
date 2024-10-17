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
    [SerializeField, Range(0f, 64f)] public float pitRadius = 32f;

    private ComputeBuffer weightsBuffer;

    private void Awake() 
    {
        CreateBuffers();
    }

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
        noiseShader.SetFloat("_PitRad", pitRadius);

        noiseShader.Dispatch(0, 
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount, 
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount, 
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount);
        weightsBuffer.GetData(noise);
        return noise;
    }
}
