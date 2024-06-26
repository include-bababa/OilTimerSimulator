Shader "Platform/OutlineShade"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _ShadeWidth ("ShadeWidth", Range(0, 1.0)) = 0.1
        _ShadeHeight ("ShadeHeight", Range(0, 1.0)) = 0.1
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

            fixed4 _Color;
            fixed _ShadeWidth;
            fixed _ShadeHeight;

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
                float u = min(i.texcoord.x, 1.0 - i.texcoord.x);
                float v = min(i.texcoord.y, 1.0 - i.texcoord.y);

                if (u > _ShadeWidth && v > _ShadeHeight)
                {
                    discard;
                }

                u = clamp(u, 0.0, _ShadeWidth);
                v = clamp(v, 0.0, _ShadeHeight);

                fixed a = (u / _ShadeWidth) * (v / _ShadeHeight);
                fixed4 color = i.color;
                color.a *= a;
                return color;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
