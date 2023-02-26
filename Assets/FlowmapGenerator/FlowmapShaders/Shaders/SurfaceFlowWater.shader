Shader "FlowmapGenerator/Surface Flow/Water" {
	Properties {
		_MainTex ("Surface Diffuse", 2D) = "white" {}
		_BumpTex ("Surface Normal", 2D) = "bump" {} 
		_SpecColor ("Surface Specular", Color) = (0.5,0.5,0.5,1)
		_Shininess ("Surface Shininess", Range(0.01, 10)) = 0.5
		
		_Color ("Flow Diffuse Color", Color) = (0.5,0.5,0.5,1)
		_FlowMainTex ("Flow Diffuse", 2D) = "white" {}
		_FlowBumpTex ("Flow Normal", 2D) = "bump" {} 		
		_NormalTiling ("Flow Normal Tiling", Vector) = (1,1,0,0)
		_FlowmapTex ("Flowmap", 2D) = "black" {}
		_FlowSpeed ("Flow Speed", float) = 1
		_NoiseTex ("Noise", 2D) = "white" {}
		_NoiseTiling ("Noise Tiling", Vector) = (1,1,0,0)
		_NoiseScale ("Noise Scale", float) = 1 
		_AnimLength ("Animation Length", float) = 8
		
		_FlowSpecColor ("Flow Specular", Color) = (0.5,0.5,0.5,1)
		_FlowShininess ("Flow Shininess", Range(0.01, 10)) = 0.5
		
		_Cube ("Reflection Cubemap", Cube) = "white" {}		
	}
	
	SubShader {
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque"}
		LOD 200
		
		CGPROGRAM
		#include "FlowmapCommon.cginc"
		#pragma target 3.0
		#pragma glsl		
		#pragma surface surf BlinnPhong
		
		sampler2D _MainTex, _BumpTex;
		sampler2D _FlowMainTex, _FlowBumpTex;
		sampler2D _FlowmapTex;
		sampler2D _NoiseTex; 
		float4 _Color;
		float4 _FlowmapUV;
		float _FlowSpeed;
		float _NoiseScale; 
		float _AnimLength;
		float _Shininess, _FlowShininess;
		float4 _FlowSpecColor;
		float2 _NormalTiling, _NoiseTiling;
		samplerCUBE _Cube;
		
		struct Input {
			float2 uv_FlowmapTex;
			float2 uv_MainTex;
			float2 uv_BumpTex;
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
			o.Normal = GetFlowmapNormalsVelocityScaled (flowUV, _FlowBumpTex, _NormalTiling, flowDir, flowmap, flowPhase0, flowPhase1, flowLerp);
			
			float flowAlpha = smoothstep(0.01, 0.1, flowmap.a) * _Color.a;
			
			o.Albedo = tex2D (_FlowMainTex, flowUV).rgb * _Color.rgb;			
			SpecCube (o, _FlowSpecColor * flowAlpha, IN.viewDir, WorldReflectionVector(IN, o.Normal), _FlowShininess, _Cube);
			
			o.Albedo = lerp(tex2D (_MainTex, IN.uv_MainTex).rgb, o.Albedo, flowAlpha);
			o.Normal = lerp(UnpackNormal(tex2D (_BumpTex, IN.uv_BumpTex)), o.Normal, flowAlpha);
		}
		ENDCG
	} 
	FallBack "Specular"
}
