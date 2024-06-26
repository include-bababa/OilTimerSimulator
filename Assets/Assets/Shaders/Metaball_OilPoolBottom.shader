Shader "Metaball/MetaballOilPoolBottom"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Scale("Scale", Range(0, 0.05)) = 0.01
        _Cutoff("Cutoff", Range(0, 1.0)) = 0.01
        _Center("Center", Range(0, 1.0)) = 0.5
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
        Blend One OneMinusSrcAlpha

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
            fixed _Center;

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
                float p = uv.y + abs(uv.x - _Center) * 0.65;
                //float p = uv.y + (uv.y < 0.5 ? abs(uv.x - 0.5) * -0.5 : 0.0);
                float uv_y = p > 0.75 ? (p - 0.75) * 0.2 : 0.0;
                fixed a = 1.0 / clamp(uv_y * uv_y, 1.0e-5, 1);
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
