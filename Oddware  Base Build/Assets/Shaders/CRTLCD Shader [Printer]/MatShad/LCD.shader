// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:True,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.02,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:31897,y:33985,varname:node_3138,prsc:2|diff-7451-OUT,spec-8510-R,gloss-2250-OUT,emission-4757-OUT;n:type:ShaderForge.SFN_Tex2d,id:5621,x:28846,y:33865,ptovrint:True,ptlb:Screen,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False|UVIN-8553-OUT;n:type:ShaderForge.SFN_Multiply,id:7619,x:31212,y:34156,varname:node_7619,prsc:2|A-4462-OUT,B-2744-OUT;n:type:ShaderForge.SFN_Slider,id:2250,x:31848,y:33902,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Gloss,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Fresnel,id:2769,x:30887,y:34390,varname:node_2769,prsc:2|EXP-1107-OUT;n:type:ShaderForge.SFN_Multiply,id:4757,x:31468,y:34207,varname:node_4757,prsc:2|A-7619-OUT,B-1052-OUT;n:type:ShaderForge.SFN_Clamp01,id:1052,x:31242,y:34390,varname:node_1052,prsc:2|IN-2991-OUT;n:type:ShaderForge.SFN_OneMinus,id:2991,x:31050,y:34390,varname:node_2991,prsc:2|IN-2769-OUT;n:type:ShaderForge.SFN_Vector1,id:1107,x:30708,y:34410,varname:node_1107,prsc:2,v1:0.3;n:type:ShaderForge.SFN_Vector3,id:9593,x:30393,y:33960,varname:node_9593,prsc:2,v1:1,v2:0,v3:0;n:type:ShaderForge.SFN_Vector3,id:7254,x:30387,y:34161,varname:node_7254,prsc:2,v1:0,v2:1,v3:0;n:type:ShaderForge.SFN_Vector3,id:2397,x:30387,y:34372,varname:node_2397,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Multiply,id:3612,x:30393,y:34048,varname:node_3612,prsc:2|A-9593-OUT,B-6838-OUT,C-2928-R;n:type:ShaderForge.SFN_Multiply,id:4883,x:30387,y:34252,varname:node_4883,prsc:2|A-7254-OUT,B-3516-OUT,C-2928-G;n:type:ShaderForge.SFN_Multiply,id:3946,x:30387,y:34466,varname:node_3946,prsc:2|A-2397-OUT,B-6786-OUT,C-2928-B;n:type:ShaderForge.SFN_Tex2d,id:2928,x:29854,y:34158,ptovrint:False,ptlb:PixelTexture,ptin:_PixelTexture,varname:_PixelTexture,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Add,id:7337,x:30645,y:34156,varname:node_7337,prsc:2|A-3612-OUT,B-4883-OUT,C-3946-OUT;n:type:ShaderForge.SFN_Clamp01,id:4462,x:30908,y:34159,varname:node_4462,prsc:2|IN-7337-OUT;n:type:ShaderForge.SFN_Tex2d,id:8510,x:31457,y:34003,ptovrint:False,ptlb:SpecularText,ptin:_SpecularText,varname:_SpecularText,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:7ae17514abd8e7141827efd07865b7b8,ntxv:1,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:7906,x:27888,y:33862,varname:node_7906,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ToggleProperty,id:3974,x:28846,y:33794,ptovrint:False,ptlb:ScreenON,ptin:_ScreenON,varname:_ScreenON,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_Posterize,id:5630,x:28111,y:33843,varname:node_5630,prsc:2|IN-7906-U,STPS-3861-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3861,x:27888,y:33791,ptovrint:False,ptlb:Pixel W,ptin:_PixelW,varname:_PixelW,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1280;n:type:ShaderForge.SFN_Posterize,id:2597,x:28111,y:33964,varname:node_2597,prsc:2|IN-7906-V,STPS-9341-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9341,x:27888,y:34041,ptovrint:False,ptlb:Pixel H,ptin:_PixelH,varname:_PixelH,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:720;n:type:ShaderForge.SFN_Append,id:6122,x:28346,y:33843,varname:node_6122,prsc:2|A-5630-OUT,B-2597-OUT;n:type:ShaderForge.SFN_Vector1,id:7451,x:31461,y:33824,varname:node_7451,prsc:2,v1:0;n:type:ShaderForge.SFN_Power,id:7350,x:29422,y:34103,varname:node_7350,prsc:2|VAL-107-OUT,EXP-4454-OUT;n:type:ShaderForge.SFN_Power,id:9247,x:29410,y:34236,varname:node_9247,prsc:2|VAL-7774-OUT,EXP-4454-OUT;n:type:ShaderForge.SFN_Power,id:3727,x:29410,y:34354,varname:node_3727,prsc:2|VAL-5102-OUT,EXP-4454-OUT;n:type:ShaderForge.SFN_Set,id:3837,x:29645,y:34236,varname:GammaG,prsc:2|IN-9247-OUT;n:type:ShaderForge.SFN_Set,id:368,x:29645,y:34354,varname:GammB,prsc:2|IN-3727-OUT;n:type:ShaderForge.SFN_Set,id:3625,x:29645,y:34103,varname:GammaR,prsc:2|IN-7350-OUT;n:type:ShaderForge.SFN_Get,id:6838,x:30160,y:34067,varname:node_6838,prsc:2|IN-3625-OUT;n:type:ShaderForge.SFN_Get,id:3516,x:30160,y:34269,varname:node_3516,prsc:2|IN-3837-OUT;n:type:ShaderForge.SFN_Get,id:6786,x:30160,y:34485,varname:node_6786,prsc:2|IN-368-OUT;n:type:ShaderForge.SFN_Multiply,id:107,x:29088,y:33832,varname:node_107,prsc:2|A-3974-OUT,B-5621-R;n:type:ShaderForge.SFN_Multiply,id:7774,x:29088,y:33955,varname:node_7774,prsc:2|A-3974-OUT,B-5621-G;n:type:ShaderForge.SFN_Multiply,id:5102,x:29088,y:34067,varname:node_5102,prsc:2|A-3974-OUT,B-5621-B;n:type:ShaderForge.SFN_Lerp,id:8553,x:28616,y:33865,varname:node_8553,prsc:2|A-6122-OUT,B-3897-UVOUT,T-6135-OUT;n:type:ShaderForge.SFN_Depth,id:7093,x:28171,y:34136,varname:node_7093,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:3897,x:28346,y:33981,varname:node_3897,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Clamp01,id:6135,x:28346,y:34136,varname:node_6135,prsc:2|IN-7093-OUT;n:type:ShaderForge.SFN_Slider,id:4454,x:28959,y:34253,ptovrint:False,ptlb:Gamma Correction,ptin:_GammaCorrection,varname:node_4454,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1.5,max:4;n:type:ShaderForge.SFN_Slider,id:2744,x:30893,y:34302,ptovrint:False,ptlb:Brightness,ptin:_Brightness,varname:node_2744,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:3,max:20;proporder:3974-2744-4454-3861-9341-2250-5621-8510-2928;pass:END;sub:END;*/

