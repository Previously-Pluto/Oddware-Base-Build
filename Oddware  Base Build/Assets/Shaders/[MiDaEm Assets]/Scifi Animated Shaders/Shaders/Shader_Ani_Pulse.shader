Shader "MiDaEm/Ani_Pulse"
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
	
}
