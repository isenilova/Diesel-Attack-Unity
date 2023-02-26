Shader "Hidden/ShallowWaterFlowmapSimulator" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
//	red height, green fluid
//	pass 0 add fluid
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _FluidAddTex;
			sampler2D _FluidRemoveTex;
			float _FluidAddMultiplier;
			float _FluidRemoveMultiplier;
			float _Timestep;
			float _Evaporation;
			
			float4 frag (v2f_img i) : COLOR
			{	
				float4 mainTex = tex2D(_MainTex, i.uv);
				float fluid = mainTex.g;
				float fluidAdd = tex2D(_FluidAddTex, i.uv).r * _FluidAddMultiplier * _Timestep;
				float fluidRemove = tex2D(_FluidRemoveTex, i.uv).r * _FluidRemoveMultiplier * _Timestep;
				
				fluid += fluidAdd;
				fluid = max(0, fluid - fluidRemove);
				fluid = fluid * (1 - _Evaporation * _Timestep);
				return float4(mainTex.r, fluid, mainTex.b, mainTex.a);
			}
			ENDCG
		}
//	pass 1 outflow
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _OutflowTex;
			sampler2D _FluidForceTex;
			float _Timestep;
			float2 _Resolution;
			float _CellSpacing;
			float _Gravity;
			float _FluidForceMultiplier;
			float _BorderCollision;
			
			float4 frag (v2f_img i) : COLOR
			{	 
				float2 delta = float2(1/_Resolution.x, 1/_Resolution.y);
				float2 neighbourN = float2(i.uv.x, (i.uv.y+delta.y));
				float2 neighbourE = float2((i.uv.x+delta.x), i.uv.y); 
				float2 neighbourS = float2(i.uv.x, (i.uv.y-delta.y));
				float2 neighbourW = float2((i.uv.x-delta.x), i.uv.y);				
				
				float4 p = tex2D(_MainTex, i.uv);
				float4 n = tex2D(_MainTex, neighbourN); 
				float4 e = tex2D(_MainTex, neighbourE); 
				float4 s = tex2D(_MainTex, neighbourS); 
				float4 w = tex2D(_MainTex, neighbourW); 
				float4 outflow = tex2D (_OutflowTex, i.uv);
				
//				pass through border edge by setting the height past the border at 0
				if(_BorderCollision == 0){
					if(i.uv.x<=delta.x*1)
						w = 0;
					if(i.uv.x>=1-delta.x*1)
						e = 0;
					if(i.uv.y<=delta.y*1)
						s = 0;
					if(i.uv.y>=1-delta.y*1)
						n = 0;
				}
				
				float4 forceTex = tex2D(_FluidForceTex, i.uv);
//				it seems the default force value is slightly too large, giving a non-zero velocity
				float2 forceDirection = (saturate(forceTex.xy-1.0/255.0*0.5) * 2.0 - 1.0);
				
				float outflowN = max(0, outflow.x + _Timestep * _Gravity * (p.r + p.g - n.r - n.g) + saturate(dot(forceDirection, float2(0,1))) * _FluidForceMultiplier * _Timestep); 
				float outflowE = max(0, outflow.y + _Timestep * _Gravity * (p.r + p.g - e.r - e.g) + saturate(dot(forceDirection, float2(1,0))) * _FluidForceMultiplier * _Timestep);
				float outflowS = max(0, outflow.z + _Timestep * _Gravity * (p.r + p.g - s.r - s.g) + saturate(dot(forceDirection, float2(0,-1))) * _FluidForceMultiplier * _Timestep);
				float outflowW = max(0, outflow.w + _Timestep * _Gravity * (p.r + p.g - w.r - w.g) + saturate(dot(forceDirection, float2(-1,0))) * _FluidForceMultiplier * _Timestep); 
				if(_BorderCollision == 1){
					if(i.uv.x<=delta.x*1)
						outflowE = 0;
					if(i.uv.x>=1-delta.x*1)
						outflowW = 0;
					if(i.uv.y<=delta.y*1)
						outflowN = 0;
					if(i.uv.y>=1-delta.y*1)
						outflowS = 0;
				}
				float outflowK = (outflowN + outflowE + outflowS + outflowW > 0) ? min(1, (p.g) / (_Timestep * (outflowN + outflowE + outflowS + outflowW))) : 0;
				outflowK *= 1-forceTex.b;					
				return float4(outflowN * outflowK, outflowE * outflowK, outflowS * outflowK, outflowW * outflowK);
			}
			ENDCG
		} 
