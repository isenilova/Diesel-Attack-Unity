Shader "Hidden/ForceFieldPreview" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_VectorPreviewTex ("Base (RGB)", 2D) = "black" {}
	}
	SubShader {
	
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "Offscreen"="Force"}
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		ZTest GEqual
		Offset 0, -5
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		sampler2D _VectorTex;
		sampler2D _VectorPreviewTex;
		float _Strength;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 alpha = tex2D (_MainTex, IN.uv_MainTex);
			half4 direction = tex2D (_VectorTex, IN.uv_MainTex);
			half4 directionPreview = tex2D (_VectorPreviewTex, IN.uv_MainTex);
			o.Albedo = 0;
			o.Emission = 1;
			o.Alpha = saturate((alpha.r * 0.9 + smoothstep(0.45, 0.5, directionPreview.g))) * 0.1;
		}
		ENDCG
		
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "Offscreen"="Force"}
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		ZTest LEqual
		Offset 0, -5
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		sampler2D _VectorTex;
		sampler2D _VectorPreviewTex;
		float _Strength;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 alpha = tex2D (_MainTex, IN.uv_MainTex);
			half4 direction = tex2D (_VectorTex, IN.uv_MainTex);
			half4 directionPreview = tex2D (_VectorPreviewTex, IN.uv_MainTex);
			o.Albedo = 0;
			o.Emission = 1;
			o.Alpha = saturate((alpha.r * 0.9 + smoothstep(0.45, 0.5, directionPreview.g)));
		}
		ENDCG
	} 
	FallBack "Transparent/VertexLit"
}
