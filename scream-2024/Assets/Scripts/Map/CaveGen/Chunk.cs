using System.Collections;
using UnityEngine;

public abstract class Chunk : MonoBehaviour
{
    [SerializeField] private MeshFilter filter;
    [SerializeField] private new MeshCollider collider;
    [Space]
    [SerializeField] private float pulsarPeriod;
    [SerializeField] private float pulsarAmplitude;

    protected Vector3 pos;
    protected float[] weights;

    public MarchingTerrain Terrain { get; private set; }
    public Vector3Int Index { get; private set; }

    public void Init(MarchingTerrain terrain, Vector3Int index, Vector3 pos)
    {
        Index = index;
        Terrain = terrain;
        this.pos = pos;
    }

    public void ConstructMesh()
    {
        weights = Terrain.Noise.GenerateNoise(pos);
        UpdateMesh();
    }

    public abstract void AdjustWeights(Vector3 hit, float r, float mult, bool alwaysApply = false);

    private void OnDestroy()
    {
        InternalDestroy();
        if (filter.sharedMesh != null)
        {
            Destroy(filter.sharedMesh);
        }
    }

    protected virtual void InternalDestroy() { }

    protected void UpdateMesh()
    {
        var mesh = ConstructMesh(weights);
        filter.sharedMesh = mesh;
        collider.sharedMesh = mesh;
    }

    protected abstract Mesh ConstructMesh(float[] weights);

    protected Mesh CreateMeshFromTriangles(Triangle[] triangles, int triLength)
    {
        var verts = new Vector3[triLength * 3];
        var tris = new int[triLength * 3];

        for (var i = 0; i < triLength; i++)
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

    protected struct Triangle
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public static int SizeOf => sizeof(float) * 3 * 3;
    }
}
