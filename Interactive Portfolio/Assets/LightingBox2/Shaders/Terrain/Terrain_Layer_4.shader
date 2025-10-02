// Made with Amplify Shader Editor v1.9.7.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LightingBox/Terrain/Terrain 4-Layers"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 10
		_TessMin( "Tess Min Distance", Float ) = 10
		_TessMax( "Tess Max Distance", Float ) = 25
		_SplatMap1("SplatMap1", 2D) = "white" {}
		_AlbedoMaps("AlbedoMaps", 2DArray) = "white" {}
		_Smoothness_1("Smoothness_1", Range( 0 , 1)) = 0.5
		_Smoothness_2("Smoothness_2", Range( 0 , 1)) = 0.5
		_Smoothness_3("Smoothness_3", Range( 0 , 1)) = 0.5
		_Smoothness_4("Smoothness_4", Range( 0 , 1)) = 0.5
		_Specular_Color_1("Specular_Color_1", Color) = (0,0,0,0)
		_Specular_Color_2("Specular_Color_2", Color) = (0,0,0,0)
		_Specular_Color_3("Specular_Color_3", Color) = (0,0,0,0)
		_Specular_Color_4("Specular_Color_4", Color) = (0,0,0,0)
		_Normal_Scale_1("Normal_Scale_1", Range( 0 , 1)) = 1
		_Normal_Scale_2("Normal_Scale_2", Range( 0 , 1)) = 1
		_Normal_Scale_3("Normal_Scale_3", Range( 0 , 1)) = 1
		_Normal_Scale_4("Normal_Scale_4", Range( 0 , 1)) = 1
		_Displacement_1("Displacement_1", Range( 0 , 3)) = 0
		_UV_0("UV_0", Vector) = (30,30,0,0)
		_UV_1("UV_1", Vector) = (30,30,0,0)
		_UV_2("UV_2", Vector) = (30,30,0,0)
		_UV_3("UV_3", Vector) = (30,30,0,0)
		_Displacement_2("Displacement_2", Range( 0 , 3)) = 0
		_Displacement_3("Displacement_3", Range( 0 , 3)) = 0
		_Displacement_4("Displacement_4", Range( 0 , 3)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#define ASE_VERSION 19701
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D_ARRAY(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#define SAMPLE_TEXTURE2D_ARRAY_LOD(tex,samplerTex,coord,lod) tex.SampleLevel(samplerTex,coord, lod)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D_ARRAY(tex,samplertex,coord) tex2DArray(tex,coord)
		#define SAMPLE_TEXTURE2D_ARRAY_LOD(tex,samplertex,coord,lod) tex2DArraylod(tex, float4(coord,lod))
		#endif//ASE Sampling Macros

		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _SplatMap1;
		uniform float4 _SplatMap1_ST;
		UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(_AlbedoMaps);
		uniform float4 _AlbedoMaps_ST;
		uniform float2 _UV_0;
		SamplerState sampler_AlbedoMaps;
		uniform float2 _UV_1;
		uniform float2 _UV_2;
		uniform float2 _UV_3;
		uniform float _Displacement_1;
		uniform float _Displacement_2;
		uniform float _Displacement_3;
		uniform float _Displacement_4;
		uniform float _Normal_Scale_1;
		uniform float _Normal_Scale_2;
		uniform float _Normal_Scale_3;
		uniform float _Normal_Scale_4;
		uniform float4 _Specular_Color_1;
		uniform float4 _Specular_Color_2;
		uniform float4 _Specular_Color_3;
		uniform float4 _Specular_Color_4;
		uniform float _Smoothness_1;
		uniform float _Smoothness_2;
		uniform float _Smoothness_3;
		uniform float _Smoothness_4;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 uv_SplatMap1 = v.texcoord * _SplatMap1_ST.xy + _SplatMap1_ST.zw;
			float4 tex2DNode9 = tex2Dlod( _SplatMap1, float4( uv_SplatMap1, 0, 0.0) );
			float2 uv_AlbedoMaps = v.texcoord.xy * _AlbedoMaps_ST.xy + _AlbedoMaps_ST.zw;
			float4 tex2DArrayNode10 = SAMPLE_TEXTURE2D_ARRAY_LOD( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_0 ),0.0), 0.0 );
			float4 tex2DArrayNode11 = SAMPLE_TEXTURE2D_ARRAY_LOD( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_1 ),1.0), 0.0 );
			float4 tex2DArrayNode14 = SAMPLE_TEXTURE2D_ARRAY_LOD( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_2 ),2.0), 0.0 );
			float4 tex2DArrayNode17 = SAMPLE_TEXTURE2D_ARRAY_LOD( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_3 ),3.0), 0.0 );
			float4 weightedBlendVar123 = tex2DNode9;
			float weightedBlend123 = ( weightedBlendVar123.x*tex2DArrayNode10.a + weightedBlendVar123.y*tex2DArrayNode11.a + weightedBlendVar123.z*tex2DArrayNode14.a + weightedBlendVar123.w*tex2DArrayNode17.a );
			float4 weightedBlendVar129 = tex2DNode9;
			float weightedBlend129 = ( weightedBlendVar129.x*_Displacement_1 + weightedBlendVar129.y*_Displacement_2 + weightedBlendVar129.z*_Displacement_3 + weightedBlendVar129.w*_Displacement_4 );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( weightedBlend123 * weightedBlend129 ) * ase_vertexNormal );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_SplatMap1 = i.uv_texcoord * _SplatMap1_ST.xy + _SplatMap1_ST.zw;
			float4 tex2DNode9 = tex2D( _SplatMap1, uv_SplatMap1 );
			float2 uv_AlbedoMaps = i.uv_texcoord * _AlbedoMaps_ST.xy + _AlbedoMaps_ST.zw;
			float4 weightedBlendVar97 = tex2DNode9;
			float3 weightedBlend97 = ( weightedBlendVar97.x*UnpackScaleNormal( SAMPLE_TEXTURE2D_ARRAY( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_0 ),4.0) ), _Normal_Scale_1 ) + weightedBlendVar97.y*UnpackScaleNormal( SAMPLE_TEXTURE2D_ARRAY( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_1 ),5.0) ), _Normal_Scale_2 ) + weightedBlendVar97.z*UnpackScaleNormal( SAMPLE_TEXTURE2D_ARRAY( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_2 ),6.0) ), _Normal_Scale_3 ) + weightedBlendVar97.w*UnpackScaleNormal( SAMPLE_TEXTURE2D_ARRAY( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_3 ),7.0) ), _Normal_Scale_4 ) );
			o.Normal = weightedBlend97;
			float4 tex2DArrayNode10 = SAMPLE_TEXTURE2D_ARRAY( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_0 ),0.0) );
			float4 tex2DArrayNode11 = SAMPLE_TEXTURE2D_ARRAY( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_1 ),1.0) );
			float4 tex2DArrayNode14 = SAMPLE_TEXTURE2D_ARRAY( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_2 ),2.0) );
			float4 tex2DArrayNode17 = SAMPLE_TEXTURE2D_ARRAY( _AlbedoMaps, sampler_AlbedoMaps, float3(( uv_AlbedoMaps * _UV_3 ),3.0) );
			float4 weightedBlendVar66 = tex2DNode9;
			float4 weightedBlend66 = ( weightedBlendVar66.x*tex2DArrayNode10 + weightedBlendVar66.y*tex2DArrayNode11 + weightedBlendVar66.z*tex2DArrayNode14 + weightedBlendVar66.w*tex2DArrayNode17 );
			o.Albedo = weightedBlend66.rgb;
			float3 temp_cast_1 = (0.0).xxx;
			o.Emission = temp_cast_1;
			float4 weightedBlendVar90 = tex2DNode9;
			float4 weightedBlend90 = ( weightedBlendVar90.x*( _Specular_Color_1 * tex2DArrayNode10.g ) + weightedBlendVar90.y*( _Specular_Color_2 * tex2DArrayNode11.g ) + weightedBlendVar90.z*( _Specular_Color_3 * tex2DArrayNode14.g ) + weightedBlendVar90.w*( _Specular_Color_4 * tex2DArrayNode17.g ) );
			o.Specular = weightedBlend90.rgb;
			float4 weightedBlendVar75 = tex2DNode9;
			float weightedBlend75 = ( weightedBlendVar75.x*( _Smoothness_1 * tex2DArrayNode10.r ) + weightedBlendVar75.y*( _Smoothness_2 * tex2DArrayNode11.r ) + weightedBlendVar75.z*( _Smoothness_3 * tex2DArrayNode14.r ) + weightedBlendVar75.w*( _Smoothness_4 * tex2DArrayNode17.r ) );
			o.Smoothness = weightedBlend75;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19701
