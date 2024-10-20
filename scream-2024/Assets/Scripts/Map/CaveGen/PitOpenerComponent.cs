using UnityEngine;

public class PitOpenerComponent : MonoBehaviour
{
    [SerializeField] public float r = 8f;
    [SerializeField] public float rate = 1f;

    public void Update()
    {
        var av = Global.Instance.Avatar;
        if (Physics.Raycast(av.transform.position, new Vector3(0, -1, 0), out var info, 100, LayerMask.GetMask("Chunk"))) 
        {
            var chunk = info.collider.GetComponent<Chunk>();
            chunk.Terrain.AdjustWeights(chunk, info.point, r, -1 * Time.deltaTime * rate);
        }
    }
}