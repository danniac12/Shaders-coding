Shader "Custom/MultiMAsk"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Tierra ("Hierba", 2D) = "white" {}
        _Arena ("Arena", 2D) = "white" {}
        _Agua ("Agua", 2D) = "white" {}

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
        sampler2D _Tierra;
        sampler2D _Arena;
        sampler2D _Agua;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Tierra;
            float2 uv_Arena;
            float2 uv_Agua;
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
            float4 c = tex2D (_MainTex, IN.uv_MainTex);
            float4 w = tex2D (_Agua, IN.uv_Agua);
            float4 a = tex2D (_Arena, IN.uv_Arena);
            float4 t = tex2D (_Tierra, IN.uv_Tierra);
			float3 ConHierba = (1 - c.g )* w;
			float3 ConArena = (1-c.r )*  t;
			float3 ConAgua = (1-c.b )* a;

            o.Albedo = ConHierba+ConArena+ConAgua;
            // Metallic and smoothness come from slider variables
        }
        ENDCG
    }
    FallBack "Diffuse"
}
