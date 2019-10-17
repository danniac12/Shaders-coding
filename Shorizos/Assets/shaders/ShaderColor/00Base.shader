// Directorio /Nombre del shader
Shader "Custom/Base/Base" {

	// Variables disponibles en el inspector (Propiedades)
	Properties { 
		_ColorAmbiente ("Color Ambiente", Color) = (1,1,1,1)
		_ColorSituacional ("Color Situacional", Color) = (1,1,1,1)
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
		float4 _ColorSituacional;

		// Información adicional provista por el juego
		struct Input {
			float2 uv_MainTex;
		};

		// Nucleo del programa
		void surf (Input IN, inout SurfaceOutputStandard o) {

			//float3 x = float3(1, 1, 1);
			//float4 y = float4(x,1);

			float4 c = (_ColorAmbiente + _ColorSituacional) / 2;
			o.Albedo = c.rgb;
		}
		ENDCG

	}// Final del primer subshader

	// Segundo subshader si existe alguno
	// Tercer subshader...

	// Si no es posible ejecutar ningún subshader ejecute Diffuse
	FallBack "Diffuse"
}
