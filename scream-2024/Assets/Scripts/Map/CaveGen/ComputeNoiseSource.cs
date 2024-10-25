﻿using UnityEngine;

public class ComputeNoiseSource : NoiseSource
{
    [SerializeField] protected ComputeShader noiseShader;

    public override void SetFloat(string name, float value) => noiseShader.SetFloat(name, value);
    public override void SetInt(string name, int value) => noiseShader.SetInt(name, value);

    private ComputeBuffer weightsBuffer;

    public override void RequestGenerate()
    {
        if (weightsBuffer == null)
        {
            CreateBuffers();
        }

        noiseShader.SetBuffer(0, "_Weights", weightsBuffer);
        noiseShader.Dispatch(0,
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount,
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount,
            GridMetrics.PointsPerChunk / GridMetrics.ThreadCount);

        IsReady = true;
    }

    public override void ReadNoise(float[] noise)
    {
        weightsBuffer.GetData(noise);
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
}
