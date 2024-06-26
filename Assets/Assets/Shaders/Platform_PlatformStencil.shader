Shader "Platform/PlatformStencil"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Stencil
            {
                Ref 2
                Comp always
                Pass replace
            }

            Cull Off
            Lighting Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            sampler2D _MainTex;
            #pragma vertex vert_img  
            #pragma fragment frag  
            #include "UnityCG.cginc"  

            fixed4 frag(v2f_img i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv);
                return c;
            }
            ENDCG
        }

    }
    FallBack "Diffuse"
}
