Shader "Holistic/Extrude" { //This is a surface shader
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Amount ("Extrude", Range(-1,1)) = 0.01
    }
    
    SubShader {

      CGPROGRAM
	      #pragma surface surf Lambert vertex:vert //Vertex vert looks for a vertex shader in this script
	      
	      struct Input {
	          float2 uv_MainTex;
	      };

	      struct appdata {
	      	float4 vertex: POSITION;
	      	float3 normal: NORMAL;
	      	float4 texcoord: TEXCOORD0; //the uv value. Required because the surface shader is using a texture
	      };

	      float _Amount;

	      void vert (inout appdata v) { //vert shader part. Unity is handling fragment shader operations in the background using this appdata (inout)
	          v.vertex.xyz += v.normal * _Amount;
	      }

	      sampler2D _MainTex;
	      void surf (Input IN, inout SurfaceOutput o) { //surface shader part
	          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
	      }

      ENDCG
    } 
    Fallback "Diffuse"
  }