// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MiDaEm/Ani_Rotate_Offset_Mobile"
{
	Properties
	{
		_Color("Color", Color) = (0.1470588,0.6117646,1,1)
		_Alpha("Alpha", 2D) = "white" {}
		_AlphaOpacity("Alpha Opacity", Range( 0 , 1)) = 1
		_DisplacementSpeed("Displacement Speed", Float) = 1
		_DisplacementAmplitude("Displacement Amplitude", Float) = 1
		_DisplacementTimeOffset("Displacement Time Offset", Float) = 0
		[Toggle]_DisplacementAutoAnimation("Displacement Auto Animation", Float) = 1
		_DisplacementAnimationProgress("Displacement Animation Progress", Range( 0 , 1)) = 1
		_CircularMask("Circular Mask", 2D) = "white" {}
		_RotationSpeed("Rotation Speed", Float) = -2
		_RotationTimeOffset("Rotation Time Offset", Range( 0 , 1)) = 0
		[Toggle]_AutoRotateAnimation("Auto Rotate Animation", Float) = 1
		_RotationAnimationProgress("Rotation Animation Progress", Range( 0 , 1)) = 0
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
		#pragma surface surf Unlit alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _DisplacementAutoAnimation;
		uniform float _DisplacementAnimationProgress;
		uniform float _DisplacementTimeOffset;
		uniform float _DisplacementSpeed;
		uniform float _DisplacementAmplitude;
		uniform sampler2D _Alpha;
		uniform float _AutoRotateAnimation;
		uniform float4 _Alpha_ST;
		uniform float _RotationAnimationProgress;
		uniform float _RotationSpeed;
		uniform float _RotationTimeOffset;
		uniform float4 _Color;
		uniform float _AlphaOpacity;
		uniform sampler2D _CircularMask;
		uniform float4 _CircularMask_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( (0.0 + (sin( lerp((-1.3 + (_DisplacementAnimationProgress - 0.0) * (1.3 - -1.3) / (1.0 - 0.0)),( ( _Time.y + _DisplacementTimeOffset ) * _DisplacementSpeed ),_DisplacementAutoAnimation) ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) * ase_vertexNormal * _DisplacementAmplitude );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			float cos24 = cos( ( _RotationAnimationProgress * 6.28318548202515 ) );
			float sin24 = sin( ( _RotationAnimationProgress * 6.28318548202515 ) );
			float2 rotator24 = mul( uv_Alpha - float2( 0.5,0.5 ) , float2x2( cos24 , -sin24 , sin24 , cos24 )) + float2( 0.5,0.5 );
			float cos35 = cos( ( ( _Time.y * _RotationSpeed ) + ( _RotationTimeOffset * 6.28318548202515 ) ) );
			float sin35 = sin( ( ( _Time.y * _RotationSpeed ) + ( _RotationTimeOffset * 6.28318548202515 ) ) );
			float2 rotator35 = mul( uv_Alpha - float2( 0.5,0.5 ) , float2x2( cos35 , -sin35 , sin35 , cos35 )) + float2( 0.5,0.5 );
			float4 tex2DNode17 = tex2D( _Alpha, lerp(rotator24,rotator35,_AutoRotateAnimation) );
			o.Emission = ( tex2DNode17.r * _Color ).rgb;
			float2 uv_CircularMask = i.uv_texcoord * _CircularMask_ST.xy + _CircularMask_ST.zw;
			o.Alpha = ( ( tex2DNode17.r * _AlphaOpacity ) * tex2D( _CircularMask, uv_CircularMask ).r );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
298;29;2038;1364;2753.212;693.7763;1.887499;True;True
Node;AmplifyShaderEditor.RangedFloatNode;30;-1916.596,808.1121;Float;False;Property;_RotationTimeOffset;Rotation Time Offset;10;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;31;-1808.925,904.8871;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;1;-1808.669,506.3286;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-1802.027,657.0027;Float;False;Property;_RotationSpeed;Rotation Speed;9;0;Create;True;0;0;False;0;-2;-2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;58;-1034.513,1132.459;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1624.324,870.8881;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1746.61,-108.4501;Float;False;Property;_RotationAnimationProgress;Rotation Animation Progress;12;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-1097.68,1346.656;Float;False;Property;_DisplacementTimeOffset;Displacement Time Offset;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-1538.238,536.3735;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;29;-1623.642,-13.37402;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;25;-1570.947,-266.6716;Float;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;56;-1077.712,1487.755;Float;False;Property;_DisplacementSpeed;Displacement Speed;3;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-831.2155,1278.657;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1082.619,931.8571;Float;False;Property;_DisplacementAnimationProgress;Displacement Animation Progress;7;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1457.739,-45.67421;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-1345.543,460.3096;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;34;-1761.067,376.5447;Float;False;Constant;_Vector1;Vector 1;6;0;Create;True;0;0;False;0;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-1782.429,121.189;Float;True;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-1592.044,-564.1722;Float;True;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-701.3177,1395.956;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;24;-1196.751,-399.2714;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;53;-735.8175,913.4222;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1.3;False;4;FLOAT;1.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;35;-1316.287,258.295;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;9;-920.3766,-222.2498;Float;False;Property;_AutoRotateAnimation;Auto Rotate Animation;11;0;Create;True;0;0;False;0;1;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;52;-506.3165,1181.757;Float;False;Property;_DisplacementAutoAnimation;Displacement Auto Animation;6;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-340.1339,217.8251;Float;False;Property;_AlphaOpacity;Alpha Opacity;2;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;49;-250.3176,959.0571;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-592.7505,-241.9499;Float;True;Property;_Alpha;Alpha;1;0;Create;True;0;0;False;0;None;bfcc8b7436addba4ebbd8e64482aa192;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;-104.9499,-123.4502;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;0.1470588,0.6117646,1,1;0,0.9586205,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;43;-101.4183,1182.928;Float;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;48;-83.21841,967.558;Float;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;37;-146.3345,420.1259;Float;True;Property;_CircularMask;Circular Mask;8;0;Create;True;0;0;False;0;None;7bbc4a7742b733a4fb16ce136e0b905c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;47;-105.4183,1412.957;Float;False;Property;_DisplacementAmplitude;Displacement Amplitude;4;0;Create;True;0;0;False;0;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;1.566707,192.3255;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;100.5488,-322.771;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;229.9815,945.4597;Float;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;205.65,192.45;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;392,-27;Float;False;True;0;Float;ASEMaterialInspector;0;0;Unlit;MiDaEm/Ani_Rotate_Offset_Mobile;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;62;0;1;2
WireConnection;62;1;41;0
WireConnection;55;0;58;2
WireConnection;55;1;59;0
WireConnection;28;0;5;0
WireConnection;28;1;29;0
WireConnection;6;0;62;0
WireConnection;6;1;32;0
WireConnection;50;0;55;0
WireConnection;50;1;56;0
WireConnection;24;0;27;0
WireConnection;24;1;25;0
WireConnection;24;2;28;0
WireConnection;53;0;54;0
WireConnection;35;0;33;0
WireConnection;35;1;34;0
WireConnection;35;2;6;0
WireConnection;9;0;24;0
WireConnection;9;1;35;0
WireConnection;52;0;53;0
WireConnection;52;1;50;0
WireConnection;49;0;52;0
WireConnection;17;1;9;0
WireConnection;48;0;49;0
WireConnection;36;0;17;1
WireConnection;36;1;38;0
WireConnection;23;0;17;1
WireConnection;23;1;16;0
WireConnection;42;0;48;0
WireConnection;42;1;43;0
WireConnection;42;2;47;0
WireConnection;20;0;36;0
WireConnection;20;1;37;1
WireConnection;0;2;23;0
WireConnection;0;9;20;0
WireConnection;0;11;42;0
ASEEND*/
//CHKSM=E09BFEFBB61BB814FD6B1E33E693337F0158B8CC