public static class GridMetrics
{
    public const int ThreadCount = 8;
    public const int PointsPerChunk = 24;
    public static int ChunkSize => PointsPerChunk - 1;
}
