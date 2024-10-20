using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private NoiseGenerator noise;
    [SerializeField] private ComputeShader marchShader;
    [Space]
    [SerializeField] private MeshFilter filter;
    [SerializeField] private new MeshCollider collider;

    [SerializeField] [HideInInspector] private float[] weights;

    public MarchingTerrain Terrain { get; private set; }
    public Vector3Int Index { get; private set; }

    private ComputeBuffer trianglesBuffer;
    private ComputeBuffer trianglesCountBuffer;
    private ComputeBuffer weightsBuffer;

    public void Init(MarchingTerrain terrain, Vector3Int index, Vector3 pos)
    {
        Index = index;
        Terrain = terrain;

        CreateBuffers();

        weights = noise.GenerateNoise(pos);
        UpdateMesh();
    }

    public void AdjustWeights(Vector3 hit, float r, float mult)
    {
        var kernel = marchShader.FindKernel("updateWeights");

        weightsBuffer.SetData(weights);
        marchShader.SetBuffer(kernel, "_Weights", weightsBuffer);
        marchShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        marchShader.SetVector("_HitPosition", hit - transform.position);
        marchShader.SetFloat("_Radius", r);
        marchShader.SetFloat("_Delta", mult);

        marchShader.Dispatch(kernel, 
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount, 
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount, 
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount);

        weightsBuffer.GetData(weights);
        UpdateMesh();
    }

    private void OnDestroy()
    {
        ReleaseBuffers();
        if (filter.sharedMesh != null)
        {
            Destroy(filter.sharedMesh);
        }
    }

    private void CreateBuffers()
    {
        trianglesBuffer = new ComputeBuffer(5 * (GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk), Triangle.SizeOf, ComputeBufferType.Append);
        trianglesCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        weightsBuffer = new ComputeBuffer(GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk, sizeof(float));
    }

    private void ReleaseBuffers()
    {
        trianglesBuffer.Release();
        trianglesCountBuffer.Release();
        weightsBuffer.Release();
    }

    private void UpdateMesh()
    {
        var mesh = ConstructMesh(weights);
        filter.sharedMesh = mesh;
        collider.sharedMesh = mesh;
    }

    private Mesh ConstructMesh(float[] weights)
    {
        marchShader.SetBuffer(0, "_Triangles", trianglesBuffer);
        marchShader.SetBuffer(0, "_Weights", weightsBuffer);

        marchShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        marchShader.SetFloat("_IsoLevel", noise.isoLevel);

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

        return CreateMeshFromTriangles(triangles);
    }

    private Mesh CreateMeshFromTriangles(Triangle[] triangles)
    {
        var verts = new Vector3[triangles.Length * 3];
        var tris = new int[triangles.Length * 3];

        for (var i = 0; i < triangles.Length; i++)
        {
            var startIndex = i * 3;
            verts[startIndex + 0] = triangles[i].a;
            verts[startIndex + 1] = triangles[i].b;
            verts[startIndex + 2] = triangles[i].c;
            tris[startIndex + 0] = startIndex;
            tris[startIndex + 1] = startIndex + 1;
            tris[startIndex + 2] = startIndex + 2;
        }

        var mesh = new Mesh
        {
            vertices = verts,
            triangles = tris
        };
        mesh.RecalculateNormals();
        return mesh;
    }

    struct Triangle
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public static int SizeOf => sizeof(float) * 3 * 3;
    }
}
