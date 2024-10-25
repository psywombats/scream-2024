using UnityEngine;

public class TestCRT : MonoBehaviour
{
    [SerializeField] private CustomRenderTexture renderTex;
    [SerializeField] private RenderTexture cusR;
    [SerializeField] private Texture dummyTex;
    [SerializeField] private Material mat;
    private Texture2D bufferTex;

    public void Test2()
    {
        if (bufferTex == null)
        {
            bufferTex = new Texture2D(24, 576, TextureFormat.RFloat, false);
        }

        RenderTexture.active = renderTex;
        bufferTex.ReadPixels(new Rect(0, 0, 24, 576), 0, 0);
        bufferTex.Apply();
        RenderTexture.active = null;

        var nativeArr = bufferTex.GetPixelData<float>(0);
        var arr = new float[24 * 24 * 24];
        nativeArr.CopyTo(arr);

        for (var i = 0; i < 12; i += 1)
        {
            Debug.Log(i + ": " + arr[i]);
        }

        Debug.Log("SWAP");
        DestroyImmediate(bufferTex);
    }

    public void Test()
    {
        Graphics.Blit(dummyTex, cusR, mat, 0);
    }
}