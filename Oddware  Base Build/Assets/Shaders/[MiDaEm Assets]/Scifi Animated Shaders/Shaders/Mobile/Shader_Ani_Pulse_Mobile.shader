// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MiDaEm/Ani_Pulse_Mobile"
{
	Properties
	{
		_Color1("Color1", Color) = (1,0.06774914,0,0)
		_Color2("Color2", Color) = (1,0.9831489,0,0)
		_Alpha("Alpha", 2D) = "white" {}
		_Speed("Speed", Float) = 5
		_TimeOffSet("Time OffSet", Float) = 0
		[Toggle]_AutoAnimation("Auto Animation", Float) = 1
		_AnimationProgress("AnimationProgress", Range( 0 , 1)) = 1
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
		uniform float _TimeOffSet;
		uniform sampler2D _Alpha;
		uniform float4 _Alpha_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float temp_output_21_0 = (0.0 + (sin( lerp((-1.5 + (_AnimationProgress - 0.0) * (1.5 - -1.5) / (1.0 - 0.0)),( ( _Time.y * _Speed ) + _TimeOffSet ),_AutoAnimation) ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			float4 lerpResult19 = lerp( _Color1 , _Color2 , temp_output_21_0);
			o.Emission = lerpResult19.rgb;
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			o.Alpha = ( temp_output_21_0 * tex2D( _Alpha, uv_Alpha ).r );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
298;23;2038;1370;831.522;442.4763;1.399999;True;True
Node;AmplifyShaderEditor.RangedFloatNode;2;-1099.21,481.4493;Float;False;Property;_Speed;Speed;3;0;Create;True;0;0;False;0;5;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;1;-1160.21,327.4493;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-946.2121,643.4493;Float;False;Property;_TimeOffSet;Time OffSet;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1087.51,206.0497;Float;False;Property;_AnimationProgress;AnimationProgress;6;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-935.2121,374.4494;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-743.2117,397.4496;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;7;-764.2116,204.4498;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1.5;False;4;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;9;-515.5115,201.0498;Float;False;Property;_AutoAnimation;Auto Animation;5;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;22;-194,198;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-103.15,426.15;Float;True;Property;_Alpha;Alpha;2;0;Create;True;0;0;False;0;None;7eab2caaaf07cde48bbc5acdf06f3d7c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;21;-34,183;Float;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-283.45,-228.85;Float;False;Property;_Color1;Color1;0;0;Create;True;0;0;False;0;1,0.06774914,0,0;0.862069,1,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-275.45,-63.85001;Float;False;Property;_Color2;Color2;1;0;Create;True;0;0;False;0;1,0.9831489,0,0;0,1,0.1724138,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;200.55,194.15;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;19;153.4499,-99.34999;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;392,-27;Float;False;True;0;Float;ASEMaterialInspector;0;0;Unlit;MiDaEm/Ani_Pulse_Mobile;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;1;2
WireConnection;4;1;2;0
WireConnection;6;0;4;0
WireConnection;6;1;3;0
WireConnection;7;0;5;0
WireConnection;9;0;7;0
WireConnection;9;1;6;0
WireConnection;22;0;9;0
WireConnection;21;0;22;0
WireConnection;20;0;21;0
WireConnection;20;1;17;1
WireConnection;19;0;16;0
WireConnection;19;1;15;0
WireConnection;19;2;21;0
WireConnection;0;2;19;0
WireConnection;0;9;20;0
ASEEND*/
//CHKSM=6DB9078D3AF7DD99846203027B38CA508FCFC6D1