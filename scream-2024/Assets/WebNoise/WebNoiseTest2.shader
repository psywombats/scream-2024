Shader "CustomRenderTexture/WebNoiseTest2"
{
    Properties
    {
        _Amplitude("Amplitude", Float) = 1
        _Freq("Freq", Float) = 1
        _Octaves("Octaves", Integer) = 1
        _NoiseScale("NoiseScale", Float) = 1
        _BaseX("BaseX", Integer) = 0
        _BaseY("BaseY", Integer) = 0
        _BaseZ("BaseZ", Integer) = 0
    }

     SubShader
     {
        Blend One Zero

        Pass
        {
            Name "New Custom Render Texture 2"

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
                float posX = (_BaseX + (IN.globalTexcoord.x * 576) % 24) * _NoiseScale;
                float posY = (_BaseY + IN.globalTexcoord.y * 24.0) * _NoiseScale;
                float posZ = (_BaseZ + floor(IN.globalTexcoord.x * 24)) * _NoiseScale;
                //float posX = (_BaseX + IN.globalTexcoord.x * 24.0) * _NoiseScale;
                //float posY = (_BaseY + IN.globalTexcoord.y * 24.0) * _NoiseScale;
                //float posZ = (_BaseZ + IN.globalTexcoord.z * 24.0) * _NoiseScale;

                fnl_state noise = fnlCreateState();
                noise.noise_type = 0;
                noise.fractal_type = 2;
                noise.frequency = _Freq;
                noise.octaves = _Octaves;

                float n = fnlGetNoise3D(noise, posX, posY, posZ);
                return n;
            }
            ENDCG
        }
    }
}
