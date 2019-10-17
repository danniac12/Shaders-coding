Shader "Custom/MascaraYBordes"
{
    Properties
    {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
		_Mascara ("mascara (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { 
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType"="TrasparentCutout"
		}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha : fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0


		    sampler2D _MainTex;
			sampler2D _Mascara;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_Mascara;
			float3 worldNormal;
			float3 viewDir;
        };

        float4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
			float4 c = tex2D (_MainTex, IN.uv_MainTex);
            float4 m = tex2D (_Mascara, IN.uv_Mascara);
			float4 mt =  (c *(1-m));
			float bordes = abs(dot(IN.viewDir, IN.worldNormal));
			float4 f = mt * (((1 - bordes)* _Color));
			o.Albedo = f.rgb;
			o.Alpha = mt;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
