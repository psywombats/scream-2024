using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingTerrain : MonoBehaviour
{
    [SerializeField] private CaveMap map;
    [Space]
    [SerializeField] private int spawnRadius = 1;
    [SerializeField] private int cullDist = 2;
    [Space]
    [SerializeField] private List<Vector3Int> pulsars;
    [SerializeField] private GameObject toFollow;
    [SerializeField] private Vector3Int bias;

    private Dictionary<Vector3Int, Chunk> chunks = new();
    private Dictionary<int, GameObject> layers = new();

    private bool isSpawningChunks;

    public NoiseGenerator Noise => map.noise;

    public GameObject Target => toFollow != null ? toFollow : (Global.Instance.Avatar == null ? null : Global.Instance.Avatar.gameObject);

    public void Update()
    {
        if (Target != null && !isSpawningChunks)
        {
            isSpawningChunks = true;
            CullChunks();
            StartCoroutine(EnsureChunksRoutine());
        }
    }

    public void AdjustWeights(Chunk chunk, Vector3 hit, float r, float mult)
    {
        var index = chunk.Index;
        for (var x = index.x - 1; x <= index.x + 1; x += 1)
        {
            for (var y = index.y - 1; y <= index.y + 1; y += 1)
            {
                for (var z = index.z - 1; z <= index.z + 1; z += 1)
                {
                    var checkIndex = new Vector3Int(x, y, z);
                    if (chunks.ContainsKey(checkIndex))
                    {
                        chunks[checkIndex].AdjustWeights(hit, r, mult);
                    }
                    // TODO: we should always have that chunk
                }
            }
        }
    }

    public IEnumerator EnsureChunksRoutine(int radius = 0, bool usePlayer = false)
    {
        isSpawningChunks = true;
        var index = usePlayer ? GetPlayerIndex() : Vector3Int.zero;
        if (radius == 0)
        {
            radius = spawnRadius;
        }
        for (var x = index.x - radius; x <= index.x + radius; x += 1)
        {
            for (var y = index.y - radius; y <= index.y + radius; y += 1)
            {
                for (var z = index.z - radius; z <= index.z + radius; z += 1)
                {
                    var checkIndex = new Vector3Int(x, y, z);
                    yield return EnsureChunkRoutine(checkIndex);
                }
            }
        }
        isSpawningChunks = false;
    }

    public void CullAll()
    {
        foreach (var chunk in chunks)
        {
            DestroyImmediate(chunk.Value);
        }
        foreach (var layer in layers)
        {
            DestroyImmediate(layer.Value);
        }
        chunks.Clear();
        layers.Clear();
    }

    private void EnsureLayer(int layerIndex)
    {
        if (!layers.ContainsKey(layerIndex))
        {
            var layer = new GameObject();
            layer.name = "Level " + layerIndex;
            layers[layerIndex] = layer;
            layer.transform.SetParent(transform);
            layer.transform.position = Vector3.zero;
        }
    }

    private List<Vector3Int> toCull = new();
    private void CullChunks()
    {
        var index = GetPlayerIndex();
        
        foreach (var entry in chunks)
        {
            if (Mathf.Abs(index.x - entry.Key.x) + Mathf.Abs(index.y - entry.Key.y) + Mathf.Abs(index.z - entry.Key.z) > cullDist * 3)
            {
                toCull.Add(entry.Key);
            }
        }

        foreach (var c in toCull)
        {
            var chunk = chunks[c];
            Destroy(chunk.gameObject);
            chunks.Remove(c);
        }
        toCull.Clear();
    }

    private Vector3Int GetPlayerIndex()
    {
        var playerPos = Target.transform.position;
        var myPos = transform.position;
        var atPos = playerPos - myPos;
        var index = GetIndexForPos(atPos) + bias;
        return index;
    }

    /// <param name="checkIndex">True if the chunk already exists</param>
    private IEnumerator EnsureChunkRoutine(Vector3Int checkIndex)
    {
        if (!chunks.ContainsKey(checkIndex))
        {
            var chunk = MakeChunk(checkIndex);
            yield return chunk.ConstructMeshRoutine();
        }
    }

    private Vector3Int GetIndexForPos(Vector3 pos)
    {
        var off = GridMetrics.PointsPerChunk / 2f;
        var adjPos = pos - new Vector3(off, off, off);
        var atPos = new Vector3Int(
            Mathf.RoundToInt(adjPos.x / GridMetrics.ChunkSize),
            Mathf.RoundToInt(adjPos.y / GridMetrics.ChunkSize),
            Mathf.RoundToInt(adjPos.z / GridMetrics.ChunkSize));
        return atPos;
    }

    private Chunk MakeChunk(Vector3Int chunkIndex)
    {
        var chunk = Instantiate(map.GetChunkPrefab()).GetComponent<Chunk>();
        var pos = (Vector3)(chunkIndex * GridMetrics.ChunkSize);
        var off = GridMetrics.ChunkSize / 2f;
        chunk.Init(this, chunkIndex, pos);
        chunks[chunkIndex] = chunk;

        chunk.gameObject.SetActive(true);
        chunk.gameObject.name = $"{chunkIndex.x}, {chunkIndex.z}";

        EnsureLayer(chunkIndex.y);
        chunk.transform.SetParent(layers[chunkIndex.y].transform);
        chunk.gameObject.transform.position = pos - new Vector3(off, off, off);
        return chunk;
    }
}
