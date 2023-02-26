Shader "FlowmapGenerator/Opaque/Solid" {
	Properties {
		_Color ("Diffuse Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Diffuse", 2D) = "white" {} 
		_DiffuseTex ("Tiling Diffuse", 2D) = "white" {} 
		_BumpTex ("Tiling Normal", 2D) = "bump" {} 		
		_Tiling ("Tiling", Vector) = (1,1,0,0)
		_FlowmapTex ("Flowmap", 2D) = "white" {}
		_FlowSpeed ("Flow Speed", Float) = 1
		_NoiseTex ("Noise", 2D) = "white" {}
		_NoiseTiling ("Noise Tiling", Vector) = (1,1,0,0)
		_NoiseScale ("Noise Scale", Float) = 1 
		_AnimLength ("Animation Length", Float) = 8
		_Cube ("Reflection Cubemap", Cube) = "white" {}
		_SpecColor ("Specular", Color) = (0.5,0.5,0.5,1)
		_Shininess ("Shininess", Range(0.01, 10)) = 0.5
	}
	
	SubShader {
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque"}
		LOD 200
		
		CGPROGRAM
		#include "FlowmapCommon.cginc"
		#pragma target 3.0
		#pragma glsl		
		#pragma surface surf BlinnPhong
		
		sampler2D _MainTex, _DiffuseTex, _BumpTex;
		sampler2D _FlowmapTex;
		sampler2D _NoiseTex; 
		float4 _Color;
		float4 _FlowmapUV;
		float _FlowSpeed;
		float _NoiseScale; 
		float _AnimLength;
		float _Shininess;
		float2 _Tiling, _NoiseTiling;
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
			half3 diffuse = GetFlowmapDiffuse (flowUV, _DiffuseTex, _Tiling, flowDir, flowPhase0, flowPhase1, flowLerp);

			o.Normal = GetFlowmapNormalsOffsetPhase (flowUV, _BumpTex, _Tiling, flowDir, flowmap, flowPhase0, flowPhase1, flowLerp);
			o.Albedo = tex2D (_MainTex, flowUV).rgb * diffuse * _Color.rgb;					
			SpecCube (o, o.Albedo * _SpecColor, IN.viewDir, WorldReflectionVector(IN, o.Normal), _Shininess, _Cube);
		}
		ENDCG
	} 
	FallBack "Specular"
}
