Shader "Scream2024/NoiseGenWeb/Bend"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("color", Color) = (.25, .5, .5, 1)
        [PerRendererData] _Amplitude("Amplitude", Float) = 8
        [PerRendererData] _Frequency("Frequency", Float) = .005
        [PerRendererData] _Octaves("Octaves", Integer) = 8
        [PerRendererData] _NoiseScale("NoiseScale", Float) = 1
        [PerRendererData] _BaseX("BaseX", Float) = 0
        [PerRendererData] _BaseY("BaseY", Float) = 0
        [PerRendererData] _BaseZ("BaseZ", Float) = 0

        [PerRendererData] _PitRad("_PitRad", Float) = 24
        [PerRendererData] _X1("_X1", Float) = -100
        [PerRendererData] _X2("_X2", Float) = 100
        [PerRendererData] _X3("_X3", Float) = -100
        [PerRendererData] _Y1("_Y1", Float) = -50
        [PerRendererData] _Y2("_Y2", Float) = 50
        [PerRendererData] _Y3("_Y3", Float) = -50
        [PerRendererData] _ScaleX("_ScaleX", Float) = 1
        [PerRendererData] _ScaleY("_ScaleY", Float) = 2
        [PerRendererData] _NoiseWeight("_NoiseWeight", Float) = .5

        [PerRendererData] _NoiseTypeIn("Noise Type In", Integer) = 0
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

            int _NoiseTypeIn;

            int _BaseX, _BaseY, _BaseZ;

            float _PitRad;
            float _X1, _X2, _X3;
            float _Y1, _Y2, _Y3;
            float _ScaleX, _ScaleY;
            float _NoiseWeight;

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

            fixed4 frag(v2f IN) : SV_Target
            {
                float3 pos;
                pos.x = floor((_BaseX + IN.uv.x * 24.0)) * _NoiseScale;
                pos.y = floor((_BaseY + (IN.uv.y * 24.0 * 24.0) % 24.0)) * _NoiseScale;
                pos.z = floor((_BaseZ + floor(IN.uv.y * 24.0))) * _NoiseScale;

                fnl_state noise = fnlCreateState();
                noise.noise_type = _NoiseTypeIn;
                noise.fractal_type = 2;
                noise.frequency = _Frequency;
                noise.octaves = _Octaves;

                float noiseVal = fnlGetNoise3D(noise, pos.x, pos.y, pos.z);

                float targetZ =
                    (_X1 * pos.x / 100 * pos.x / 100 * pos.x / 100) +
                    (_X2 * pos.x / 100 * pos.x / 100) +
                    (_X3 * pos.x / 100);
                float targetY =
                    (_Y1 * pos.x / 100 * pos.x / 100 * pos.x / 100) +
                    (_Y2 * pos.x / 100 * pos.x / 100) +
                    (_Y3 * pos.x / 100);
                float offZ = (targetZ - pos.z) * _ScaleX;
                float offY = (targetY - pos.y) * _ScaleY;
                float offCenter = sqrt(offZ * offZ + offY * offY);
                offCenter += noiseVal * _NoiseWeight;
                float n = offCenter / _PitRad;

                return n;
            }
            ENDCG
        }
    }
}
