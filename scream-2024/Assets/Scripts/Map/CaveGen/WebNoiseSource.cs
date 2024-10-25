using UnityEngine;

public class WebNoiseSource : NoiseSource
{
    [SerializeField] private Material origMaterial;
    private Material newMat;
    private CustomRenderTexture renderTex;

    private bool updating;
    private float[] noiseRef;

    public void Start()
    {
        newMat = new Material(origMaterial);
    }

    public void Update()
    {
        if (updating)
        {
            updating = false;
            IsReady = true;
        }
    }

    public override void SetFloat(string name, float value) => newMat.SetFloat(name, value);
    public override void SetInt(string name, int value) => newMat.SetInteger(name, value);

    public override void RequestGenerate()
    {
        renderTex.material = newMat;
        renderTex.Update();
        updating = true;
    }

    public override void ReadNoise(float[] noise)
    {
        //Texture2D tex = new Texture2D(…);
        //RenderTexture.active = < your render texture>
        //tex.ReadPixels(…);
        //RenderTexture.active = null;
    }

    private void OnDestroy()
    {
        Destroy(newMat);
    }
}
