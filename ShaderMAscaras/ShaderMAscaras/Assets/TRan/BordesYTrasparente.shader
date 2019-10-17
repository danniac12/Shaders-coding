Shader "Custom/BordesYTrasparente"
{
     Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
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


        struct Input
        {
            float2 uv_MainTex;
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
			float bordes = abs(dot(IN.viewDir, IN.worldNormal));
			float4 f = (1 - bordes)* _Color;
			o.Albedo = f.rgb;
			o.Alpha = f;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
