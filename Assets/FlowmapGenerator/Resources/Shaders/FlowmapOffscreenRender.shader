Shader "Hidden/FlowmapOffscreenRender" {
	
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "Offscreen"="Fluid"}
		Blend One One
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float _Strength;
		float _Renderable;
		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			if(_Renderable == 0){
				discard;
			}
			half4 alpha = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = 0;
			o.Emission = 1;
			o.Alpha = alpha.r * _Strength;
		}
		ENDCG
	}
	
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "Offscreen"="Force"}
		Blend One One
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		sampler2D _VectorTex;
		float _Strength;
		float2 _VectorScale;
		float _VectorInvert;
		float _Renderable;
		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			if(_Renderable == 0){
				discard;
			}
			half4 alpha = tex2D (_MainTex, IN.uv_MainTex);
			float4 forceTex = tex2D (_VectorTex, IN.uv_MainTex);
			float2 direction = float3(forceTex.x * _VectorScale.x, forceTex.y * _VectorScale.y, 0);
			direction = lerp (direction, 1-direction, _VectorInvert);
			o.Albedo = 0;
			o.Emission = float3(direction, forceTex.b);
			o.Alpha = alpha.r * _Strength;
		}
		ENDCG
	} 
	
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "Offscreen"="Heightmap"}
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float _Strength;
		float _Renderable;
		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			if(_Renderable == 0){
				discard;
			}
			half4 alpha = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = 0;
			o.Emission = 1;
			o.Alpha = alpha.r * _Strength;
		}
		ENDCG
	}
	FallBack off
}
