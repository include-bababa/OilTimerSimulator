Shader "Metaball/MetaballCasingRounded"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Scale("Scale", Range(0.5, 2)) = 1.05
        _Cutoff("Cutoff", Range(0, 0.05)) = 0.01
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
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
                float u = (i.texcoord.x - 0.5) * 2 * _Scale;
                float v = (i.texcoord.y - 0.5) * 2 * _Scale;

                float sqr = u * u + v * v;
                if (sqr < 1.0)
                {
                    return i.color * 10e3;
                }

                float edge = _Scale * _Scale;
                clip(edge - sqr);
                fixed a = (edge - sqr) / (edge - 1.0);
                fixed4 color = i.color;
                color.a *= a * 0.05;
                clip(color.a - _Cutoff);
                return color;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
