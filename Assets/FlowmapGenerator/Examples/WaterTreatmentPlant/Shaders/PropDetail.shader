Shader "FlowmapGenerator/WaterTreatmentPlant/PropDetail" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpTex ("Normal", 2D) = "bump" {}
		_Color ("Color", Color) = (1,1,1,1)
		_DetailTex ("Detail", 2D) = "gray" {}
		_DetailNormalTex ("Normal", 2D) = "bump" {} 
		_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
		_Shininess ("Shininess", Range (0.03, 1)) = 1
		_SpecCube ("Specular Cubemap", Cube) = "" {} 
		_FresnelBias ("Fresnel Bias", Float) = 0.05
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0 
		#pragma glsl

		sampler2D _MainTex, _BumpTex, _DetailTex, _DetailNormalTex;
		float4 _Color;
		float _Shininess, _FresnelBias; 
		samplerCUBE _SpecCube;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DetailTex; 
			float3 viewDir;  
			float3 worldRefl;
			float3 worldNormal;
			INTERNAL_DATA
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 mainDiffuse = tex2D (_MainTex, IN.uv_MainTex);
			half4 detailDiffuse = tex2D (_DetailTex, IN.uv_DetailTex);
			
			half3 mainNormal = UnpackNormal (tex2D (_BumpTex, IN.uv_MainTex));
			half3 detailNormal = UnpackNormal (tex2D (_DetailNormalTex, IN.uv_DetailTex));
			
			o.Albedo = mainDiffuse.rgb * _Color.rgb * detailDiffuse.r * 2;
			o.Normal = normalize(float3(mainNormal.xy + detailNormal.xy, mainNormal.z));
			
			half fresnel = lerp(pow(1.001 - dot(normalize(IN.viewDir), o.Normal), 5), 1, _FresnelBias);
			
			o.Alpha = 1; 
			o.Gloss = _SpecColor.rgb * fresnel * detailDiffuse.g * 3;
			o.Specular = _Shininess; 
			o.Emission = texCUBElod (_SpecCube, float4(WorldReflectionVector (IN, o.Normal), floor((1-_Shininess) * 9))).rgb * o.Gloss;
		}
		ENDCG
	} 
	FallBack "Specular"
}
