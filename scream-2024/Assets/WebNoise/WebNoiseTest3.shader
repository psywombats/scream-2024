Shader "CustomRenderTexture/WebNoiseTest3"
{
    Properties
    {
        _Amplitude("Amplitude", Float) = 1
        _Freq("Freq", Float) = 1
        _Octaves("Octaves", Integer) = 1
        _NoiseScale("NoiseScale", Float) = 1
        _BaseX("BaseX", Float) = 0
        _BaseY("BaseY", Float) = 0
        _BaseZ("BaseZ", Float) = 0
    }

     SubShader
     {

        Blend One Zero
        Pass
        {
            Name "New Custom Render Texture 3"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #include "FastNoiseLite.hlsl"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float _Amplitude;
            float _Freq;
            float _NoiseScale;
            int _Octaves;

            int _BaseX, _BaseY, _BaseZ;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float posX = IN.globalTexcoord.x * 24;
                float posY = IN.globalTexcoord.y * 24;
                float posZ = IN.globalTexcoord.y * 24;
                posX = posX;
                posY = (posY * 24.0) % 24.0;
                posZ = floor(posZ);
                posX = (_BaseX + posX) * _NoiseScale;
                posY = (_BaseY + posY) * _NoiseScale;
                posZ = (_BaseZ + posZ) * _NoiseScale;

                fnl_state noise = fnlCreateState();
                noise.noise_type = 0;
                noise.fractal_type = 2;
                noise.frequency = _Freq;
                noise.octaves = _Octaves;

                float n = fnlGetNoise3D(noise, posX, posY, posZ);
                return posX / 24;
            }
            ENDCG
        }
    }
}
