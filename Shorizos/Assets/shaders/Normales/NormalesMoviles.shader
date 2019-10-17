Shader "Custom/NormalesMoviles"
{
     Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Mascara("Mascara", 2D) = "white" {}
        _Textue("Texture", 2D) = "white" {}
		_Normales("normales", 2D) ="white" {}
		_ValorX("ValorX ",Range(-1,1)) = 0.5
		_ValorY("ValorY",Range(-1,1)) = 0.5
		_Profundida("Profundo", Range(0,2)) = 0.5
		_frecuencia("Frecuencia",Range(-10,10)) = 0.5
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
        sampler2D _Textue;
        sampler2D _Normales;
        sampler2D _Mascara;
		float _ValorX;
		float _ValorY;
		float _Profundida;
		float _frecuencia;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Mascara;
            float2 uv_Normales;
            float2 uv_Textue;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        { // se toma encuenta todo lo anterior en los shader de movimientos anteriores para aplicarlos unicamente a un textura con el normalmap
			float2 UvMov = IN.uv_Textue;
			float distanciaX = sin(_ValorX * _Time.y)/_frecuencia;
			float distanciaY = sin(_ValorY * _Time.y)/_frecuencia;
			UvMov += float2(distanciaX,distanciaY);
			float4 m = tex2D (_Mascara, IN.uv_Mascara);
            float4 m1 = tex2D (_Textue,IN.uv_Textue);
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			float3 R = m.rgb * m1;
			float3 k = c *(1 - m);

			float3 t = k + R;
			o.Albedo = t.rgb;
			float4 n = tex2D(_Normales,UvMov);
			float3 norman = UnpackNormal(n* _Profundida).rgb;
			float3 s = norman;
			o.Normal = normalize(s);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
