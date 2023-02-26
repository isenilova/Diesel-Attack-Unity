Shader "FlowmapGenerator/Opaque/Water Foam" {
	Properties {
		_Color ("Diffuse Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Diffuse", 2D) = "white" {} 
		_BumpTex ("Normal", 2D) = "bump" {} 		
		_NormalTiling ("Normal Tiling", Vector) = (1,1,0,0)
		_FlowmapTex ("Flowmap", 2D) = "white" {}
		_FlowSpeed ("Flow Speed", Float) = 1
		_NoiseTex ("Noise", 2D) = "white" {}
		_NoiseTiling ("Noise Tiling", Vector) = (1,1,0,0)
		_NoiseScale ("Noise Scale", Float) = 1 
		_AnimLength ("Animation Length", Float) = 8		
		_SpecColor ("Specular", Color) = (0.5,0.5,0.5,1)
		_Shininess ("Shininess", Range(0.01, 10)) = 0.5
		_Cube ("Reflection Cubemap", Cube) = "white" {}
		_FoamTex ("Tiling Foam (R=Diffuse, G=Mask)", 2D) = "white" {}
		_FoamColor ("Foam Color", Color) = (1,1,1,1)
		_FoamTiling ("Foam Tiling", Float) = 1
		_FoamSpeed ("Foam Speed", Float) = 1 
	}
	
	SubShader {
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque"}
		LOD 200
		
		CGPROGRAM
		#pragma target 3.0
		#pragma glsl
		#include "FlowmapCommon.cginc"
		#pragma surface surf BlinnPhong

		sampler2D _MainTex, _BumpTex;
		sampler2D _FlowmapTex;
		sampler2D _NoiseTex; 
		float4 _Color;
		float4 _FlowmapUV;
		float _FlowSpeed;
		float _NoiseScale; 
		float _AnimLength;
		float _Shininess;
		float2 _NormalTiling, _NoiseTiling;
		samplerCUBE _Cube;
		sampler2D _FoamTex;
		float4 _FoamColor;
		float _FoamTiling, _FoamSpeed;
		
		struct Input {
			float2 uv_FlowmapTex;
			float3 worldRefl;
			float3 worldPos;
			float3 viewDir;
			INTERNAL_DATA
		};
						
		void surf (Input IN, inout SurfaceOutput o) {
			float2 flowUV = IN.uv_FlowmapTex;
			float flowPhase0, flowPhase1, flowLerp;
			float2 flowDir;
			float4 flowmap;
			
			GetFlowmapValues (flowUV, _FlowmapTex, _NoiseTex, _NoiseScale, _NoiseTiling, _FlowSpeed, _AnimLength, flowDir, flowmap, flowPhase0, flowPhase1, flowLerp);
			o.Normal = GetFlowmapNormalsVelocityScaled (flowUV, _BumpTex, _NormalTiling, flowDir, flowmap, flowPhase0, flowPhase1, flowLerp);
			float4 foam = GetFoam (flowUV, flowmap, flowDir, flowPhase0, flowPhase1, flowLerp, _FoamTex, _FoamColor, _FoamTiling, _FoamSpeed, o.Normal);
			o.Albedo = lerp(tex2D (_MainTex, flowUV).rgb * _Color.rgb, foam.rgb, foam.a);
			
//			make sure to remove spec where the foam is
			SpecCube (o, _SpecColor * (1-foam.a), IN.viewDir, WorldReflectionVector(IN, o.Normal), _Shininess, _Cube);
		}
		ENDCG
	} 
	FallBack "Specular"
}
