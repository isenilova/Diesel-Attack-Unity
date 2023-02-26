Shader "Hidden/DepthToHeightClipped" {
	SubShader {
		Tags { "RenderType"="Opaque" }
		Cull Front
//		ZTest LEqual
		
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
			
			if(IN.worldPos.y>_HeightmapRenderDepthMin)
				o.Emission = 0;
			else
				o.Emission = 1;
				// saturate((4 - (-1*(IN.worldPos.y))) / (4));
		}
		ENDCG
	} 
	FallBack off
}
