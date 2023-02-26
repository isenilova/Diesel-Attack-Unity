// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Particle_shder"
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
		_mask1("mask1", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 0.5
		_mask1_N("mask1_N", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
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
		uniform sampler2D _mask1_N;
		uniform float4 _mask1_N_ST;
		uniform float4 _Color2;
		uniform float _Emmision_Intencity;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform sampler2D _mask1;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord37 = i.uv_texcoord * _Tiling;
			float2 panner41 = ( _Time.y * float2( 0.02,0 ) + uv_TexCoord37);
			float4 tex2DNode43 = tex2D( _TextureSample0, panner41 );
			float2 panner42 = ( _Time.y * float2( 0.1,0 ) + uv_TexCoord37);
			float4 tex2DNode44 = tex2D( _d79kuab604e967d49304514856e3c121b3ce026, panner42 );
			float4 temp_output_49_0 = ( _Color0 * ( ( tex2DNode43.r + tex2DNode44.b ) * tex2DNode44 ) );
			float4 normalizeResult51 = normalize( temp_output_49_0 );
			float2 panner50 = ( _Time.y * float2( 0.15,0 ) + uv_TexCoord37);
			float2 uv_mask1_N = i.uv_texcoord * _mask1_N_ST.xy + _mask1_N_ST.zw;
			o.Normal = BlendNormals( ( normalizeResult51 + tex2D( _13191normal, panner50 ) ).rgb , tex2D( _mask1_N, uv_mask1_N ).rgb );
			float4 lerpResult58 = lerp( _Color2 , temp_output_49_0 , tex2DNode43.b);
			o.Albedo = lerpResult58.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float fresnelNdotV55 = dot( ase_normWorldNormal, ase_worldViewDir );
			float fresnelNode55 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV55, 1.0 ) );
			o.Emission = ( ( _Emmision_Intencity * temp_output_49_0 ) * ( 1.0 - fresnelNode55 ) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = ( tex2D( _mask1, i.uv_texcoord ) * _Opacity ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
			sampler3D _DitherMaskLOD;
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
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
-1907;23;1906;1020;1733.476;223.4095;1.751369;True;True
Node;AmplifyShaderEditor.Vector2Node;36;-4079.315,-220.489;Float;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;False;0;1,1;3,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;39;-3454.668,-400.7958;Float;False;Constant;_Vector1;Vector 1;3;0;Create;True;0;0;False;0;0.02,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;38;-3368.725,118.0084;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-3772.08,-228.416;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;40;-3429.944,-49.84431;Float;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;0.1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;41;-3048.298,-420.1044;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;42;-2970.182,-62.87091;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;43;-2633.531,-449.4894;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;1fef3f148d7667a42b29a101e33e3f60;1fef3f148d7667a42b29a101e33e3f60;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;44;-2612.9,-118.0132;Float;True;Property;_d79kuab604e967d49304514856e3c121b3ce026;d79kuab-604e967d-4930-4514-856e-3c121b3ce026;1;0;Create;True;0;0;False;0;1fef3f148d7667a42b29a101e33e3f60;1fef3f148d7667a42b29a101e33e3f60;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-2197.408,-219.1005;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;46;-2046.844,-573.8087;Float;False;Property;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.11877,0.0764062,0.1603774,0;0.2821289,0.127581,0.4433962,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-2085.109,-224.8222;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;48;-3189.518,365.0456;Float;False;Constant;_Vector2;Vector 2;3;0;Create;True;0;0;False;0;0.15,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;50;-2729.755,352.0189;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1722.181,-394.9177;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;51;-1520.406,-19.87651;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;52;-1681.635,274.1049;Float;True;Property;_13191normal;13191-normal;5;0;Create;True;0;0;False;0;f26c7c53dc078304f9c71b6e92551f51;f26c7c53dc078304f9c71b6e92551f51;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-736.5264,1456.205;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;55;-1092.158,346.3731;Float;True;Standard;TangentNormal;ViewDir;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1268.372,-123.6026;Float;False;Property;_Emmision_Intencity;Emmision_Intencity;8;0;Create;True;0;0;False;0;0;2;-50;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;56;-1151.672,-602.7147;Float;False;Property;_Color2;Color 2;4;0;Create;True;0;0;False;0;0.0745098,0.1607843,0.1444379,0;0,0.4811321,0.2517085,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-68.74694,969.0878;Float;False;Property;_Opacity;Opacity;10;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;27;-227.7603,1347.071;Float;True;Property;_mask1;mask1;9;0;Create;True;0;0;False;0;23dcf34fd55ee1f479b1f93522b099b8;23dcf34fd55ee1f479b1f93522b099b8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;-742.8917,416.506;Float;True;Property;_mask1_N;mask1_N;11;0;Create;True;0;0;False;0;bc169512e8bc052419abe15f70b55907;bc169512e8bc052419abe15f70b55907;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;62;-812.3945,309.6169;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-947.0757,-66.93452;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-1129.685,135.842;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;26;249.7239,296.3749;Float;False;Property;_Metallic;Metallic;6;0;Create;True;0;0;False;0;0;0.915;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;60;-244.2388,160.9594;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-544.5866,265.8871;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OrthoParams;61;-1503.168,483.1225;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;367.7401,658.0151;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;63;-28.91974,642.3144;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;66;-508.8405,712.329;Float;True;Standard;TangentNormal;ViewDir;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;-0.1;False;2;FLOAT;1;False;3;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;253.72,430.8227;Float;False;Property;_Smoothness;Smoothness;7;0;Create;True;0;0;False;0;0;0.582;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-567.9434,-38.36341;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;442.9067,1210.679;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;823.9688,181.608;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Particle_shder;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;37;0;36;0
WireConnection;41;0;37;0
WireConnection;41;2;39;0
WireConnection;41;1;38;0
WireConnection;42;0;37;0
WireConnection;42;2;40;0
WireConnection;42;1;38;0
WireConnection;43;1;41;0
WireConnection;44;1;42;0
WireConnection;45;0;43;1
WireConnection;45;1;44;3
WireConnection;47;0;45;0
WireConnection;47;1;44;0
WireConnection;50;0;37;0
WireConnection;50;2;48;0
WireConnection;50;1;38;0
WireConnection;49;0;46;0
WireConnection;49;1;47;0
WireConnection;51;0;49;0
WireConnection;52;1;50;0
WireConnection;27;1;30;0
WireConnection;62;0;55;0
WireConnection;57;0;54;0
WireConnection;57;1;49;0
WireConnection;53;0;51;0
WireConnection;53;1;52;0
WireConnection;60;0;53;0
WireConnection;60;1;33;0
WireConnection;59;0;57;0
WireConnection;59;1;62;0
WireConnection;65;0;63;0
WireConnection;65;1;28;0
WireConnection;63;0;66;0
WireConnection;58;0;56;0
WireConnection;58;1;49;0
WireConnection;58;2;43;3
WireConnection;29;0;27;0
WireConnection;29;1;28;0
WireConnection;0;0;58;0
WireConnection;0;1;60;0
WireConnection;0;2;59;0
WireConnection;0;3;26;0
WireConnection;0;4;24;0
WireConnection;0;9;29;0
ASEEND*/
//CHKSM=C78BB459E191E410330740DD6B2A843BFD39EE71