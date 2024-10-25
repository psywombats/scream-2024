using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WebChunk : Chunk
{
    private int triIndex;
    private Triangle[] tris = new Triangle[256];
    private float[] cubeValues = new float[8];

    public override void AdjustWeights(Vector3 hit, float r, float mult, bool alwaysApply = false)
    {
        throw new NotImplementedException();
    }

    protected override Mesh ConstructMesh(float[] weights)
    {
        var iso = Terrain.Noise.isoLevel;
        triIndex = 0;
        for (var x = 0; x < GridMetrics.PointsPerChunk - 1; x += 1)
        {
            for (var y = 0; y < GridMetrics.PointsPerChunk - 1; y += 1)
            {
                for (var z = 0; z < GridMetrics.PointsPerChunk - 1; z += 1)
                {
                    var id = new Vector3(x, y, z);

                    cubeValues[0] = weights[(x + 0) + GridMetrics.PointsPerChunk * ((y + 0) + GridMetrics.PointsPerChunk * (z + 1))];
                    cubeValues[1] = weights[(x + 1) + GridMetrics.PointsPerChunk * ((y + 0) + GridMetrics.PointsPerChunk * (z + 1))];
                    cubeValues[2] = weights[(x + 1) + GridMetrics.PointsPerChunk * ((y + 0) + GridMetrics.PointsPerChunk * (z + 0))];
                    cubeValues[3] = weights[(x + 0) + GridMetrics.PointsPerChunk * ((y + 0) + GridMetrics.PointsPerChunk * (z + 0))];
                    cubeValues[4] = weights[(x + 0) + GridMetrics.PointsPerChunk * ((y + 1) + GridMetrics.PointsPerChunk * (z + 1))];
                    cubeValues[5] = weights[(x + 1) + GridMetrics.PointsPerChunk * ((y + 1) + GridMetrics.PointsPerChunk * (z + 1))];
                    cubeValues[6] = weights[(x + 1) + GridMetrics.PointsPerChunk * ((y + 1) + GridMetrics.PointsPerChunk * (z + 0))];
                    cubeValues[7] = weights[(x + 0) + GridMetrics.PointsPerChunk * ((y + 1) + GridMetrics.PointsPerChunk * (z + 0))];

                    int cubeIndex = 0;
                    if (cubeValues[0] < iso) cubeIndex |= 1;
                    if (cubeValues[1] < iso) cubeIndex |= 2;
                    if (cubeValues[2] < iso) cubeIndex |= 4;
                    if (cubeValues[3] < iso) cubeIndex |= 8;
                    if (cubeValues[4] < iso) cubeIndex |= 16;
                    if (cubeValues[5] < iso) cubeIndex |= 32;
                    if (cubeValues[6] < iso) cubeIndex |= 64;
                    if (cubeValues[7] < iso) cubeIndex |= 128;

                    var edges = MarchingConstants.triTable[cubeIndex];

                    for (int i = 0; edges[i] != -1; i += 3)
                    {
                        // First edge lies between vertex e00 and vertex e01
                        int e00 = MarchingConstants.edgeConnections[edges[i]][0];
                        int e01 = MarchingConstants.edgeConnections[edges[i]][1];

                        // Second edge lies between vertex e10 and vertex e11
                        int e10 = MarchingConstants.edgeConnections[edges[i + 1]][0];
                        int e11 = MarchingConstants.edgeConnections[edges[i + 1]][1];

                        // Third edge lies between vertex e20 and vertex e21
                        int e20 = MarchingConstants.edgeConnections[edges[i + 2]][0];
                        int e21 = MarchingConstants.edgeConnections[edges[i + 2]][1];
                        Triangle tri;
                        tri.a = Lerp(MarchingConstants.cornerOffsets[e00], cubeValues[e00], MarchingConstants.cornerOffsets[e01], cubeValues[e01], iso) + id;
                        tri.b = Lerp(MarchingConstants.cornerOffsets[e10], cubeValues[e10], MarchingConstants.cornerOffsets[e11], cubeValues[e11], iso) + id;
                        tri.c = Lerp(MarchingConstants.cornerOffsets[e20], cubeValues[e20], MarchingConstants.cornerOffsets[e21], cubeValues[e21], iso) + id;


                        tris[triIndex] = tri;
                        triIndex += 1;
                        if (triIndex >= tris.Length)
                        {
                            Array.Resize(ref tris, tris.Length * 2);
                        }
                    }
                }
            }
        }
        return CreateMeshFromTriangles(tris, triIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Lerp(int[] edgeVertex1, float valueAtVertex1, int[] edgeVertex2, float valueAtVertex2, float isoLevel)
    {
        return new Vector3(
            edgeVertex1[0] + (isoLevel - valueAtVertex1) * (edgeVertex2[0] - edgeVertex1[0]) / (valueAtVertex2 - valueAtVertex1),
            edgeVertex1[1] + (isoLevel - valueAtVertex1) * (edgeVertex2[1] - edgeVertex1[1]) / (valueAtVertex2 - valueAtVertex1),
            edgeVertex1[2] + (isoLevel - valueAtVertex1) * (edgeVertex2[2] - edgeVertex1[2]) / (valueAtVertex2 - valueAtVertex1)
        );
    }
}
