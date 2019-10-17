﻿Shader "Custom/SoloRojo"
{
	Properties
    {
        _Textura("Textura(RGB)", 2D) = "white"{}
        _Textura2("Textura(RGB)", 2D) = "Red"{}
		_Color("Color", color) =(1,1,1,1)
		_Valor("Valor",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
		float4 _Color;
        sampler2D _Textura;
        sampler2D _Textura2;
		float _Valor;

        struct Input
        {
            float2 uv_Textura;
            float2 uv_Textura2;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here

        UNITY_INSTANCING_BUFFER_END(Props)


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            float4 c = tex2D (_Textura, IN.uv_Textura);
            float4 c1 = tex2D (_Textura2, IN.uv_Textura2);
            float3 c2= ((c.rgb -(c1.g))-c1.b);
			//Se tiene encuenta una resta de colores solo para conservar rojo de la textura
			o.Albedo = lerp(c.rgb,c2.rgb,_Valor);

            // Metallic and smoothness come from slider variables
        }
        ENDCG
    }
    FallBack "Diffuse"
}
