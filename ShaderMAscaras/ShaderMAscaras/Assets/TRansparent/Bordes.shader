Shader "Custom/Bordes"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
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

        struct Input
        {
            float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
        };
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Se pone lo de diapositivas con los bordes y se pone la logica de las mascaras para poder aplicar los bordes y ademas se pone un condicional para tener el negro en vez de un color trasparente
			float Bordes = abs(dot(IN.viewDir,IN.worldNormal));
			float4 t = tex2D(_MainTex,IN.uv_MainTex);
			if(Bordes < 0.35){
				Bordes = 0 ;
			}
			else
			{
			Bordes =1; 
			}
			float4 a = Bordes;
			float4 c = (((1-a)* _Color));
			if((c.r+c.g+c.b) == 0){
				c=(1-a)*-1;
			}
            o.Albedo = t +c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
