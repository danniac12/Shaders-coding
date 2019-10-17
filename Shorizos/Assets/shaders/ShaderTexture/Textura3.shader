Shader "Custom/Textura3"
{
     Properties
    {
        _Textura("Textura(RGB)", 2D) = "white"{}
        _Textura2("Textura(RGB)", 2D) = "white"{}
        _Textura3("Textura(RGB)", 2D) = "white"{}
        _Textura4("Textura(RGB)", 2D) = "white"{}
		_Valor("Valor",Range(0,3)) = 0.5
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

        sampler2D _Textura;
		sampler2D _Textura2;
		sampler2D _Textura3;
		sampler2D _Textura4;
		float _Valor;

        struct Input
        {
            float2 uv_Textura;
            float2 uv_Textura2;
            float2 uv_Textura3;
            float2 uv_Textura4;
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
            float4 c = tex2D (_Textura, IN.uv_Textura);
            float4 c2 = tex2D (_Textura2, IN.uv_Textura2);
            float4 c3 = tex2D (_Textura3, IN.uv_Textura3);
            float4 c4 = tex2D (_Textura4, IN.uv_Textura4);
			// se hace un condicional para cambiar de lerp en lerp y se toma valor para restar  para que se mantenga 1
			if(_Valor >= 0 && _Valor <1){
				o.Albedo = lerp(c.rgb,c2.rgb,_Valor);
			}
			else if(_Valor >=1 && _Valor < 2){
				o.Albedo = lerp(c2.rgb,c3.rgb,_Valor-1);
			}
			else if(_Valor >=2 && _Valor <=3){
				o.Albedo = lerp(c3.rgb,c4.rgb,_Valor-2);
			}
            

            // Metallic and smoothness come from slider variables
        }
        ENDCG
    }
    FallBack "Diffuse"
}
