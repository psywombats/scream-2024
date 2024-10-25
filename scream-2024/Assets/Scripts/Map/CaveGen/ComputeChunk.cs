using UnityEngine;

public class ComputeChunk : Chunk
{
    [SerializeField] private ComputeShader marchShader;
    
    private ComputeBuffer trianglesBuffer;
    private ComputeBuffer trianglesCountBuffer;
    private ComputeBuffer weightsBuffer;

    protected override void InternalDestroy()
    {
        base.InternalDestroy();
        ReleaseBuffers();
    }

    private void CreateBuffers()
    {
        trianglesBuffer = new ComputeBuffer(5 * (GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk), Triangle.SizeOf, ComputeBufferType.Append);
        trianglesCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        weightsBuffer = new ComputeBuffer(GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk, sizeof(float));
    }

    private void ReleaseBuffers()
    {
        if (weightsBuffer != null)
        {
            trianglesBuffer.Release();
            trianglesCountBuffer.Release();
            weightsBuffer.Release();
        }
    }

    public override void AdjustWeights(Vector3 hit, float r, float mult, bool alwaysApply = false)
    {
        var kernel = marchShader.FindKernel("updateWeights");

        if (weightsBuffer == null)
        {
            CreateBuffers();
        }

        weightsBuffer.SetData(weights);
        marchShader.SetBuffer(kernel, "_Weights", weightsBuffer);
        marchShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        marchShader.SetVector("_HitPosition", hit - transform.position);
        marchShader.SetFloat("_Radius", r);
        marchShader.SetFloat("_Delta", mult);
        marchShader.SetFloat("_AllGood", alwaysApply ? 1 : 0);

        marchShader.Dispatch(kernel,
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount,
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount,
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount);

        weightsBuffer.GetData(weights);
        UpdateMesh();
    }

    protected override Mesh ConstructMesh(float[] weights)
    {
        if (weightsBuffer == null)
        {
            CreateBuffers();
        }

        marchShader.SetBuffer(0, "_Triangles", trianglesBuffer);
        marchShader.SetBuffer(0, "_Weights", weightsBuffer);

        marchShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        marchShader.SetFloat("_IsoLevel", Terrain.Noise.isoLevel);

        weightsBuffer.SetData(weights);
        trianglesBuffer.SetCounterValue(0);

        marchShader.Dispatch(0,
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount,
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount,
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount);

        var triCounts = new int[1];
        ComputeBuffer.CopyCount(trianglesBuffer, trianglesCountBuffer, 0);
        trianglesCountBuffer.GetData(triCounts);
        var triCount = triCounts[0];

        var triangles = new Triangle[triCount];
        trianglesBuffer.GetData(triangles);

        return CreateMeshFromTriangles(triangles, triangles.Length);
    }
}
