Shader "FlowmapGenerator/WaterTreatmentPlant/WaterCubemap" {
	Properties {
		_FlowmapTex ("Flowmap", 2D) = "white" {}
		_FlowScale ("Flow Speed", Float) = 1
		_AnimLength ("Animation Length", Float) = 8
		
		_NormalTex ("Water Normal", 2D) = "bump" {}
		_NormalTiling ("Normal Tiling", Float) = 1
		
		_NoiseTex ("Noise", 2D) = "white" {}
		_NoiseTiling ("Noise Tiling", Float) = 1
		_NoiseScale ("Noise Scale", Float) = 1
		
		_HeightTex ("Water Depth", 2D) = "white" {}
		
		_ShallowColor ("Shallow Color", Color) = (1,1,1,1)
		_DeepColor ("Deep Color", Color) = (1,1,1,1)
		_SpecColor ("Specular", Color) = (0.5,0.5,0.5,1)
		_Shininess ("Shininess", Range(0.01, 10)) = 0.5
		_EnvMap ("Envmap", CUBE) = "" {}		
		
		_FoamTex ("Foam", 2D) = "white" {}
		_FoamTiling ("Foam Tiling", Float) = 1
		_FoamSpeed ("Foam Speed", Float) = 1 
		_FoamColor ("Foam Color", Color) = (1,1,1,1)
	}
	
	SubShader {
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque"}
		LOD 200
		
		CGPROGRAM
		#pragma target 3.0
		#pragma glsl
		#pragma surface surf BlinnPhong nolightmap addshadow

		sampler2D _DiffuseTex;
		sampler2D _FoamTex;
		sampler2D _HeightTex;
		float4 _DeepColor;
		float4 _ShallowColor;
		float4 _FoamColor;
		sampler2D _FlowmapTex;
		float4 _FlowmapUV;
		sampler2D _NormalTex;
		sampler2D _NoiseTex;
		samplerCUBE _EnvMap;
		float _FlowScale;
		float _NoiseScale;
		float _NoiseTiling;
		float _NormalTiling;
		float _FoamTiling;
		float _FoamSpeed; 
		float _Shininess;
		float _AnimLength; 
		float _EditorTime;
		
		struct Input {
			float2 uv_DiffuseTex;
			float3 viewDir;
			float3 worldRefl;
			float4 screenPos;
			float3 worldPos;
			INTERNAL_DATA
		};

		void surf (Input IN, inout SurfaceOutput o) { 
//			calculate UV from flowmap generator
			float2 flowUV = IN.worldPos.xz / _FlowmapUV.xy + 0.5 - _FlowmapUV.zw / _FlowmapUV.xy;
//			calculate flow phases
			half phaseOffset = tex2D (_NoiseTex, flowUV * _NoiseTiling).r;
			float4 flowTex = tex2D (_FlowmapTex, flowUV);
			half2 flow = (flowTex.xy * 2 - 1) * -_FlowScale;			
			float FlowPhase0 = frac(_NoiseScale * phaseOffset * 0.5 + (_Time.y + _EditorTime) / _AnimLength);
			float FlowPhase1 = frac(_NoiseScale * phaseOffset * 0.5 + (_Time.y + _EditorTime) / _AnimLength + 0.5);
			float FlowLerp = abs(0.5 - FlowPhase0) / 0.5; 
			
//			small non-directional detail ripples
			half3 normalTex0 = UnpackNormal(tex2D (_NormalTex, 1.5 * _NormalTiling * flowUV + (_Time.y + _EditorTime) * float2(-0.075, -0.053)));
			half3 normalTex1 = UnpackNormal(tex2D (_NormalTex, 1.5 * _NormalTiling * flowUV * float2(1,1) + (_Time.y + _EditorTime) * float2(0.083, -0.031)));
			half3 detailNormal = normalize(float3(normalTex0.xy + normalTex1.xy, normalTex0.z)) * max(1-length(flow), 0.5); 
			o.Normal = detailNormal;
			
//			flow normal map
			normalTex0 = UnpackNormal(tex2D (_NormalTex, _NormalTiling * flowUV + flow * FlowPhase0 + o.Normal * 0.01));
			normalTex1 = UnpackNormal(tex2D (_NormalTex, _NormalTiling * flowUV + flow * FlowPhase1 + o.Normal * 0.01));
			detailNormal = lerp(normalTex0, normalTex1, FlowLerp); 
			o.Normal = normalize (half3(o.Normal.xy * 0.25 + detailNormal.xy * length(flow), detailNormal.z));
						
			half4 foamTex0 = tex2D(_FoamTex, _FoamTiling * flowUV + flow * (FlowPhase0 * 2 - 1) * _FoamSpeed + o.Normal * 0.005);
			half4 foamTex1 = tex2D(_FoamTex, _FoamTiling * flowUV + flow * (FlowPhase1 * 2 - 1) * _FoamSpeed + o.Normal * 0.005 + 0.5);
			half4 foam = lerp(foamTex0, foamTex1, FlowLerp);
			foam.a = foam.g * _FoamColor.a;
			foam.rgb = foam.r * _FoamColor.rgb;
			foam.a *= max(0.05, pow(saturate(dot(o.Normal, float3(0,0,1)) * 1), 2)); 
			foam.a *= flowTex.b;
			foam.a = smoothstep(0.0, 0.2, foam.a);  
			
			half fresnel = lerp(pow(1.001 - dot(normalize(IN.viewDir), o.Normal), 5), 1, 0.05);
			
			half4 heightTex = tex2D(_HeightTex, flowUV);
			half3 diffuse = lerp(_DeepColor.rgb, _ShallowColor.rgb, heightTex.r);
			diffuse = lerp(diffuse, foam.rgb, foam.a);			
			half3 envmap = texCUBE (_EnvMap, WorldReflectionVector (IN, o.Normal)).rgb; 
						 			
			o.Albedo = diffuse;
			o.Gloss = _SpecColor.rgb * lerp(1, 0.05, foam.a);
			o.Emission = envmap * o.Gloss * fresnel;
			o.Specular = lerp(_Shininess, _Shininess * 0.1, foam.a);
			o.Alpha = 1; 
		}
		ENDCG 
		 
	} 
	
	FallBack "Diffuse"
}
