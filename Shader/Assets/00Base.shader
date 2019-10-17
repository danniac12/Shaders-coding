// Directorio /Nombre del shader
Shader "Custom/Base/Promedio" {

	// Variables disponibles en el inspector (Propiedades)
	Properties { 
		_ColorAmbiente ("Color Ambiente", Color) = (1,1,1,1)
		_ColorSecundario ("Color Secundario", Color) = (0.25,.75,.75,1)
	}

	// Primer subshader
	SubShader { 
		LOD 200
		
		CGPROGRAM
		// Método para el cálculo de la luz
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		// Declaración de variables
		float4 _ColorAmbiente;
		float4 _ColorSecundario;

		// Información adicional provista por el juego
		struct Input {
			float2 uv_MainTex;
		};

		// Nucleo del programa
		void surf (Input IN, inout SurfaceOutputStandard o) {

			float3 x = float3(1, 1, 1);
			float4 y = float4(x,1);

			float4 e = _ColorAmbiente;
			float4 c = _ColorSecundario;
			float4 total = (e+c)*0.5;
			o.Albedo = total.rgb;
			

		}
		ENDCG

	}// Final del primer subshader

	// Segundo subshader si existe alguno
	// Tercer subshader...

	// Si no es posible ejecutar ningún subshader ejecute Diffuse
	FallBack "Diffuse"
}