Shader "Printer/LCD" {
    Properties {
        [MaterialToggle] _ScreenON ("ScreenON", Float ) = 1
        _Brightness ("Brightness", Range(0, 20)) = 3
        _GammaCorrection ("Gamma Correction", Range(1, 4)) = 1.5
        _PixelW ("Pixel W", Float ) = 1280
        _PixelH ("Pixel H", Float ) = 720
        _Gloss ("Gloss", Range(0, 1)) = 0
        [HDR]_MainTex ("Screen", 2D) = "black" {}
        _SpecularText ("SpecularText", 2D) = "gray" {}
        [HDR]_PixelTexture ("PixelTexture", 2D) = "black" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Gloss;
            uniform sampler2D _PixelTexture; uniform float4 _PixelTexture_ST;
            uniform sampler2D _SpecularText; uniform float4 _SpecularText_ST;
            uniform fixed _ScreenON;
            uniform float _PixelW;
            uniform float _PixelH;
            uniform float _GammaCorrection;
            uniform float _Brightness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 projPos : TEXCOORD7;
                LIGHTING_COORDS(8,9)
                UNITY_FOG_COORDS(10)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD11;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float4 _SpecularText_var = tex2D(_SpecularText,TRANSFORM_TEX(i.uv0, _SpecularText));
                float3 specularColor = float3(_SpecularText_var.r,_SpecularText_var.r,_SpecularText_var.r);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 indirectSpecular = (gi.indirect.specular)*specularColor;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float node_7451 = 0.0;
                float3 diffuseColor = float3(node_7451,node_7451,node_7451);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float2 node_8553 = lerp(float2(floor(i.uv0.r * _PixelW) / (_PixelW - 1),floor(i.uv0.g * _PixelH) / (_PixelH - 1)),i.uv0,saturate(partZ));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_8553, _MainTex));
                float GammaR = pow((_ScreenON*_MainTex_var.r),_GammaCorrection);
                float4 _PixelTexture_var = tex2D(_PixelTexture,TRANSFORM_TEX(i.uv0, _PixelTexture));
                float GammaG = pow((_ScreenON*_MainTex_var.g),_GammaCorrection);
                float GammB = pow((_ScreenON*_MainTex_var.b),_GammaCorrection);
                float3 emissive = ((saturate(((float3(1,0,0)*GammaR*_PixelTexture_var.r)+(float3(0,1,0)*GammaG*_PixelTexture_var.g)+(float3(0,0,1)*GammB*_PixelTexture_var.b)))*_Brightness)*saturate((1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),0.3))));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Gloss;
            uniform sampler2D _PixelTexture; uniform float4 _PixelTexture_ST;
            uniform sampler2D _SpecularText; uniform float4 _SpecularText_ST;
            uniform fixed _ScreenON;
            uniform float _PixelW;
            uniform float _PixelH;
            uniform float _GammaCorrection;
            uniform float _Brightness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 projPos : TEXCOORD7;
                LIGHTING_COORDS(8,9)
                UNITY_FOG_COORDS(10)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float4 _SpecularText_var = tex2D(_SpecularText,TRANSFORM_TEX(i.uv0, _SpecularText));
                float3 specularColor = float3(_SpecularText_var.r,_SpecularText_var.r,_SpecularText_var.r);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float node_7451 = 0.0;
                float3 diffuseColor = float3(node_7451,node_7451,node_7451);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Gloss;
            uniform sampler2D _PixelTexture; uniform float4 _PixelTexture_ST;
            uniform sampler2D _SpecularText; uniform float4 _SpecularText_ST;
            uniform fixed _ScreenON;
            uniform float _PixelW;
            uniform float _PixelH;
            uniform float _GammaCorrection;
            uniform float _Brightness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float4 projPos : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float2 node_8553 = lerp(float2(floor(i.uv0.r * _PixelW) / (_PixelW - 1),floor(i.uv0.g * _PixelH) / (_PixelH - 1)),i.uv0,saturate(partZ));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_8553, _MainTex));
                float GammaR = pow((_ScreenON*_MainTex_var.r),_GammaCorrection);
                float4 _PixelTexture_var = tex2D(_PixelTexture,TRANSFORM_TEX(i.uv0, _PixelTexture));
                float GammaG = pow((_ScreenON*_MainTex_var.g),_GammaCorrection);
                float GammB = pow((_ScreenON*_MainTex_var.b),_GammaCorrection);
                o.Emission = ((saturate(((float3(1,0,0)*GammaR*_PixelTexture_var.r)+(float3(0,1,0)*GammaG*_PixelTexture_var.g)+(float3(0,0,1)*GammB*_PixelTexture_var.b)))*_Brightness)*saturate((1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),0.3))));
                
                float node_7451 = 0.0;
                float3 diffColor = float3(node_7451,node_7451,node_7451);
                float4 _SpecularText_var = tex2D(_SpecularText,TRANSFORM_TEX(i.uv0, _SpecularText));
                float3 specColor = float3(_SpecularText_var.r,_SpecularText_var.r,_SpecularText_var.r);
                float roughness = 1.0 - _Gloss;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
