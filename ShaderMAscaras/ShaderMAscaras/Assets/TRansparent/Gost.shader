Shader "Custom/Gost"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
       _MainTex ("Albedo (RGB)", 2D) = "white" {}
	_frecuencia("frecuencia",Range(0,2)) = 0.5
       _Mask ("Mask", 2D) = "white" {}

    }
    SubShader
    {
        Tags 
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType"="TransparentCutout"
		}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

         float4 _Color;
	     sampler2D _MainTex, _Mask;
		 float _frecuencia;
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Mask;
			float3 worldNormal;
			float3 viewDir;
        };

       

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float Bordes = abs(dot(IN.viewDir, IN.worldNormal));
			float4 t = tex2D(_MainTex, IN.uv_MainTex);
			if (Bordes < 0.35) {
				Bordes = abs(0.2*sin(_frecuencia * _Time.y));
			}
			else
			{
				Bordes = 1;
			}
			float4 a = Bordes;
			float4 c = (((a)* _Color));
			o.Albedo = t + c.rgb;
			o.Alpha = t + c.a;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
