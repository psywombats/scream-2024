using UnityEngine;

[RequireComponent(typeof(CaveMap))]
public class CaveAutogen : MonoBehaviour
{
    [SerializeField] private int radius = 1;

    public void Start()
    {
        GetComponent<CaveMap>().terrain.EnsureChunks(radius);
    }
}