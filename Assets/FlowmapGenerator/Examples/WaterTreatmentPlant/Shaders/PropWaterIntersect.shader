Shader "FlowmapGenerator/WaterTreatmentPlant/PropWaterIntersect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpTex ("Normal", 2D) = "bump" {}
		_Color ("Color", Color) = (1,1,1,1)
		_DirtColor ("Dirt Color", Color) = (1,1,1,1)
		_WaterHeight ("Water Height", Float) = 0
		_DirtFade ("Dirt Fade", Float) = 0.25
		_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
		_Shininess ("Shininess", Range (0.03, 1)) = 1
		_SpecCube ("Specular Cubemap", Cube) = "" {} 
		_FresnelBias ("Fresnel Bias", Float) = 0.05
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong nolightmap
		#pragma target 3.0 
		#pragma glsl

		sampler2D _MainTex, _BumpTex;
		float4 _Color, _DirtColor;
		float _Shininess, _FresnelBias, _WaterHeight, _DirtFade; 
		samplerCUBE _SpecCube;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;  
			float3 worldRefl;
			float3 worldPos;
			INTERNAL_DATA
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 mainDiffuse = tex2D (_MainTex, IN.uv_MainTex);			
			float dirtFade = 1-smoothstep(_WaterHeight, _WaterHeight + _DirtFade, IN.worldPos.y);
			
			o.Albedo = mainDiffuse.rgb * lerp(float3(1,1,1), _DirtColor.rgb, dirtFade * _DirtColor.a) * _Color.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpTex, IN.uv_MainTex));
			
			half fresnel = lerp(pow(1.001 - dot(normalize(IN.viewDir), o.Normal), 5), 1, _FresnelBias);
			
			o.Alpha = 1; 
			o.Gloss = saturate(_SpecColor.rgb * lerp (1, 2, dirtFade)) * fresnel;
			o.Specular = lerp (_Shininess, 0.7, dirtFade);
//			o.Emission = texCUBElod (_SpecCube, float4(WorldReflectionVector (IN, o.Normal), floor((1-o.Specular) * 7))).rgb * o.Gloss;
		}
		ENDCG
	} 
	FallBack "Specular"
}