//	pass 2 surface
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _OutflowTex;
			float _Timestep;
			float2 _Resolution;			
			
			float4 frag (v2f_img i) : COLOR
			{	 
				float2 delta = float2(1/_Resolution.x, 1/_Resolution.y);
				float2 neighbourN = float2(i.uv.x, (i.uv.y+delta.y));
				float2 neighbourE = float2((i.uv.x+delta.x), i.uv.y); 
				float2 neighbourS = float2(i.uv.x, (i.uv.y-delta.y));
				float2 neighbourW = float2((i.uv.x-delta.x), i.uv.y);				
				
				float4 p = tex2D(_MainTex, i.uv); 
				float4 outflowCurrent = tex2D(_OutflowTex, i.uv);
				float4 n = tex2D(_OutflowTex, neighbourN); 
				float4 e = tex2D(_OutflowTex, neighbourE); 
				float4 s = tex2D(_OutflowTex, neighbourS); 
				float4 w = tex2D(_OutflowTex, neighbourW); 
				float inflow = n.z + e.w + s.x + w.y;
				float outflow = outflowCurrent.x + outflowCurrent.y + outflowCurrent.z + outflowCurrent.w;
				return float4(p.r, p.g + _Timestep * (inflow - outflow), p.b, p.a);
			}
			ENDCG
		}
//	pass 3 velocity
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _OutflowTex;
			float _Timestep;
			float2 _Resolution;
			float _VelocityScale;
			
			float4 frag (v2f_img i) : COLOR
			{	 
				half4 heightFluid = tex2D(_MainTex, i.uv);
				half4 outflow = tex2D(_OutflowTex, i.uv);
				
				float2 delta = float2(1/_Resolution.x, 1/_Resolution.y);
				float2 neighbourN = float2(i.uv.x, (i.uv.y+delta.y));
				float2 neighbourE = float2((i.uv.x+delta.x), i.uv.y); 
				float2 neighbourS = float2(i.uv.x, (i.uv.y-delta.y));
				float2 neighbourW = float2((i.uv.x-delta.x), i.uv.y);
				
				half4 outflow_north = tex2D(_OutflowTex, neighbourN);
				half4 outflow_east = tex2D(_OutflowTex, neighbourE);
				half4 outflow_south = tex2D(_OutflowTex, neighbourS);
				half4 outflow_west = tex2D(_OutflowTex, neighbourW);
				
				float inflow_north = outflow_north.z;
				float inflow_east = outflow_east.w;
				float inflow_south = outflow_south.x;
				float inflow_west = outflow_west.y;
				
				float flowDelta = _Timestep * ((inflow_north+inflow_east+inflow_south+inflow_west) -
									(outflow.x+outflow.y+outflow.z+outflow.w));				
				float flowDeltaX = 0.5f * (inflow_west-inflow_east + (outflow.y - outflow.w));
				float flowDeltaY = 0.5f * (inflow_south-inflow_north + (outflow.x - outflow.z));
				float fluidHeightDelta = 0.5f * (heightFluid.g + (heightFluid.g+flowDelta));
				float velocityX = 0;
				float velocityY = 0;
				if(fluidHeightDelta!=0){
					velocityX = flowDeltaX / (fluidHeightDelta);
					velocityY = flowDeltaY / (fluidHeightDelta);
				}
				half4 output;
				output = half4(velocityX, velocityY, 0, 1);
				output.x = clamp(output.x * _VelocityScale, -1, 1) * 0.5 + 0.5;
				output.y = clamp(output.y * _VelocityScale, -1, 1) * 0.5 + 0.5;
				output.z = 0;
				output.w = 1;
				return output;
			}
			ENDCG
		}
//		pass 4 fill with color
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			float4 _Color;
			float4 frag (v2f_img i) : COLOR
			{	 
				return _Color;
			}
			ENDCG
		}
//		pass 5 velocity accumulation
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _VelocityTex;
			sampler2D _VelocityAccumTex;
			sampler2D _FluidInputTex;
			float _Delta;
			
			float4 frag (v2f_img i) : COLOR
			{	 
				float4 heightFluid = tex2D(_MainTex, i.uv);
				float4 velocity = tex2D(_VelocityTex, i.uv);
				float4 velocityAccum = tex2D(_VelocityAccumTex, i.uv);
				float4 newVelocity = lerp(velocityAccum, velocity, _Delta);
				newVelocity.z = velocityAccum.z;
				newVelocity.w = 1;
				return newVelocity;
				
			}
			ENDCG
		}
//		pass 6 copy only red channel
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			float4 frag (v2f_img i) : COLOR
			{	 
				return float4(tex2D(_MainTex, i.uv).r, 0,0,0);
			}
			ENDCG
		}
//		pass 7 blur horizontal
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float _BlurSpread;
			float _Strength;
			float2 _Resolution;
			
			float4 frag (v2f_img i) : COLOR
			{	 
				float delta = 1/_Resolution.x;
				float4 center = tex2D(_MainTex, i.uv);
				float4 left = 	tex2D(_MainTex, i.uv + float2(-delta * _BlurSpread, 0));
				float4 right = 	tex2D(_MainTex, i.uv + float2(delta * _BlurSpread, 0));
				return lerp(center, center * 0.5 + left * 0.25 + right * 0.25, _Strength);
			}
			ENDCG
		}
