Shader "Hidden/RemoveFluidPreview" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "Offscreen"="Fluid"}
		Blend One One
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		ZTest GEqual
		Offset 0, -10
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float _Strength;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = 0;
			o.Emission = 1;
			o.Alpha = c.r * 0.1;
		}
		ENDCG
		
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "Offscreen"="Fluid"}
		Blend One One
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		ZTest LEqual
		Offset 0, -10
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float _Strength;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = 0;
			o.Emission = 1;
			o.Alpha = c.r;
		}
		ENDCG
	} 
	FallBack "Transparent/VertexLit"
}
