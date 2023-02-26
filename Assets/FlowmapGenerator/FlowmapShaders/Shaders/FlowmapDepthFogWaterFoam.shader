Shader "FlowmapGenerator/Depth Fog/Water Foam" {
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
		_FoamTex ("Foam (Diffuse R, Mask G)", 2D) = "white" {}
		_FoamColor ("Foam Color", Color) = (1,1,1,1)
		_FoamTiling ("Foam Tiling", Float) = 1
		_FoamSpeed ("Foam Speed", Float) = 1 
		_EdgeFadeDepth ("Edge Fade Depth", float) = 1
		_EdgeFadePower ("Edge Fade Power", float) = 1
		_FogStart ("Fog Start", float) = -1
		_FogEnd ("Fog End", float) = 4
		_FogColor ("Fog Color", Color) = (0.5,0.5,0.5,1)
		_RefractionDistortion ("Refraction Distortion", float) = 0.01
	}
	
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent"}
		LOD 200
		ZWrite Off
		
		GrabPass { "_GrabPassTexture" }		
		
		CGPROGRAM
		#include "FlowmapCommon.cginc"
		#pragma target 3.0
		#pragma glsl
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
		sampler2D _CameraDepthTexture;
		float _EdgeFadeDepth, _EdgeFadePower;
		sampler2D _FoamTex;
		float4 _FoamColor;
		float _FoamTiling, _FoamSpeed;
		float _FogStart, _FogEnd;
		float4 _FogColor;
		float _RefractionDistortion;
		sampler2D _GrabPassTexture;
		
		struct Input {
			float2 uv_FlowmapTex;
			float2 uv_BumpTex;
			float3 worldRefl;
			float3 worldPos;
			float3 viewDir;
			float4 screenPos;
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
			o.Albedo = tex2D(_MainTex, flowUV).rgb * _Color.rgb;
			o.Albedo = lerp(o.Albedo, foam.rgb, foam.a);
						
			float3 background;
			float fog;
			float2 screenUV = GetScreenUV(IN.screenPos);
			float sceneZ =  GetSceneZ (screenUV, _CameraDepthTexture);
			float edgeFade = GetEdgeFade (IN.screenPos, sceneZ, _EdgeFadeDepth, _EdgeFadePower);
			DepthFogLinear (o, IN.screenPos, sceneZ, screenUV, _GrabPassTexture, 
				edgeFade, _FogStart, _FogEnd, _FogColor, _RefractionDistortion, fog, background);
			half backgroundLerp = fog * edgeFade;
			
			o.Albedo *= tex2D (_MainTex, flowUV).rgb * backgroundLerp;
			SpecCube (o, _SpecColor * (1-foam.a) * backgroundLerp, IN.viewDir, WorldReflectionVector(IN, o.Normal), _Shininess, _Cube);
			o.Emission = background + o.Emission;
			o.Alpha = 1;		
		}
		ENDCG
	} 
	FallBack "Specular"
}
