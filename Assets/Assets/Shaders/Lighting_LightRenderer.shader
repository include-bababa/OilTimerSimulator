Shader "Lighting/LightRenderer"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Scale("Scale", Range(0, 1)) = 0.1
        _Cutoff("Cutoff", Range(0, 1)) = 0.1
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

            sampler2D _MainTex;
            half4 _Color;
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
                fixed4 color = i.color * tex2D(_MainTex, i.texcoord);
                color *= _Scale;
                color *= i.texcoord.y < 0.25 ? clamp(i.texcoord.y - 0.15, 0, 0.1) * 10.0 :
                    i.texcoord.y > 0.75 ? clamp(0.85 - i.texcoord.y, 0, 0.1) * 10.0 :
                    1.0;
                clip(color.a - _Cutoff);
                return color;
            }

            ENDCG
        }
    }
        FallBack "Diffuse"
}