//		pass 8 blur vertical
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float _BlurSpread;
			float _Strength;
			float2 _Resolution;
			
			float4 frag (v2f_img i) : COLOR
			{	 
				float delta = 1/_Resolution.y;
				float4 center = tex2D(_MainTex, i.uv);
				float4 down = 	tex2D(_MainTex, i.uv + float2(0, -delta * _BlurSpread));
				float4 up = 	tex2D(_MainTex, i.uv + float2(0, delta * _BlurSpread));
				return lerp(center, center * 0.5 + down * 0.25 + up * 0.25, _Strength);
			}
			ENDCG
		}
//		pass 9 copy to height
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			sampler2D _NewHeightTex;
			float _IsFloatRGBA;
			
			float4 frag (v2f_img i) : COLOR
			{	 
				float4 originalHeight = tex2D(_MainTex, i.uv);
				float4 newHeightTex = tex2D(_NewHeightTex, i.uv);
				float newHeight = (_IsFloatRGBA == 1 ? DecodeFloatRGBA (newHeightTex) : newHeightTex.r);
				return float4(newHeight, originalHeight.g,originalHeight.b,originalHeight.a);
			}
			ENDCG
		}
//		pass 10 foam accumulation, takes height/fluid as _MainTex
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _VelocityAccumTex;
			sampler2D _FluidAddTex;
			float _Delta; 
			float _FoamVelocityScale; 
			float2 _Resolution;
			
			float4 frag (v2f_img i) : COLOR
			{	  
				float4 velocityAccum = tex2D(_VelocityAccumTex, i.uv);
				float2 velocity = velocityAccum.xy * 2 - 1;
				float4 heightFluid = tex2D(_MainTex, i.uv);
				float fluidAdd = tex2D(_FluidAddTex, i.uv).r;
				float foam = velocityAccum.b;
				
				float2 delta = float2(1/_Resolution.x, 1/_Resolution.y); 
				float2 neighbourN = float2(i.uv.x, (i.uv.y+delta.y));
				float2 neighbourE = float2((i.uv.x+delta.x), i.uv.y); 
				float2 neighbourS = float2(i.uv.x, (i.uv.y-delta.y));
				float2 neighbourW = float2((i.uv.x-delta.x), i.uv.y);				
				
				float2 n = tex2D(_VelocityAccumTex, neighbourN).xy * 2 - 1; 
				float2 e = tex2D(_VelocityAccumTex, neighbourE).xy * 2 - 1; 
				float2 s = tex2D(_VelocityAccumTex, neighbourS).xy * 2 - 1; 
				float2 w = tex2D(_VelocityAccumTex, neighbourW).xy * 2 - 1;   
				float velocityMagnitude = length(velocity);
				float velocityDelta = 100 * ((length(n.xy) - velocityMagnitude) + (length(e.xy) - velocityMagnitude) + (length(s.xy) - velocityMagnitude) + (length(w.xy) - velocityMagnitude));
					
				foam = pow(1-saturate(length(velocity) * _FoamVelocityScale), 2);
				foam *= 1-fluidAdd;
				foam = (saturate((foam * 1.2 - 0.5) * 4) + 0.5) * saturate(velocityDelta);
				velocityAccum.b = lerp (velocityAccum.b, foam, _Delta);
				velocityAccum.a = 1;
				return velocityAccum;
				
			}
			ENDCG
		}
//		pass 11 heightmap fields
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			sampler2D _HeightmapFieldsTex;
			float4 frag (v2f_img i) : COLOR
			{	 
				float4 originalHeight = tex2D(_MainTex, i.uv);
				float heightmapFields = tex2D(_HeightmapFieldsTex, i.uv).r;
				return float4(originalHeight.r + heightmapFields.r, originalHeight.g, originalHeight.b, originalHeight.a);
			}
			ENDCG
		}
//		pass 12 copy foam to new RT
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			
			float4 frag (v2f_img i) : COLOR
			{	 
				float4 velocityAccum = tex2D(_MainTex, i.uv);
				return float4(velocityAccum.bbb, 1);
				
			}
			ENDCG
		}
//		pass 13 copy flowmap without foam to new RT
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			
			float4 frag (v2f_img i) : COLOR
			{	 
				float4 velocityAccum = tex2D(_MainTex, i.uv);
				return float4(velocityAccum.x, velocityAccum.y, 0, 1);
				
			}
			ENDCG
		}
//		pass 14 set initial fluid depth
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float _DeepWater;
			float _FluidAmount;
			
			float4 frag (v2f_img i) : COLOR
			{	 
				float4 heightFluid = tex2D(_MainTex, i.uv);
//				if deep water, no fluid where height is greater than 0, if not deep water just add the same amount of fluid everywhere
				return float4(heightFluid.x, (_DeepWater == 1 ? (1-ceil(heightFluid.x)) * _FluidAmount : _FluidAmount), heightFluid.z, heightFluid.w);
				
			}
			ENDCG
		}
//		pass 15 write fluid depth to flowmap alpha
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _HeightFluidTex;
			
			float4 frag (v2f_img i) : COLOR
			{	 
				float4 flowmap = tex2D(_MainTex, i.uv);
				return float4(flowmap.r, flowmap.g, flowmap.b, tex2D(_HeightFluidTex, i.uv).g);
				
			}
			ENDCG
		}
	}
	FallBack off
}
