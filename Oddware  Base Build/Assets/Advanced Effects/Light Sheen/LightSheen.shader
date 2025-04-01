Shader "UI/SheenEffect"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SheenColor ("Sheen Color", Color) = (1, 1, 1, 1)
        _SheenIntensity ("Sheen Intensity", Range(0, 1)) = 0.5
        _SheenSpeed ("Sheen Speed", Float) = 1.0
        _SheenWidth ("Sheen Width", Range(0.01, 0.5)) = 0.1
        _TimeOffset ("Sheen Time Offset", Float) = 0.0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "CanvasOverlay" = "True"
        }

        Stencil
        {
            Ref 1
            Comp Equal
            Pass Keep
            Fail Keep
            ZFail Keep
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _SheenColor;
            float _SheenIntensity;
            float _SheenSpeed;
            float _SheenWidth;
            float _TimeOffset;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the base texture (UI element texture)
                fixed4 col = tex2D(_MainTex, i.uv);

                // Compute sheen movement over time
                float sheenPos = sin((_Time.y + _TimeOffset) * _SheenSpeed + i.uv.x * 3.14159);
                
                // Create the sheen highlight using a smoothstep for soft edges
                float sheenMask = smoothstep(0.5 - _SheenWidth, 0.5 + _SheenWidth, sheenPos);

                // Calculate the final sheen color, modulated by sheen intensity
                fixed4 sheen = _SheenColor * sheenMask * _SheenIntensity;

                // Combine the base texture and the sheen effect
                fixed4 result = col + sheen;

                // Ensure alpha blending for transparency
                result.a = col.a;

                return result;
            }
            ENDCG
        }
    }

    FallBack "UI/Default"
}
