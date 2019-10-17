Shader "Custom/Global"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Color2 ("Color 2", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
       _Position("Position",vector)=(0,0,0)
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
 
        fixed4 _Color;
        fixed4 _Color2;

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

			if(IN.worldPos.x < _Position.x+3 && IN.worldPos.x > _Position.x-3 && IN.worldPos.z < _Position.z+3 && IN.worldPos.z > _Position.z-3){

				if (IN.worldPos.x < _Position.x + 1 && IN.worldPos.x > _Position.x - 1 && IN.worldPos.z < _Position.z + 1 && IN.worldPos.z > _Position.z - 1) {
					c.r *= (1-	_Color2.r);
					c.g *=(1- _Color2.g);
					c.b *= (1-_Color2.b);
				
				}
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
