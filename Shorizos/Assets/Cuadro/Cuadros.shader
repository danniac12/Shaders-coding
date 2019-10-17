Shader "Custom/Cuadros"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Hit ("Hit (RGB)", 2D) = "white" {}
        _Norm ("Normal (RGB)", 2D) = "white" {}
		_Valor("Valor", Range(0,1)) = 0
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
        sampler2D _Hit;
        sampler2D _Norm;
        float _Valor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Hit;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 h = tex2D (_Hit, IN.uv_Hit);
			float4 n = tex2D(_Norm,IN.uv_MainTex);
			//c = 1 -c;
			o.Albedo = lerp(h.rgb,c.rgb,_Valor);
			float3 normal = UnpackNormal(n).rgb;
			o.Normal = normalize(normal);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
