Shader "FlowmapGenerator/BlowingSand" {
Properties {
	_SandColor ("Blowing Sand Color", Color) = (1,1,1,1)
	_DiffuseTex ("Global Diffuse", 2D) = "gray" {}
	_SplatMask ("Splat Mask", 2D) = "red" {}
	
	_SplatDiffuse0 ("Layer 0 (R)", 2D) = "white" {}
	_SplatNormal0 ("Layer 0 Normal", 2D) = "bump" {}
	_SplatDiffuse1 ("Layer 1 (G)", 2D) = "white" {}
	_SplatNormal1 ("Layer 1 Normal", 2D) = "bump" {}
 	
 	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_GlitterNormal ("Glitter Normal", 2D) = "bump" {}
	 
	_FlowmapTex ("Flowmap", 2D) = "gray" {}  
	_FlowScale ("Flow Speed", float) = 1
	_NoiseTex ("Noise", 2D) = "gray" {} 
	_NoiseScale ("Noise scale", float) = 1
	_BlowingSandTex ("Blowing Sand", 2D) = "black" {}
	_BlowingSandTiling ("Blowing Sand Tiling", float) = 0.03
	_DistortNormalTex ("Distortion Normal", 2D) = "bump" {}	
}
	
SubShader {
	Tags {"Queue" = "Geometry" "RenderType" = "Opaque"}
	CGPROGRAM
	#pragma surface surf BlinnPhong
	#pragma target 3.0
	#pragma glsl
	
	struct Input { 
		float3 worldNormal;
		float3 viewDir;
		float4 screenPos;
		float3 worldPos;
		INTERNAL_DATA
	};
	
	sampler2D _SplatMask;
	sampler2D _SplatDiffuse0, _SplatNormal0, _SplatDiffuse1, _SplatNormal1;
	sampler2D _FlowmapTex, _DiffuseTex, _NoiseTex, _BlowingSandTex, _GlitterNormal, _DistortNormalTex;
	float4 _SandColor; 
	float _EditorTime, _FlowScale, _NoiseScale, _BlowingSandTiling;  
	half _Shininess;
	 
	inline float LinearDepthFade (float depth, float fadeMin, float fadeMax){
		return saturate((fadeMax-depth)/(fadeMax-fadeMin));
	}
				
	void surf (Input IN, inout SurfaceOutput o) { 
//		world projected uv from 0->1
		float2 worldUv = float2(1,1) * IN.worldPos.xz/300 + 0.5;  
//		world projected tiling uv
		float2 tileUv = float2(-1,-1) * IN.worldPos.xz; 
		
//		mask in tiling diffuse textures
		half4 splat_control = tex2D (_SplatMask, worldUv);
		half3 tilingDiffuse = tex2D (_SplatDiffuse1, tileUv * 0.25).rgb;
		tilingDiffuse = lerp(tilingDiffuse, tilingDiffuse * 2 * tex2D (_SplatDiffuse0, tileUv * 0.25).g, splat_control.r);		
				 
//		load flowmap texture 
		float4 flowTex = tex2D (_FlowmapTex, worldUv); 
//		expand flowmap vectors
		float2 flow = float2((flowTex.xy * 2 - 1) * _FlowScale);		
		float _AnimLength = 6;
//		offset the phase offset with a bit of noise
		half4 noise = tex2D (_NoiseTex, worldUv);
		half phaseOffset = _NoiseScale * noise.g; 
		float flowPhase0 = frac(phaseOffset + (_Time.y + _EditorTime) / _AnimLength);
		float flowPhase1 = frac(phaseOffset + (_Time.y + _EditorTime) / _AnimLength + 0.5);
		float flowLerp = abs(0.5 - flowPhase0) * 2;
		
//		use a scrolling normal map to distort the blowing sand uv
		half2 distortion = UnpackNormal(tex2D(_DistortNormalTex, worldUv * 3 + (_Time.y + _EditorTime) * 0.03)).xy;
//		fade the blowing sand in and out
		half windMask = max(sin(tex2D(_BlowingSandTex, worldUv).b * 6.28 + (_Time.y + _EditorTime)*0.3) * 0.5 + 0.5, 0);
//		use the flowmap to move the blowing sand mask around
		half4 blowingSand = lerp(tex2D(_BlowingSandTex, tileUv * _BlowingSandTiling + (flowPhase0 * 2 - 1) * flow + distortion * 0.04), 
								 tex2D(_BlowingSandTex, tileUv * _BlowingSandTiling + (flowPhase1 * 2 - 1) * flow + distortion * 0.04), flowLerp);
		
//		a mask for the blowing sand, taking into account the Sand Color alpha, the wind mask, and the speed of the flow
		half blowingSandMask = saturate(_SandColor.a * windMask * (blowingSand.r + blowingSand.g * LinearDepthFade(length(IN.viewDir), 30, 100)) * saturate(length(flow * 3)));
//		overlay the global diffuse texture
		half3 globalDiffuse = tex2D(_DiffuseTex, worldUv).rgb;
//		lerp to blowing sand color based on the blowing sand mask
		o.Albedo = lerp(tilingDiffuse * globalDiffuse * 2, _SandColor.rgb * lerp(globalDiffuse * 2, 1, 0.5), blowingSandMask); 
		
//		sparkling sand, masked using a screen space mapped texture
//		offset slightly with time so that it flickers a little bit even when standing still
		half3 glitterNormal = UnpackNormal(tex2D(_GlitterNormal, 3 * (IN.screenPos.xy/IN.screenPos.w) + float2((_Time.y + _EditorTime) * 0.002, 0)));
		half3 glitter = lerp((1.001 - pow(saturate(dot(normalize(IN.viewDir), glitterNormal) * 8), 4)) * 8 * _SpecColor.rgb , 0, blowingSandMask * 2
		 	+ saturate(((tex2D(_SplatDiffuse1, tileUv * 0.2).r * 1.15 - 0.5) * 16) + 0.5));
		
//		lerp from detaul normal to a flat normal where there's blowing sand		  
		o.Normal = float3(0,0,1);
		half4 detailNormal = tex2D(_SplatNormal0, tileUv * 0.25) * splat_control.r + 
		 tex2D(_SplatNormal1, tileUv * 0.1) * splat_control.g + float4(0.5,0.5,1,0.5) * (1-splat_control.r-splat_control.g); 
		o.Normal = lerp(normalize(float3(UnpackNormal(detailNormal).xy + o.Normal.xy, o.Normal.z)), float3(0,0,1), blowingSandMask);
		
//		add glitter in emission
		o.Emission = _SpecColor.rgb * saturate(glitter);
//		add a slight bit of specular
		o.Gloss = (1-blowingSandMask) * _SpecColor.rgb * saturate((tilingDiffuse * 1.25 - 0.5) * 3 + 0.5) * lerp(pow(saturate(dot(o.Normal, normalize(IN.viewDir))), 4), 1, 0.02);
		o.Specular = _Shininess;
	}
	ENDCG  
	}
}
