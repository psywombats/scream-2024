using UnityEngine;

public class TacticsMap : GameMap
{
    [SerializeField] private TacticsTerrainMesh mesh;

    public static int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
