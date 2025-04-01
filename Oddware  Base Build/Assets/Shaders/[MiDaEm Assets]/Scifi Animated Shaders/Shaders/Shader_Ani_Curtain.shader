Shader "MiDaEm/Ani_Curtain"
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
		#pragma target 3.0
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

}
