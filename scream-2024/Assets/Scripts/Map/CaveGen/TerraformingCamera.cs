using UnityEngine;

public class TerraformingCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField][Range(.5f, 16f)] private float brushSize = 2f;
    [SerializeField] private GameObject sphere;

    private bool active;
    private bool haltChecks;
    private RaycastHit hit;

    public void Update()
    {
        if (haltChecks)
        {
            haltChecks = false;
            return;
        }
        active = Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 1000);
        sphere.SetActive(active);
        if (active)
        {
            sphere.transform.position = hit.point;
            sphere.transform.localScale = new Vector3(brushSize, brushSize, brushSize);
        }

        brushSize += Input.mouseScrollDelta.y * Time.deltaTime * 50;
    }

    public void LateUpdate()
    {
        if (!active)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            haltChecks = true;
            Terraform(1);
        }
        else if (Input.GetMouseButton(1))
        {
            haltChecks = true;
            Terraform(-1);
        }
    }

    private void Terraform(int sign)
    {
        var chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
        {
            return;
        }
        chunk.Terrain.AdjustWeights(chunk, hit.point, brushSize / 2f, sign);
    }
}
