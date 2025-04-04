// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "UberShaderFX_Hero_B"
{
	Properties
	{
		[Gamma]_MainTex("DiffuseMap", 2D) = "white" {}
		[NoScaleOffset]_MaskA_Text("MaskMap", 2D) = "white" {}
		_MaskA_OffsetX("MaskA_OffsetX", Float) = 0
		_MaskA_OffsetY("MaskA_OffsetY", Float) = 0
		[Toggle(_HORIZVERTGRADIENTCOLOR_ON)] _HorizVertGradientColor("Horiz/Vert GradientColor", Float) = 0
		[Toggle(_HORIZVERTGRADIENT_ON)] _HorizVertGradient("Horiz/Vert Gradient", Float) = 0
		_GradientColorA("GradientColorA", Color) = (0,0,0,0)
		_GradientColorB("GradientColorB", Color) = (0,0,0,0)
		_Grad_Min_Color("Grad_Min_Color", Range( 0 , 1)) = 0.5
		_Grad_Min("Grad_Min", Range( 0 , 1)) = 0
		_GradientFaloff("GradientFaloff", Range( 0 , 1)) = 0
		_GradientFaloff_Color("GradientFaloff_Color", Range( 0 , 1)) = 0
		[Toggle]_ShapesMode("ShapesMode", Float) = 0
		_EmissiveMult("EmissiveMult", Range( 0 , 10)) = 1
		_ScaleOffset("ScaleOffset", Vector) = (1,1,0,0)
		[Toggle]_GradientMode("GradientMode", Float) = 0
		[Toggle]_GradientColor("GradientColor", Float) = 0
		_MaskScaleOffset("MaskScaleOffset", Vector) = (1,1,0,0)
		_AtlasPtc("AtlasPtc", 2D) = "white" {}
		[Enum(Arrow,0,Diamond,1,Exclamation mark,2,Price,3,Plus,4,Question Mark,5)]_Shape("Shape", Int) = 0
		[Toggle]_Outline("Outline", Float) = 0
		_OutlineIntensity("OutlineIntensity", Range( 0 , 25)) = 1
		[Toggle]_OutlineOnly("OutlineOnly", Float) = 0
		[Toggle]_Dissolve("Dissolve", Float) = 0
		[NoScaleOffset]_DissolveMask("Dissolve Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _HORIZVERTGRADIENTCOLOR_ON
		#pragma shader_feature_local _HORIZVERTGRADIENT_ON
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _ShapesMode;
		uniform sampler2D _MainTex;
		uniform float4 _ScaleOffset;
		uniform float _OutlineOnly;
		uniform float _Outline;
		uniform sampler2D _AtlasPtc;
		uniform int _Shape;
		uniform float _OutlineIntensity;
		uniform float _EmissiveMult;
		uniform float _GradientColor;
		uniform float4 _GradientColorA;
		uniform float4 _GradientColorB;
		uniform float _Grad_Min_Color;
		uniform float _GradientFaloff_Color;
		uniform float _Dissolve;
		uniform float _GradientMode;
		uniform sampler2D _MaskA_Text;
		SamplerState sampler_MaskA_Text;
		uniform float4 _MaskScaleOffset;
		uniform float _MaskA_OffsetX;
		uniform float _MaskA_OffsetY;
		uniform float _Grad_Min;
		uniform float _GradientFaloff;
		uniform sampler2D _DissolveMask;
		SamplerState sampler_DissolveMask;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 break121 = _ScaleOffset;
			float2 appendResult26 = (float2(break121.z , break121.w));
			float2 appendResult14 = (float2(break121.x , break121.y));
			float2 panner23 = ( 1.0 * _Time.y * appendResult26 + (i.uv_texcoord*appendResult14 + 0.0));
			float temp_output_4_0_g1 = 3.0;
			float temp_output_5_0_g1 = 2.0;
			float2 appendResult7_g1 = (float2(temp_output_4_0_g1 , temp_output_5_0_g1));
			float totalFrames39_g1 = ( temp_output_4_0_g1 * temp_output_5_0_g1 );
			float2 appendResult8_g1 = (float2(totalFrames39_g1 , temp_output_5_0_g1));
			float clampResult42_g1 = clamp( (float)_Shape , 0.0001 , ( totalFrames39_g1 - 1.0 ) );
			float temp_output_35_0_g1 = frac( ( ( 0.0001 + clampResult42_g1 ) / totalFrames39_g1 ) );
			float2 appendResult29_g1 = (float2(temp_output_35_0_g1 , ( 1.0 - temp_output_35_0_g1 )));
			float2 temp_output_15_0_g1 = ( ( i.uv_texcoord / appendResult7_g1 ) + ( floor( ( appendResult8_g1 * appendResult29_g1 ) ) / appendResult7_g1 ) );
			float4 break62 = tex2D( _AtlasPtc, temp_output_15_0_g1 );
			float clampResult65 = clamp( break62.g , 0.0 , 1.0 );
			float temp_output_185_0 = ( break62.r * _OutlineIntensity );
			float4 temp_cast_1 = ((( _OutlineOnly )?( temp_output_185_0 ):( (( _Outline )?( max( clampResult65 , temp_output_185_0 ) ):( clampResult65 )) ))).xxxx;
			#ifdef _HORIZVERTGRADIENTCOLOR_ON
				float staticSwitch140 = i.uv_texcoord.y;
			#else
				float staticSwitch140 = i.uv_texcoord.x;
			#endif
			float smoothstepResult142 = smoothstep( _Grad_Min_Color , ( _Grad_Min_Color + _GradientFaloff_Color ) , staticSwitch140);
			float4 lerpResult45 = lerp( _GradientColorA , _GradientColorB , smoothstepResult142);
			float4 Color157 = ( (( _ShapesMode )?( temp_cast_1 ):( tex2D( _MainTex, panner23 ) )) * ( _EmissiveMult * (( _GradientColor )?( lerpResult45 ):( i.vertexColor )) ) );
			o.Emission = Color157.rgb;
			float4 break131 = _MaskScaleOffset;
			float2 appendResult79 = (float2(break131.z , break131.w));
			float2 appendResult73 = (float2(break131.x , break131.y));
			float2 appendResult76 = (float2(_MaskA_OffsetX , _MaskA_OffsetY));
			float2 panner70 = ( 1.0 * _Time.y * appendResult79 + (i.uv_texcoord*appendResult73 + appendResult76));
			float4 tex2DNode5 = tex2D( _MaskA_Text, panner70 );
			#ifdef _HORIZVERTGRADIENT_ON
				float staticSwitch46 = i.uv_texcoord.y;
			#else
				float staticSwitch46 = i.uv_texcoord.x;
			#endif
			float smoothstepResult107 = smoothstep( _Grad_Min , ( _Grad_Min + _GradientFaloff ) , staticSwitch46);
			float clampResult192 = clamp( i.vertexColor.a , 0.0 , 1.0 );
			float Opacity125 = ( (( _GradientMode )?( smoothstepResult107 ):( tex2DNode5.r )) * ( clampResult192 * (( _ShapesMode )?( (( _OutlineOnly )?( temp_output_185_0 ):( (( _Outline )?( max( clampResult65 , temp_output_185_0 ) ):( clampResult65 )) )) ):( 1.0 )) ) );
			float clampResult193 = clamp( Opacity125 , 0.0 , 1.0 );
			float temp_output_238_0 = ( 1.0 - i.vertexColor.a );
			float clampResult220 = clamp( ( tex2D( _DissolveMask, i.uv_texcoord ).r - ( ( temp_output_238_0 - 0.5 ) * 2.0 ) ) , 0.0 , 1.0 );
			o.Alpha = (( _Dissolve )?( ( clampResult193 * clampResult220 ) ):( clampResult193 ));
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ParticlesMasterShader_GUI"
}
/*ASEBEGIN
Version=18500
0;6;2560;1373;5390.688;5592.64;1;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;150;-4661.106,-2991.855;Inherit;True;Property;_AtlasPtc;AtlasPtc;38;0;Create;True;0;0;False;0;False;ce1fd9b901064364ebec763d672f66cf;ce1fd9b901064364ebec763d672f66cf;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;147;-4653.316,-2763.628;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;156;-4551.439,-2625.724;Inherit;False;Property;_Shape;Shape;39;1;[Enum];Create;True;6;Arrow;0;Diamond;1;Exclamation mark;2;Price;3;Plus;4;Question Mark;5;0;False;0;False;0;0;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-3976.731,-2817.932;Inherit;False;Constant;_Float1;Float 1;44;0;Create;True;0;0;False;0;False;0.0001;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;67;-3483.898,-4217.636;Inherit;False;957.562;965.4409;Offset And Panner;14;79;78;77;76;75;74;73;72;71;70;69;68;131;132;;0.2311321,1,0.8974844,1;0;0
Node;AmplifyShaderEditor.FunctionNode;146;-3789.185,-2986.959;Inherit;False;Flipbook;-1;;1;53c2488c220f6564ca6c90721ee16673;2,71,1,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;3;False;5;FLOAT;2;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;17.22;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.CommentaryNode;63;-3462.704,-3143.167;Inherit;False;866;407;Shapes;5;60;61;59;62;64;;0,1,0.5549088,1;0;0
Node;AmplifyShaderEditor.BreakToComponentsNode;62;-2849.704,-2991.167;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;186;-2534.375,-2700.513;Inherit;False;Property;_OutlineIntensity;OutlineIntensity;41;0;Create;True;0;0;False;0;False;1;0;0;25;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;132;-2973.008,-3624.701;Inherit;False;Property;_MaskScaleOffset;MaskScaleOffset;37;0;Create;True;0;0;False;0;False;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;-2200.647,-2866.308;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;65;-2427.28,-3076.601;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-3393.622,-3468.195;Inherit;False;Property;_MaskA_OffsetX;MaskA_OffsetX;10;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-3394.622,-3368.195;Inherit;False;Property;_MaskA_OffsetY;MaskA_OffsetY;11;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;131;-2757.56,-3846.778;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;66;-3516.772,-2258.9;Inherit;False;957.562;965.4409;Offset And Panner;14;12;11;23;20;16;14;27;24;26;15;21;22;120;121;;0.2311321,1,0.8974844,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;109;-3894.076,-5402.929;Inherit;False;1373.871;1123.648;GradientColor;11;44;106;105;108;46;48;47;107;45;50;49;;1,0.7341389,0,1;0;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;194;-1730.627,-2892.295;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;68;-3396.312,-3813.711;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;76;-3217.625,-3407.195;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;73;-3201.048,-3643.983;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;120;-2966.266,-1475.465;Inherit;False;Property;_ScaleOffset;ScaleOffset;33;0;Create;True;0;0;False;0;False;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;183;-1533.559,-3075.783;Inherit;False;Property;_Outline;Outline;40;0;Create;True;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;69;-2986.477,-3816.266;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;79;-3181.348,-4146.258;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-3349.031,-4504.281;Inherit;False;Property;_Grad_Min;Grad_Min;24;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-3333.031,-4395.281;Inherit;False;Property;_GradientFaloff;GradientFaloff;26;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-3844.076,-4786.677;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;70;-2731.336,-4167.635;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;139;-4979.566,-5125.563;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;138;-4468.522,-4734.167;Inherit;False;Property;_GradientFaloff_Color;GradientFaloff_Color;27;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;46;-3513.061,-4770.877;Inherit;False;Property;_HorizVertGradient;Horiz/Vert Gradient;15;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;129;-1069.011,-3456.717;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;137;-4484.522,-4843.167;Inherit;False;Property;_Grad_Min_Color;Grad_Min_Color;23;0;Create;True;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-1273.375,-2780.751;Inherit;False;Constant;_Float0;Float 0;32;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;190;-1295.15,-3211.928;Inherit;False;Property;_OutlineOnly;OutlineOnly;42;0;Create;True;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;108;-3022.409,-4484.729;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;235;-2128.934,-1325.586;Inherit;False;1828.282;797.9758;DISSOLVE;12;217;220;233;224;219;231;234;232;218;222;223;238;;1,1,1,1;0;0
Node;AmplifyShaderEditor.BreakToComponentsNode;121;-2813.71,-1699.299;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ToggleSwitchNode;127;-900.8137,-2779.136;Inherit;False;Property;_ShapesMode;ShapesMode;32;0;Create;False;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;233;-1737.032,-747.6104;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;14;-3233.922,-1685.247;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;192;-806.9031,-3353.54;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-1891.826,-4202.006;Inherit;True;Property;_MaskA_Text;MaskMap;7;1;[NoScaleOffset];Create;False;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-3429.186,-1854.975;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;141;-4157.899,-4823.615;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;107;-2905.274,-4643.852;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;140;-4648.552,-5109.763;Inherit;False;Property;_HorizVertGradientColor;Horiz/Vert GradientColor;14;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;142;-4040.766,-4982.738;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;11;-3019.35,-1857.531;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;135;-1428.18,-4672.259;Inherit;False;Property;_GradientMode;GradientMode;34;0;Create;False;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;26;-3214.222,-2187.522;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;47;-3703.786,-5363.823;Inherit;False;Property;_GradientColorA;GradientColorA;16;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;238;-1513.532,-647.9812;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;48;-3706.41,-5176.834;Inherit;False;Property;_GradientColorB;GradientColorB;17;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-716.5844,-3152.623;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;118;-2266.441,-1973.165;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;-532.1274,-4167.501;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;231;-1394.865,-1005.931;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;45;-2702.205,-5243.991;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;23;-2764.21,-2208.9;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;217;-2078.934,-1235.486;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;-1235.077,-1007.063;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;136;-1955.237,-1980.086;Inherit;False;Property;_GradientColor;GradientColor;35;0;Create;False;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;125;-290.0223,-4173.512;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;224;-1719.547,-1263.573;Inherit;True;Property;_DissolveMask;Dissolve Mask;45;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;427d49edbf9e3dc4a80cf09db4542712;427d49edbf9e3dc4a80cf09db4542712;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-2423.808,-2289.044;Inherit;True;Property;_MainTex;DiffuseMap;0;1;[Gamma];Create;False;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;117;-2097.877,-1562.897;Inherit;False;Property;_EmissiveMult;EmissiveMult;31;0;Create;True;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;218;-1108.934,-1231.486;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;122;-1678.331,-2284.306;Inherit;False;Property;_ShapesMode;ShapesMode;31;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;-1778.163,-1876.969;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;126;-969.1267,-2073.864;Inherit;False;125;Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1386.702,-2278.336;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;193;-768.4494,-2068.238;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;220;-906.9344,-1229.486;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;157;-1130.812,-2281.75;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;112;-2514.926,2195.953;Inherit;False;1412.972;542.0398;DissolveOperations;7;99;100;101;110;98;103;116;;0.1338398,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;113;-2507.24,2767.866;Inherit;False;1408.361;711.2222;DonutShape;7;56;32;28;53;42;51;52;;0.5720265,0.1396849,0.6886792,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;222;-698.5715,-1253.595;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;53;-1734.121,3072.817;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;242;-276.5942,-1616.61;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;244;-419.5942,-1644.61;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;134;-1551.488,-4001.496;Inherit;False;Property;_UseShapeAlpha;UseShapeAlpha;34;0;Create;True;0;0;False;0;False;0;2;0;FLOAT;1;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;59;-3126.323,-3093.466;Inherit;False;float PI = 3.14159265359@$float TWO_PI = 6.28318530718@$ float3 color = float3(0,0,0)@$ float d = 0.0@$$//Circle Mask$$float2 dist = Coord-float2(0.5,0.5)@$float _radius = 1.0@$float CircleMask =  1.-smoothstep(_radius-(_radius*0.5),_radius+(_radius*0.5),dot(dist,dist)*4.0)@$/////$$  // Remap the space to -1. to 1.$  Coord = Coord *2.-1.@$$  // Number of sides of your shape$  int N = Sides@$$  // Angle and radius from the current pixel$  float a = atan2(Coord.x,Coord.y)+PI@$  float r = TWO_PI/float(N)@$$  // Shaping function that modulate the distance$$ d = cos(floor(.5+a/r)*r-a)*length(Coord)@$$// Add Glow$float d2= pow(clamp(1-d,0,1),2)@$$$$float temp = 1.0 - smoothstep(.4, .41, d)@$$color = float3(temp,temp,temp) + (d2*GlowFactor * CircleMask)@$$$  float4 Out_Color = float4(color,1.0)@$  $  return Out_Color@;4;False;3;True;Coord;FLOAT2;0,0;In;;Inherit;False;True;Sides;FLOAT;3;In;;Inherit;False;True;GlowFactor;FLOAT;0;In;;Inherit;False;My Custom Expression;True;False;0;3;0;FLOAT2;0,0;False;1;FLOAT;3;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StaticSwitch;115;-753.5186,2097.951;Inherit;False;Property;_lol;lol;30;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CeilOpNode;56;-1279.128,3058.78;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-3381.047,-3657.983;Inherit;False;Property;_MaskA_TileX;MaskA_TileX;8;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;239;-387.5942,-1977.61;Inherit;False;Constant;_Vector0;Vector 0;48;0;Create;True;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;99;-1986.71,2514.278;Inherit;False;Property;_Min;Min;22;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-3464.1,-2108.683;Inherit;False;Property;_ScrollingSpeedY;ScrollingSpeedY;6;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-3410.92,-1601.247;Inherit;False;Property;_TileY;TileY;2;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-3466.772,-2194.203;Inherit;False;Property;_ScrollingSpeedX;ScrollingSpeedX;5;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;110;-1997.321,2245.953;Inherit;True;Property;_TextureSample0;Texture Sample 0;28;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;243;-572.5942,-1779.61;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;32;-2171.592,3325.269;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-3378.047,-3559.983;Inherit;False;Property;_MaskA_TileY;MaskA_TileY;9;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;98;-1542.953,2374.708;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LengthOpNode;144;-4445.373,-4955.129;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-3433.898,-4152.939;Inherit;False;Property;_MaskA_ScrollingSpeedX;MaskA_ScrollingSpeedX;12;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;60;-3361.704,-3093.167;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;100;-1970.71,2623.278;Inherit;False;Property;_NewRange;NewRange;25;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-2457.24,3328.156;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;245;-316.5942,-1732.61;Inherit;False;Constant;_Float2;Float 2;48;0;Create;True;0;0;False;0;False;2.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;49;-3542.728,-4612.298;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LengthOpNode;50;-3309.882,-4616.243;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;103;-1278.954,2376.683;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;2.2;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;143;-4678.219,-4951.184;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;234;-1517.315,-874.2786;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;223;-524.6529,-1275.586;Inherit;False;Property;_Dissolve;Dissolve;44;0;Create;True;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-3421.488,-2864.906;Inherit;False;Property;_EdgyGlowFactor;EdgyGlowFactor;21;0;Create;True;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;240;-49.59424,-1751.61;Inherit;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;158;-645.2307,-2211.455;Inherit;False;157;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;42;-1971.601,3192.987;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.25;False;2;FLOAT;0.42;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;123;-1097.243,-2928.552;Inherit;False;Property;_UseShapeAlpha;UseShapeAlpha;36;0;Create;True;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;114;-925.1027,2410.651;Inherit;False;Property;_ToggleSwitch0;Toggle Switch0;29;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-3378.704,-1385.436;Inherit;False;Property;_OffsetY;OffsetY;4;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-2303.876,3071.175;Inherit;True;Property;_InnerRadius;InnerRadius;19;0;Create;True;0;0;False;0;False;0.42;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-3201.2,-1445.45;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;116;-2454.143,2273.96;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;101;-1660.088,2533.83;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;219;-1856.198,-882.1714;Inherit;False;Property;_DissolveFactor;DissolveFactor;43;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-3413.92,-1699.247;Inherit;False;Property;_TileX;TileX;1;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-3424.704,-2967.167;Inherit;False;Property;_Sides;Sides;20;1;[IntRange];Create;True;0;0;False;0;False;3;3;3;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2309.948,2817.866;Inherit;True;Property;_OuterRadius;OuterRadius;18;0;Create;True;0;0;False;0;False;0.5;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-3397.216,-1491.941;Inherit;False;Property;_OffsetX;OffsetX;3;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-3432.624,-4067.419;Inherit;False;Property;_MaskA_ScrollingSpeedY;MaskA_ScrollingSpeedY;13;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-120.7773,-2244.2;Float;False;True;-1;2;ParticlesMasterShader_GUI;0;0;Unlit;UberShaderFX_Hero_B;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;146;51;150;0
WireConnection;146;13;147;0
WireConnection;146;24;156;0
WireConnection;146;2;154;0
WireConnection;62;0;146;53
WireConnection;185;0;62;0
WireConnection;185;1;186;0
WireConnection;65;0;62;1
WireConnection;131;0;132;0
WireConnection;194;0;65;0
WireConnection;194;1;185;0
WireConnection;76;0;74;0
WireConnection;76;1;75;0
WireConnection;73;0;131;0
WireConnection;73;1;131;1
WireConnection;183;0;65;0
WireConnection;183;1;194;0
WireConnection;69;0;68;0
WireConnection;69;1;73;0
WireConnection;69;2;76;0
WireConnection;79;0;131;2
WireConnection;79;1;131;3
WireConnection;70;0;69;0
WireConnection;70;2;79;0
WireConnection;46;1;44;1
WireConnection;46;0;44;2
WireConnection;190;0;183;0
WireConnection;190;1;185;0
WireConnection;108;0;106;0
WireConnection;108;1;105;0
WireConnection;121;0;120;0
WireConnection;127;0;124;0
WireConnection;127;1;190;0
WireConnection;14;0;121;0
WireConnection;14;1;121;1
WireConnection;192;0;129;4
WireConnection;5;1;70;0
WireConnection;141;0;137;0
WireConnection;141;1;138;0
WireConnection;107;0;46;0
WireConnection;107;1;106;0
WireConnection;107;2;108;0
WireConnection;140;1;139;1
WireConnection;140;0;139;2
WireConnection;142;0;140;0
WireConnection;142;1;137;0
WireConnection;142;2;141;0
WireConnection;11;0;12;0
WireConnection;11;1;14;0
WireConnection;135;0;5;1
WireConnection;135;1;107;0
WireConnection;26;0;121;2
WireConnection;26;1;121;3
WireConnection;238;0;233;4
WireConnection;130;0;192;0
WireConnection;130;1;127;0
WireConnection;133;0;135;0
WireConnection;133;1;130;0
WireConnection;231;0;238;0
WireConnection;45;0;47;0
WireConnection;45;1;48;0
WireConnection;45;2;142;0
WireConnection;23;0;11;0
WireConnection;23;2;26;0
WireConnection;232;0;231;0
WireConnection;136;0;118;0
WireConnection;136;1;45;0
WireConnection;125;0;133;0
WireConnection;224;1;217;0
WireConnection;3;1;23;0
WireConnection;218;0;224;1
WireConnection;218;1;232;0
WireConnection;122;0;3;0
WireConnection;122;1;190;0
WireConnection;119;0;117;0
WireConnection;119;1;136;0
WireConnection;4;0;122;0
WireConnection;4;1;119;0
WireConnection;193;0;126;0
WireConnection;220;0;218;0
WireConnection;157;0;4;0
WireConnection;222;0;193;0
WireConnection;222;1;220;0
WireConnection;53;0;42;0
WireConnection;244;0;243;3
WireConnection;134;1;5;1
WireConnection;59;0;60;0
WireConnection;59;1;61;0
WireConnection;59;2;64;0
WireConnection;115;1;103;0
WireConnection;115;0;56;0
WireConnection;56;0;53;0
WireConnection;110;1;116;0
WireConnection;32;0;28;0
WireConnection;98;0;110;0
WireConnection;98;1;99;0
WireConnection;98;2;101;0
WireConnection;144;0;143;0
WireConnection;49;0;44;0
WireConnection;50;0;49;0
WireConnection;103;0;98;0
WireConnection;143;0;139;0
WireConnection;234;0;219;0
WireConnection;234;1;238;0
WireConnection;223;0;193;0
WireConnection;223;1;222;0
WireConnection;240;0;239;0
WireConnection;240;1;244;0
WireConnection;240;2;242;0
WireConnection;240;3;242;0
WireConnection;42;0;32;0
WireConnection;42;1;51;0
WireConnection;42;2;52;0
WireConnection;123;0;124;0
WireConnection;123;1;190;0
WireConnection;114;0;103;0
WireConnection;114;1;56;0
WireConnection;15;0;121;2
WireConnection;15;1;121;3
WireConnection;101;0;99;0
WireConnection;101;1;100;0
WireConnection;0;2;158;0
WireConnection;0;9;223;0
ASEEND*/
//CHKSM=A556B57CD9018D8D85CE97E3E5BF03EA88D8AC67