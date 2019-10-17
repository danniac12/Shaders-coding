Shader "Custom/TexturaMoviendo"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ValorX("ValorX ",Range(-1,1)) = 0.5
		_ValorY("ValorY",Range(-1,1)) = 0.5
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
		float _ValorX;
		float _ValorY;

        struct Input
        {
            float2 uv_MainTex;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			//se utiliza lo visto en las diapositivas si le agregar slider para dar versatibilidad al desplazamiento
			float2 UvMov = IN.uv_MainTex;
			float distanciaX = (_ValorX * _Time.y);
			float distanciaY = (_ValorY * _Time.y);
			UvMov += float2(distanciaX,distanciaY);
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, UvMov);

            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
