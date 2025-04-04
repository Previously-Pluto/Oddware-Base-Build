Shader "Holistic/AdvOutline" {
	
	 Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _OutlineColor ("Outline Color", Color) = (0,0,0,1)
	  _Outline ("Outline Width", Range (.002, 0.1)) = .005
    }
    
   SubShader {

      CGPROGRAM
	      #pragma surface surf Lambert
	      struct Input {
	          float2 uv_MainTex;
	      };

	      sampler2D _MainTex;
	      void surf (Input IN, inout SurfaceOutput o) {
	          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
	      }
      ENDCG

//Geometry is drawn first, and then the outline shows up on top of the geometry and highlights its contours
      Pass {
			Cull Front //This is a whole bunny, but this cuts the front off the outline. You're looking at the backfaces of the 2nd render

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
				
			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
			};
			
			float _Outline;
			float4 _OutlineColor;
			
			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				float3 norm   = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal)); //Returning the normal in the form of a world coordinate
				float2 offset = TransformViewToProjection(norm.xy);

				o.pos.xy += offset * o.pos.z * _Outline;
				o.color = _OutlineColor;
				return o;
			}


			fixed4 frag(v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}

    } 
    Fallback "Diffuse"
}

