Shader "Metaball/MetaballGearTooth"
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
                fixed2 uv = i.texcoord;
                float uv_x = uv.x < 0.25 ? (0.25 - uv.x) * 0.5 :
                    uv.x > 0.75 ? (uv.x - 0.75) * 0.5 : 0.0;
                float uv_y = uv.y < 0.25 ? (0.25 - uv.y) * 0.5 :
                    uv.y > 0.75 ? (uv.y - 0.75) * 0.5 : 0.0;
                fixed a = 1.0 / (uv_x * uv_x + uv_y * uv_y);
                a *= _Scale;
                fixed4 color = i.color * a;
                clip(color.a - _Cutoff);
                return color;
            }

            ENDCG
        }
    }
        FallBack "Diffuse"
}
