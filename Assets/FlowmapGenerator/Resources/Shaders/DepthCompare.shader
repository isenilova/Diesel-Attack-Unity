Shader "Hidden/DepthCompare" {
	
	SubShader 
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag 
			#include "UnityCG.cginc"
			
            sampler2D _HeightBelowSurfaceTex;
            sampler2D _HeightIntersectingTex;
            sampler2D _OverhangMaskTex;

			float4 frag (v2f_img i) : COLOR
			{
				float4 belowSurface = tex2D(_HeightBelowSurfaceTex, i.uv);
				float4 intersect = tex2D(_HeightIntersectingTex, i.uv);
				float4 overhang = tex2D(_OverhangMaskTex, i.uv);
				return lerp(belowSurface, intersect, overhang);
			}

			ENDCG
		}
	} 
	FallBack off
}
