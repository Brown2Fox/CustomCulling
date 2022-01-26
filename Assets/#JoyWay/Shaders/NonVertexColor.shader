// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "NonVertexColor"
{
	Properties
	{
		_Basecolor("Base color", Color) = (0.6698113,0.6099246,0.5592293,1)
		_Tex1color("Tex1 color", Color) = (0.6698113,0.6099246,0.5592293,1)
		_Tex1colorpower("Tex1 color power", Float) = 0
		_Tex2color("Tex2 color", Color) = (0.6698113,0.6099246,0.5592293,1)
		_Tex2colorpower("Tex2 color power", Float) = 0
		_Metallic("Metallic", Color) = (0.6698113,0.6099246,0.5592293,1)
		_Tex2("Tex2", 2D) = "white" {}
		_Tex2UV("Tex2 UV", Vector) = (0.5,0.5,0,0)
		_Tex2UVoffset("Tex2 UV offset", Vector) = (0.5,0.5,0,0)
		_Mask("Mask", 2D) = "white" {}
		[Normal]_Normalmap("Normal map", 2D) = "bump" {}
		_Normalpower("Normal power", Float) = 1
		_Smoothness("Smoothness", Float) = 0
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv2_texcoord2;
			float2 uv_texcoord;
		};

		uniform float _Normalpower;
		uniform sampler2D _Normalmap;
		uniform float4 _Normalmap_ST;
		uniform float4 _Basecolor;
		uniform float4 _Tex2color;
		uniform sampler2D _Tex2;
		uniform float2 _Tex2UV;
		uniform float2 _Tex2UVoffset;
		uniform float _Tex2colorpower;
		uniform float4 _Tex1color;
		uniform float _Tex1colorpower;
		uniform sampler2D _Mask;
		uniform float4 _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv1_Normalmap = i.uv2_texcoord2 * _Normalmap_ST.xy + _Normalmap_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _Normalmap, uv1_Normalmap ), _Normalpower );
			float2 uv_TexCoord23 = i.uv_texcoord * _Tex2UV + _Tex2UVoffset;
			float4 tex2DNode10 = tex2D( _Tex2, uv_TexCoord23 );
			float4 lerpResult2 = lerp( ( _Tex2color * tex2DNode10 * _Tex2colorpower ) , ( _Tex1color * tex2DNode10 * _Tex1colorpower ) , tex2D( _Mask, i.uv2_texcoord2 ));
			o.Albedo = ( _Basecolor * lerpResult2 ).rgb;
			o.Metallic = _Metallic.r;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18100
-1908;70;1900;919;1820.842;568.67;1;True;True
Node;AmplifyShaderEditor.Vector2Node;24;-1862.529,-87.69002;Inherit;False;Property;_Tex2UVoffset;Tex2 UV offset;11;0;Create;True;0;0;False;0;False;0.5,0.5;0.85,0.22;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;33;-1862.043,-229.4301;Inherit;False;Property;_Tex2UV;Tex2 UV;10;0;Create;True;0;0;False;0;False;0.5,0.5;0.2,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1590.883,-191.3855;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-974.5334,-570.1448;Inherit;False;Property;_Tex1colorpower;Tex1 color power;2;0;Create;True;0;0;False;0;False;0;5.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;31;-1168.601,-424.4286;Inherit;False;Property;_Tex2color;Tex2 color;3;0;Create;True;0;0;False;0;False;0.6698113,0.6099246,0.5592293,1;0.5566037,0.5329743,0.5329743,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;-873.842,-93.67004;Inherit;False;Property;_Tex2colorpower;Tex2 color power;4;0;Create;True;0;0;False;0;False;0;5.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1216.407,-205.9336;Inherit;True;Property;_Tex2;Tex2;9;0;Create;True;0;0;False;0;False;-1;None;36e9f4dff7ddcea409624c80629868cd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-1565.821,95.91991;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;29;-1093.631,-864.5782;Inherit;False;Property;_Tex1color;Tex1 color;1;0;Create;True;0;0;False;0;False;0.6698113,0.6099246,0.5592293,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-1216.998,49.96983;Inherit;True;Property;_Mask;Mask;12;0;Create;True;0;0;False;0;False;-1;None;89135a16d4ec3484885ce2c98294cdd3;True;1;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-835.2335,-299.4328;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-692.4153,-610.3417;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;4;-426.9469,-697.1176;Inherit;False;Property;_Basecolor;Base color;0;0;Create;True;0;0;False;0;False;0.6698113,0.6099246,0.5592293,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;2;-605.0582,-310.5468;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-1419.135,381.786;Inherit;False;1;7;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-1288.961,554.4293;Inherit;False;Property;_Normalpower;Normal power;16;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;42;-1800.154,466.5486;Inherit;False;Property;_NormalUVoffset;Normal UV offset;15;0;Create;True;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;3;-2097.638,-684.8081;Inherit;True;Property;_Tex1;Tex1;6;0;Create;True;0;0;False;0;False;-1;None;cfd835350e87d9e429a67fac3f587514;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-183.8669,334.24;Inherit;False;Property;_Metallic;Metallic;5;0;Create;True;0;0;False;0;False;0.6698113,0.6099246,0.5592293,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;19;-1805.592,332.2896;Inherit;False;Property;_NormalUV;Normal UV;14;0;Create;True;0;0;False;0;False;0.5,0.5;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;7;-1130.887,289.2423;Inherit;True;Property;_Normalmap;Normal map;13;1;[Normal];Create;True;0;0;False;0;False;-1;None;221b3a60b31fcd949bdbe3f664ca9308;True;1;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;28;-2568.257,-730.5213;Inherit;False;Property;_Tex1UV;Tex1 UV;7;0;Create;True;0;0;False;0;False;0.5,0.5;0.8,0.8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;12;-173.3741,524.927;Inherit;False;Property;_Smoothness;Smoothness;17;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;26;-2565.791,-594.3554;Inherit;False;Property;_Tex1UVoffset;Tex1 UV offset;8;0;Create;True;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-104.7812,-253.3848;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-2343.334,-673.8929;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;81.083,-122.5503;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;NonVertexColor;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;33;0
WireConnection;23;1;24;0
WireConnection;10;1;23;0
WireConnection;11;1;41;0
WireConnection;32;0;31;0
WireConnection;32;1;10;0
WireConnection;32;2;44;0
WireConnection;30;0;29;0
WireConnection;30;1;10;0
WireConnection;30;2;43;0
WireConnection;2;0;32;0
WireConnection;2;1;30;0
WireConnection;2;2;11;0
WireConnection;20;0;19;0
WireConnection;20;1;42;0
WireConnection;3;1;25;0
WireConnection;7;1;20;0
WireConnection;7;5;9;0
WireConnection;5;0;4;0
WireConnection;5;1;2;0
WireConnection;25;0;28;0
WireConnection;25;1;26;0
WireConnection;0;0;5;0
WireConnection;0;1;7;0
WireConnection;0;3;22;0
WireConnection;0;4;12;0
ASEEND*/
//CHKSM=52C3A43E494CF22EF947AD7CAC3FD5A00361731A