using UnityEngine;

public class TestCRT : MonoBehaviour
{
    [SerializeField] private CustomRenderTexture crt;
    private Texture2D tex;

    public void Update()
    {
        tex = new Texture2D(24, 576, TextureFormat.RFloat, false);
    }

    public void Test()
    {
        crt.Update();

        RenderTexture.active = crt;
        tex.ReadPixels(new Rect(0, 0, 576, 24), 0, 0);
        RenderTexture.active = null;

        var nativeArr = tex.GetPixelData<float>(0);
        var arr = new float[24 * 24 * 24];
        nativeArr.CopyTo(arr);
        for (var i = 0; i < 12; i += 1)
        {
            Debug.Log(i + ": " + arr[i]);
        }

        Debug.Log("SWAP");
    }
}