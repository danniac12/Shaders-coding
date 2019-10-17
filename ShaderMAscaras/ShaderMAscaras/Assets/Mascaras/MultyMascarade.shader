Shader "Custom/MultyMascarade"{

    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Textura ("Textura1", 2D) = "white"{}
        _Textura1 ("Textura2", 2D) = "white"{}
        _Textura2 ("Textura3", 2D) = "white"{}
        _Factor ("Factor", Range(0,3)) = 0.0
  
     
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
		sampler2D _Textura;
		sampler2D _Textura1;
		sampler2D _Textura2;
        half  _Factor;
        
       
        fixed4 _Color;

		  struct Input
        {
		    float2 uv_Textura;
		    float2 uv_Textura1;
		    float2 uv_Textura2;
            float2 uv_MainTex;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c1 = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 t1 = tex2D (_Textura, IN.uv_Textura);
			fixed4 t2 = tex2D (_Textura1, IN.uv_Textura);
			fixed4 t3 = tex2D (_Textura2, IN.uv_Textura);

           
			
			
			if(_Factor >=0){
			 o.Albedo = lerp(c1.rgb,t1.rgb,_Factor);			 
			}

			if(_Factor >=1){
			  o.Albedo = lerp(t1.rgb,t2.rgb,_Factor-1);
		    }
		    if(_Factor >=2){
			   o.Albedo = lerp(t2.rgb,t3.rgb,_Factor-2);
		    }
           
		   
            o.Alpha = c1.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
