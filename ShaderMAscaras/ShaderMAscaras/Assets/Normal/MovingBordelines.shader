Shader "Custom/MovingBordelines"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
	   _Factor("Factor",Range(0,10)) = 0
	   _DrawLine("Draw line",Range(0,10)) = 0
   
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

        struct Input
        {
            float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
        };
		float _Factor;
		float _DrawLine;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			//se almacena los valores de cambio en X y Y *tiempo y  se les da un valor  
			
			float DistanciaY = 0.25*_Time.y;
			float DistanciaX = 0 * _Time.y;
			//se aplica la funcion sin()en la variable mov y es multiplicada por el valor guardado de _Time.y y el _Factor
			//mov += sin(mov.y + DistanciaY * _Factor); 
			// se le da esa valiable mov a la funcion dot() dentro de la funcion abs para mopstrar los bordes y dar el efecto 
		    float bord = abs(dot(IN.viewDir , IN.worldNormal))*_DrawLine;
			fixed4 m = tex2D (_MainTex, IN.uv_MainTex);   
			if (bord < 0.35) {
				bord = 0;
			}
			else {
				bord = 1;
			}
			
				bord += sin(bord + DistanciaY );
		
			// se aplica  el cambio de color en los bordes
            fixed4 c =( _Color *(1-bord));
			fixed4 p = bord * m;
			fixed4 u = c + p;						
            o.Albedo = u.rgb;
            // Metallic and smoothness come from slider variables
       
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
