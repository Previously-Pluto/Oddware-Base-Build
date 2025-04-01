Shader "MiDaEm/Ani_Scanline"
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
		#pragma target 3.0
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

}
