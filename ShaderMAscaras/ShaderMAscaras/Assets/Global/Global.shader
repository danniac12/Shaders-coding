Shader "Custom/Global"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
       
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
       _Position("Position",vector)=(0,0,0)
       _Rango("Rango",Range(0,5)) = 0
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
			float3 worldPos;
        };
		float3 _Position;           
		float _Rango;
        fixed4 _Color;
       

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
		  float distancia = distance(_Position,IN.worldPos);
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			//condiciona el rango en el que se muestra el area pintada en el material
			if(IN.worldPos.x < _Position.x+ _Rango && IN.worldPos.x > _Position.x- _Rango && IN.worldPos.z < _Position.z+ _Rango && IN.worldPos.z > _Position.z- _Rango){

				
				c.r *= 2;
				c.g *= 0.5;
				c.b *= 0.5;
			}
			
            o.Albedo = c.rgb;         
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
