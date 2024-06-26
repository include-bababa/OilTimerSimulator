Shader "Lighting/PointLight"
{
    Properties
    {
        _Radius("Radius", Range(0, 10)) = 0.5
        _Scale("Scale", Range(0, 0.05)) = 0.1
        _Max("Max", Range(0, 1)) = 0.5
        _Cutoff("Cutoff", Range(0, 1)) = 0.05
    }
        SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            fixed _Radius;
            fixed _Scale;
            fixed _Max;
            fixed _Cutoff;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
                return OUT;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed2 uv = (i.texcoord - 0.5) / _Radius;
                fixed a = 1.0 / (uv.x * uv.x + uv.y * uv.y);
                a *= _Scale;
                fixed4 color = i.color * a;
                color.r = clamp(color.r, 0, _Max);
                color.g = clamp(color.g, 0, _Max);
                color.b = clamp(color.b, 0, _Max);
                color.a = clamp(color.a, 0, _Max);
                clip(color.a - _Cutoff);
                return color;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
