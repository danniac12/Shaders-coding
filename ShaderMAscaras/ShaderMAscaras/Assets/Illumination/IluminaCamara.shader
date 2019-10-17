Shader "Custom/IluminaCamara"
{
  
    Properties
    {
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

        sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _Color1;

        struct Input
        {
            float2 uv_MainTex;
        };

     

  

        void surf (Input IN, inout SurfaceOutput o)
        {
          o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
        } 

		
		float4 LightingExample(SurfaceOutput s, float3 lightDir,float3 miraCamara, float3 atten){
		 // cambia el flotante que recibe el metodo dot para que el objetoreciba iluminacion siempre que este en direccion de la camara
		float Ndotl = (dot(s.Normal, miraCamara));
		float4 light;

	
		light.rgb = s.Albedo * _Color1 * (atten * Ndotl);

		light.a = s.Alpha;



		return light;



		}
        ENDCG
    }
    FallBack "Diffuse"
}
