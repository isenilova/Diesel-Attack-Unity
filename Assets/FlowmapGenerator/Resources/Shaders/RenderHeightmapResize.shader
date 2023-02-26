Shader "Hidden/RenderHeightmapResize" {
	SubShader {
	
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _Heightmap;
			float2 _AspectRatio;
			
			float4 frag (v2f_img i) : COLOR
			{	
				return tex2D (_Heightmap, ((i.uv - 0.5) * _AspectRatio) + 0.5);
			}
			ENDCG
		}
	}
	FallBack off
}
