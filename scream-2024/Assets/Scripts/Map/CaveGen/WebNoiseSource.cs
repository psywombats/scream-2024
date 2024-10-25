using UnityEngine;

public class WebNoiseSource : NoiseSource
{
    [SerializeField] private RenderTexture renderTex;
    [SerializeField] private Material referenceMat;
    [SerializeField] private Texture dummyTex;
    private Texture2D bufferTex;

    private Material mat;
    private Material Mat
    {
        get
        {
            if (mat == null)
            {
                mat = new Material(referenceMat);
            }
            return mat;
        }
    }

    public override void SetFloat(string name, float value) => Mat.SetFloat(name, value);
    public override void SetInt(string name, int value) => Mat.SetInteger(name, value);

    public override void GenerateNoise(float[] weights)
    {
        if (bufferTex == null)
        {
            bufferTex = new Texture2D(24, 576, TextureFormat.RFloat, false);
        }

        Graphics.Blit(dummyTex, renderTex, Mat, 0);
        RenderTexture.active = renderTex;
        bufferTex.ReadPixels(new Rect(0, 0, 24, 576), 0, 0);
        RenderTexture.active = null;

        var nativeArr = bufferTex.GetPixelData<float>(0);
        nativeArr.CopyTo(weights);
        if (!Application.isPlaying)
        {
            DestroyImmediate(mat);
            mat = null;
        }
    }

    private void OnDestroy()
    {
        Destroy(bufferTex);
        Destroy(mat);
    }
}
