// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MiDaEm/Ani_Curtain_Mobile"
{
	Properties
	{
		_Color1("Color1", Color) = (0.9135497,1,0.145098,1)
		_Color2("Color2", Color) = (0.3846044,1,0.145098,1)
		_Alpha("Alpha", 2D) = "white" {}
		[Toggle]_VerticalAnimation("Vertical Animation", Float) = 1
		[Toggle]_InvertDirection("InvertDirection", Float) = 1
		_Speed("Speed", Float) = 1
		_TimeOffset("Time Offset", Float) = 0
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
		uniform float _AutoAnimation;
		uniform float _AnimationProgress;
		uniform float _Speed;
		uniform float _TimeOffset;
		uniform float _VerticalAnimation;
		uniform sampler2D _Alpha;
		uniform float4 _Alpha_ST;
		uniform float _InvertDirection;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			float clampResult46 = clamp( ( sin( lerp((-1.5 + (_AnimationProgress - 0.0) * (1.5 - -1.5) / (1.0 - 0.0)),( ( _Time.y * _Speed ) + _TimeOffset ),_AutoAnimation) ) + lerp(uv_Alpha.x,uv_Alpha.y,_VerticalAnimation) ) , 0.0 , 1.0 );
			float4 lerpResult42 = lerp( _Color1 , _Color2 , clampResult46);
			o.Emission = lerpResult42.rgb;
			o.Alpha = ( lerp(( 1.0 - clampResult46 ),clampResult46,_InvertDirection) * tex2D( _Alpha, uv_Alpha ).r );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
298;29;2038;1364;2875.363;971.6808;1.971106;True;True
Node;AmplifyShaderEditor.TimeNode;1;-1971.375,46.74801;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-1872.145,289.397;Float;False;Property;_Speed;Speed;5;0;Create;True;0;0;False;0;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1783.192,-150.3922;Float;False;Property;_AnimationProgress;Animation Progress;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1697.037,180.5944;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1822.495,423.7072;Float;False;Property;_TimeOffset;Time Offset;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-1458.707,163.0186;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;50;-1499.295,-147.5786;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1.5;False;4;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;9;-1110.044,16.61182;Float;False;Property;_AutoAnimation;Auto Animation;7;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-1270.916,385.9005;Float;True;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;48;-845.4379,27.11275;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;49;-898.3359,345.7162;Float;False;Property;_VerticalAnimation;Vertical Animation;3;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-689.2354,76.6108;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;46;-511.0339,83.2095;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;45;-238.1772,44.77627;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;44;-35.28307,103.5762;Float;False;Property;_InvertDirection;InvertDirection;4;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-99.54965,424.9501;Float;True;Property;_Alpha;Alpha;2;0;Create;True;0;0;False;0;None;bfcc8b7436addba4ebbd8e64482aa192;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;-354.7499,-448.2507;Float;False;Property;_Color1;Color1;0;0;Create;True;0;0;False;0;0.9135497,1,0.145098,1;0.1470588,0.6117646,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;43;-354.1834,-276.224;Float;False;Property;_Color2;Color2;1;0;Create;True;0;0;False;0;0.3846044,1,0.145098,1;0.1470588,0.6117646,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;42;-28.68278,-316.8205;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;205.65,192.45;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;392,-27;Float;False;True;0;Float;ASEMaterialInspector;0;0;Unlit;MiDaEm/Ani_Curtain_Mobile;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;40;0;1;2
WireConnection;40;1;41;0
WireConnection;6;0;40;0
WireConnection;6;1;30;0
WireConnection;50;0;5;0
WireConnection;9;0;50;0
WireConnection;9;1;6;0
WireConnection;48;0;9;0
WireConnection;49;0;33;1
WireConnection;49;1;33;2
WireConnection;47;0;48;0
WireConnection;47;1;49;0
WireConnection;46;0;47;0
WireConnection;45;0;46;0
WireConnection;44;0;45;0
WireConnection;44;1;46;0
WireConnection;42;0;16;0
WireConnection;42;1;43;0
WireConnection;42;2;46;0
WireConnection;20;0;44;0
WireConnection;20;1;17;1
WireConnection;0;2;42;0
WireConnection;0;9;20;0
ASEEND*/
//CHKSM=53E2138063D8062E3BDB9C7E8B8037ADCCB309DA