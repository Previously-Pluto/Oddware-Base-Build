﻿Shader "Holistic/GlassUpdated"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_ScaleUV ("Scale", Range(1,1000)) = 1
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent"}
		GrabPass{}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 uvgrab : TEXCOORD1;
				float2 uvbump : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize; //The size of the pixels that are in the texture. "Resolution" of the pixel per say
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			float _ScaleUV;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                
                //add this to check if the image needs flipping
				# if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                # else
                float scale = 1.0f;
                # endif
                
                //include scale in this formulae as below
                o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
				
                
                o.uvgrab.zw = o.vertex.zw;
				o.uv = TRANSFORM_TEX( v.uv, _MainTex );
				o.uvbump = TRANSFORM_TEX( v.uv, _BumpMap );
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half2 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump )).rg; 
				float2 offset = bump * _ScaleUV * _GrabTexture_TexelSize.xy; //change textel size blended with the UV map
				i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
				
				fixed4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				fixed4 tint = tex2D(_MainTex, i.uv); //blends texture with grab texture
				col *= tint;
				return col;
			}
			ENDCG
		}
	}
}

