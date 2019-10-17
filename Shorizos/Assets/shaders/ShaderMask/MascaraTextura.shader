Shader "Custom/MascaraTextura"
{
     Properties
    {
		_Textura("texture", 2D) = "White"{}
		_Mascara("Mascara", 2D) = "White"{}
		_MascaraT("Mascarat", 2D) = "White"{}
		_Color("Color",Color) = (1,1,1,1)
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
		float4 _Color;
        struct Input
        {
            float2 uv_Textura;
            float2 uv_Mascara;
            float2 uv_MascaraT;
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // SE hace lo mismo que el primero pero enves de tomar la plantila inicial de la mascara y luego se le suma la mascara pero con su textura
            float4 c = tex2D (_Textura, IN.uv_Textura);
            float4 m = tex2D (_Mascara, IN.uv_Mascara);
            float4 m1 = tex2D (_MascaraT, IN.uv_MascaraT);
			float3 R = m.rgb * m1;
			float3 k = c *(1 - m);

			float3 t = k + R;
			o.Albedo = t.rgb;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
