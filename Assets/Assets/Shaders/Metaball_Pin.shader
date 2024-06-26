Shader "Metaball/Pin"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Scale("Scale", Range(0, 0.05)) = 0.01
        _Cutoff("Cutoff", Range(0, 0.05)) = 0.01
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
        BlendOp RevSub
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

            sampler2D _MainTex;
            fixed4 _Color;
            fixed _Scale;
            fixed _Cutoff;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float base = 1.0 / _Scale;
                float u = (i.texcoord.x - 0.5) * base;
                float v = (i.texcoord.y - 0.5) * base;

                float sqr = u * u + v * v;
                if (sqr < 1.0)
                {
                    return i.color * 10e5;
                }

                float edge = min((0.5 * base) * (0.5 * base), (0.5 * base) * (0.5 * base));
                fixed a = (edge - sqr) / (edge - 1.0);
                fixed4 color = i.color;
                color.a *= a;
                clip(color.a - _Cutoff);
                return color;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
