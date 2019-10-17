Shader "Custom/Semaforo"
{
   Properties { 
		_ColorVerde ("Color Verde", Color) = (1,1,1,1)
		_ColorAmarillo ("Color Amarillo", Color) = (1,1,1,1)
		_ColorRojo ("Color Rojo", Color) = (1,1,1,1)
		_Valor("Valor",Range(1,29)) = 10

	}

	// Primer subshader
	SubShader { 
		LOD 200
		
		CGPROGRAM
		// Método para el cálculo de la luz
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		// Declaración de variables
		float4 _ColorVerde;
		float4 _ColorAmarillo;
		float4 _ColorRojo;
		float _Valor;

		// Información adicional provista por el juego
		struct Input {
			float2 uv_MainTex;
		};

		// Nucleo del programa
		void surf (Input IN, inout SurfaceOutputStandard o) {
			//float3 x = float3(1, 1, 1);
			//float4 y = float4(x,1);

			if(_Valor >=1  && _Valor < 10){
				
				float4 c = ((_ColorVerde/ _Valor) + (_ColorAmarillo/ (-_Valor +10)));
			o.Albedo = c.rgb;
			}
			else if(_Valor >= 10 && _Valor <=20){
				float4 c = ((_ColorAmarillo/ (_Valor - 10)) + (_ColorRojo/ (-_Valor +20)));
			o.Albedo = c.rgb;
			}
			else if(_Valor >= 20 && _Valor <=30){
				float4 c = ((_ColorRojo/ (_Valor - 20)) + (_ColorVerde/ (-_Valor +30)));
			o.Albedo = c.rgb;
			}
		}
		ENDCG

	}// Final del primer subshader

	// Segundo subshader si existe alguno
	// Tercer subshader...

	// Si no es posible ejecutar ningún subshader ejecute Diffuse
	FallBack "Diffuse"
}