Node;AmplifyShaderEditor.CommentaryNode;125;-2025.839,-1783.219;Inherit;False;1455.618;965.606;Albedo;10;10;14;17;11;33;47;32;46;6;3;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;3;-2005.124,-1726.299;Float;True;Property;_AlbedoMaps;AlbedoMaps;6;0;Create;True;0;0;0;False;0;False;None;None;False;white;LockedToTexture2DArray;Texture2DArray;-1;0;2;SAMPLER2DARRAY;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.Vector2Node;202;-4043.795,-3007.813;Float;False;Property;_UV_0;UV_0;20;0;Create;True;0;0;0;False;0;False;30,30;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;203;-4035.429,-2863.778;Float;False;Property;_UV_1;UV_1;21;0;Create;True;0;0;0;False;0;False;30,30;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;205;-4035.427,-2559.931;Float;False;Property;_UV_3;UV_3;23;0;Create;True;0;0;0;False;0;False;30,30;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;204;-4035.428,-2707.892;Float;False;Property;_UV_2;UV_2;22;0;Create;True;0;0;0;False;0;False;30,30;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1836.68,-1016.791;Inherit;False;0;3;2;3;2;SAMPLER2DARRAY;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;130;478.0417,656.0351;Inherit;False;1271.142;491.2252;Displacement;8;112;113;115;129;114;128;126;127;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-1277.886,-1022.007;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1285.046,-1454.932;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1280.181,-1232.558;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1291.37,-1667.703;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;128;535.4833,969.0829;Float;False;Property;_Displacement_4;Displacement_4;26;0;Create;True;0;0;0;False;0;False;0;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;535.4833,881.0829;Float;False;Property;_Displacement_3;Displacement_3;25;0;Create;True;0;0;0;False;0;False;0;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-971.4817,-1543.736;Inherit;True;Property;_TextureArray1;Texture Array 1;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;10;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;1;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;14;-960.2587,-1326.269;Inherit;True;Property;_TextureArray2;Texture Array 2;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;10;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;2;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;17;-961.4342,-1120.175;Inherit;True;Property;_TextureArray3;Texture Array 3;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;10;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;3;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;9;-4010.896,-1127.317;Inherit;True;Property;_SplatMap1;SplatMap1;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;126;538.4833,794.0829;Float;False;Property;_Displacement_2;Displacement_2;24;0;Create;True;0;0;0;False;0;False;0;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;541.3897,707.4896;Float;False;Property;_Displacement_1;Displacement_1;19;0;Create;True;0;0;0;False;0;False;0;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;136;-1867.461,-2979.216;Inherit;False;1048.55;476.4971;Comment;9;75;78;77;76;79;74;73;72;71;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;10;-987.0203,-1762.673;Inherit;True;Property;_AlbedoArray;AlbedoArray;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.CommentaryNode;124;-1954.95,-109.6801;Inherit;False;897.999;741.8158;Specular;9;90;86;88;89;87;81;80;82;83;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;135;-2154.518,-4246.142;Inherit;False;1188.032;1077.329;Normal;12;94;200;95;96;131;132;110;133;108;106;107;109;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-1810.119,-2825.099;Float;False;Property;_Smoothness_2;Smoothness_2;8;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SummedBlendNode;129;911.4785,852.512;Inherit;False;5;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-1810.119,-2721.098;Float;False;Property;_Smoothness_3;Smoothness_3;9;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-1810.119,-2626.099;Float;False;Property;_Smoothness_4;Smoothness_4;10;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SummedBlendNode;123;1021.046,457.6179;Inherit;False;5;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-1817.461,-2919.253;Float;False;Property;_Smoothness_1;Smoothness_1;7;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;80;-1903.12,-51.77887;Float;False;Property;_Specular_Color_1;Specular_Color_1;11;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;False;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ColorNode;82;-1904.951,282.227;Float;False;Property;_Specular_Color_3;Specular_Color_3;13;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;False;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ColorNode;81;-1903.951,122.2266;Float;False;Property;_Specular_Color_2;Specular_Color_2;12;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;False;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ColorNode;83;-1901.951,443.2281;Float;False;Property;_Specular_Color_4;Specular_Color_4;14;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;False;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;132;-1739.269,-3599.948;Float;False;Property;_Normal_Scale_3;Normal_Scale_3;17;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-1743.931,-3791.053;Float;False;Property;_Normal_Scale_2;Normal_Scale_2;16;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;-2006.082,-4061.81;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-2004.021,-3872.426;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;133;-1747.344,-3413.063;Float;False;Property;_Normal_Scale_4;Normal_Scale_4;18;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-2007.722,-3679.644;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-2010.165,-3488.397;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-1746.957,-3980.619;Float;False;Property;_Normal_Scale_1;Normal_Scale_1;15;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-1412.312,-2733.695;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;1329.35,708.1938;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-1412.183,-2603.296;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-1419.313,-2829.074;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;115;1344.534,977.7064;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-1419.313,-2929.216;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-1554.256,-17.49036;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-1553.681,128.5852;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-1545.48,447.4471;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-1542.814,291.2885;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;200;-1384.216,-4100.677;Inherit;True;Property;_NormallArray;NormallArray;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Instance;10;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;4;False;7;SAMPLERSTATE;;False;6;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;94;-1383.499,-3901.142;Inherit;True;Property;_TextureArray0;Texture Array 0;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Instance;10;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;5;False;7;SAMPLERSTATE;;False;6;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;95;-1381.529,-3706.459;Inherit;True;Property;_TextureArray4;Texture Array 4;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Instance;10;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;6;False;7;SAMPLERSTATE;;False;6;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;96;-1385.425,-3518.84;Inherit;True;Property;_TextureArray5;Texture Array 5;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Instance;10;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;7;False;7;SAMPLERSTATE;;False;6;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SummedBlendNode;90;-1305.961,216.4508;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SummedBlendNode;75;-1007.892,-2826.422;Inherit;False;5;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SummedBlendNode;66;-202.7146,-1519.491;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SummedBlendNode;97;-758.7121,-3840.461;Inherit;False;5;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;1580.184,766.9326;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;206;3022.685,-1850.606;Inherit;False;Constant;_Float0;Float 0;23;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3282.172,-1878.023;Float;False;True;-1;6;ASEMaterialInspector;0;0;StandardSpecular;LightingBox/Terrain/Terrain 4-Layers;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;0;10;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;0;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;2;3;0
WireConnection;47;0;6;0
WireConnection;47;1;205;0
WireConnection;33;0;6;0
WireConnection;33;1;203;0
WireConnection;46;0;6;0
WireConnection;46;1;204;0
WireConnection;32;0;6;0
WireConnection;32;1;202;0
WireConnection;11;1;33;0
WireConnection;14;1;46;0
WireConnection;17;1;47;0
WireConnection;10;0;3;0
WireConnection;10;1;32;0
WireConnection;129;0;9;0
WireConnection;129;1;114;0
WireConnection;129;2;126;0
WireConnection;129;3;127;0
WireConnection;129;4;128;0
WireConnection;123;0;9;0
WireConnection;123;1;10;4
WireConnection;123;2;11;4
WireConnection;123;3;14;4
WireConnection;123;4;17;4
WireConnection;106;0;6;0
WireConnection;106;1;202;0
WireConnection;107;0;6;0
WireConnection;107;1;203;0
WireConnection;108;0;6;0
WireConnection;108;1;204;0
WireConnection;109;0;6;0
WireConnection;109;1;205;0
WireConnection;78;0;73;0
WireConnection;78;1;14;1
WireConnection;113;0;123;0
WireConnection;113;1;129;0
WireConnection;79;0;74;0
WireConnection;79;1;17;1
WireConnection;77;0;72;0
WireConnection;77;1;11;1
WireConnection;76;0;71;0
WireConnection;76;1;10;1
WireConnection;86;0;80;0
WireConnection;86;1;10;2
WireConnection;87;0;81;0
WireConnection;87;1;11;2
WireConnection;89;0;83;0
WireConnection;89;1;17;2
WireConnection;88;0;82;0
WireConnection;88;1;14;2
WireConnection;200;1;106;0
WireConnection;200;5;110;0
WireConnection;94;1;107;0
WireConnection;94;5;131;0
WireConnection;95;1;108;0
WireConnection;95;5;132;0
WireConnection;96;1;109;0
WireConnection;96;5;133;0
WireConnection;90;0;9;0
WireConnection;90;1;86;0
WireConnection;90;2;87;0
WireConnection;90;3;88;0
WireConnection;90;4;89;0
WireConnection;75;0;9;0
WireConnection;75;1;76;0
WireConnection;75;2;77;0
WireConnection;75;3;78;0
WireConnection;75;4;79;0
WireConnection;66;0;9;0
WireConnection;66;1;10;0
WireConnection;66;2;11;0
WireConnection;66;3;14;0
WireConnection;66;4;17;0
WireConnection;97;0;9;0
WireConnection;97;1;200;0
WireConnection;97;2;94;0
WireConnection;97;3;95;0
WireConnection;97;4;96;0
WireConnection;112;0;113;0
WireConnection;112;1;115;0
WireConnection;0;0;66;0
WireConnection;0;1;97;0
WireConnection;0;2;206;0
WireConnection;0;3;90;0
WireConnection;0;4;75;0
WireConnection;0;11;112;0
ASEEND*/
//CHKSM=D3F908C46AFF7ABD0FEE0534E893466B6AB74337