Shader "FlowmapGenerator/Preview/Flowmap" {
	Properties {
		_FlowmapTex ("Flowmap", 2D) = "gray" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _FlowmapTex;

		struct Input {
			float2 uv_FlowmapTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_FlowmapTex, IN.uv_FlowmapTex);
			o.Albedo = 0;
			o.Emission = c.rgb;
			o.Alpha = 1;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
