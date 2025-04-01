Shader "MiDaEm/Ani_Ring_Pulse"
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
		#pragma target 3.0
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

}
