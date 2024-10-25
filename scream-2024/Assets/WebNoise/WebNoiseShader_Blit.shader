Shader "Scream2024/WebNoiseTest_Blit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("color", Color) = (.25, .5, .5, 1)
        [PerRendererData] _Amplitude("Amplitude", Float) = 1
        [PerRendererData] Frequency("Frequency", Float) = 1
        [PerRendererData] _Octaves("Octaves", Integer) = 1
        [PerRendererData] _NoiseScale("NoiseScale", Float) = 1
        [PerRendererData] _BaseX("BaseX", Float) = 0
        [PerRendererData] _BaseY("BaseY", Float) = 0
        [PerRendererData] _BaseZ("BaseZ", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "FastNoiseLite.hlsl"

            float _Amplitude;
            float _Frequency;
            float _NoiseScale;
            int _Octaves;

            int _BaseX, _BaseY, _BaseZ;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                float posX = IN.uv.x * 24.0;
                float posY = IN.uv.y * 24.0;
                float posZ = IN.uv.y * 24.0;
                posX = posX;
                posY = (posY * 24.0) % 24.0;
                posZ = floor(posZ);
                posX = (_BaseX + posX) * _NoiseScale;
                posY = (_BaseY + posY) * _NoiseScale;
                posZ = (_BaseZ + posZ) * _NoiseScale;

                fnl_state noise = fnlCreateState();
                noise.noise_type = 0;
                noise.fractal_type = 2;
                noise.frequency = _Frequency;
                noise.octaves = _Octaves;

                float n = fnlGetNoise3D(noise, posX, posY, posZ);
                return n;
            }
            ENDCG
        }
    }
}
