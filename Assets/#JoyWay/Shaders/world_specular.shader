// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "blending"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Albedotiling("Albedo tiling", Vector) = (1,1,0,0)
		_brightness("brightness", Range( 0 , 1)) = 0
		_Specular("Specular", 2D) = "white" {}
		_Speculartiling("Specular tiling", Vector) = (1,1,0,0)
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.872
		_Metallic_shining("Metallic_shining", Range( 0 , 50)) = 0
		_Normalmap("Normal map", 2D) = "white" {}
		_Normaltiling("Normal tiling", Vector) = (1,1,0,0)
		_Normal_power("Normal_power", Range( 0 , 100)) = 0.2159999
		_AO("AO", 2D) = "white" {}
		_AOtiling("AO tiling", Vector) = (1,1,0,0)
		_Ambient_occlusion_darkness("Ambient_occlusion_darkness", Range( 0 , 100)) = 0.872
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Forward Rendering Options)]
		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		#define ASE_TEXTURE_PARAMS(textureName) textureName

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
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Normalmap;
		uniform float2 _Normaltiling;
		uniform float _Normal_power;
		uniform sampler2D _Albedo;
		uniform float2 _Albedotiling;
		uniform float _brightness;
		uniform sampler2D _Specular;
		uniform float2 _Speculartiling;
		uniform float _Smoothness;
		uniform float _Metallic_shining;
		uniform sampler2D _AO;
		uniform float2 _AOtiling;
		uniform float _Ambient_occlusion_darkness;


		inline float3 TriplanarSamplingSNF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			xNorm.xyz = half3( UnpackScaleNormal( xNorm, normalScale.y ).xy * float2( nsign.x, 1.0 ) + worldNormal.zy, worldNormal.x ).zyx;
			yNorm.xyz = half3( UnpackScaleNormal( yNorm, normalScale.x ).xy * float2( nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y ).xzy;
			zNorm.xyz = half3( UnpackScaleNormal( zNorm, normalScale.y ).xy * float2( -nsign.z, 1.0 ) + worldNormal.xy, worldNormal.z ).xyz;
			return normalize( xNorm.xyz * projNormal.x + yNorm.xyz * projNormal.y + zNorm.xyz * projNormal.z );
		}


		inline float4 TriplanarSamplingSF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 triplanar113 = TriplanarSamplingSNF( _Normalmap, ase_worldPos, ase_worldNormal, 1.0, _Normaltiling, _Normal_power, 0 );
			float3 tanTriplanarNormal113 = mul( ase_worldToTangent, triplanar113 );
			o.Normal = tanTriplanarNormal113;
			float4 triplanar118 = TriplanarSamplingSF( _Albedo, ase_worldPos, ase_worldNormal, 1.0, _Albedotiling, 1.0, 0 );
			o.Albedo = ( triplanar118 * _brightness ).xyz;
			float4 triplanar122 = TriplanarSamplingSF( _Specular, ase_worldPos, ase_worldNormal, 1.0, _Speculartiling, 1.0, 0 );
			o.Metallic = pow( triplanar122.x , 1.0 );
			o.Smoothness = ( _Smoothness * ( triplanar122.w * _Metallic_shining ) );
			float4 triplanar124 = TriplanarSamplingSF( _AO, ase_worldPos, ase_worldNormal, 1.0, _AOtiling, 1.0, 0 );
			o.Occlusion = ( triplanar124 * _Ambient_occlusion_darkness ).x;
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
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
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
Version=18100
-1908;70;1900;919;1483.142;-136.3124;1.6;True;True
Node;AmplifyShaderEditor.Vector2Node;123;-650.6614,738.069;Float;False;Property;_Speculartiling;Specular tiling;4;0;Create;True;0;0;False;0;False;1,1;0.05,0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;121;-691.4064,501.8076;Inherit;True;Property;_Specular;Specular;3;0;Create;True;0;0;False;0;False;None;bc9b853297ba93e4a9bf80ed5c9f4f6f;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-674.8325,1118.542;Inherit;False;Property;_Metallic_shining;Metallic_shining;6;0;Create;True;0;0;False;0;False;0;1;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;122;-379.2789,575.8876;Inherit;True;Spherical;World;False;Top Texture 2;_TopTexture2;white;0;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;119;-402.236,-7.181231;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;False;None;a2aa02873f4c5064390a6bae0e4a8746;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;126;-1072.221,1354.338;Inherit;True;Property;_AO;AO;10;0;Create;True;0;0;False;0;False;None;b947abb711cf5fe4d922766348f8d32e;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Vector2Node;125;-1031.476,1590.599;Float;False;Property;_AOtiling;AO tiling;11;0;Create;True;0;0;False;0;False;1,1;0.05,0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;120;-361.491,229.0801;Float;False;Property;_Albedotiling;Albedo tiling;1;0;Create;True;0;0;False;0;False;1,1;0.05,0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;81;-407.7416,-273.642;Inherit;False;Property;_Normal_power;Normal_power;9;0;Create;True;0;0;False;0;False;0.2159999;1;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;114;-396.6574,-479.8116;Inherit;True;Property;_Normalmap;Normal map;7;0;Create;True;0;0;False;0;False;None;13105cfdebd81044390074197a9569be;True;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TriplanarNode;124;-763.294,1430.018;Inherit;True;Spherical;World;False;Top Texture 3;_TopTexture3;white;0;None;Mid Texture 3;_MidTexture3;white;-1;None;Bot Texture 3;_BotTexture3;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;74;-111.4743,285.4557;Inherit;False;Property;_brightness;brightness;2;0;Create;True;0;0;False;0;False;0;0.441;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;118;-90.10872,66.89855;Inherit;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;0;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-185.6569,951.8544;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-275.4035,1494.144;Inherit;False;Property;_Ambient_occlusion_darkness;Ambient_occlusion_darkness;12;0;Create;True;0;0;False;0;False;0.872;1.5;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;115;-363.8367,-184.8875;Float;False;Property;_Normaltiling;Normal tiling;8;0;Create;True;0;0;False;0;False;1,1;0.05,0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;14;-5.676476,495.276;Inherit;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;False;0.872;0.582;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;113;-31.73341,-257.0475;Inherit;True;Spherical;World;True;Top Texture 1;_TopTexture1;white;0;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;50;-1241.793,220.6647;Inherit;False;Constant;_Vector1;Vector 1;8;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;49;-1254.788,65.26578;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;379.7711,184.4704;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;91.08331,1237.557;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;348.7751,561.6794;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;107;175.6407,865.829;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;21;925.9473,311.7539;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;blending;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;1;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;122;0;121;0
WireConnection;122;3;123;0
WireConnection;124;0;126;0
WireConnection;124;3;125;0
WireConnection;118;0;119;0
WireConnection;118;3;120;0
WireConnection;101;0;122;4
WireConnection;101;1;89;0
WireConnection;113;0;114;0
WireConnection;113;8;81;0
WireConnection;113;3;115;0
WireConnection;75;0;118;0
WireConnection;75;1;74;0
WireConnection;104;0;124;0
WireConnection;104;1;82;0
WireConnection;117;0;14;0
WireConnection;117;1;101;0
WireConnection;107;0;122;1
WireConnection;21;0;75;0
WireConnection;21;1;113;0
WireConnection;21;3;107;0
WireConnection;21;4;117;0
WireConnection;21;5;104;0
ASEEND*/
//CHKSM=CEC4C88C9E3A610B73C30ECAA84640246A4C9CC1