Shader "Holistic/GlassUpdatedUI"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Normalmap", 2D) = "bump" {}
        _GlassOpacity ("Glass Opacity", Float) = 0.5
        _ScaleUV ("Scale", Range(1,1000)) = 1

        _WaterTex ("Water", 2D) = "white" {}
        _FoamTex ("Foam", 2D) = "white" {}
        _ScrollX ("Scroll X", Range(-5,5)) = 1
        _ScrollY ("Scroll Y", Range(-5,5)) = 1
        _Tint("Colour Tint", Color) = (1,1,1,1)
        _Freq("Frequency", Range(0,5)) = 3
        _Speed("Speed",Range(0,100)) = 10
        _Amp("Amplitude",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent" // Ensure it renders in the correct order
            "IgnoreProjector" = "True" // UI ignores projectors
            "RenderType" = "Transparent"
            "CanvasOverlay" = "True" // Required for UI Canvas
        }
        GrabPass {}
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

                float3 normal: NORMAL;
                float4 texcoord: TEXCOORD0;
                float4 texcoord1: TEXCOORD1;
                float4 texcoord2: TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _FoamTex;
            float _ScrollX;
            float _ScrollY;

            struct Input
            {
                float2 uv_MainTex;
                float3 vertColor;
            };

            float4 _Tint;
            float _Freq;
            float _Speed;
            float _Amp;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 uvgrab : TEXCOORD1;
                float2 uvbump : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            //The size of the pixels that are in the texture. "Resolution" of the pixel per say
            sampler2D _WaterTex;
            float4 _MainTex_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _ScaleUV;
            float _GlassOpacity;

            v2f vert(appdata v)
            {
                v2f o;

                //Wave Logic
                //UNITY_INITIALIZE_OUTPUT(Input, o);
            float t = _Time * _Speed;
            float waveHeightX = sin(t + v.vertex.x * _Freq) * _Amp +
                sin(t * 2* 0.8 + v.vertex.x * _Freq * 2* 0.8) * _Amp; //second sin wave adds varying height and complexity
            float waveHeightY = sin(t + v.vertex.z * _Freq * 0.8) * _Amp* 0.8 +
                sin(t * 2* 0.8 + v.vertex.z * _Freq * 2* 0.8) * _Amp* 0.8; //second sin wave adds varying height and complexity
            v.vertex.y = v.vertex.y + waveHeightX;
            v.vertex.y = v.vertex.y +  waveHeightY;
            v.normal = normalize(float3(v.normal.x + waveHeightX, v.normal.y, v.normal.z + waveHeightY));
         //  o.vertColor = waveHeightX + 1.1;
                
                //Glass Logic
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
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvbump = TRANSFORM_TEX(v.uv, _BumpMap);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half2 bump = UnpackNormal(tex2D(_BumpMap, i.uvbump)).rg;
                float2 offset = bump * _ScaleUV * _GrabTexture_TexelSize.xy;
                //change textel size blended with the UV map
                i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;

                fixed4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
                fixed4 tint = tex2D(_MainTex, i.uv); //blends texture with grab texture
                col *= tint;
                col.a *= _GlassOpacity;
                return col;
            }
            ENDCG
        }
    }
}