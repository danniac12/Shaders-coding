// Directorio /Nombre del shader
Shader "Custom/Base/Distance" {

	// Variables disponibles en el inspector (Propiedades)
	Properties { 
		
		_ColorSecundario ("Color Secundario", Color) = (0.25,.75,.75,1)
		_ColorPrimario("Color Primario",Color) =(1,1,1,1)
		_ColorEmi("Colo Emi",color)=(1,1,1,1)
		_Valor("Valor",Range(0,10)) = 5

	}

	// Primer subshader
	SubShader { 
		LOD 200
		
		CGPROGRAM
		// Método para el cálculo de la luz
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		// Declaración de variables
	
		float4 _ColorSecundario;
		float4 _ColorPrimario;
		float _Valor;
		float4 _ColorEmi;
		

		// Información adicional provista por el juego
		struct Input {
			float2 uv_MainTex;
		};

		// Nucleo del programa
		void surf (Input IN, inout SurfaceOutputStandard o) {

		   
		    half3 e = _ColorEmi;		
			float4 a = _ColorPrimario;
			float4 c = _ColorSecundario;
		   	float4 total = ((a/ _Valor) + (c /(-_Valor +10) ));
			o.Albedo = total.rgb;
			o.Emission = e;
			 
			

		}
		ENDCG

	}// Final del primer subshader

	// Segundo subshader si existe alguno
	// Tercer subshader...

	// Si no es posible ejecutar ningún subshader ejecute Diffuse
	FallBack "Diffuse"
}