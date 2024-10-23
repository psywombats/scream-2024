using UnityEngine;

[ExecuteInEditMode]
public class BillboardingComponent : MonoBehaviour
{
    [SerializeField] private Camera toBillTo;

    public void Update()
    {
        if (toBillTo != null)
        {
            transform.eulerAngles = toBillTo.transform.eulerAngles;
        }
    }
}