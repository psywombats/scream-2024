using UnityEngine;

public class ChunkHolder : MonoBehaviour
{
    [SerializeField] private WebChunk webChunkPrefab;
    [SerializeField] private ComputeChunk computeChunkPrefab;
    [SerializeField] private WebNoiseSource webNoiseSource;
    [SerializeField] private ComputeNoiseSource computeNoiseSource;
    [Space]
    [SerializeField] private bool webOverride;

    public bool UseWebSource() => Application.platform == RuntimePlatform.WebGLPlayer || webOverride;

    public GameObject GetChunk() { return UseWebSource() ? webChunkPrefab.gameObject : computeChunkPrefab.gameObject; }

    public NoiseSource NoiseSource => UseWebSource() ? webNoiseSource : computeNoiseSource;
}
