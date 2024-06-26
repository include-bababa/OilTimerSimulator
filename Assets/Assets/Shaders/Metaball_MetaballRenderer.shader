Shader "Metaball/MetaballRenderer"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Cutoff ("Cutoff", Range(0, 1)) = 0.5
        _Stroke ("Stroke", Range(0, 1)) = 0.1
        _StrokeColor ("StrokeColor", Color) = (1, 0, 0, 1)
        _OilHeight ("OilHeight", Range(0, 1)) = 1.0
        _EmptyColor ("EmptyColor", Color) = (0.5, 0.5, 0.5, 0.5)
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
            float4 _MainTex_TexelSize;
            float4 _Color;
            float _Cutoff;
            float _Stroke;
            float4 _StrokeColor;
            float _OilHeight;
            float4 _EmptyColor;

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
                float2 uv = i.texcoord;
                float4 main_color = tex2D(_MainTex, i.texcoord);
                if (_OilHeight < uv.y)
                {
                    return _EmptyColor * main_color.a;
                }

                clip(main_color.a - _Cutoff);

                float ratio = main_color.a / _Stroke;
                float texelsize_x = _MainTex_TexelSize.x;
                float texelsize_y = _MainTex_TexelSize.y;
            
                float4 color_l = tex2D(_MainTex, float2(i.texcoord.x - texelsize_x, i.texcoord.y));
                float4 color_r = tex2D(_MainTex, float2(i.texcoord.x + texelsize_x, i.texcoord.y));
                float4 color_u = tex2D(_MainTex, float2(i.texcoord.x, i.texcoord.y - texelsize_y));
                float4 color_d = tex2D(_MainTex, float2(i.texcoord.x, i.texcoord.y + texelsize_y));

                float dx = pow(clamp(color_r.a / _Stroke, 0, 5), 3.0) - pow(clamp(color_l.a / _Stroke, 0, 5), 3.0);
                float dy = pow(clamp(color_d.a / _Stroke, 0, 5), 3.0) - pow(clamp(color_u.a / _Stroke, 0, 5), 3.0);
                float d = cos(clamp(dx + 0.65, -1, 1) * 1.57) * cos(clamp(dy + 0.65, -1, 1) * 1.57);
                float spec = step(ratio, 1.5) * pow(d, 6.0) * 20.0;

                //float dx = color_r.a * color_r.a - color_l.a * color_l.a;
                //float dy = color_d.a * color_d.a - color_u.a * color_u.a;
                //float d = pow(dx - 0.5, 2.0) + pow(dy - 0.5, 2.0);
                //float spec = pow(d, 10.0);

                main_color = ratio > 1.0 ? _Color : (_Color * ratio * ratio + _StrokeColor * (1.0 - ratio * ratio));
                main_color += fixed4(spec, spec, spec, (1 - main_color.a) * spec);
                // color = color.a < _Stroke ? _StrokeColor : _Color;
                return fixed4(main_color);
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
