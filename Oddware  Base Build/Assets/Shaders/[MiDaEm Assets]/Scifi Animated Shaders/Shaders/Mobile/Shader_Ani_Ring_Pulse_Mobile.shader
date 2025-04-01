// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MiDaEm/Ani_Ring_Pulse_Mobile"
{
	Properties
	{
		_Color1("Color1", Color) = (1,0.06774914,0,0)
		_Color2("Color2", Color) = (1,0.9831489,0,0)
		_Alpha("Alpha", 2D) = "white" {}
		_Speed("Speed", Float) = -2
		_TimeOffSet("Time OffSet", Range( 0 , 1)) = 0
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
		uniform sampler2D _Alpha;
		uniform float4 _Alpha_ST;
		uniform float4 _Color2;
		uniform float _AutoAnimation;
		uniform float _AnimationProgress;
		uniform float _Speed;
		uniform float _TimeOffSet;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			float4 tex2DNode27 = tex2D( _Alpha, uv_Alpha );
			float2 temp_output_50_0 = (float2( -1,-1 ) + (uv_Alpha - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
			float2 temp_output_51_0 = ( temp_output_50_0 * temp_output_50_0 );
			float2 temp_output_45_0 = (float2( -1,-1 ) + (uv_Alpha - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
			float2 temp_cast_0 = (( sin( ( frac( lerp((-1.5 + (_AnimationProgress - 0.0) * (1.5 - -1.5) / (1.0 - 0.0)),( ( ( _Time.y / 5.0 ) * _Speed ) + _TimeOffSet ),_AutoAnimation) ) * -1.0 ) ) * 1.0 )).xx;
			float2 temp_output_43_0 = ( ( temp_output_45_0 * temp_output_45_0 ) - temp_cast_0 );
			float2 temp_cast_1 = (( sin( ( frac( lerp((-1.5 + (_AnimationProgress - 0.0) * (1.5 - -1.5) / (1.0 - 0.0)),( ( ( _Time.y / 5.0 ) * _Speed ) + _TimeOffSet ),_AutoAnimation) ) * -1.0 ) ) * 1.0 )).xx;
			float temp_output_40_0 = ( (temp_output_43_0).x + (temp_output_43_0).y );
			float clampResult31 = clamp( ( ( pow( temp_output_40_0 , 3.0 ) * ( 1.0 - pow( ( temp_output_40_0 + -0.1 ) , 5.0 ) ) ) * 2.0 ) , 0.0 , 1.0 );
			float temp_output_30_0 = ( ( 1.0 - pow( ( (temp_output_51_0).x + (temp_output_51_0).y ) , 1.5 ) ) * clampResult31 );
			o.Emission = ( ( _Color1 * tex2DNode27.r ) + ( ( _Color2 * tex2DNode27.r ) * temp_output_30_0 ) ).rgb;
			o.Alpha = ( tex2DNode27.r * temp_output_30_0 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
298;29;2038;1364;2145.436;1004.567;1.787674;True;True
Node;AmplifyShaderEditor.TimeNode;1;-4169.226,423.0948;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-3813.311,629.4456;Float;False;Property;_Speed;Speed;3;0;Create;True;0;0;False;0;-2;-2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;68;-3883.9,473.6776;Float;False;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-3649.313,522.4449;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-3658.568,791.4459;Float;False;Property;_TimeOffSet;Time OffSet;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-3801.611,354.0459;Float;False;Property;_AnimationProgress;AnimationProgress;6;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;7;-3479.953,352.446;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1.5;False;4;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-3457.313,545.4452;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;9;-3229.612,349.0461;Float;False;Property;_AutoAnimation;Auto Animation;5;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;67;-2918.175,351.3674;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-2704.558,353.1122;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;46;-3052.8,6.582148;Float;True;0;27;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;65;-2524.811,356.6022;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;45;-2790.508,32.3767;Float;True;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-2347.339,356.6027;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-2490.076,32.24261;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;43;-2123.961,258.0826;Float;True;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;41;-1891.617,233.4619;Float;True;True;False;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;63;-1888.403,466.5369;Float;True;False;True;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-1620.77,319.1569;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-2836.348,-429.7484;Float;True;0;27;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;50;-2563.795,-444.0888;Float;True;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-1368.411,416.8071;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-2271.874,-430.2629;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;37;-1281.632,632.3782;Float;False;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;53;-1997.986,-424.483;Float;True;True;False;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;36;-1373.221,169.0329;Float;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;62;-2000.08,-151.1915;Float;True;False;True;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;35;-1165.657,493.4969;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1154.007,227.0667;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-1702.709,-419.0579;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-958.2865,344.2729;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;56;-1461.291,-416.4457;Float;True;2;0;FLOAT;0;False;1;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;31;-746.9371,413.408;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;58;-1162.091,-365.1226;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;-538.572,-274.9052;Float;False;Property;_Color2;Color2;1;0;Create;True;0;0;False;0;1,0.9831489,0,0;1,0.9686275,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;27;-546.8723,-511.5471;Float;True;Property;_Alpha;Alpha;2;0;Create;True;0;0;False;0;None;1fd68f2407b70b74a945312d51850ed3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;-516.6414,-713.1088;Float;False;Property;_Color1;Color1;0;0;Create;True;0;0;False;0;1,0.06774914,0,0;1,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-176.2849,-394.8957;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-495.6878,102.0427;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;27.07481,-371.5015;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-159.8444,-679.3306;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;193.6649,-381.097;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;128.7943,-621.119;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;479.6247,-440.0741;Float;False;True;0;Float;ASEMaterialInspector;0;0;Unlit;MiDaEm/Ani_Ring_Pulse_Mobile;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;68;0;1;2
WireConnection;4;0;68;0
WireConnection;4;1;2;0
WireConnection;7;0;5;0
WireConnection;6;0;4;0
WireConnection;6;1;3;0
WireConnection;9;0;7;0
WireConnection;9;1;6;0
WireConnection;67;0;9;0
WireConnection;66;0;67;0
WireConnection;65;0;66;0
WireConnection;45;0;46;0
WireConnection;64;0;65;0
WireConnection;44;0;45;0
WireConnection;44;1;45;0
WireConnection;43;0;44;0
WireConnection;43;1;64;0
WireConnection;41;0;43;0
WireConnection;63;0;43;0
WireConnection;40;0;41;0
WireConnection;40;1;63;0
WireConnection;50;0;49;0
WireConnection;38;0;40;0
WireConnection;51;0;50;0
WireConnection;51;1;50;0
WireConnection;37;0;38;0
WireConnection;53;0;51;0
WireConnection;36;0;40;0
WireConnection;62;0;51;0
WireConnection;35;0;37;0
WireConnection;34;0;36;0
WireConnection;34;1;35;0
WireConnection;54;0;53;0
WireConnection;54;1;62;0
WireConnection;32;0;34;0
WireConnection;56;0;54;0
WireConnection;31;0;32;0
WireConnection;58;0;56;0
WireConnection;28;0;15;0
WireConnection;28;1;27;1
WireConnection;30;0;58;0
WireConnection;30;1;31;0
WireConnection;24;0;28;0
WireConnection;24;1;30;0
WireConnection;26;0;16;0
WireConnection;26;1;27;1
WireConnection;23;0;26;0
WireConnection;23;1;24;0
WireConnection;29;0;27;1
WireConnection;29;1;30;0
WireConnection;0;2;23;0
WireConnection;0;9;29;0
ASEEND*/
//CHKSM=54BE92088817BECA3451472A53E3E8EC6C3F1201