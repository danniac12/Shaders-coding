Shader "Custom/BordesTitilan"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_frecuencia("frecuencia",Range(0,5)) = 0.5
		_frec("Amplitud",Range(0,1)) = 0.5
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
		float _frecuencia;
		float _frec;

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
            //todo lo anterior se utliza pero se le pone un a latencia a bordes para que empieze a titilar
			float Bordes = abs(dot(IN.viewDir,IN.worldNormal));
			float4 t = tex2D(_MainTex,IN.uv_MainTex);
			if(Bordes < 0.35){
				Bordes = abs(_frec*sin(_frecuencia * _Time.y));
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
