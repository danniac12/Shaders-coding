Shader "Custom/Normales"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Normal ("Normal", 2D) = "white" {}
		_Valor ("valor", Range(0,2)) =  0.5
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

        sampler2D _MainTex;
		sampler2D _Normal;
		float _Valor; 
        struct Input
        {
            float2 uv_MainTex;
			float2 uv_Normal;
        };
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Se hace los mismo en las diapositivas pero se multiplica por algo para manterner una profundiad
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
			float4 n = tex2D(_Normal,IN.uv_Normal);
			float3 norman = UnpackNormal(n* _Valor).rgb;
			float3 s = norman;
			o.Normal = normalize(s);

            // Metallic and smoothness come from slider variables
        }
        ENDCG
    }
    FallBack "Diffuse"
}
