Shader "Custom/Normal"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NormalTex ("Normal Map", 2D) = "white" {}
        _Factor ("Factor", Range(0,1)) = 0.5
        
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

        sampler2D _MainTex, _NormalTex;

        struct Input
        {
            float2 uv_MainTex, uv_NormalTex;
        };

        half _Factor;
        
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // almacenamos la textura en la variable c
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			//almacenamos el normal map en la variable n
            fixed4 n = tex2D (_NormalTex, IN.uv_NormalTex) * _Color;
			//multiplicamos la variable n por la variable factor para tener control de la fuerza con la que se muestra el normal map
			float3 normal = UnpackNormal((n)*_Factor).rgb;
			
			o.Normal = normalize(normal);
            // Metallic and smoothness come from slider variables
         
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
