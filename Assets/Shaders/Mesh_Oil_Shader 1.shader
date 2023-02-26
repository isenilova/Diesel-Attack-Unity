// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mesh_oil_map"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_d79kuab604e967d49304514856e3c121b3ce026("d79kuab-604e967d-4930-4514-856e-3c121b3ce026", 2D) = "white" {}
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_Color0("Color 0", Color) = (0.11877,0.0764062,0.1603774,0)
		_Color2("Color 2", Color) = (0.0745098,0.1607843,0.1444379,0)
		_13191normal("13191-normal", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Emmision_Intencity("Emmision_Intencity", Range( -50 , 50)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float4 _Color0;
		uniform sampler2D _TextureSample0;
		uniform float2 _Tiling;
		uniform sampler2D _d79kuab604e967d49304514856e3c121b3ce026;
		uniform sampler2D _13191normal;
		uniform float4 _Color2;
		uniform float _Emmision_Intencity;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord20 = i.uv_texcoord * _Tiling;
			float2 panner26 = ( _Time.y * float2( 0.02,0 ) + uv_TexCoord20);
			float4 tex2DNode27 = tex2D( _TextureSample0, panner26 );
			float2 panner1 = ( _Time.y * float2( 0.1,0 ) + uv_TexCoord20);
			float4 tex2DNode24 = tex2D( _d79kuab604e967d49304514856e3c121b3ce026, panner1 );
			float4 temp_output_32_0 = ( _Color0 * ( ( tex2DNode27.r + tex2DNode24.b ) * tex2DNode24 ) );
			float4 normalizeResult43 = normalize( temp_output_32_0 );
			float2 panner53 = ( _Time.y * float2( 0.2,0 ) + uv_TexCoord20);
			float4 tex2DNode51 = tex2D( _13191normal, panner53 );
			float4 lerpResult63 = lerp( normalizeResult43 , tex2DNode51 , 0.3);
			o.Normal = lerpResult63.rgb;
			float4 lerpResult59 = lerp( _Color2 , temp_output_32_0 , tex2DNode27.b);
			o.Albedo = lerpResult59.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float fresnelNdotV47 = dot( ase_normWorldNormal, ase_worldViewDir );
			float fresnelNode47 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV47, 2.0 ) );
			o.Emission = ( ( _Emmision_Intencity * temp_output_32_0 ) * ( 1.0 - fresnelNode47 ) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
7;1;1906;1021;1359.964;413.1109;1;True;True
Node;AmplifyShaderEditor.Vector2Node;45;-3331.349,-426.9263;Float;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;False;0;1,1;5,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;30;-2706.705,-607.2327;Float;False;Constant;_Vector1;Vector 1;3;0;Create;True;0;0;False;0;0.02,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-3024.117,-434.8533;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;10;-2620.762,-88.42899;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;13;-2681.981,-256.2817;Float;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;0.1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;1;-2222.218,-269.3083;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;26;-2300.335,-626.5413;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;24;-1864.936,-324.4506;Float;True;Property;_d79kuab604e967d49304514856e3c121b3ce026;d79kuab-604e967d-4930-4514-856e-3c121b3ce026;1;0;Create;True;0;0;False;0;1fef3f148d7667a42b29a101e33e3f60;1fef3f148d7667a42b29a101e33e3f60;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;27;-1885.567,-655.9263;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;1fef3f148d7667a42b29a101e33e3f60;1fef3f148d7667a42b29a101e33e3f60;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-1449.445,-425.5378;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;54;-2441.555,158.6084;Float;False;Constant;_Vector2;Vector 2;3;0;Create;True;0;0;False;0;0.2,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;34;-1298.881,-780.2455;Float;False;Property;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.11877,0.0764062,0.1603774,0;0.4811321,0.06068332,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1337.146,-431.2595;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-974.2165,-601.3546;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;47;-386.9826,314.3867;Float;True;Standard;TangentNormal;ViewDir;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-520.4074,-330.04;Float;False;Property;_Emmision_Intencity;Emmision_Intencity;8;0;Create;True;0;0;False;0;0;26.6;-50;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;53;-1981.791,145.5817;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;62;-52.14473,247.5184;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;43;-772.4413,-226.3139;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-574.2688,206.4935;Float;False;Constant;_mix;mix;9;0;Create;True;0;0;False;0;0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;51;-933.671,67.66767;Float;True;Property;_13191normal;13191-normal;5;0;Create;True;0;0;False;0;f26c7c53dc078304f9c71b6e92551f51;f26c7c53dc078304f9c71b6e92551f51;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;57;-403.7075,-809.1515;Float;False;Property;_Color2;Color 2;4;0;Create;True;0;0;False;0;0.0745098,0.1607843,0.1444379,0;0,0.01090296,0.245283,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-199.1116,-273.3719;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;203.3771,59.44993;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;63;-321.9009,32.12015;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;370.5226,966.1342;Float;False;Property;_Metallic;Metallic;6;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;59;180.0203,-244.8008;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-381.7204,-70.59541;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;39;374.5188,1100.583;Float;False;Property;_Smoothness;Smoothness;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;61;1086.962,-79.76193;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Mesh_oil_map;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;45;0
WireConnection;1;0;20;0
WireConnection;1;2;13;0
WireConnection;1;1;10;0
WireConnection;26;0;20;0
WireConnection;26;2;30;0
WireConnection;26;1;10;0
WireConnection;24;1;1;0
WireConnection;27;1;26;0
WireConnection;25;0;27;1
WireConnection;25;1;24;3
WireConnection;31;0;25;0
WireConnection;31;1;24;0
WireConnection;32;0;34;0
WireConnection;32;1;31;0
WireConnection;53;0;20;0
WireConnection;53;2;54;0
WireConnection;53;1;10;0
WireConnection;62;0;47;0
WireConnection;43;0;32;0
WireConnection;51;1;53;0
WireConnection;40;0;41;0
WireConnection;40;1;32;0
WireConnection;50;0;40;0
WireConnection;50;1;62;0
WireConnection;63;0;43;0
WireConnection;63;1;51;0
WireConnection;63;2;64;0
WireConnection;59;0;57;0
WireConnection;59;1;32;0
WireConnection;59;2;27;3
WireConnection;56;0;43;0
WireConnection;56;1;51;0
WireConnection;61;0;59;0
WireConnection;61;1;63;0
WireConnection;61;2;50;0
WireConnection;61;3;38;0
WireConnection;61;4;39;0
ASEEND*/
//CHKSM=0722264283D5C168AE6FB0AAC9714C5A11FC8716