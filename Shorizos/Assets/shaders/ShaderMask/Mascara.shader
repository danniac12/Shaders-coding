Shader "Custom/Mascara"
{
    Properties
    {
		_Textura("texture", 2D) = "White"{}
		_Mascara("Mascara", 2D) = "White"{}
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
		float4 _Color;
        struct Input
        {
            float2 uv_Textura;
            float2 uv_Mascara;
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // se toma Segun las diapositivas el contrario de la mascara tomada para poder agregar el color
            float4 c = tex2D (_Textura, IN.uv_Textura);
            float4 m = tex2D (_Mascara, IN.uv_Mascara);
			float3 R = m.rgb * _Color;
			float3 k = c *(1 - m);

			float3 t = k + R;
			o.Albedo = t.rgb;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
