Shader "Scream2024/NoiseGenWeb/Shaft"
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

        [PerRendererData] _PitRad("_PitRad", Float) = 32
        [PerRendererData] _PitHard("_PitHard", Float) = 1
        [PerRendererData] _PitOffX("_PitOffX", Float) = .5
        [PerRendererData] _PitOffZ("_PitOffZ", Float) = .1
        [PerRendererData] _Curvature("_Curvature", Float) = .1
        [PerRendererData] _WallWeight("_WallWeight", Float) = .5

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
            float _PitHard;
            float _PitOffX;
            float _PitOffZ;
            float _Curvature;
            float _WallWeight;

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

            float sinus(float theta)
            {
                float PI = 3.14159265358979323846;
                float a = theta + PI / 2.0, b = PI * 2.0;
                theta = ((a > 0) ? a - b * ((int)(a / b)) : (-a + b * (((int)(a / b))))) - PI / 2.0;
                if (theta > PI / 2.0)
                    theta = PI - theta;
                float x3 = (theta * theta * theta);
                float x5 = (x3 * theta * theta);
                return theta - x3 / 6.0 + x5 / 120.0;
            }

            float cosinus(float x)
            {
                return sinus(x + 1.57079632679489661923);
            }

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

                float biasX = _PitOffX;
                float biasY = _PitOffZ;
                if (_Curvature > 0)
                {
                    biasX *= sinus(pos.y * _Curvature);
                    biasY *= cosinus(pos.y * _Curvature);
                }
                else
                {
                    biasX *= pos.y;
                    biasY *= pos.y;
                }
                float offCenter = sqrt((pos.x - biasX) * (pos.x - biasX) + (pos.z - biasY) * (pos.z - biasY));
                offCenter += noiseVal * _WallWeight;
                float n = offCenter / _PitRad;

                return n;
            }
            ENDCG
        }
    }
}
