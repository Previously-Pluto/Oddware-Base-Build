// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MiDaEm/Ani_Scanline_Mobile"
{
	Properties
	{
		_Color1("Color 1", Color) = (0,0.3488501,0.6132076,0)
		_Color2("Color 2", Color) = (0.1179245,0.8937367,1,0)
		_Alpha("Alpha", 2D) = "white" {}
		_AlphaOpacity("Alpha Opacity", Range( 0 , 1)) = 1
		_LineMask("Line Mask", 2D) = "white" {}
		_LineOpacity("Line Opacity", Range( 0 , 1)) = 1
		_VerticalSpeed("Vertical Speed", Float) = 1
		_HorizontalSpeed("HorizontalSpeed", Float) = 1
		_Angle("Angle", Range( 0 , 360)) = 0
		_TimeOffset("Time Offset", Range( 0 , 1)) = 0
		[Toggle]_AutoAnimation("Auto Animation", Float) = 1
		_AnimationProgress("Animation Progress", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color1;
		uniform float4 _Color2;
		uniform sampler2D _LineMask;
		uniform float4 _LineMask_ST;
		uniform float _Angle;
		uniform float _AutoAnimation;
		uniform float _AnimationProgress;
		uniform float _HorizontalSpeed;
		uniform float _VerticalSpeed;
		uniform float _TimeOffset;
		uniform float _LineOpacity;
		uniform sampler2D _Alpha;
		uniform float4 _Alpha_ST;
		uniform float _AlphaOpacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_LineMask = i.uv_texcoord * _LineMask_ST.xy + _LineMask_ST.zw;
			float cos35 = cos( ( ( _Angle / 180.0 ) * UNITY_PI ) );
			float sin35 = sin( ( ( _Angle / 180.0 ) * UNITY_PI ) );
			float2 rotator35 = mul( uv_LineMask - float2( 0.5,0.5 ) , float2x2( cos35 , -sin35 , sin35 , cos35 )) + float2( 0.5,0.5 );
			float4 appendResult32 = (float4(_HorizontalSpeed , _VerticalSpeed , 0.0 , 0.0));
			float4 tex2DNode28 = tex2D( _LineMask, ( ( float4( rotator35, 0.0 , 0.0 ) + ( lerp(_AnimationProgress,_Time.y,_AutoAnimation) * appendResult32 ) ) + _TimeOffset ).xy );
			float4 lerpResult19 = lerp( _Color1 , _Color2 , ( tex2DNode28.r * _LineOpacity ));
			o.Emission = lerpResult19.rgb;
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			float4 tex2DNode17 = tex2D( _Alpha, uv_Alpha );
			o.Alpha = ( ( tex2DNode28.r * tex2DNode17.r ) + ( tex2DNode17.r * _AlphaOpacity ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
298;29;2038;1364;1771.313;363.1787;1.5606;True;True
Node;AmplifyShaderEditor.RangedFloatNode;38;-2523.22,94.19249;Float;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;False;0;180;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-2610.668,-34.18843;Float;False;Property;_Angle;Angle;8;0;Create;True;0;0;False;0;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-2058.951,925.0682;Float;False;Property;_VerticalSpeed;Vertical Speed;6;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-2080.857,780.4742;Float;False;Property;_HorizontalSpeed;HorizontalSpeed;7;0;Create;True;0;0;False;0;1;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;41;-2358.095,218.9248;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;1;-2284.459,624.3152;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-2291.702,520.2007;Float;False;Property;_AnimationProgress;Animation Progress;11;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;39;-2278.507,22.28937;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;42;-1933.615,306.3187;Float;False;Constant;_Vector0;Vector 0;12;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-2075.632,100.3193;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;9;-1875.268,530.3249;Float;False;Property;_AutoAnimation;Auto Animation;10;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;32;-1764.74,762.641;Float;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1864.93,-67.56143;Float;False;0;28;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;35;-1551.931,179.9651;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1479.537,598.4353;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1290.769,416.2419;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1341.685,693.4902;Float;False;Property;_TimeOffset;Time Offset;9;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-1070.037,403.2369;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-866.2438,550.3989;Float;False;Property;_LineOpacity;Line Opacity;5;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;28;-880.9575,320.618;Float;True;Property;_LineMask;Line Mask;4;0;Create;True;0;0;False;0;84ab4da4bef142f45ab3375d3747c9f8;7eab2caaaf07cde48bbc5acdf06f3d7c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-405.2744,612.4599;Float;True;Property;_Alpha;Alpha;2;0;Create;True;0;0;False;0;6a237f4a9231eb947a6054f5a0ef6828;7eab2caaaf07cde48bbc5acdf06f3d7c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-387.1981,846.4995;Float;False;Property;_AlphaOpacity;Alpha Opacity;3;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-77.01849,323.8246;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;36.78216,636.0198;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-221.0309,149.5995;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-283.45,-228.85;Float;False;Property;_Color1;Color 1;0;0;Create;True;0;0;False;0;0,0.3488501,0.6132076,0;0.862069,1,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-275.45,-63.85001;Float;False;Property;_Color2;Color 2;1;0;Create;True;0;0;False;0;0.1179245,0.8937367,1,0;0,1,0.1724138,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;19;153.4499,-99.34999;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;165.6888,334.9028;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;392,-27;Float;False;True;0;Float;ASEMaterialInspector;0;0;Unlit;MiDaEm/Ani_Scanline_Mobile;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;39;0;37;0
WireConnection;39;1;38;0
WireConnection;40;0;39;0
WireConnection;40;1;41;0
WireConnection;9;0;5;0
WireConnection;9;1;1;2
WireConnection;32;0;2;0
WireConnection;32;1;3;0
WireConnection;35;0;36;0
WireConnection;35;1;42;0
WireConnection;35;2;40;0
WireConnection;31;0;9;0
WireConnection;31;1;32;0
WireConnection;30;0;35;0
WireConnection;30;1;31;0
WireConnection;33;0;30;0
WireConnection;33;1;34;0
WireConnection;28;1;33;0
WireConnection;24;0;28;1
WireConnection;24;1;17;1
WireConnection;26;0;17;1
WireConnection;26;1;27;0
WireConnection;23;0;28;1
WireConnection;23;1;29;0
WireConnection;19;0;16;0
WireConnection;19;1;15;0
WireConnection;19;2;23;0
WireConnection;25;0;24;0
WireConnection;25;1;26;0
WireConnection;0;2;19;0
WireConnection;0;9;25;0
ASEEND*/
//CHKSM=8E2F68D4D4F556C817007A970858B2AA9B5DEB9A