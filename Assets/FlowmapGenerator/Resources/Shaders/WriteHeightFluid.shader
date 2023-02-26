Shader "Hidden/WriteHeightFluid" {
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _RenderTex;
			
			float4 frag (v2f_img i) : COLOR
			{	
				float4 mainTex = tex2D(_RenderTex, i.uv);
				
				float4 c;
				c.r = mainTex.r;
				c.g = pow(mainTex.g, 1.0/2.2);
				c.b = 1;
				c.a = 1;				
				return c;
			}
			ENDCG
		}
	}
	FallBack off
}