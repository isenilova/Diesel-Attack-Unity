Shader "Hidden/DepthToHeight" {
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert 
		float _HeightmapRenderDepthMin;
		float _HeightmapRenderDepthMax;

		struct Input {
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {		
			o.Albedo = 0;
			o.Alpha = 1;
			o.Emission = saturate((_HeightmapRenderDepthMax - IN.worldPos.y) / (_HeightmapRenderDepthMax - _HeightmapRenderDepthMin));
		}
		ENDCG
	} 
	FallBack off
}
