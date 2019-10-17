Shader "Custom/MascaraTexturaMobil"
{
    Properties
    {
		_Textura("texture", 2D) = "White"{}
		_Mascara("Mascara", 2D) = "White"{}
		_MascaraT("Mascarat", 2D) = "White"{}
		_MascaraD("Mascarat", 2D) = "White"{}
		_Color("Color",Color) = (1,1,1,1)
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

        sampler2D _Textura;
        sampler2D _Mascara;
        sampler2D _MascaraT;
        sampler2D _MascaraD;
		float4 _Color;
		float _Valor;
        struct Input
        {
            float2 uv_Textura;
            float2 uv_Mascara;
            float2 uv_MascaraT;
            float2 uv_MascaraD;
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Se utiliza el fundamento anterior pero ya para hacer el lerp para adicionar una suma de mascaras mas un color 
            float4 c = tex2D (_Textura, IN.uv_Textura);
            float4 m = tex2D (_Mascara, IN.uv_Mascara);
            float4 m1 = tex2D (_MascaraT, IN.uv_MascaraT);
            float4 m2 = tex2D (_MascaraD, IN.uv_MascaraD);
			float3 R = m.rgb * m1;
			float3 s = c *(1-R);
			float3 G = m2.rgb *_Color;
			float3 k = c *(1 - R);
			float3 t = k + (G);
			o.Albedo = lerp(k.rgb,t.rgb,_Valor);

        }
        ENDCG
    }
    FallBack "Diffuse"
}
