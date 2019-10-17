Shader "Custom/MovingNormla"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Texture ("Texture", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _NormalTex ("Normal Map", 2D) = "white" {}
        _Factor ("factor", Range(0,10)) = 0.5
        
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

        sampler2D _MainTex, _NormalTex,_Mask,_Texture;

        struct Input
        {
            float2 uv_MainTex, uv_NormalTex,uv_Mask,uv_Texture;
        };

        half _Factor;
       
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {//almacenamos en la variables DistanciaX y DistanciaY la velocidad con la que se movera en cada eje y asignamos los uv del Normal map  la variable mov
			float2 mov = IN.uv_NormalTex ;
		    float DistanciaY = 0.25*_Time.y;
			float DistanciaX = 0*_Time.y;
			//mov se suma a si misma  el flotante con los valores anteriores y es multiplicada por el valor de _Factor
			mov += float2(DistanciaX,DistanciaY)*_Factor;
			// se asignan la textura y mascara 
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 textu = tex2D (_Texture, IN.uv_Texture) * _Color;
			//se realisan las operaciones para dar resultado a la mascara combinada con la textura 
            fixed4 m = tex2D (_Mask, IN.uv_Mask) * _Color;
			fixed4 cambio = c*(1-m);
			fixed4 col = textu * m;
			float4 total  = cambio + col;			
            o.Albedo = total.rgb;              
			//se asignan los valores de los uv del Normal map 
		    fixed4 n = tex2D (_NormalTex, mov) * _Color;
			float3 normal = UnpackNormal(n).rgb;	
			o.Normal = normalize(normal);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
