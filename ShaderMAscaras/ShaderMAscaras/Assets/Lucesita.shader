Shader "Custom/Lucesita"
{
    Properties
    {
	//Declaramos  los colores y el material 
        _Color ("Color", Color) = (1,1,1,1)
		_Color1 ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Example

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
		//Los declaramos de manera interna los elementos de las propiedades
        sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _Color1;

        struct Input
        {
		    //Accedemos y lo convertimos a uv.maping 
            float2 uv_MainTex;
        };

     
	     
  

        void surf (Input IN, inout SurfaceOutput o)
        {
		 

		 //Le asignamos el albedo por defecto 
          o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;



        } 


		float4 LightingExample(SurfaceOutput s, float3 lightDir, float3 atten){

		//Hacemos el valor punto entre estos 2 valores
		
		float Ndotl = (dot(s.Normal, lightDir));
		float4 light;

		//Convertimos el color del objeto a azul completo

		_Color1.r *= 0.1;
		_Color1.g *= 0.1;
		_Color1.b *= 0.10;

		//Accedemos al calculo de la luz con la atenuacion 
		light.rgb = s.Albedo * _Color1 * (atten * Ndotl);

		light.a = s.Alpha;



		return light;



		}
        ENDCG
    }
    FallBack "Diffuse"
}
