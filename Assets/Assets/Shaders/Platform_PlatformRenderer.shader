Shader "Platform/PlatformRenderer"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _AlphaTex("AlphaTex", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _MinAlpha("MinAlpha", Range(0, 1)) = 0.01
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

        Stencil
        {
            Ref 2
            Comp equal
        }
        ZTest Always

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            sampler2D _AlphaTex;
            half4 _Color;
            fixed _MinAlpha;

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
                color.a *= clamp(tex2D(_AlphaTex, i.texcoord), _MinAlpha, 1.0);
                return color;
            }

            ENDCG
        }
    }
        FallBack "Diffuse"
}
